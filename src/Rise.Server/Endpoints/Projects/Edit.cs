using Rise.Shared.Identity;
using Rise.Shared.Projects;

namespace Rise.Server.Endpoints.Projects;

/// <summary>
/// Edit a <see cref="Project"/>.
/// See https://fast-endpoints.com/
/// </summary>
/// <param name="projectService"></param>
public class Edit(IProjectService projectService) : Endpoint<ProjectRequest.Edit, Result>
{
    public override void Configure()
    {
        Put("/api/projects");
        Roles(AppRoles.Technician, AppRoles.Administrator);
    }

    public override Task<Result> ExecuteAsync(ProjectRequest.Edit req, CancellationToken ctx)
    {
        return projectService.EditAsync(req, ctx);
    }
}