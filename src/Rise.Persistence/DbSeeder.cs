using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Common;
using Rise.Domain.Locations;
using Rise.Domain.Products;
using Rise.Domain.Projects;
using Rise.Domain.SchoolEvents;
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
        await LocationsAsync();
        await StudentActivitiesAsync();
        await SchoolEventsAsync();
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

    private async Task LocationsAsync()
    {
        if (dbContext.Locations.Any())
            return;
        
        dbContext.Locations.AddRange(new List<Location>
        {
            new Location("Schoonmeersen", "Valentin Vaerwyckweg", 1, 9000, "Gent","B"),
            new Location("Campus Gent - Sint-Pietersplein", "Sint-Pietersplein", 7, 9000, "Gent",""),
            new Location("Campus Gent - Ledeganck", "Karel Lodewijk Ledeganckstraat", 35, 9000, "Gent",""),
            new Location("Stadshal Gent", "Emile Braunplein", 1, 9000, "Gent",""),
            new Location("NTGent - Voorplein", "Sint-Baafsplein", 17, 9000, "Gent","")
        });
        await dbContext.SaveChangesAsync();
    }

    private async Task StudentActivitiesAsync()
    {
        if (dbContext.StudentActivities.Any())
            return;


        var locations = await dbContext.Locations.ToListAsync();

        if (!locations.Any())
        {
            throw new InvalidOperationException("Locations moeten eerst geseed worden!");
        }

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
            new StudentActivity("Activity 1", "Description for Activity 1", new DateTime(2023, 11, 15), new TimeRange (new TimeOnly( 10, 0  ), new TimeOnly(12, 0)), "/img/banner30.webp", locations[0], studentClubs[0]),
            new StudentActivity("Activity 2", "Description for Activity 2", new DateTime(2023, 12, 5), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)), "/img/banner31.webp", locations[1], studentClubs[1]),
            new StudentActivity("Activity 3", "Description for Activity 3", new DateTime(2024, 1, 20), new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)), "/img/banner32.webp", locations[2], studentClubs[2]),
            new StudentActivity("Activity 4", "Description for Activity 4", new DateTime(2024, 2, 10), new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)), "/img/banner33.webp", locations[3], studentClubs[3]),
            new StudentActivity("Activity 5", "Description for Activity 5", new DateTime(2024, 3, 25), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)), "/img/banner34.webp", locations[4], studentClubs[4]),
            new StudentActivity("Activity 6", "Description for Activity 6", new DateTime(2024, 4, 5), new TimeRange(new TimeOnly(8, 30), new TimeOnly(10, 30)), "/img/banner36.webp", locations[0], studentClubs[1]),
            new StudentActivity("Activity 7", "Description for Activity 7", new DateTime(2024, 5, 12), new TimeRange(new TimeOnly(15, 0), new TimeOnly(17, 0)), "/img/banner37.webp", locations[1], studentClubs[2]),
            new StudentActivity("Activity 8", "Description for Activity 8", new DateTime(2024, 6, 18), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), "/img/banner38.webp", locations[2], studentClubs[3]),
            new StudentActivity("Activity 9", "Description for Activity 9", new DateTime(2024, 7, 22), new TimeRange(new TimeOnly(18, 0), new TimeOnly(20, 0)), "/img/banner39.webp", locations[3], studentClubs[4]),
            new StudentActivity("Activity 10", "Description for Activity 10", new DateTime(2024, 8, 30), new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)), "/img/banner40.webp", locations[4], studentClubs[0]),
            new StudentActivity("Activity 11", "Description for Activity 11", new DateTime(2024, 9, 14), new TimeRange(new TimeOnly(12, 0), new TimeOnly(14, 0)), "/img/banner41.webp", locations[0], studentClubs[1]),
            new StudentActivity("Activity 12", "Description for Activity 12", new DateTime(2024, 10, 3), new TimeRange(new TimeOnly(14, 30), new TimeOnly(16, 30)), "/img/banner42.webp", locations[1], studentClubs[2]),
            new StudentActivity("Activity 13", "Description for Activity 13", new DateTime(2024, 11, 11), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), "/img/banner43.webp", locations[2], studentClubs[3]),
            new StudentActivity("Activity 14", "Description for Activity 14", new DateTime(2024, 12, 1), new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)), "/img/banner44.webp", locations[3], studentClubs[4]),
            new StudentActivity("Activity 15", "Description for Activity 15", new DateTime(2025, 1, 19), new TimeRange(new TimeOnly(9, 30), new TimeOnly(11, 30)), "/img/banner46.webp", locations[4], studentClubs[0]),
            new StudentActivity("Activity 16", "Description for Activity 16", new DateTime(2025, 2, 8), new TimeRange(new TimeOnly(16, 0), new TimeOnly(18, 0)), "/img/banner47.webp", locations[0], studentClubs[1]),
            new StudentActivity("Activity 17", "Description for Activity 17", new DateTime(2025, 3, 16), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)), "/img/banner48.webp", locations[1], studentClubs[2]),
            new StudentActivity("Activity 18", "Description for Activity 18", new DateTime(2025, 4, 27), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)), "/img/banner49.webp", locations[2], studentClubs[3]),
            new StudentActivity("Activity 19", "Description for Activity 19", new DateTime(2025, 5, 9), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), "/img/banner50.webp", locations[3], studentClubs[4]),
            new StudentActivity("Activity 20", "Description for Activity 20", new DateTime(2025, 6, 21), new TimeRange(new TimeOnly(17, 0), new TimeOnly(19, 0)), "/img/banner51.webp", locations[4], studentClubs[0])
        });
        await dbContext.SaveChangesAsync();
    }

    private async Task SchoolEventsAsync()
    {
        if (dbContext.SchoolEvents.Any())
            return;


        var locations = await dbContext.Locations.ToListAsync();

        if (!locations.Any())
        {
            throw new InvalidOperationException("Locations moeten eerst geseed worden!");
        }
        
        var dateTimeNow = DateTime.Now;
        dbContext.SchoolEvents.AddRange(new List<SchoolEvent>
        {
            new SchoolEvent("SchoolEvent 1", "Description for SchoolEvent 1", dateTimeNow.AddDays(-2), new TimeRange (new TimeOnly( 10, 0  ), new TimeOnly(12, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Sport", "/img/banner10.webp", locations[0]),
            new SchoolEvent("SchoolEvent 2", "Description for SchoolEvent 2", dateTimeNow.AddDays(-2), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)),0.5m,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Science",  "/img/banner11.webp", locations[1]),
            new SchoolEvent("SchoolEvent 3", "Description for SchoolEvent 3", dateTimeNow.AddDays(-2), new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)),1,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Wellbeing",  "/img/banner12.webp", locations[2]),
            new SchoolEvent("SchoolEvent 4", "Description for SchoolEvent 4", dateTimeNow.AddDays(-2), new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)),2,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Culture",  "/img/banner13.webp", locations[3]),
            new SchoolEvent("SchoolEvent 5", "Description for SchoolEvent 5", dateTimeNow.AddDays(-1), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)),3.5m,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner14.webp", locations[4]),
            new SchoolEvent("SchoolEvent 6", "Description for SchoolEvent 6", dateTimeNow.AddDays(-1), new TimeRange(new TimeOnly(8, 30), new TimeOnly(10, 30)),5,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Wellbeing",  "/img/banner16.webp", locations[0]),
            new SchoolEvent("SchoolEvent 7", "Description for SchoolEvent 7", dateTimeNow, new TimeRange(new TimeOnly(15, 0), new TimeOnly(17, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner17.webp", locations[1]),
            new SchoolEvent("SchoolEvent 8", "Description for SchoolEvent 8", dateTimeNow, new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Culture",  "/img/banner18.webp", locations[2]),
            new SchoolEvent("SchoolEvent 9", "Description for SchoolEvent 9", dateTimeNow, new TimeRange(new TimeOnly(18, 0), new TimeOnly(20, 0)),10,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner19.webp", locations[3]),
            new SchoolEvent("SchoolEvent 10", "Description for SchoolEvent 10", dateTimeNow, new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Culture",  "/img/banner20.webp", locations[4]),
            new SchoolEvent("SchoolEvent 11", "Description for SchoolEvent 11", dateTimeNow, new TimeRange(new TimeOnly(12, 0), new TimeOnly(14, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Nature",  "/img/banner21.webp", locations[0]),
            new SchoolEvent("SchoolEvent 12", "Description for SchoolEvent 12", dateTimeNow, new TimeRange(new TimeOnly(14, 30), new TimeOnly(16, 30)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Wellbeing",  "/img/banner22.webp", locations[1]),
            new SchoolEvent("SchoolEvent 13", "Description for SchoolEvent 13", dateTimeNow, new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner23.webp", locations[2]),
            new SchoolEvent("SchoolEvent 14", "Description for SchoolEvent 14", dateTimeNow, new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Nature",  "/img/banner24.webp", locations[3]),
            new SchoolEvent("SchoolEvent 15", "Description for SchoolEvent 15", dateTimeNow, new TimeRange(new TimeOnly(9, 30), new TimeOnly(11, 30)),40,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Nature",  "/img/banner26.webp", locations[4]),
            new SchoolEvent("SchoolEvent 16", "Description for SchoolEvent 16", dateTimeNow, new TimeRange(new TimeOnly(16, 0), new TimeOnly(18, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Wellbeing",  "/img/banner27.webp", locations[0]),
            new SchoolEvent("SchoolEvent 17", "Description for SchoolEvent 17", dateTimeNow.AddDays(1), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner28.webp", locations[1]),
            new SchoolEvent("SchoolEvent 18", "Description for SchoolEvent 18", dateTimeNow.AddDays(1), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Wellbeing",  "/img/banner29.webp", locations[2]),
            new SchoolEvent("SchoolEvent 19", "Description for SchoolEvent 19", dateTimeNow.AddDays(1), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Culture",  "/img/banner30.webp", locations[3]),
            new SchoolEvent("SchoolEvent 20", "Description for SchoolEvent 20", dateTimeNow.AddDays(1), new TimeRange(new TimeOnly(17, 0), new TimeOnly(19, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Science",  "/img/banner31.webp", locations[4])
        });
        await dbContext.SaveChangesAsync();
    }
}