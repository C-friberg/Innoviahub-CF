using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.AIDtos;
using api.Enums;
using api.Models;

namespace api.Services
{
    public class AIBookingService
    {
        private readonly AvailabilityService _availabilityService;
        public AIBookingService(AvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        public async Task<Resource?> FindAvailableResourceAsync(BookingIntentDto intent)
        {
            if (intent.ResourceType == null ||
                intent.Date == null ||
                intent.StartTime == null ||
                intent.EndTime == null)
            {
                return null;
            }

            if (!Enum.TryParse<ResourceType>(intent.ResourceType, true, out var resourceType))
            {
                return null;
            }

            var startTime = DateTime.SpecifyKind(intent.Date.Value.ToDateTime(intent.StartTime.Value), DateTimeKind.Utc);

            var endTime = DateTime.SpecifyKind(intent.Date.Value.ToDateTime(intent.EndTime.Value), DateTimeKind.Utc);

            return await _availabilityService.GetFirstAvailableAsync(resourceType, startTime, endTime);
        }
    }
}