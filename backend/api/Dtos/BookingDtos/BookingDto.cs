using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.BookingDtos
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ResourceId { get; set; }
        public string ResourceType { get; set; } = string.Empty;
        public string ResourceName {get; set;} = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}