using Xunit;
using Moq;
using api.Models;
using api.Interfaces;
using api.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace api.Tests.Services
{
    public class AvailabilityServiceTests
    {
        [Fact]
        public async Task GetFirstAvailableAsync_WhenFirstIsBooked_ReturnsSecondResource()
        {
            var startTime = new DateTime(2026, 10, 9, 9, 0, 0, DateTimeKind.Utc);
            var endTime = new DateTime(2026, 10, 9, 12, 0, 0, DateTimeKind.Utc);

            var resources = new List<Resource>
            {
                new Resource
                {
                    ResourceId = 40,
                    ResourceType = Enums.ResourceType.VRHeadset
                },
                new Resource
                {
                    ResourceId = 41,
                    ResourceType = Enums.ResourceType.VRHeadset
                }
            };

            var mockResourceRepository = new Mock<IResourceRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();

            mockResourceRepository.Setup(x => x.GetByTypeAsync(Enums.ResourceType.VRHeadset)).ReturnsAsync(resources);

            // här är en resurs otillgänglig. Vi vill att testet ska boka Vr headset med id 41.
            mockBookingRepository.Setup(x => x.IsResourceAvailableAsync(startTime, endTime, 40)).ReturnsAsync(false);

            mockBookingRepository.Setup(x => x.IsResourceAvailableAsync(startTime, endTime, 41)).ReturnsAsync(true);

            var service = new AvailabilityService(mockBookingRepository.Object, mockResourceRepository.Object);

            //Act
            var result = await service.GetFirstAvailableAsync(Enums.ResourceType.VRHeadset, startTime, endTime);

            //Assert 
            Assert.NotNull(result);
            Assert.Equal(41, result.ResourceId);

        }


        [Fact]
        public async Task GetFirstAvailableAsync_WhenAllResourcesAreBooked_ReturnsNull()
        {
            var startTime = new DateTime(2026, 10, 9, 9, 0, 0, DateTimeKind.Utc);
            var endTime = new DateTime(2026, 10, 9, 12, 0, 0, DateTimeKind.Utc);

            var resources = new List<Resource>
            {
                new Resource
                {
                    ResourceId = 40,
                    ResourceType = Enums.ResourceType.VRHeadset
                },

                new Resource
                {
                    ResourceId = 41,
                    ResourceType = Enums.ResourceType.VRHeadset
                }
            };

            var mockResourceRepository = new Mock<IResourceRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();

            mockResourceRepository.Setup(x => x.GetByTypeAsync(Enums.ResourceType.VRHeadset)).ReturnsAsync(resources);

            mockBookingRepository.Setup(x => x.IsResourceAvailableAsync(startTime, endTime, It.IsAny<int>())).ReturnsAsync(false);

            var service = new AvailabilityService(mockBookingRepository.Object, mockResourceRepository.Object);

            //act
            var result = await service.GetFirstAvailableAsync(Enums.ResourceType.VRHeadset, startTime, endTime);

            //Assert
            Assert.Null(result);

            mockBookingRepository.Verify(x => x.IsResourceAvailableAsync(startTime, endTime, 40), Times.Once);
            mockBookingRepository.Verify(x => x.IsResourceAvailableAsync(startTime, endTime, 41), Times.Once);

        }
    }
}