using api.Data;
using api.Dtos.DeviceDtos;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly HttpClient _httpClient;
        private readonly Guid _tenantId;
        private readonly AppDbContext _context;

        public DeviceService(HttpClient httpClient, IConfiguration configuration, AppDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
            _tenantId = configuration.GetValue<Guid?>("DeviceRegistry:TenantId")
                ?? throw new InvalidOperationException("DeviceRegistry:TenantId saknas.");
        }

        public async Task<List<DeviceDto>> GetAllAsync()
        {
            var devices = await _httpClient.GetFromJsonAsync<List<DeviceDto>>(
                $"api/tenants/{_tenantId}/devices")
                ?? throw new InvalidOperationException("DeviceRegistry returnerade ett tomt svar.");

            var links = await _context.Sensors.AsNoTracking()
                .Where(sensor => sensor.DeviceRegistryDeviceId != null && sensor.Resource != null)
                .Select(sensor => new
                {
                    DeviceId = sensor.DeviceRegistryDeviceId!.Value,
                    sensor.ResourceId,
                    ResourceType = sensor.Resource!.ResourceType
                })
                .ToDictionaryAsync(sensor => sensor.DeviceId);

            foreach (var device in devices)
            {
                if (links.TryGetValue(device.Id, out var link))
                {
                    device.ResourceId = link.ResourceId;
                    device.ResourceType = link.ResourceType;
                }
            }

            return devices;
        }
    }
}
