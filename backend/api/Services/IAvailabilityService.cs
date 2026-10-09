using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.ResourceDtos;
using api.Enums;
using api.Models;

namespace api.Services
{
    public interface IAvailabilityService
    {
        Task<Resource?> GetFirstAvailableAsync(ResourceType resourceType, DateTime startTime, DateTime endTime);
        Task<ResourceAvailabilityDto?> GetResourceAvailabilityAsync(int resourceId, DateTime startTime, DateTime endTime);

        Task<ResourceTypeAvailabilityDto> GetResourceTypeAvailabilityAsync(ResourceType type, DateTime startTime, DateTime endTime);
    }
}