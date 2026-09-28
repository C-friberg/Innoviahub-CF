using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;

namespace api.Dtos.ResourceDtos
{
    public class ResourceTypeAvailabilityDto
    {
        public ResourceType ResourceType { get; set; }
        public int TotalResources { get; set; }
        public int AvailableResources { get; set; }
    }
}