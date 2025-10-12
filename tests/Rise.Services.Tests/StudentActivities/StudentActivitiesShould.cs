using System.Security.Claims;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.StudentActivities;
using Rise.Persistence;
using Rise.Services.StudentActivities;
using Rise.Shared.Common;
using Rise.Shared.Locations;
using Rise.Shared.StudentActivities;
using Rise.Shared.StudentClubs;

namespace Rise.Services.Tests.StudentActivities;

public class StudentActivitiesShould
{
    [Fact]
    public async Task GetAllStudentActivities_ReturnsAllActivities()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(
                databaseName: nameof(
                    GetAllStudentActivities_ReturnsAllActivities)) // Do NOT use InMemoryDatabase... it's not reliable. Use a real database and come up with a strategy to clean up the database between tests.
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var studentClub = StudentActivityTestDataFactory.CreateDefaultStudentClub();
        var location = StudentActivityTestDataFactory.CreateDefaultLocation();

        var date1 = new DateTime(2025, 9, 10);
        var date2 = new DateTime(2025, 10, 10);
        var studentActivity1 = StudentActivityTestDataFactory.CreateStudentActivity("Cantus", "Desc1",
            date1, location, studentClub);

        var studentActivity2 = StudentActivityTestDataFactory.CreateStudentActivity("Quiz", "Desc2", date2,
            location, studentClub);


        dbContext.Locations.Add(location);
        dbContext.StudentClubs.Add(studentClub);
        dbContext.StudentActivities.AddRange(studentActivity1, studentActivity2);
        await dbContext.SaveChangesAsync();

        IStudentActivityService service = new StudentActivityService(dbContext, null);

        // Act
        var result = await service.GetIndexAsync(new QueryRequest.SkipTake { }, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.StudentActivities.Count().ShouldBe(2);
        result.Value.TotalCount.ShouldBe(2);
        result.Value.StudentActivities.Any(a => a.Title == "Cantus").ShouldBeTrue();
        result.Value.StudentActivities.Any(a => a.Title == "Quiz").ShouldBeTrue();


        var activityCantus = result.Value.StudentActivities.FirstOrDefault(a => a.Title == "Cantus");
        activityCantus.ShouldNotBeNull();
        activityCantus.Description.ShouldBe("Desc1");
        activityCantus.Date.ShouldBe(new DateTime(2025, 9, 10));
        activityCantus.StartTime.ShouldBe(new DateTime(2025, 9, 10, 18, 0, 0));
        activityCantus.EndTime.ShouldBe(new DateTime(2025, 9, 10, 22, 0, 0));
        activityCantus.ImageUrl.ShouldBe("/images/cantus.png");
        activityCantus.Location.Name.ShouldBe("Test Location");
        activityCantus.StudentClub.Name.ShouldBe("Club A");

        var activityQuiz = result.Value.StudentActivities.FirstOrDefault(a => a.Title == "Quiz");
        activityQuiz.ShouldNotBeNull();
        activityQuiz.Description.ShouldBe("Desc2");
        activityQuiz.Date.ShouldBe(new DateTime(2025, 10, 10));
        activityQuiz.StartTime.ShouldBe(new DateTime(2025, 10, 10, 18, 0, 0));
        activityQuiz.EndTime.ShouldBe(new DateTime(2025, 10, 10, 22, 0, 0));
        activityQuiz.ImageUrl.ShouldBe("/images/quiz.png");
        activityQuiz.Location.Name.ShouldBe("Test Location");
        activityQuiz.StudentClub.Name.ShouldBe("Club A");
        activityQuiz.StudentClub.GetType().GetProperty("Description").ShouldBeNull();
        activityQuiz.StudentClub.GetType().GetProperty("LogoUrl").ShouldBeNull();
    }

    [Fact]
    public async Task GetAllStudentActivities_CorrectPagination()
    {
        const int AMOUNT_OF_ACTIVITIES = 5;
        const int SKIP = 1;
        const int TAKE = 2;

        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetAllStudentActivities_CorrectPagination))
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var studentClub = StudentActivityTestDataFactory.CreateDefaultStudentClub();
        var location = StudentActivityTestDataFactory.CreateDefaultLocation();

        var activities = StudentActivityTestDataFactory.CreateTestActivities(AMOUNT_OF_ACTIVITIES, location, studentClub);

        dbContext.Locations.Add(location);
        dbContext.StudentClubs.Add(studentClub);
        dbContext.StudentActivities.AddRange(activities);
        await dbContext.SaveChangesAsync();

        var service = new StudentActivityService(dbContext, null);

        //Act
        var result = await service.GetIndexAsync(
            new QueryRequest.SkipTake { Skip = SKIP, Take = TAKE },
            CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.TotalCount.ShouldBe(AMOUNT_OF_ACTIVITIES);
        result.Value.StudentActivities.Count().ShouldBe(TAKE);
        result.Value.StudentActivities.Any(a => a.Title == "ActivityTest2").ShouldBeTrue();
        result.Value.StudentActivities.Any(a => a.Title == "ActivityTest3").ShouldBeTrue();
        result.Value.StudentActivities.Any(a => a.Title == "ActivityTest1").ShouldBeFalse();
    }


    [Fact]
    public async Task GetStudentActivitiesById_Returns()
    {
        const int AMOUNT_OF_ACTIVITIES = 5;
        const int TO_TEST_ACTIVITY_ID = 2;

        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetStudentActivitiesById_Returns))
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var studentClub = StudentActivityTestDataFactory.CreateDefaultStudentClub();
        var location = StudentActivityTestDataFactory.CreateDefaultLocation();

        var activities = StudentActivityTestDataFactory.CreateTestActivities(AMOUNT_OF_ACTIVITIES, location, studentClub);

        dbContext.Locations.Add(location);
        dbContext.StudentClubs.Add(studentClub);
        dbContext.StudentActivities.AddRange(activities);
        await dbContext.SaveChangesAsync();

        var service = new StudentActivityService(dbContext, null);

        //Act
        var result = await service.GetDetailByIdAsync(TO_TEST_ACTIVITY_ID, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.StudentActivity.ShouldNotBeOfType(typeof(Array));
        result.Value.StudentActivity.Id.ShouldBe(TO_TEST_ACTIVITY_ID);
        result.Value.StudentActivity.Title.ShouldBe($"ActivityTest{TO_TEST_ACTIVITY_ID}");
        result.Value.StudentActivity.Description.ShouldBe($"DescTest{TO_TEST_ACTIVITY_ID}");
        result.Value.StudentActivity.ImageUrl.ShouldBe($"/images/activitytest{TO_TEST_ACTIVITY_ID}.png");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(999)]
    [InlineData(6)]
    public async Task GetStudentActivitiesById_Invalid(int invalidId)
    {
        const int AMOUNT_OF_ACTIVITIES = 5;
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"GetStudentActivitiesById_Invalid + {invalidId}")
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var studentClub = StudentActivityTestDataFactory.CreateDefaultStudentClub();
        var location = StudentActivityTestDataFactory.CreateDefaultLocation();

        var activities = StudentActivityTestDataFactory.CreateTestActivities(AMOUNT_OF_ACTIVITIES, location, studentClub);

        dbContext.Locations.Add(location);
        dbContext.StudentClubs.Add(studentClub);
        dbContext.StudentActivities.AddRange(activities);
        await dbContext.SaveChangesAsync();

        var service = new StudentActivityService(dbContext, null);


        // Act
        var result = await service.GetDetailByIdAsync(invalidId, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Invalid);
    }
    
    
}