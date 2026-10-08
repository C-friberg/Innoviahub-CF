namespace api.Tests;

using Xunit;
using Moq;
using api.Dtos.AIDtos;
using api.Services;
using api.Models;

public class AIBookingServiceTests
{
    [Fact]
    public async Task FindAvailableResourceAsync_WhenEndTimeIsNull_ReturnsNull()
    {
        //Ararnge
        var intent = new BookingIntentDto
        {
            ResourceType = "VRHeadset",
            Date = new DateOnly(2026, 10, 9),
            StartTime = new TimeOnly(9, 0),
            EndTime = null
        };

        var service = new AIBookingService(null!);

        //Act
        var result = await service.FindAvailableResourceAsync(intent);

        //Assert
        Assert.Null(result);

    }

    [Fact]
    public async Task FindAvailableResourceAsync_WhenResourceISInvalid_ReturnsNull()
    {
        //Arrange
        var intent = new BookingIntentDto
        {
            ResourceType = "Casper",
            Date = new DateOnly(2026, 10, 9),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        var service = new AIBookingService(null!);

        //Act

        var result = await service.FindAvailableResourceAsync(intent);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindAvailableResourceAsync_WhenResourceIsAvailable_ReturnsResource()
    {
        var intent = new BookingIntentDto
        {
            ResourceType = "VRHeadset",
            Date = new DateOnly(2026, 10, 9),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        var expectedResource = new Resource
        {
            ResourceId = 40,
            ResourceType = Enums.ResourceType.VRHeadset
        };

        var mockAvailabilityService = new Mock<IAvailabilityService>();

        mockAvailabilityService.Setup(x => x.GetFirstAvailableAsync(
            Enums.ResourceType.VRHeadset,
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>()
        )).ReturnsAsync(expectedResource);

        var service = new AIBookingService(mockAvailabilityService.Object);

        //Act
        var result = await service.FindAvailableResourceAsync(intent);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(40, result.ResourceId);
    }

    [Fact]
    public async Task FindAvailableResourceAsync_WhenNoResourceIsAvailable_ReturnsNull()
    {
        //Arrange
        var intent = new BookingIntentDto
        {
            ResourceType = "VRHeadset",
            Date = new DateOnly(2026, 10, 9),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        var mockAvailabilityService = new Mock<IAvailabilityService>();

        mockAvailabilityService.Setup(x => x.GetFirstAvailableAsync(
            Enums.ResourceType.VRHeadset,
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>()
        )).ReturnsAsync((Resource?)null);

        var service = new AIBookingService(mockAvailabilityService.Object);

        //Act
        var result = await service.FindAvailableResourceAsync(intent);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindAvailableResourceAsync_WhenIntentIsValid_PassesCorrectParameters()
    {
        //Arrange
        var intent = new BookingIntentDto
        {
            ResourceType = "VRHeadset",
            Date = new DateOnly(2026, 10, 9),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(12, 0)
        };

        var expectedStart = new DateTime(2026, 10, 9, 9, 0, 0, DateTimeKind.Utc);
        var expectedEnd = new DateTime(2026, 10, 9, 12, 0, 0, DateTimeKind.Utc);

        var mockAvailabilityService = new Mock<IAvailabilityService>();

        var service = new AIBookingService(mockAvailabilityService.Object);

        //Act
        await service.FindAvailableResourceAsync(intent);

        //Assert
        mockAvailabilityService.Verify(
            x => x.GetFirstAvailableAsync(
                Enums.ResourceType.VRHeadset,
                expectedStart,
                expectedEnd),
                Times.Once);
    }
}