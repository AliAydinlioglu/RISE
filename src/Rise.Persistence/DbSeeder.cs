using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Common;
using Rise.Domain.Products;
using Rise.Domain.Projects;
using Rise.Domain.StudentActivities;
using Rise.Persistence.Models.Identity;
using Rise.Persistence.SeedData;
using Rise.Shared.Identity;

namespace Rise.Persistence;
/// <summary>
/// Seeds the database
/// </summary>
/// <param name="dbContext"></param>
/// <param name="roleManager"></param>
/// <param name="userManager"></param>
public class DbSeeder(ApplicationDbContext dbContext, RoleManager<ApplicationRole> roleManager, 
    UserManager<ApplicationUser> userManager)
{
    public async Task SeedAsync()
    {
        await RolesAsync();
        await UsersAsync();
        await ProductsAsync();
        await ProjectsAsync();
        await StudentActivitiesAsync();
        await CalendarSeeder.Seed(dbContext);
        await NavigationItemSeeder.Seed(dbContext);
        await ContactSeeder.Seed(dbContext);
    }

    private async Task RolesAsync()
    {
        if (dbContext.DomainRoles.Any())
            return;

        await RoleSeeder.Seed(roleManager);
    }
    
    private async Task  UsersAsync()
    {
        if (dbContext.Users.Any())
            return;
        
        await dbContext.DomainRoles.ToListAsync();

        var regularStudent = new ApplicationUser(
            "regular@example.com", 
            "Regular", 
            "Student", 
            "TIN/TIAO-2", 
            DateTimeOffset.UtcNow, 
            Guid.NewGuid(), 
            "Microsoft Entra");
        await userManager.CreateAsync(regularStudent);
        
        var distanceStudent = new ApplicationUser(
            "distance@example.com", 
            "Distance", 
            "Student", 
            "TIN/TIAO-3", 
            DateTimeOffset.UtcNow, 
            Guid.NewGuid(), 
            "Microsoft Entra");
        await userManager.CreateAsync(distanceStudent);
        
        await userManager.AddToRoleAsync(regularStudent, nameof(AppRoles.RegularStudent));
        await userManager.AddToRoleAsync(distanceStudent,  nameof(AppRoles.DistanceStudent));
        
        await dbContext.SaveChangesAsync();
    }
    
    private async Task  ProductsAsync()
    {
        if (dbContext.Products.Any())
            return;
        
        dbContext.Products.AddRange(
            new Product{ Name = "Laptop", Description = "15-inch display, 16GB RAM" },
            new Product{ Name = "Smartphone", Description = "6.5-inch screen, 128GB storage" },
            new Product{ Name = "Headphones", Description = "Wireless noise-cancelling" },
            new Product{ Name = "Keyboard", Description = "Mechanical RGB backlit" },
            new Product{ Name = "Mouse", Description = "Ergonomic wireless mouse" },
            new Product{ Name = "Monitor", Description = "27-inch 4K UHD display" },
            new Product{ Name = "Printer", Description = "All-in-one inkjet printer" },
            new Product{ Name = "Camera", Description = "Mirrorless 24MP with 4K video" },
            new Product{ Name = "Smartwatch", Description = "Heart rate monitor, GPS" },
            new Product{ Name = "Speaker", Description = "Bluetooth portable speaker" }
        );

        await dbContext.SaveChangesAsync();
    }
    
    private async Task  ProjectsAsync()
    {
        if (dbContext.Projects.Any())
            return;
        
        var technicians = await dbContext.Technicians.ToListAsync();
        
        if (!technicians.Any())
            return;
        
        var addresses = new List<Domain.Projects.Address>
        {
            new Domain.Projects.Address("Koningstraat 12", "Bus 3A", "Brussel", "1000"),
            new Domain.Projects.Address("Meir 45", "", "Antwerpen", "2000"),
            new Domain.Projects.Address("Veldstraat 78", "2e verdieping", "Gent", "9000"),
            new Domain.Projects.Address("Rue de la Loi 175", "", "Bruxelles", "1040"),
            new Domain.Projects.Address("Place Saint-Lambert 8", "Bureau 12", "Liège", "4000"),
        };

        var rnd = new Random(123); // Using a seed so the random is always the same.
        
        var projects = new List<Project>
        {
            new("Website Redesign", technicians[rnd.Next(technicians.Count)], addresses[0]),
            new("Mobile App Development", technicians[rnd.Next(technicians.Count)], addresses[1]),
            new("Database Migration", technicians[rnd.Next(technicians.Count)], addresses[2]),
            new("E-commerce Platform", technicians[rnd.Next(technicians.Count)], addresses[3]),
            new("CRM Integration", technicians[rnd.Next(technicians.Count)], addresses[4])
        };
        
        
        
        dbContext.Projects.AddRange(projects);
        await dbContext.SaveChangesAsync();
    }
    
    private async Task StudentActivitiesAsync()
    {
        if (dbContext.StudentActivities.Any())
            return;
        
       
        var locations = new List<Location>
        {
            new Location("Schoonmeersen", "Valentin Vaerwyckweg", 1, 9000, "Gent","B"),
            new Location("Campus Gent - Sint-Pietersplein", "Sint-Pietersplein", 7, 9000, "Gent",""),
            new Location("Campus Gent - Ledeganck", "Karel Lodewijk Ledeganckstraat", 35, 9000, "Gent",""),
            new Location("Stadshal Gent", "Emile Braunplein", 1, 9000, "Gent",""),
            new Location("NTGent - Voorplein", "Sint-Baafsplein", 17, 9000, "Gent","")
        };

        var studentClubs = new List<StudentClub>
        {
            new StudentClub("Club A", "Club A Description", "images/clubA.png"),
            new StudentClub("Club B", "Club B Description", "images/clubB.png"),
            new StudentClub("Club C", "Club C Description", "images/clubC.png"),
            new StudentClub("Club D", "Club D Description", "images/clubD.png"),
            new StudentClub("Club E", "Club E Description", "images/clubE.png")
        };
        

        dbContext.StudentActivities.AddRange( new List<StudentActivity>
        {
            new StudentActivity("Activity 1", "Description for Activity 1", new DateTime(2023, 11, 15), new TimeRange (new TimeOnly( 10, 0  ), new TimeOnly(12, 0)), "images/activity1.png", locations[0], studentClubs[0]),
            new StudentActivity("Activity 2", "Description for Activity 2", new DateTime(2023, 12, 5), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)), "images/activity2.png", locations[1], studentClubs[1]),
            new StudentActivity("Activity 3", "Description for Activity 3", new DateTime(2024, 1, 20), new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)), "images/activity3.png", locations[2], studentClubs[2]),
            new StudentActivity("Activity 4", "Description for Activity 4", new DateTime(2024, 2, 10), new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)), "images/activity4.png", locations[3], studentClubs[3]),
            new StudentActivity("Activity 5", "Description for Activity 5", new DateTime(2024, 3, 25), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)), "images/activity5.png", locations[4], studentClubs[4]),
            new StudentActivity("Activity 6", "Description for Activity 6", new DateTime(2024, 4, 5), new TimeRange(new TimeOnly(8, 30), new TimeOnly(10, 30)), "images/activity6.png", locations[0], studentClubs[1]),
            new StudentActivity("Activity 7", "Description for Activity 7", new DateTime(2024, 5, 12), new TimeRange(new TimeOnly(15, 0), new TimeOnly(17, 0)), "images/activity7.png", locations[1], studentClubs[2]),
            new StudentActivity("Activity 8", "Description for Activity 8", new DateTime(2024, 6, 18), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), "images/activity8.png", locations[2], studentClubs[3]),
            new StudentActivity("Activity 9", "Description for Activity 9", new DateTime(2024, 7, 22), new TimeRange(new TimeOnly(18, 0), new TimeOnly(20, 0)), "images/activity9.png", locations[3], studentClubs[4]),
            new StudentActivity("Activity 10", "Description for Activity 10", new DateTime(2024, 8, 30), new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)), "images/activity10.png", locations[4], studentClubs[0]),
            new StudentActivity("Activity 11", "Description for Activity 11", new DateTime(2024, 9, 14), new TimeRange(new TimeOnly(12, 0), new TimeOnly(14, 0)), "images/activity11.png", locations[0], studentClubs[1]),
            new StudentActivity("Activity 12", "Description for Activity 12", new DateTime(2024, 10, 3), new TimeRange(new TimeOnly(14, 30), new TimeOnly(16, 30)), "images/activity12.png", locations[1], studentClubs[2]),
            new StudentActivity("Activity 13", "Description for Activity 13", new DateTime(2024, 11, 11), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), "images/activity13.png", locations[2], studentClubs[3]),
            new StudentActivity("Activity 14", "Description for Activity 14", new DateTime(2024, 12, 1), new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)), "images/activity14.png", locations[3], studentClubs[4]),
            new StudentActivity("Activity 15", "Description for Activity 15", new DateTime(2025, 1, 19), new TimeRange(new TimeOnly(9, 30), new TimeOnly(11, 30)), "images/activity15.png", locations[4], studentClubs[0]),
            new StudentActivity("Activity 16", "Description for Activity 16", new DateTime(2025, 2, 8), new TimeRange(new TimeOnly(16, 0), new TimeOnly(18, 0)), "images/activity16.png", locations[0], studentClubs[1]),
            new StudentActivity("Activity 17", "Description for Activity 17", new DateTime(2025, 3, 16), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)), "images/activity17.png", locations[1], studentClubs[2]),
            new StudentActivity("Activity 18", "Description for Activity 18", new DateTime(2025, 4, 27), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)), "images/activity18.png", locations[2], studentClubs[3]),
            new StudentActivity("Activity 19", "Description for Activity 19", new DateTime(2025, 5, 9), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), "images/activity19.png", locations[3], studentClubs[4]),
            new StudentActivity("Activity 20", "Description for Activity 20", new DateTime(2025, 6, 21), new TimeRange(new TimeOnly(17, 0), new TimeOnly(19, 0)), "images/activity20.png", locations[4], studentClubs[0])
        });
        await dbContext.SaveChangesAsync();
    }
}