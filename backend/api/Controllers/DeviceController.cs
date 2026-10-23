using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService; 
        private readonly ILogger<DeviceController> _logger; 

        public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger)
        {
            _deviceService = deviceService;
            _logger = logger; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var devices = await _deviceService.GetAllAsync();
                return Ok(devices);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Kude inte hämta enheter från DeviceRegistry");
                return Problem(statusCode: StatusCodes.Status502BadGateway, detail: "Kunde inte hämta enheter från DeviceRegistry");
            }
            catch(OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Anropet till DeviceRegistry tog för lång tid");
                return Problem(statusCode: StatusCodes.Status504GatewayTimeout, detail: "DeviceRegistry svarade inte i tid"); 
            }
        }
    }
}