using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.DeviceDtos
{
    public class DeviceDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid? RoomId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Serial { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}