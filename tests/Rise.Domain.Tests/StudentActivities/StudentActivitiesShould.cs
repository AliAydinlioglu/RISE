using NSubstitute;
using Rise.Domain.Common;
using Rise.Domain.Exceptions;
using Rise.Domain.StudentActivities;

namespace Rise.Domain.Tests.StudentActivities;

public class StudentActivitiesShould
{
    Location _location = Substitute.For<Location>();
    StudentClub _studentClub = Substitute.For<StudentClub>();

    [Fact]
    public void BeCreated()
    {
        var sa = new StudentActivity(
            "Cantus",
            "A singing event",
            new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero),
            new TimeRange(new TimeOnly(17, 0, 0),
                new TimeOnly(23, 0, 0)),
            "/images/cantus.png",
            _location,
            _studentClub
        );

        sa.Title.ShouldBe("Cantus");
        sa.Description.ShouldBe("A singing event");
        sa.Date.ShouldBe(new DateTimeOffset(new DateTime(2025, 9, 10), TimeSpan.Zero));
        sa.TimeRange.ShouldBe(new TimeRange(new TimeOnly(17, 0, 0),
            new TimeOnly(23, 0, 0)));
        sa.ImageUrl.ShouldBe("/images/cantus.png");
        sa.Location.ShouldNotBeNull();
        sa.StudentClub.ShouldNotBeNull();
    }

    [Theory]
    [InlineData("20:00:00", "18:00:00", "EndTime must be after StartTime")]
    [InlineData("23:59:59", "23:00:00", "EndTime must be after StartTime")]
    [InlineData("23:59:59", "0:00:00", "EndTime must be after StartTime")]
    [InlineData("09:00:00", "09:00:00", "EndTime must be after StartTime")]
    public void ThrowExceptionWhenStartTimeIsAfterOrEqualsEndTime(
        string startTimeStr, string endTimeStr, string expectedErrorMessage)
    {
        var date = DateTimeOffset.Parse(startTimeStr);

        var exception = Should.Throw<InvalidTimeRangeException>(() => new StudentActivity(
            "Invalid Event",
            "An event with invalid time range",
            date,
            new TimeRange (TimeOnly.Parse(startTimeStr), TimeOnly.Parse(endTimeStr)),
            "/images/event.png",
            _location,
            _studentClub
        ));

        exception.GetType().ShouldBe(typeof(InvalidTimeRangeException));
        exception.Message.ShouldBe(expectedErrorMessage);
    }
    
   

    [Theory]
    [InlineData(null, "2025-09-10T00:00:00", false, false, false,
        typeof(ArgumentNullException))]
    [InlineData("", "2025-09-10T00:00:00", false, false, false,
        typeof(ArgumentException))]
    [InlineData("   ", "2025-09-10T00:00:00", false, false, false,
        typeof(ArgumentException))]
    [InlineData("Valid title", null, false, false, false,
        typeof(ArgumentOutOfRangeException))]
    [InlineData("Valid title", "2025-09-10T00:00:00", true, false, false,
        typeof(ArgumentNullException))]
    [InlineData("Valid title", "2025-09-10T00:00:00", false, true, false,
        typeof(ArgumentNullException))]
    [InlineData("Valid title", "2025-09-10T00:00:00", false, false, true,
        typeof(ArgumentNullException))]
    public void ThrowExceptionWhenRequiredFieldIsNullOrEmpty(
        string title, string? dateStr, bool isTimeRangeNull, bool isLocationNull,
        bool isStudentClubNull, Type expectedExceptionType)
    {
        var TEST_TIMERANGE = new TimeRange(new TimeOnly(20, 0, 0), new TimeOnly(23, 0, 0));
        var date = string.IsNullOrWhiteSpace(dateStr) ? DateTime.MinValue : DateTime.Parse(dateStr);


        var location = isLocationNull ? null! : _location;
        var studentClub = isStudentClubNull ? null! : _studentClub;

        var exception = Should.Throw<Exception>(() => new StudentActivity(
            title,
            "An event with missing required fields",
            date,
            isTimeRangeNull ? null : TEST_TIMERANGE,
            "/images/event.png",
            location,
            studentClub
        ));

        exception.GetType().ShouldBe(expectedExceptionType);
    }
}