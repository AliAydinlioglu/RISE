using Microsoft.EntityFrameworkCore;
using Rise.Shared.Identity;
using Microsoft.AspNetCore.Identity;
using Rise.Persistence.Configurations.Identity;
using Rise.Persistence.Models.Identity;

namespace Rise.Server.Endpoints.Identity.Roles;

/// <summary>
/// List all roles.
/// See https://fast-endpoints.com/ 
/// </summary>
/// <param name="roleManager"></param>
public class Index(RoleManager<ApplicationRole> roleManager) : EndpointWithoutRequest<Result<List<KeyValuePair<Guid, string>>>>
{
    public override void Configure()
    {
        Get("/api/identity/roles");
        Roles(AppRoles.Administrator);
    }

    public override async Task<Result<List<KeyValuePair<Guid, string>>>> ExecuteAsync(CancellationToken ctx)
    {
        var roles = await roleManager.Roles.Select(r => new KeyValuePair<Guid, string>(r.Id, r.Name!)).ToListAsync(ctx);
        return Result.Success(roles);
    }
}