using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Common;
using Rise.Domain.Products;
using Rise.Domain.Projects;

namespace Rise.Persistence;
/// <summary>
/// Seeds the database
/// </summary>
/// <param name="dbContext"></param>
/// <param name="roleManager"></param>
/// <param name="userManager"></param>
public class DbSeeder(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    const string PasswordDefault = "A1b2C3!";
    public async Task SeedAsync()
    {
        await RolesAsync();
        await UsersAsync();
        await ProductsAsync();
        await ProjectsAsync();
        await StudentActivitiesAsync();
        await CalendarSeeder.Seed(dbContext);
    }

    private async Task RolesAsync()
    {
        if (dbContext.Roles.Any())
            return;

        await roleManager.CreateAsync(new IdentityRole("Administrator"));
        await roleManager.CreateAsync(new IdentityRole("Secretary"));
        await roleManager.CreateAsync(new IdentityRole("Technician"));
    }
    
    private async Task  UsersAsync()
    {
        if (dbContext.Users.Any())
            return;
        
        await dbContext.Roles.ToListAsync();

        var admin = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(admin, PasswordDefault);
        
        var secretary = new IdentityUser
        {
            UserName = "secretary@example.com",
            Email = "secretary@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(secretary, PasswordDefault);
        
        var technicianAccount1 = new IdentityUser
        {
            UserName = "technician1@example.com",
            Email = "technician1@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(technicianAccount1, PasswordDefault);
        
        var technicianAccount2 = new IdentityUser
        {
            UserName = "technician2@example.com",
            Email = "technician2@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(technicianAccount2, PasswordDefault);
                
        var user = new IdentityUser
        {
            UserName = "user@example.com",
            Email = "user@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(user, PasswordDefault);
        
        await userManager.AddToRoleAsync(admin, "Administrator");
        await userManager.AddToRoleAsync(secretary, "Secretary");
        await userManager.AddToRoleAsync(technicianAccount1, "Technician");
        await userManager.AddToRoleAsync(technicianAccount2, "Technician");

        dbContext.Technicians.AddRange(
            new Technician("Tech 1", "Awesome", technicianAccount1.Id),
            new Technician("Tech 2", "Less Awesome", technicianAccount2.Id));
        
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
        
        var addresses = new List<Address>
        {
            new Address("Koningstraat 12", "Bus 3A", "Brussel", "1000"),
            new Address("Meir 45", "", "Antwerpen", "2000"),
            new Address("Veldstraat 78", "2e verdieping", "Gent", "9000"),
            new Address("Rue de la Loi 175", "", "Bruxelles", "1040"),
            new Address("Place Saint-Lambert 8", "Bureau 12", "Liège", "4000"),
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
            new StudentActivity("Activity 2", "Description for Activity 2", new DateTime(2023, 12, 5), new TimeRange (new TimeOnly (14, 0, 0 ), new TimeOnly( 16, 0, 0)), "images/activity2.png", locations[1], studentClubs[1]),
            new StudentActivity("Activity 3", "Description for Activity 3", new DateTime(2024, 1, 20), new TimeRange (new TimeOnly ( 9, 0, 0 ), new TimeOnly( 11, 0, 0)), "images/activity3.png", locations[2], studentClubs[2]),
            new StudentActivity("Activity 4", "Description for Activity 4", new DateTime(2024, 2, 10), new TimeRange (new TimeOnly ( 13, 0, 0 ), new TimeOnly( 15, 0, 0)), "images/activity4.png", locations[3], studentClubs[3]),
            new StudentActivity("Activity 5", "Description for Activity 5", new DateTime(2024, 3, 25), new TimeRange (new TimeOnly ( 11, 0, 0 ), new TimeOnly( 13, 0, 0)), "images/activity5.png", locations[4], studentClubs[4])
        });
        await dbContext.SaveChangesAsync();
    }
}