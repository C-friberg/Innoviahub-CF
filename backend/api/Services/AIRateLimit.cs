using System.Globalization;
using System.Security.Claims;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace api.Services
{
    public static class AIRateLimit
    {
        public const string PolicyName = "ai-per-user";

        public static void Configure(RateLimiterOptions options)
        {
            options.AddPolicy(PolicyName, context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }));

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        Math.Ceiling(retryAfter.TotalSeconds).ToString(CultureInfo.InvariantCulture);
                }

                await Results.Problem(
                    statusCode: StatusCodes.Status429TooManyRequests,
                    detail: "Du har nått gränsen på fem AI-anrop per minut. Vänta en stund och försök igen.")
                    .ExecuteAsync(context.HttpContext);
            };
        }
    }
}
