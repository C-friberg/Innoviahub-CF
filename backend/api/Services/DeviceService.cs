using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.DeviceDtos;
using api.Interfaces;

namespace api.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly HttpClient _httpClient;
        private readonly Guid _tenantId; 
        public DeviceService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient; 
            _tenantId = configuration.GetValue<Guid?>("DeviceRegistry:TenantId")
            ?? throw new InvalidOperationException("DeviceRegistry:TenantId saknas");
        }
        public async Task<List<DeviceDto>> GetAllAsync()
        {
            var devices = await _httpClient.GetFromJsonAsync<List<DeviceDto>>($"api/tenants/{_tenantId}/devices");
            return devices ?? throw new InvalidOperationException("DeviceRegistry returnerade ett tomt svar. ");
            
        }
    }
}