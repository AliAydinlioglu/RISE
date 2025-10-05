using System.Security.Claims;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.StudentActivities;
using Rise.Persistence;
using Rise.Services.StudentActivities;
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

        var studentClub = new StudentClub();
        var location = new Location();
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
       

        dbContext.Location.Add(location);
        dbContext.StudentClub.Add(studentClub);
        dbContext.StudentActivities.AddRange(studentActivity1, studentActivity2);
        await dbContext.SaveChangesAsync();

        var service = new StudentActivitiesService(dbContext, null);

        // Act
        var result = await service.GetIndexAsync();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Count().ShouldBe(2);
        result.Value.Any(a => a.Title == "Cantus").ShouldBeTrue();
        result.Value.Any(a => a.Title == "Quiz").ShouldBeTrue();
    }
}