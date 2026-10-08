using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Models;

namespace api.Services
{
    public interface IAvailabilityService
    {
        Task<Resource?> GetFirstAvailableAsync(ResourceType resourceType, DateTime startTime, DateTime endTime); 
    }
}