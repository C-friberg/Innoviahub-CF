using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.BookingDtos;
using api.Models;

namespace api.Mappers
{
    public static class BookingMapper
    {
        public static BookingDto ToBookingDto(this Booking bookingModel)
        {
            return new BookingDto
            {
                BookingId = bookingModel.BookingId,
                StartTime = bookingModel.StartTime,
                EndTime = bookingModel.EndTime,
                ResourceId = bookingModel.ResourceId,
                ResourceType = bookingModel.Resource?.ResourceType.ToString() ?? "Okänd resurs",
                ResourceName = bookingModel.Resource != null ? GetResourceName(bookingModel.Resource) : "Okänd resurs",
                UserId = bookingModel.UserId,
                UserEmail = bookingModel.User?.Email ?? "Okänd användare"
            };
        }
        public static Booking ToBookingFromCreateDto(this CreateBookingDto bookingRequestDto, string userId)
        {
            return new Booking
            {
                ResourceId = bookingRequestDto.ResourceId,
                StartTime = bookingRequestDto.StartTime,
                EndTime = bookingRequestDto.EndTime,
                UserId = userId
            };
        }

        private static string GetResourceName(Resource resource)
        {
            return resource.ResourceType switch
            {
                Enums.ResourceType.Skrivbord =>
                    $"Skrivbord {resource.ResourceId}",

                Enums.ResourceType.Mötesrum =>
                    $"Mötesrum {resource.ResourceId - 19}",

                Enums.ResourceType.AIServer =>
                    $"AI-server {resource.ResourceId - 29}",

                Enums.ResourceType.VRHeadset =>
                    $"VR-headset {resource.ResourceId - 39}",

                _ => "Okänd resurs"
            };
        }
    }
}