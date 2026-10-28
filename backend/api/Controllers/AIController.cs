using System.Net;
using System.Text.Json;
using api.Dtos.AIDtos;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;
        private readonly AIBookingService _aIBookingService;
        private readonly ILogger<AIController> _logger;

        public AIController(
            IAIService aiService,
            AIBookingService aIBookingService,
            ILogger<AIController> logger)
        {
            _aiService = aiService;
            _aIBookingService = aIBookingService;
            _logger = logger;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat(ChatRequestDto chatRequest)
        {
            api.Dtos.AIDtos.BookingIntentDto intent;
            try
            {
                intent = await _aiService.AskAsync(chatRequest.Question);
            }
            catch (HttpRequestException ex)
            {
                // Log only the upstream status, never credentials or response bodies.
                _logger.LogWarning("AI request failed with upstream status {StatusCode}.", ex.StatusCode);

                if (ex.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                {
                    return Problem(
                        statusCode: StatusCodes.Status502BadGateway,
                        detail: "AI-tjänsten kunde inte autentiseras. Kontakta administratören.");
                }

                if (ex.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    return Problem(
                        statusCode: StatusCodes.Status503ServiceUnavailable,
                        detail: "AI-tjänsten är tillfälligt begränsad. Försök igen senare.");
                }

                return Problem(
                    statusCode: StatusCodes.Status502BadGateway,
                    detail: "AI-tjänsten är inte tillgänglig just nu. Försök igen senare.");
            }
            catch (OperationCanceledException) when (!HttpContext.RequestAborted.IsCancellationRequested)
            {
                _logger.LogWarning("AI request timed out.");
                return Problem(
                    statusCode: StatusCodes.Status504GatewayTimeout,
                    detail: "AI-tjänsten svarade inte i tid. Försök igen senare.");
            }
            catch (JsonException)
            {
                _logger.LogWarning("AI returned an invalid response format.");
                return Problem(
                    statusCode: StatusCodes.Status502BadGateway,
                    detail: "AI-tjänstens svar kunde inte tolkas. Försök igen.");
            }

            var resource = await _aIBookingService.FindAvailableResourceAsync(intent);
            if (resource == null)
            {
                return NotFound("Ingen ledig resurs hittades.");
            }

            return Ok(new { intent, resourceId = resource.ResourceId });
        }
    }
}
