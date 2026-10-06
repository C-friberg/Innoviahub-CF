using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public AIController(IAIService aiService, AIBookingService aIBookingService)
        {
            _aiService = aiService;
            _aIBookingService = aIBookingService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat(ChatRequestDto chatRequest)
        {
            var intent = await _aiService.AskAsync(chatRequest.Question);
            var resource = await _aIBookingService.FindAvailableResourceAsync(intent);

            if (resource == null)
            {
                return NotFound("Ingen ledig resurs hittades.");
            }


            return Ok(new { intent, resourceId = resource.ResourceId });
        }
    }
}