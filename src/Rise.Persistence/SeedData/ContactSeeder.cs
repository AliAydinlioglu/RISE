using Rise.Domain.Common;
using Rise.Domain.Contact;

namespace Rise.Persistence.SeedData;

public static class ContactSeeder
{
    public static async Task Seed(ApplicationDbContext dbContext)
    {
        if (dbContext.Services.Any())
            return;

        var services = new List<Facility>
        {
            CreateStudentensecretariaatSchoonmeersen(),
            CreateBibliotheekSchoonmeersen(),
            CreateStandaardStudentenShopSchoonmeersen(),
            CreateIBaMaFlex(),
            CreateStakingWatNu(),
            CreatePsychosocialeOndersteuning(),
            CreateOngevalMelden(),
            CreateStudentensecretariaatMercator(),
            CreateBibliotheekMercator(),
            CreateStandaardStudentenShopGent()
        };

        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Helper method to get dates for the current week (Monday-Friday)
    /// This ensures opening hours are always current
    /// </summary>
    private static List<DateOnly> GetCurrentWeekDates()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var dayOfWeek = (int)today.DayOfWeek;

        // Calculate Monday of current week (0=Sunday, 1=Monday, etc.)
        var monday = today.AddDays(-(dayOfWeek == 0 ? 6 : dayOfWeek - 1));

        return new List<DateOnly>
        {
            monday, // Monday
            monday.AddDays(1), // Tuesday
            monday.AddDays(2), // Wednesday
            monday.AddDays(3), // Thursday
            monday.AddDays(4) // Friday
        };
    }

    private static Facility CreateStudentensecretariaatSchoonmeersen()
    {
        var service = new Facility(
            "Studentensecretariaat Schoonmeersen",
            new FacilityCategory("Administratief")
        );

        service.DescribeService(
            "Het studentensecretariaat helpt je met administratieve vragen over je inschrijving, studiebewijzen, en studiefinanciering.");

        var address = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location = new FacilityLocation(address, "Schoonmeersen");
        service.ChangeLocation(location);

        var weekDates = GetCurrentWeekDates();
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(weekDates[0], new List<TimeRange> // Monday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(weekDates[1], new List<TimeRange> // Tuesday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(18, 30))
            }),
            new ContactPeriod(weekDates[2], new List<TimeRange> // Wednesday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(weekDates[3], new List<TimeRange> // Thursday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(weekDates[4], new List<TimeRange> // Friday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            })
        };
        service.ChangeOpeningsHours(openingHours);

        service.AddRemark("Aangepaste openingsuren tijdens de examens.");

