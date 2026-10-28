using System.Net;
using System.Text.Json;
using api.Controllers;
using api.Dtos.AIDtos;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace api.Tests.Controllers
{
    public class AIControllerTests
    {
        [Theory]
        [InlineData(401, 502)]
        [InlineData(403, 502)]
        [InlineData(429, 503)]
        [InlineData(500, 502)]
        public async Task Chat_UpstreamFailure_ReturnsSafeProblem(int upstreamStatus, int expectedStatus)
        {
            await AssertFailure(
                new HttpRequestException("sensitive upstream details", null, (HttpStatusCode)upstreamStatus),
                expectedStatus);
        }

        [Fact]
        public async Task Chat_NetworkFailure_ReturnsBadGateway()
        {
            await AssertFailure(new HttpRequestException("sensitive upstream details"), 502);
        }

        [Fact]
        public async Task Chat_Timeout_ReturnsGatewayTimeout()
        {
            await AssertFailure(new TaskCanceledException("sensitive upstream details"), 504);
        }

        [Fact]
        public async Task Chat_InvalidJson_ReturnsBadGateway()
        {
            await AssertFailure(new JsonException("sensitive upstream details"), 502);
        }

        private static async Task AssertFailure(Exception exception, int expectedStatus)
        {
            var aiService = new Mock<IAIService>();
            aiService.Setup(service => service.AskAsync(It.IsAny<string>())).ThrowsAsync(exception);
            var availability = new Mock<IAvailabilityService>(MockBehavior.Strict);
            var controller = new AIController(
                aiService.Object,
                new AIBookingService(availability.Object),
                NullLogger<AIController>.Instance)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            var result = Assert.IsType<ObjectResult>(
                await controller.Chat(new ChatRequestDto { Question = "Boka ett mötesrum" }));
            var problem = Assert.IsType<ProblemDetails>(result.Value);

            Assert.Equal(expectedStatus, result.StatusCode);
            Assert.Equal(expectedStatus, problem.Status);
            Assert.False(string.IsNullOrWhiteSpace(problem.Detail));
            Assert.DoesNotContain("sensitive upstream details", problem.Detail);
            availability.VerifyNoOtherCalls();
        }
    }
}
