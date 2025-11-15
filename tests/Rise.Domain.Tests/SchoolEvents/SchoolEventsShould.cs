using NSubstitute;
using Rise.Domain.Common;
using Rise.Domain.Exceptions;
using Rise.Domain.Locations;
using Rise.Domain.SchoolEvents;

namespace Rise.Domain.Tests.SchoolEvents;

public class SchoolEventsShould
{
    Location _location = Substitute.For<Location>();

    [Fact]
    public void BeCreated()
    {
        var schoolEvent = new SchoolEvent(
            "Info Sessie Bachelorproef",
            "Informatie over je bachelorproef",
            new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero),
            new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0)),
            price: 0m,
            registerLink: "https://hogent.be/registreer",
            capacity: 50,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: "/images/infosessie.png",
            _location
        );

        schoolEvent.Title.ShouldBe("Info Sessie Bachelorproef");
        schoolEvent.Description.ShouldBe("Informatie over je bachelorproef");
        schoolEvent.Date.ShouldBe(new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero));
        schoolEvent.TimeRange.ShouldBe(new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0)));
        schoolEvent.Price.ShouldBe(0m);
        schoolEvent.RegisterLink.ShouldBe("https://hogent.be/registreer");
        schoolEvent.Capacity.ShouldBe(50);
        schoolEvent.Registrable.ShouldBeTrue();
        schoolEvent.Publicity.ShouldBe("Public");
        schoolEvent.ImageUrl.ShouldBe("/images/infosessie.png");
        schoolEvent.Location.ShouldNotBeNull();
    }

    [Theory]
    [InlineData("20:00:00", "18:00:00", "EndTime must be after StartTime")]
    [InlineData("23:59:59", "23:00:00", "EndTime must be after StartTime")]
    [InlineData("23:59:59", "0:00:00", "EndTime must be after StartTime")]
    [InlineData("09:00:00", "09:00:00", "EndTime must be after StartTime")]
    public void ThrowExceptionWhenStartTimeIsAfterOrEqualsEndTime(
        string startTimeStr, string endTimeStr, string expectedErrorMessage)
    {
        var date = new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero);

        var exception = Should.Throw<InvalidTimeRangeException>(() => new SchoolEvent(
            "Invalid Event",
            "An event with invalid time range",
            date,
            new TimeRange(TimeOnly.Parse(startTimeStr), TimeOnly.Parse(endTimeStr)),
            price: 0m,
            registerLink: "https://hogent.be/registreer",
            capacity: 50,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: "/images/event.png",
            _location
        ));

        exception.GetType().ShouldBe(typeof(InvalidTimeRangeException));
        exception.Message.ShouldBe(expectedErrorMessage);
    }

    [Theory]
    [InlineData(null, "2025-09-10T00:00:00", false, false, "https://hogent.be/registreer", false, false,
        typeof(ArgumentNullException))]
    [InlineData("", "2025-09-10T00:00:00", false, false, "https://hogent.be/registreer", false, false,
        typeof(ArgumentException))]
    [InlineData("   ", "2025-09-10T00:00:00", false, false, "https://hogent.be/registreer", false, false,
        typeof(ArgumentException))]
    [InlineData("Valid title", null, false, false, "https://hogent.be/registreer", false, false,
        typeof(ArgumentOutOfRangeException))]
    [InlineData("Valid title", "2025-09-10T00:00:00", true, false, "https://hogent.be/registreer", false, false,
        typeof(ArgumentNullException))]
    [InlineData("Valid title", "2025-09-10T00:00:00", false, true, "https://hogent.be/registreer", false, false,
        typeof(ArgumentNullException))]
    [InlineData("Valid title", "2025-09-10T00:00:00", false, false, null, false, false,
        typeof(ArgumentNullException))]
    [InlineData("Valid title", "2025-09-10T00:00:00", false, false, "https://hogent.be/registreer", true, false,
        typeof(ArgumentException))]
    [InlineData("Valid title", "2025-09-10T00:00:00", false, false, "https://hogent.be/registreer", false, true,
        typeof(ArgumentException))]
    public void ThrowExceptionWhenRequiredFieldIsNullOrEmpty(
        string title, string? dateStr, bool isTimeRangeNull, bool isLocationNull,
        string? registerLink, bool isRegisterLinkEmpty, bool isPublicityEmpty,
        Type expectedExceptionType)
    {
        var TEST_TIMERANGE = new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0));
        var date = string.IsNullOrWhiteSpace(dateStr)
            ? DateTimeOffset.MinValue
            : DateTimeOffset.Parse(dateStr);
        var location = isLocationNull ? null! : _location;
        var actualRegisterLink = isRegisterLinkEmpty ? "" : (registerLink);
        var publicity = isPublicityEmpty ? "" : "Public";

        var exception = Should.Throw<Exception>(() => new SchoolEvent(
            title,
            "An event with missing required fields",
            date,
            isTimeRangeNull ? null : TEST_TIMERANGE,
            price: 0m,
            registerLink: actualRegisterLink,
            capacity: 50,
            registrable: true,
            publicity: publicity,
            category: "Sport",
            imageUrl: "/images/event.png",
            location
        ));

        exception.GetType().ShouldBe(expectedExceptionType);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void ThrowExceptionWhenCapacityIsZeroOrNegative(int invalidCapacity)
    {
        var exception = Should.Throw<ArgumentException>(() => new SchoolEvent(
            "Valid Event",
            "Event with invalid capacity",
            new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero),
            new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0)),
            price: 0m,
            registerLink: "https://hogent.be/registreer",
            capacity: invalidCapacity,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: "/images/event.png",
            _location
        ));

        exception.GetType().ShouldBe(typeof(ArgumentException));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-10)]
    [InlineData(-100.50)]
    public void ThrowExceptionWhenPriceIsNegative(decimal invalidPrice)
    {
        var exception = Should.Throw<ArgumentException>(() => new SchoolEvent(
            "Valid Event",
            "Event with invalid price",
            new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero),
            new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0)),
            price: invalidPrice,
            registerLink: "https://hogent.be/registreer",
            capacity: 50,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: "/images/event.png",
            _location
        ));

        exception.GetType().ShouldBe(typeof(ArgumentException));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5.50)]
    [InlineData(100)]
    [InlineData(999.99)]
    public void AcceptValidPrice(decimal validPrice)
    {
        var schoolEvent = new SchoolEvent(
            "Valid Event",
            "Event with valid price",
            new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero),
            new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0)),
            price: validPrice,
            registerLink: "https://hogent.be/registreer",
            capacity: 50,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: "/images/event.png",
            _location
        );

        schoolEvent.Price.ShouldBe(validPrice);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(500)]
    public void AcceptValidCapacity(int validCapacity)
    {
        var schoolEvent = new SchoolEvent(
            "Valid Event",
            "Event with valid capacity",
            new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero),
            new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0)),
            price: 0m,
            registerLink: "https://hogent.be/registreer",
            capacity: validCapacity,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: "/images/event.png",
            _location
        );

        schoolEvent.Capacity.ShouldBe(validCapacity);
    }

    [Fact]
    public void AllowNullDescription()
    {
        var schoolEvent = new SchoolEvent(
            "Valid Event",
            description: null,
            new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero),
            new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0)),
            price: 0m,
            registerLink: "https://hogent.be/registreer",
            capacity: 50,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: "/images/event.png",
            _location
        );

        schoolEvent.Description.ShouldBeNull();
    }

    [Fact]
    public void AllowNullImageUrl()
    {
        var schoolEvent = new SchoolEvent(
            "Valid Event",
            "Event without image",
            new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero),
            new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0)),
            price: 0m,
            registerLink: "https://hogent.be/registreer",
            capacity: 50,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: null,
            _location
        );

        schoolEvent.ImageUrl.ShouldBeNull();
    }
}