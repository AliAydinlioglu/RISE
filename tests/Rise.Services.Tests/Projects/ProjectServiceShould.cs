using System.Security.Claims;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Projects;
using Rise.Persistence;
using Rise.Services.Projects;
using Rise.Services.Tests.Fakers;
using Rise.Shared.Projects;

namespace Rise.Services.Tests.Projects;

public class ProjectServiceShould
{
    
    [Fact]
    public async Task EditProjectWhenTechnicianIsTheSame()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(EditProjectWhenTechnicianIsTheSame)) // Do NOT use InMemoryDatabase... it's not reliable. Use a real database and come up with a strategy to clean up the database between tests.
            .Options;
        
        using var dbContext = new ApplicationDbContext(options);

        var technicianAccountId = Guid.NewGuid().ToString();
        var technician = new Technician("FName", "LName", technicianAccountId);
        var project = new Project("Old Project", technician, new Address("Street", "Bus", "City", "Zip"));

        dbContext.Technicians.Add(technician);
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, technicianAccountId)
        ], "mock"));

        var sessionProvider = new FakeSessionContextProvider(claimsPrincipal);

        var service = new ProjectService(dbContext, sessionProvider);

        var request = new ProjectRequest.Edit
        {
            ProjectId = project.Id,
            Name = "New Project"
        };

        // Act
        var result = await service.EditAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var updatedProject = await dbContext.Projects.FirstAsync();
        updatedProject.Name.ShouldBe("New Project");
    }
    
    [Fact]
    public async Task NotEditProjectWhenTechnicianIsDifferent()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(NotEditProjectWhenTechnicianIsDifferent)) // Do NOT use InMemoryDatabase... it's not reliable. Use a real database and come up with a strategy to clean up the database between tests.
            .Options;
        
        using var dbContext = new ApplicationDbContext(options);

        var correctTechAccountId = Guid.NewGuid().ToString();
        var otherTechAccountId = Guid.NewGuid().ToString();

        var projectTechnician = new Technician("FName", "LName", correctTechAccountId);
        var loggedInTechnician = new Technician("Other", "Tech", otherTechAccountId);

        var project = new Project("Old Project", projectTechnician, new Address("Street", "Bus", "City", "Zip"));

        dbContext.Technicians.AddRange(projectTechnician, loggedInTechnician);
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, otherTechAccountId)
        ], "mock"));

        var sessionProvider = new FakeSessionContextProvider(claimsPrincipal);
        var service = new ProjectService(dbContext, sessionProvider);

        var request = new ProjectRequest.Edit
        {
            ProjectId = project.Id,
            Name = "New Project"
        };

        // Act
        var result = await service.EditAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
        var unchangedProject = await dbContext.Projects.FirstAsync();
        unchangedProject.Name.ShouldBe("Old Project");
    }
}