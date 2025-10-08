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
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetAllStudentActivities_ReturnsAllActivities)) // Do NOT use InMemoryDatabase... it's not reliable. Use a real database and come up with a strategy to clean up the database between tests.
            .Options;
        
        using var dbContext = new ApplicationDbContext(options);

        var studentClub = new StudentClub(
            "Club A",
            "A student club description",
            "/images/clubA.png"
            );
        var location = new Location(
            "Test Location",
            "Test street",
            42,
            1234,
            "Test City");
        
        var studentActivity1 = new StudentActivity(
            "Cantus",
            "Desc1",
            new DateTime(2025, 9, 10),
            new DateTime(2025, 9, 10, 17, 0, 0),
            new DateTime(2025, 9, 10, 23, 0, 0),
            "/images/cantus.png",
            location,
            studentClub
        );
        
        var studentActivity2 = new StudentActivity(
            "Quiz",
            "Desc2",
            new DateTime(2025, 10, 10),
            new DateTime(2025, 10, 10, 18, 0, 0),
            new DateTime(2025, 10, 10, 22, 0, 0),
            "/images/quiz.png",
            location,
            studentClub
        );
       

        dbContext.Locations.Add(location);
        dbContext.StudentClubs.Add(studentClub);
        dbContext.StudentActivities.AddRange(studentActivity1, studentActivity2);
        await dbContext.SaveChangesAsync();

        var service = new StudentActivitiesService(dbContext, null);

        // Act
        var result = await service.GetIndexAsync(
            new QueryRequest.SkipTake {}, 
            CancellationToken.None);

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
        activityCantus.StartTime.ShouldBe(new DateTime(2025, 9, 10, 17 , 0, 0));
        activityCantus.EndTime.ShouldBe(new DateTime(2025, 9, 10, 23, 0, 0));
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
    public async Task GetStudentActivites_CorrectPagination()
    {
        
        const int AMOUNT_OF_ACTIVITIES = 5;
        const int SKIP = 1;
        const int TAKE = 2;
        
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetStudentActivites_CorrectPagination))
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var studentClub = new StudentClub("Club A", "A student club description", "/images/clubA.png");
        var location = new Location("Test Location", "Test street", 42, 1234, "Test City");

        var activities = new List<StudentActivity>();
        for (var i = 1; i <= AMOUNT_OF_ACTIVITIES; i++)
        {
            var date = new DateTime(2025, 9, 10 + i);
            activities.Add(new StudentActivity(
                $"ActivityTest{i}",
                $"DescTest{i}",
                date,
                new DateTime(date.Year, date.Month, date.Day, 18, 0, 0),
                new DateTime(date.Year, date.Month, date.Day, 22, 0, 0),
                $"/images/activity{i}.png",
                location,
                studentClub));
        }

        dbContext.Locations.Add(location);
        dbContext.StudentClubs.Add(studentClub);
        dbContext.StudentActivities.AddRange(activities);
        await dbContext.SaveChangesAsync();

        var service = new StudentActivitiesService(dbContext, null);
        
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
}