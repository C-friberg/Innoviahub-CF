using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.DeviceDtos;

namespace api.Interfaces
{
    public interface IDeviceService
    {
        Task<List<DeviceDto>> GetAllAsync();
    }
}