        service.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:studentensecretariaat.dit@hogent.be",
            CommunicationTypes.Email));
        service.AddCommunicationChannel(new CommunicationChannel("Telefoon", "tel:09 243 31 00",
            CommunicationTypes.Phone));

        return service;
    }

    private static Facility CreateBibliotheekSchoonmeersen()
    {
        var service = new Facility(
            "Bibliotheek Schoonmeersen",
            new FacilityCategory("Ondersteunend")
        );

        service.DescribeService(
            "De bibliotheek biedt een uitgebreide collectie boeken, tijdschriften en online bronnen.");

        var address = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location = new FacilityLocation(address, "Schoonmeersen");
        service.ChangeLocation(location);

        var weekDates = GetCurrentWeekDates();
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(weekDates[0], // Monday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(weekDates[1], // Tuesday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(weekDates[2], // Wednesday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(weekDates[3], // Thursday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(weekDates[4], // Friday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(16, 45)) })
        };
        service.ChangeOpeningsHours(openingHours);

        service.AddRemark("Gesloten op weekend en feestdagen");

        service.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:bibschoonmeersen@hogent.be",
            CommunicationTypes.Email));
        service.AddCommunicationChannel(new CommunicationChannel("Website",
            "https://www.hogent.be/student/bibliotheken/", CommunicationTypes.Form));
        service.AddCommunicationChannel(new CommunicationChannel("Telefoon", "tel:09 243 35 60",
            CommunicationTypes.Phone));
        service.AddCommunicationChannel(new CommunicationChannel("Facebook",
            "https://www.facebook.com/BIBSchoonmeersen/", CommunicationTypes.SocialMedia));
        service.AddCommunicationChannel(new CommunicationChannel("Vimeo",
            "https://vimeo.com/hogent", CommunicationTypes.SocialMedia));
        service.AddCommunicationChannel(new CommunicationChannel("Pinterest",
            "https://www.pinterest.com/bibschoo/", CommunicationTypes.SocialMedia));

        return service;
    }

    private static Facility CreateStandaardStudentenShopSchoonmeersen()
    {
        var service = new Facility(
            "Standaard Studenten Shop Schoonmeersen",
            new FacilityCategory("Ondersteunend")
        );

        service.DescribeService("Bij de Standaard kan je terecht voor studiemateriaal, boeken en veel meer.");

        var address = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location = new FacilityLocation(address, "Schoonmeersen");
        service.ChangeLocation(location);

        var weekDates = GetCurrentWeekDates();
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(weekDates[1], // Tuesday
                new List<TimeRange> { new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)) }),
            new ContactPeriod(weekDates[2], // Wednesday
                new List<TimeRange> { new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)) }),
            new ContactPeriod(weekDates[3], // Thursday
                new List<TimeRange> { new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)) }),
            new ContactPeriod(weekDates[4], // Friday
                new List<TimeRange> { new TimeRange(new TimeOnly(9, 0), new TimeOnly(16, 0)) })
        };
        service.ChangeOpeningsHours(openingHours);

        service.AddRemark("Gesloten op weekend en feestdagen");

        service.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:ks.hogent@standaardboekhandel.be",
            CommunicationTypes.Email));
        service.AddCommunicationChannel(new CommunicationChannel("Website",
            "https://hogent.standaardstudentshop.be/Practical", CommunicationTypes.Form));
        service.AddCommunicationChannel(new CommunicationChannel("Telefoon", "tel:09 292 10 25",
            CommunicationTypes.Phone));

        return service;
    }

    private static Facility CreateStudentensecretariaatMercator()
    {
        var service = new Facility(
            "Studentensecretariaat Mercator",
            new FacilityCategory("Administratief")
        );

        service.DescribeService(
            "Het studentensecretariaat helpt je met administratieve vragen over je inschrijving, studiebewijzen, en studiefinanciering.");

        var address = new StructuredAddress("Henleykaai", 84, 9000, "Gent", "");
        var location = new FacilityLocation(address, "Mercator");
        service.ChangeLocation(location);

        var weekDates = GetCurrentWeekDates();
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(weekDates[0], new List<TimeRange> // Monday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(weekDates[1], new List<TimeRange> // Tuesday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(weekDates[2], new List<TimeRange> // Wednesday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(weekDates[3], new List<TimeRange> // Thursday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(weekDates[4], new List<TimeRange> // Friday
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            })
        };
        service.ChangeOpeningsHours(openingHours);

        service.AddRemark("Aangepaste openingsuren tijdens de examens.");

        service.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:info@hogent.be",
            CommunicationTypes.Email));
        service.AddCommunicationChannel(new CommunicationChannel("Telefoon", "tel:09 243 31 20",
            CommunicationTypes.Phone));

        return service;
    }

    private static Facility CreateBibliotheekMercator()
    {
        var service = new Facility(
            "Bibliotheek Mercator",
            new FacilityCategory("Ondersteunend")
        );

        service.DescribeService(
            "De bibliotheek biedt een uitgebreide collectie boeken, tijdschriften en online bronnen.");

        var address = new StructuredAddress("Henleykaai", 84, 9000, "Gent", "");
        var location = new FacilityLocation(address, "Mercator");
        service.ChangeLocation(location);

        var weekDates = GetCurrentWeekDates();
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(weekDates[0], // Monday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(weekDates[1], // Tuesday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(weekDates[2], // Wednesday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(weekDates[3], // Thursday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(weekDates[4], // Friday
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(16, 45)) })
        };
        service.ChangeOpeningsHours(openingHours);

        service.AddRemark("Gesloten op weekend en feestdagen \n10-11 november \nKerstverlof");

        service.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:bibmercator@hogent.be",
            CommunicationTypes.Email));
        service.AddCommunicationChannel(new CommunicationChannel("Website",
            "https://www.hogent.be/student/bibliotheken/", CommunicationTypes.Form));
        service.AddCommunicationChannel(new CommunicationChannel("Telefoon", "tel:09 243 35 80",
            CommunicationTypes.Phone));

        return service;
    }

    private static Facility CreateStandaardStudentenShopGent()
    {
        var service = new Facility(
            "Standaard Studenten Shop Gent",
            new FacilityCategory("Ondersteunend")
        );

        service.DescribeService("Bij de Standaard kan je terecht voor studiemateriaal, boeken en veel meer.");

        var address = new StructuredAddress("Bagattenstraat", 51, 9000, "Gent", "");
        var location = new FacilityLocation(address, "Gent Campus");
        service.ChangeLocation(location);

        var weekDates = GetCurrentWeekDates();
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(weekDates[0], // Monday
                new List<TimeRange> { new TimeRange(new TimeOnly(11, 0), new TimeOnly(18, 0)) }),
            new ContactPeriod(weekDates[1], // Tuesday
                new List<TimeRange> { new TimeRange(new TimeOnly(11, 0), new TimeOnly(15, 0)) }),
            new ContactPeriod(weekDates[2], // Wednesday
                new List<TimeRange> { new TimeRange(new TimeOnly(11, 0), new TimeOnly(18, 0)) }),
            new ContactPeriod(weekDates[3], // Thursday
                new List<TimeRange> { new TimeRange(new TimeOnly(11, 0), new TimeOnly(15, 0)) }),
            new ContactPeriod(weekDates[4], // Friday
                new List<TimeRange> { new TimeRange(new TimeOnly(11, 0), new TimeOnly(15, 0)) })
        };
        service.ChangeOpeningsHours(openingHours);

        service.AddRemark("Gesloten op vakantieperiodes en op feest-, brug- en weekenddagen");

        service.AddCommunicationChannel(new CommunicationChannel("E-mail", "ks.hogent@standaardboekhandel.be",
            CommunicationTypes.Email));
        service.AddCommunicationChannel(new CommunicationChannel("Website",
            "https://hogent.standaardstudentshop.be/Practical", CommunicationTypes.Form));

        return service;
    }


    private static Facility CreateIBaMaFlex()
    {
        var service = new Facility(
            "iBaMaFlex!",
            new FacilityCategory("Administratief")
        );

        service.DescribeService("Online studentenplatform voor studievoortgang, punten en examens.");

        service.AddRemark("24/7 online beschikbaar");

        service.AddCommunicationChannel(new CommunicationChannel("Platform", "https://ibamaflex.hogent.be/Main.aspx",
            CommunicationTypes.Form));

        return service;
    }

    private static Facility CreateStakingWatNu()
    {
        var service = new Facility(
            "Staking: Wat nu?",
            new FacilityCategory("Administratief")
        );

        service.DescribeService(
            "Info over wat te doen bij stakingen van openbaar vervoer. Check hier of je lessen doorgaan en wat je rechten zijn.");


        service.AddCommunicationChannel(new CommunicationChannel("Staking info pagina",
            "https://www.hogent.be/student/praktische-info/staking/", CommunicationTypes.Form));
        service.AddCommunicationChannel(new CommunicationChannel("Instagram updates",
            "https://www.instagram.com/p/CmMefR_K3zh/?hl=en", CommunicationTypes.SocialMedia));

        return service;
    }

    private static Facility CreatePsychosocialeOndersteuning()
    {
        var service = new Facility(
            "Psychosociale ondersteuning",
            new FacilityCategory("Veiligheid en welzijn")
        );

        service.DescribeService(
            "Vertrouwelijke gesprekken met professionele begeleiders voor studiestress en persoonlijke problemen.");

        var address = new StructuredAddress("Overwale", 42, 9000, "Gent", "");
        var location = new FacilityLocation(address, "Schoonmeersen");
        service.ChangeLocation(location);


        service.AddRemark("Maak een afspraak met het zorgteam.");
        service.AddCommunicationChannel(new CommunicationChannel("Afspraak form",
            "https://agenda.appoint.be/e/aHN0UVJrME14VXVPZGJZS2h5UDFZY1QzTkc5Q2QrNUV6UGJMSThUb1l0SEhUQ0VnVS9xZkdBUVY3cHNwQUNxRkc3TzFtd1BWWk41M2dJWW5nckNwK0dtajhZVjRnYVZCYUd1SElJVWtoV3g3S3RhK2hkSlJaNFdSWEVUbzhhSUw%3D",
            CommunicationTypes.Form));
        service.AddCommunicationChannel(new CommunicationChannel("Email", "mailto:zorg@hogent.be",
            CommunicationTypes.Email));
        service.AddCommunicationChannel(new CommunicationChannel("Telefoon", "tel:09 243 37 38",
            CommunicationTypes.Phone));

        return service;
    }

    private static Facility CreateOngevalMelden()
    {
        var service = new Facility(
            "Ongeval melden",
            new FacilityCategory("Veiligheid en welzijn")
        );

        service.DescribeService(
            "Ben je getuige of slachtoffer van een ongeval? Meld dit altijd aan de Dienst Preventie en Welzijn");

        service.AddCommunicationChannel(new CommunicationChannel("Formulier",
            "https://forms.office.com/Pages/ResponsePage.aspx?id=DjH3XBoJxUus1ybHIdTMzUsTYRfbH5tBmkqR2cgN-61UODNZSzkyUzZOWE9HQ0lBQU1VNTRYVEJYRSQlQCN0PWcu",
            CommunicationTypes.Form));
        service.AddCommunicationChannel(new CommunicationChannel("Ongeval melden", "tel:09 243 33 20",
            CommunicationTypes.Phone));
        service.AddCommunicationChannel(new CommunicationChannel("Noodgevallen", "tel:+32 9 248 88 88",
            CommunicationTypes.Phone));
        service.AddCommunicationChannel(new CommunicationChannel("Levensbedreigend", "tel:112",
            CommunicationTypes.Phone));

        return service;
    }
}