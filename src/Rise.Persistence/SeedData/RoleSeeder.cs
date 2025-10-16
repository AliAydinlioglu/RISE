using Microsoft.AspNetCore.Identity;
using Rise.Persistence.Configurations.Identity;

namespace Rise.Persistence.SeedData;

public static class RoleSeeder
{
    public readonly static List<ApplicationRole> IdentityRoles = [
        new ApplicationRole("c2f6e036-255f-4eb1-ad50-8fd763086531","Public"),
        new ApplicationRole("34a97d57-5b43-4774-81f2-06f38fd93983", "RegularStudent"),
        new ApplicationRole("9295f60f-af78-4a91-9ca7-96d1d3612107", "DistanceStudent"),
    ];

    public static async Task Seed(RoleManager<ApplicationRole> roleManager)
    {
        foreach (var role in IdentityRoles)
        {
           await roleManager.CreateAsync(role);
        }
    }
}
