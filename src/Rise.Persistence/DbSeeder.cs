using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Common;
using Rise.Domain.Locations;
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
        await LocationsAsync();
        await StudentActivitiesAsync();
        await SchoolEventsAsync();
        await CalendarSeeder.Seed(dbContext);
        await NavigationItemSeeder.Seed(dbContext);
        await ContactSeeder.Seed(dbContext);
        await NewsSeeder.Seed(dbContext);
        await MenuSeeder.Seed(dbContext);
        await NotificationsSubscriptionsSeeder.Seed(dbContext);
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
            "regular@rise2526t2campusappoutlook.onmicrosoft.com", 
            "Regular", 
            "Student", 
            "TIN/TIAO-2", 
            DateTimeOffset.UtcNow, 
            Guid.Parse("0b1720b9-f0e1-43fa-88ea-4b4fecd6352b"), 
            SsoProviders.MicrosoftEntra);

        await userManager.CreateAsync(regularStudent);
        await userManager.AddToRoleAsync(regularStudent, nameof(AppRoles.RegularStudent));

        var distanceStudent = new ApplicationUser(
            "distance@rise2526t2campusappoutlook.onmicrosoft.com", 
            "Distance", 
            "Student", 
            "TIN/TIAO-3", 
            DateTimeOffset.UtcNow, 
            Guid.Parse("8efbf76c-571a-4f10-a720-f4f8e405e467"), 
            SsoProviders.MicrosoftEntra);

        await userManager.CreateAsync(distanceStudent);
        await userManager.AddToRoleAsync(distanceStudent, nameof(AppRoles.DistanceStudent));


        var wimDedulle = new ApplicationUser(
            "wim@rise2526t2campusappoutlook.onmicrosoft.com",
            "Wim",
            "Dedulle",
            "TIN/TIAO-3",
            DateTimeOffset.UtcNow,
            Guid.Parse("08de2d04-12de-465b-81d8-0b798b03d28e"),
            SsoProviders.MicrosoftEntra);

        await userManager.CreateAsync(wimDedulle);
        await userManager.AddToRoleAsync(wimDedulle, nameof(AppRoles.DistanceStudent));

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
            new StudentActivity("Kennismakingsborrel", "Start het academiejaar goed! Kom kennismaken met medestudenten en maak nieuwe vrienden tijdens onze gezellige borrel.", new DateTime(2023, 11, 15), new TimeRange (new TimeOnly( 10, 0  ), new TimeOnly(12, 0)), "/img/banner30.webp", locations[0], studentClubs[0]),
            new StudentActivity("Game Night", "Zin om te gamen met medestudenten? Breng je favoriete console mee of doe mee aan onze toernooien. Pizza en drankjes aanwezig!", new DateTime(2023, 12, 5), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)), "/img/banner31.webp", locations[1], studentClubs[1]),
            new StudentActivity("Coding Workshop", "Leer nieuwe programmeertechnieken en werk samen aan uitdagende code challenges. Geschikt voor alle niveaus.", new DateTime(2024, 1, 20), new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)), "/img/banner32.webp", locations[2], studentClubs[2]),
            new StudentActivity("Sport & BBQ", "Sportieve middag met volleybal, voetbal en andere teamsporten. Afsluiten doen we met een gezellige BBQ!", new DateTime(2024, 2, 10), new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)), "/img/banner33.webp", locations[3], studentClubs[3]),
            new StudentActivity("Filmavond", "Chillen met een goede film en popcorn. Stemmen voor de film gebeurt via onze social media. Gratis toegang!", new DateTime(2024, 3, 25), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)), "/img/banner34.webp", locations[4], studentClubs[4]),
            new StudentActivity("Ontbijt Networking", "Start je dag goed met een gezond ontbijt en netwerk met studenten uit verschillende richtingen.", new DateTime(2024, 4, 5), new TimeRange(new TimeOnly(8, 30), new TimeOnly(10, 30)), "/img/banner36.webp", locations[0], studentClubs[1]),
            new StudentActivity("Culturele Avond", "Ontdek verschillende culturen door muziek, dans en culinaire hoogstandjes. Iedereen is welkom!", new DateTime(2024, 5, 12), new TimeRange(new TimeOnly(15, 0), new TimeOnly(17, 0)), "/img/banner37.webp", locations[1], studentClubs[2]),
            new StudentActivity("Hackathon", "24-uur durende programmeer marathon! Vorm teams en bouw innovatieve oplossingen. Prijzen te winnen!", new DateTime(2024, 6, 18), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), "/img/banner38.webp", locations[2], studentClubs[3]),
            new StudentActivity("Cantus", "Traditionele studentencantus met bekende liedjes. Liedjesboek wordt voorzien, stemverheffing vereist!", new DateTime(2024, 7, 22), new TimeRange(new TimeOnly(18, 0), new TimeOnly(20, 0)), "/img/banner39.webp", locations[3], studentClubs[4]),
            new StudentActivity("Yoga & Mindfulness", "Ontspan tussen de examens door met een yoga sessie. Perfect voor beginners, breng je eigen matje mee.", new DateTime(2024, 8, 30), new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)), "/img/banner40.webp", locations[4], studentClubs[0]),
            new StudentActivity("Pubquiz", "Test je algemene kennis tijdens onze legendarische pubquiz. Vorm teams van 4-6 personen. Leuke prijzen!", new DateTime(2024, 9, 14), new TimeRange(new TimeOnly(12, 0), new TimeOnly(14, 0)), "/img/banner41.webp", locations[0], studentClubs[1]),
            new StudentActivity("Karaokenight", "Laat je zangtalent horen of kom gewoon genieten van de optredens. Alle genres welkom!", new DateTime(2024, 10, 3), new TimeRange(new TimeOnly(14, 30), new TimeOnly(16, 30)), "/img/banner42.webp", locations[1], studentClubs[2]),
            new StudentActivity("Workshop CV & Solliciteren", "Bereid je voor op de arbeidsmarkt. Leer hoe je een sterk CV schrijft en overtuigend solliciteert.", new DateTime(2024, 11, 11), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), "/img/banner43.webp", locations[2], studentClubs[3]),
            new StudentActivity("Kerstdiner", "Gezellig samen tafelen met een heerlijk kerstmenu. Dress code: festive! Schrijf snel in, plaatsen zijn beperkt.", new DateTime(2024, 12, 1), new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)), "/img/banner44.webp", locations[3], studentClubs[4]),
            new StudentActivity("Nieuwjaarsreceptie", "Vier het nieuwe jaar met medestudenten en docenten. Toast op een succesvol 2025!", new DateTime(2025, 1, 19), new TimeRange(new TimeOnly(9, 30), new TimeOnly(11, 30)), "/img/banner46.webp", locations[4], studentClubs[0]),
            new StudentActivity("Escape Room Challenge", "Los puzzels op en ontsnaap binnen de tijd. Teamwork is essentieel! Inschrijven per team van 4-5 personen.", new DateTime(2025, 2, 8), new TimeRange(new TimeOnly(16, 0), new TimeOnly(18, 0)), "/img/banner47.webp", locations[0], studentClubs[1]),
            new StudentActivity("Fotowedstrijd", "Laat je creativiteit zien! Upload je beste campus foto's en maak kans op mooie prijzen.", new DateTime(2025, 3, 16), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)), "/img/banner48.webp", locations[1], studentClubs[2]),
            new StudentActivity("Battlenight", "Epische battle tussen studentenclubs! Verschillende challenges en games. Supporteren is gratis!", new DateTime(2025, 4, 27), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)), "/img/banner49.webp", locations[2], studentClubs[3]),
            new StudentActivity("Studiepauze Picknick", "Neem een pauze van het studeren en geniet van een gezellige picknick op campus. Eten en drinken voorzien.", new DateTime(2025, 5, 9), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), "/img/banner50.webp", locations[3], studentClubs[4]),
            new StudentActivity("Zomerfuif", "Sluit het academiejaar af met de grootste studentenfuif! DJ, cocktails en goede vibes gegarandeerd.", new DateTime(2025, 6, 21), new TimeRange(new TimeOnly(17, 0), new TimeOnly(19, 0)), "/img/banner51.webp", locations[4], studentClubs[0])
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
            new SchoolEvent("HOGENT Sportdag", "Doe mee aan onze jaarlijkse sportdag! Verschillende sporten, teambuilding en gezonde competitie. Voor alle niveaus.", dateTimeNow.AddDays(-2), new TimeRange (new TimeOnly( 10, 0  ), new TimeOnly(12, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Sport", "/img/banner10.webp", locations[0]),
            new SchoolEvent("Wetenschapsquiz", "Test je wetenschappelijke kennis! Interessante vragen over technologie, biologie, chemie en meer. Prijzen te winnen!", dateTimeNow.AddDays(-2), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)),0.5m,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Science",  "/img/banner11.webp", locations[1]),
            new SchoolEvent("Mindfulness Workshop", "Leer omgaan met stress en druk. Praktische mindfulness oefeningen voor betere mentale gezondheid tijdens je studies.", dateTimeNow.AddDays(-2), new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)),1,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Wellbeing",  "/img/banner12.webp", locations[2]),
            new SchoolEvent("Theateravond", "Geniet van een voorstelling door studenten Theater. Een avond vol drama, comedy en talent van onze eigen campus!", dateTimeNow.AddDays(-2), new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)),2,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Culture",  "/img/banner13.webp", locations[3]),
            new SchoolEvent("Gastlezing AI & Machine Learning", "Topspreker uit de industrie vertelt over de nieuwste ontwikkelingen in AI. Q&A sessie inbegrepen.", dateTimeNow.AddDays(-1), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)),3.5m,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner14.webp", locations[4]),
            new SchoolEvent("Yoga voor Studenten", "Perfecte start van je dag met een ontspannende yoga sessie. Alle niveaus welkom, matjes aanwezig.", dateTimeNow.AddDays(-1), new TimeRange(new TimeOnly(8, 30), new TimeOnly(10, 30)),5,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Wellbeing",  "/img/banner16.webp", locations[0]),
            new SchoolEvent("Open Lab Dag", "Ontdek onze state-of-the-art laboratoria! Demonstraties, experimenten en gesprekken met onderzoekers.", dateTimeNow, new TimeRange(new TimeOnly(15, 0), new TimeOnly(17, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner17.webp", locations[1]),
            new SchoolEvent("Interculturele Kookworkshop", "Leer gerechten maken uit verschillende culturen. Samen koken, samen eten, samen leren!", dateTimeNow, new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Culture",  "/img/banner18.webp", locations[2]),
            new SchoolEvent("Startup Pitch Night", "Studenten pitchen hun startup ideeën aan een jury van ondernemers. Beste pitch wint mentorship en startkapitaal!", dateTimeNow, new TimeRange(new TimeOnly(18, 0), new TimeOnly(20, 0)),10,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner19.webp", locations[3]),
            new SchoolEvent("Poëzieavond", "Luister naar gesproken poëzie van studenten en gastdichters. Open mic voor wie zelf wil voordragen!", dateTimeNow, new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Culture",  "/img/banner20.webp", locations[4]),
            new SchoolEvent("Duurzaamheid op Campus", "Workshop over duurzaam leven als student. Tips voor budget-vriendelijk en milieubewust leven.", dateTimeNow, new TimeRange(new TimeOnly(12, 0), new TimeOnly(14, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Nature",  "/img/banner21.webp", locations[0]),
            new SchoolEvent("Meditatie & Ademhalingsoefeningen", "Leer effectieve ontspanningstechnieken voor tijdens examens. Rustige omgeving, ervaren begeleider.", dateTimeNow, new TimeRange(new TimeOnly(14, 30), new TimeOnly(16, 30)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Wellbeing",  "/img/banner22.webp", locations[1]),
            new SchoolEvent("Robotica Demonstratie", "Zie de nieuwste robotica projecten van onze studenten in actie. Live demos en hands-on ervaring mogelijk!", dateTimeNow, new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner23.webp", locations[2]),
            new SchoolEvent("Botanische Wandeling", "Ontdek de flora rond campus met een expert botanicus. Leer over lokale planten en biodiversiteit.", dateTimeNow, new TimeRange(new TimeOnly(13, 0), new TimeOnly(15, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Nature",  "/img/banner24.webp", locations[3]),
            new SchoolEvent("Excursie Natuurreservaat", "Dagtrip naar natuurgebied met gids. Ontdek ecosystemen en doe mee aan citizen science project. Inclusief vervoer!", dateTimeNow, new TimeRange(new TimeOnly(9, 30), new TimeOnly(11, 30)),40,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Nature",  "/img/banner26.webp", locations[4]),
            new SchoolEvent("Studenten Welzijnscafé", "Informele bijeenkomst over mentale gezondheid. Praat met professionals en medestudenten in vertrouwelijke setting.", dateTimeNow, new TimeRange(new TimeOnly(16, 0), new TimeOnly(18, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Wellbeing",  "/img/banner27.webp", locations[0]),
            new SchoolEvent("Cybersecurity Workshop", "Leer over online veiligheid, privacy en hoe je jezelf beschermt tegen cyber threats. Praktische tips en oefeningen.", dateTimeNow.AddDays(1), new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Science",  "/img/banner28.webp", locations[1]),
            new SchoolEvent("Gezonde Kookcursus", "Leer gezonde en betaalbare maaltijden maken voor studenten. Recepten, tips en proeverij inbegrepen!", dateTimeNow.AddDays(1), new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Wellbeing",  "/img/banner29.webp", locations[2]),
            new SchoolEvent("Wereldmuziek Concert", "Ervaar muziek uit alle hoeken van de wereld. Optredens door internationale studenten en lokale artiesten.", dateTimeNow.AddDays(1), new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Public","Culture",  "/img/banner30.webp", locations[3]),
            new SchoolEvent("3D Printing Demo", "Ontdek de mogelijkheden van 3D printing. Print je eigen object en leer over toepassingen in verschillende sectoren.", dateTimeNow.AddDays(1), new TimeRange(new TimeOnly(17, 0), new TimeOnly(19, 0)),0,"https://forms.office.com/pages/responsepage.aspx?id=DjH3XBoJxUus1ybHIdTMzcYqySHRUtBEo2I4fMv60GRUODYzODRBNEJSRTZEQkZVMExQR1VIQUFBUy4u&route=shorturl",5,true,"Student","Science",  "/img/banner31.webp", locations[4])
        });
        await dbContext.SaveChangesAsync();
    }
}