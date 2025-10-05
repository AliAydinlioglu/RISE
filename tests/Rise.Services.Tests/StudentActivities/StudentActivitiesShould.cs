using System.Security.Claims;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
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

        var studentClubDto = new StudentClubDto.Summary
        {
            Id = 1,
            Name = "KSA",
        };
        var locationDto = new LocationDto.Index
        {
            Id = 1,
            Name="Schoonmeersen",
            Street = "Street",
            HouseNumber = "152",
            City = "City",
            Postcode = 9100
        };
        var activity1 = new StudentActivityDto.Index
        {
            Id = 1,
            Title = "Cantus",
            Description = "Desc1",
            Date = new DateTime(2025, 9, 10),
            StartTime = new DateTime(2025, 9, 10, 17, 0, 0),
            EndTime = new DateTime(2025, 9, 10, 23, 0, 0),
            Location = locationDto,
            StudentClub = studentClubDto
        };
        var activity2 = new StudentActivityDto.Index
        {
            Id = 2,
            Title = "Quiz",
            Description = "Desc2",
            Date = new DateTime(2025, 10, 10),
            StartTime = new DateTime(2025, 10, 10, 18, 0, 0),
            EndTime = new DateTime(2025, 10, 10, 22, 0, 0),
            Location = locationDto,
            StudentClub = studentClubDto
        };

        dbContext.Location.Add(locationDto);
        dbContext.StudentClub.Add(studentClubDto);
        dbContext.StudentActivities.AddRange(activity1, activity2);
        await dbContext.SaveChangesAsync();

        var service = new StudentActivitiesService(dbContext, null);

        // Act
        var result = await service.GetIndexAsync();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Count().ShouldBe(2);
        result.Value.Any(a => a.Name == "Cantus").ShouldBeTrue();
        result.Value.Any(a => a.Name == "Quiz").ShouldBeTrue();
    }
}