using NSubstitute;
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
            new DateTime(2025, 9, 10),
            new DateTime(2025, 9, 10, 17, 0, 0),
            new DateTime(2025, 9, 10, 23, 0, 0),
            "/images/cantus.png",
            _location,
            _studentClub
        );

        sa.Title.ShouldBe("Cantus");
        sa.Description.ShouldBe("A singing event");
        sa.Date.ShouldBe(new DateTime(2025, 9, 10));
        sa.StartTime.ShouldBe(new DateTime(2025, 9, 10, 17, 0, 0));
        sa.EndTime.ShouldBe(new DateTime(2025, 9, 10, 23, 0, 0));
        sa.ImageUrl.ShouldBe("/images/cantus.png");
        sa.Location.ShouldNotBeNull();
        sa.StudentClub.ShouldNotBeNull();
    }

    [Theory]
    [InlineData("2025-09-10T20:00:00", "2025-09-10T18:00:00", "Start time must be before end time.")]
    [InlineData("2025-09-10T23:59:59", "2025-09-10T23:00:00", "Start time must be before end time.")]
    [InlineData("2025-09-11T09:00:00", "2025-09-10T18:00:00", "Start time must be before end time.")]
    [InlineData("2025-09-11T09:00:00", "2025-09-11T09:00:00", "Start time must be before end time.")]
    public void ThrowExceptionWhenStartTimeIsAfterOrEqualsEndTime(
        string startTimeStr, string endTimeStr, string expectedErrorMessage)
    {
        var startTime = DateTime.Parse(startTimeStr);
        var endTime = DateTime.Parse(endTimeStr);
        var date = startTime.Date;

        var exception = Should.Throw<InvalidTimeRangeException>(() => new StudentActivity(
            "Invalid Event",
            "An event with invalid time range",
            date,
            startTime,
            endTime,
            "/images/event.png",
            _location,
            _studentClub
        ));
        
        exception.GetType().ShouldBe(typeof(InvalidTimeRangeException));
        exception.Message.ShouldBe(expectedErrorMessage);
    }
    
    [Theory]
    [InlineData(null,"2025-09-10T00:00:00" , "2025-09-10T20:00:00", "2025-09-10T23:00:00",false ,false, typeof(ArgumentNullException))]
    [InlineData("","2025-09-10T00:00:00" , "2025-09-10T20:00:00", "2025-09-10T23:00:00",false ,false, typeof(ArgumentException))]
    [InlineData("   ","2025-09-10T00:00:00" , "2025-09-10T20:00:00", "2025-09-10T23:00:00",false ,false, typeof(ArgumentException))]
    [InlineData("Valid title",null , "2025-09-10T20:00:00", "2025-09-10T23:00:00",false ,false, typeof(ArgumentOutOfRangeException))]
    [InlineData("Valid title","2025-09-10T00:00:00" , null, "2025-09-10T23:00:00",false ,false, typeof(ArgumentOutOfRangeException))]
    [InlineData("Valid title","2025-09-10T00:00:00" , "2025-09-10T23:00:00", null,false ,false, typeof(ArgumentOutOfRangeException))]
    [InlineData("Valid title","2025-09-10T00:00:00" , "2025-09-10T23:00:00", "2025-09-10T23:59:00",true ,false, typeof(ArgumentNullException))]
    [InlineData("Valid title","2025-09-10T00:00:00" , "2025-09-10T23:00:00", "2025-09-10T23:59:00",false ,true, typeof(ArgumentNullException))]
    public void ThrowExceptionWhenRequiredFieldIsNullOrEmpty(
        string title, string? dateStr, string startTimeStr, string endTimeStr, bool isLocationNull, bool isStudentClubNull, Type expectedExceptionType)
    {
        var startTime = string.IsNullOrWhiteSpace(startTimeStr) ? DateTime.MinValue : DateTime.Parse(startTimeStr);
        var endTime = string.IsNullOrWhiteSpace(endTimeStr) ? DateTime.MinValue : DateTime.Parse(endTimeStr);
        var date = string.IsNullOrWhiteSpace(dateStr) ? DateTime.MinValue : DateTime.Parse(dateStr);


        var location = isLocationNull ? null! : _location;
        var studentClub = isStudentClubNull ? null! : _studentClub;
        
        var exception =  Should.Throw<Exception>(() => new StudentActivity(
            title,
            "An event with missing required fields",
            date,
            startTime,
            endTime,
            "/images/event.png",
            location,
            studentClub
        ));
       
        exception.GetType().ShouldBe(expectedExceptionType);
       
       
    }

}