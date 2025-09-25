using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Identity;
using Rise.Shared.Projects;

namespace Rise.Services.Projects;

/// <summary>
/// Service for projects. Note the use of <see cref="ISessionContextProvider"/> to get the current user in this layer of the application.
/// </summary>
/// <param name="dbContext"></param>
/// <param name="sessionContextProvider"></param>
public class ProjectService(ApplicationDbContext dbContext, ISessionContextProvider sessionContextProvider) : IProjectService
{
    public async Task<Result> EditAsync(ProjectRequest.Edit req, CancellationToken ctx = default)
    {
        var project = await dbContext.Projects
            .Include(x => x.Technician)
            .SingleOrDefaultAsync(x => x.Id == req.ProjectId, ctx);

        if (project is null)
            return Result.NotFound($"Project with Id '{req.ProjectId}' was not found.");

        // Currently logged-in user must be the same as the project's technician.
        var loggedInTechnician = await dbContext.Technicians
            .SingleOrDefaultAsync(x => x.AccountId == sessionContextProvider.User!.GetUserId(), ctx);
        
        if(loggedInTechnician is null || !project.CanBeEditedBy(loggedInTechnician))
            return Result.Unauthorized("You are not authorized to edit this project.");

        project.Edit(req.Name);

        await dbContext.SaveChangesAsync(ctx);

        return Result.Success();
    }
}