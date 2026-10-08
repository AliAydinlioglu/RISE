using Rise.Domain.Identity;
using Rise.Domain.Navigation;
using Rise.Shared.Identity;

namespace Rise.Persistence.SeedData;

public static class NavigationItemSeeder
{
    public static readonly List<NavigationItem> Items = [
        new NavigationItem(1,"Kalender", "fa-calendar", "kalender"),
        new NavigationItem(2,"Activiteiten", "fa-activity", "student-activities"),
        new NavigationItem(3,"Home", "fa-home", "/")
    ];

    public static readonly List<ContentLocation> ContentLocations = [
        new ContentLocation(1, "HEADER"),
        new ContentLocation(2, "BODY"),
        new ContentLocation(3, "FOOTER")
    ];

    public static async Task Seed(ApplicationDbContext dbContext)
    {
        //This should be the Asp.Net identity role
        var publicRole = new Role(new Guid(AppRoles.Public), nameof(AppRoles.Public));
        var distancelearningstudentRole = new Role(new Guid(AppRoles.RegularStudent), nameof(AppRoles.RegularStudent));
        var regularstudentRole = new Role(new Guid(AppRoles.DistanceStudent), nameof(AppRoles.DistanceStudent));

        //Home item for public in header
        await dbContext.RoleNavigationItems.AddAsync(new RoleNavigationItemContentLocation(
            publicRole, Items[2], ContentLocations[0], 1));
        //Home item for public in footer
        await dbContext.RoleNavigationItems.AddAsync(new RoleNavigationItemContentLocation(
            publicRole, Items[2], ContentLocations[2], 2));

        //Kalendar item for regular studens in header
        await dbContext.RoleNavigationItems.AddAsync(new RoleNavigationItemContentLocation(
            regularstudentRole, Items[0], ContentLocations[0], 1));
        //Kalendar item for distance learning students in header
        await dbContext.RoleNavigationItems.AddAsync(new RoleNavigationItemContentLocation(
            distancelearningstudentRole, Items[0], ContentLocations[0], 1));

        //Activity item for regular students in footer
        await dbContext.RoleNavigationItems.AddAsync(new RoleNavigationItemContentLocation(
            regularstudentRole, Items[1], ContentLocations[2], 2));
        //Activity item for distance learning students in footer
        await dbContext.RoleNavigationItems.AddAsync(new RoleNavigationItemContentLocation(
            distancelearningstudentRole, Items[1], ContentLocations[2], 2));

        //Activity item for regular students in body
        await dbContext.RoleNavigationItems.AddAsync(new RoleNavigationItemContentLocation(
            regularstudentRole, Items[1], ContentLocations[1], 1));

        await dbContext.SaveChangesAsync();
    }
}
