using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.IdentityModel.Tokens.Jwt;
using api.Controllers;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace api.Tests.Controllers
{
    public class AIAccessTests
    {
        private const string TestKey = "test-only-signing-key-that-is-at-least-32-characters";
        private const string Origin = "https://innovia-3-frontend-production.up.railway.app";

        [Fact]
        public async Task Chat_WithoutValidJwt_IsRejectedBeforeCallingAI()
        {
            var (app, client, ai) = await StartApplication();
            await using var lifetime = app;
            using var httpClient = client;

            var anonymous = await PostChat(client, null);
            var invalid = await PostChat(client, "invalid-token");

            Assert.Equal(HttpStatusCode.Unauthorized, anonymous.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, invalid.StatusCode);
            Assert.Equal(Origin, anonymous.Headers.GetValues("Access-Control-Allow-Origin").Single());
            ai.Verify(service => service.AskAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Chat_SixthRequestIsLimited_ButAnotherUserHasOwnAllowance()
        {
            var (app, client, ai) = await StartApplication();
            await using var lifetime = app;
            using var httpClient = client;
            var firstToken = CreateToken("first-user");

            for (var index = 0; index < 5; index++)
            {
                using var response = await PostChat(client, firstToken);
                Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
            }

            using var limited = await PostChat(client, firstToken);
            Assert.Equal(HttpStatusCode.TooManyRequests, limited.StatusCode);
            Assert.NotNull(limited.Headers.RetryAfter);
            Assert.Equal(Origin, limited.Headers.GetValues("Access-Control-Allow-Origin").Single());
            var detail = await limited.Content.ReadAsStringAsync();
            Assert.Contains("429", detail);

            using var otherUser = await PostChat(client, CreateToken("second-user"));
            Assert.Equal(HttpStatusCode.BadGateway, otherUser.StatusCode);
            ai.Verify(service => service.AskAsync(It.IsAny<string>()), Times.Exactly(6));
        }

        private static async Task<HttpResponseMessage> PostChat(HttpClient client, string? token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/ai/chat");
            request.Headers.Add("Origin", Origin);
            if (token != null) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new { question = "Boka ett mötesrum" });
            return await client.SendAsync(request);
        }

        private static string CreateToken(string userId)
        {
            var token = new JwtSecurityToken(
                issuer: "test", audience: "test",
                claims: new[] { new Claim(ClaimTypes.NameIdentifier, userId) },
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestKey)), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static async Task<(WebApplication App, HttpClient Client, Mock<IAIService> AI)> StartApplication()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Production" });
            builder.WebHost.UseKestrel().UseUrls("http://127.0.0.1:0");
            builder.Logging.ClearProviders();
            builder.Services.AddControllers().AddApplicationPart(typeof(AIController).Assembly);
            builder.Services.AddProblemDetails();
            builder.Services.AddCors(options => options.AddPolicy("Frontend", policy =>
                policy.WithOrigins(Origin).AllowAnyHeader().AllowAnyMethod()));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "test",
                    ValidAudience = "test",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestKey)),
                    ValidateIssuer = true, ValidateAudience = true,
                    ValidateIssuerSigningKey = true, ValidateLifetime = true
                };
            });
            builder.Services.AddAuthorization();
            builder.Services.AddRateLimiter(AIRateLimit.Configure);
            var ai = new Mock<IAIService>();
            ai.Setup(service => service.AskAsync(It.IsAny<string>()))
                .ThrowsAsync(new HttpRequestException("test failure", null, HttpStatusCode.Unauthorized));
            builder.Services.AddSingleton(ai.Object);
            builder.Services.AddSingleton(new AIBookingService(new Mock<IAvailabilityService>().Object));

            var app = builder.Build();
            app.UseRouting();
            app.UseCors("Frontend");
            app.UseExceptionHandler();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.MapControllers();
            await app.StartAsync();
            var address = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!
                .Addresses.Single();
            return (app, new HttpClient { BaseAddress = new Uri(address) }, ai);
        }
    }
}
