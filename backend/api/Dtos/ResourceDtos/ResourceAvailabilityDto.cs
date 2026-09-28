using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.ResourceDtos
{
    public class ResourceAvailabilityDto
    {
        public int ResourceId { get; set; }
        public bool IsAvailable { get; set; }
    }
}