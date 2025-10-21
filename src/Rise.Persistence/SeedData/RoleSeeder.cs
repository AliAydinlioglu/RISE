using Microsoft.AspNetCore.Identity;
using Rise.Persistence.Configurations.Identity;
using Rise.Shared.Identity;

namespace Rise.Persistence.SeedData;

public static class RoleSeeder
{
    public readonly static List<ApplicationRole> IdentityRoles = [
        new ApplicationRole(AppRoles.Public, nameof(AppRoles.Public)),
        new ApplicationRole(AppRoles.RegularStudent, nameof(AppRoles.RegularStudent)),
        new ApplicationRole(AppRoles.DistanceStudent, nameof(AppRoles.DistanceStudent)),
    ];

    public static async Task Seed(RoleManager<ApplicationRole> roleManager)
    {
        foreach (var role in IdentityRoles)
        {
           await roleManager.CreateAsync(role);
        }
    }
}
