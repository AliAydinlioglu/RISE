using System.Security.Claims;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;

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
        
        using var dbContext = new ApplicationDbContext()(options);
        
        var studentClub = new StudentClub("KSA", "KSA description", "/images/ksa.png");
        var activity1 = new StudentActivities("Cantus", "Desc1", new DateTime(2025,9,10), new DateTime(2025,9,10,17,0,0), new DateTime(2025,9,10,23,0,0), new Location("Street", "152", "City", 9100), studentClub);
        var activity2 = new StudentActivities("Quiz", "Desc2", new DateTime(2025,10,10), new DateTime(2025,10,10,18,0,0), new DateTime(2025,10,10,22,0,0), new Location("Street", "153", "City", 9100), studentClub);


        dbContext.StudentClub.Add(studentClub);
        dbContext.StudentActivities.AddRange(activity1, activity2);
        await dbContext.SaveChangesAsync();

        var service = new StudentActivitiesService(dbContext, null);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Count().ShouldBe(2);
        result.Value.Any(a => a.Name == "Cantus").ShouldBeTrue();
        result.Value.Any(a => a.Name == "Quiz").ShouldBeTrue();
    }
}