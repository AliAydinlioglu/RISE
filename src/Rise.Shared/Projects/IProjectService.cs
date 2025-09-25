namespace Rise.Shared.Projects;

/// <summary>
/// Provides methods for managing project-related operations.
/// </summary>
public interface IProjectService
{
    Task<Result> EditAsync(ProjectRequest.Edit req, CancellationToken ctx);
}