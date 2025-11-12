using Rise.Domain.Common;
using Rise.Domain.Contact;

namespace Rise.Persistence.SeedData;

public static class ContactSeeder
{
    public static async Task Seed(ApplicationDbContext dbContext)
    {
        if (dbContext.Services.Any())
            return;

        var services = new List<Service>
        {
            CreateStudentensecretariaatSchoonmeersen(),
            CreateBibliotheekSchoonmeersen(),
            CreateStandaardStudentenShopSchoonmeersen(),
            CreateIBaMaFlex(),
            CreateStakingWatNu(),
            CreatePsychosocialeOndersteuning(),
            CreateOngevalMelden(),
            CreateOverigeVragen(),
            CreateStudentensecretariaatMercator(),
            CreateBibliotheekMercator(),
            CreateStandaardStudentenShopGent()
        };

        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();
    }

    private static Service CreateStudentensecretariaatSchoonmeersen()
    {
        var service = new Service(
            "Studentensecretariaat Schoonmeersen",
            new ServiceCategory("Administratief")
        );

        service.DescribeService(
            "Het studentensecretariaat helpt je met administratieve vragen over je inschrijving, studiebewijzen, en studiefinanciering.");

        var address = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location = new ServiceLocation(address, "Schoonmeersen");
        service.ChangeLocation(location);

        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 3), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(new DateOnly(2025, 11, 4), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(18, 30))
            }),
            new ContactPeriod(new DateOnly(2025, 11, 5), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(new DateOnly(2025, 11, 6), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(new DateOnly(2025, 11, 7), new List<TimeRange>
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

    private static Service CreateBibliotheekSchoonmeersen()
    {
        var service = new Service(
            "Bibliotheek Schoonmeersen",
            new ServiceCategory("Ondersteunend")
        );

        service.DescribeService(
            "De bibliotheek biedt een uitgebreide collectie boeken, tijdschriften en online bronnen.");

        var address = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location = new ServiceLocation(address, "Schoonmeersen");
        service.ChangeLocation(location);

        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 3),
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(new DateOnly(2025, 11, 4),
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(new DateOnly(2025, 11, 5),
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(new DateOnly(2025, 11, 6),
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(new DateOnly(2025, 11, 7),
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

    private static Service CreateStandaardStudentenShopSchoonmeersen()
    {
        var service = new Service(
            "Standaard Studenten Shop Schoonmeersen",
            new ServiceCategory("Ondersteunend")
        );

        service.DescribeService("Bij de Standaard kan je terecht voor studiemateriaal, boeken en veel meer.");

        var address = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location = new ServiceLocation(address, "Schoonmeersen");
        service.ChangeLocation(location);

        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 4),
                new List<TimeRange> { new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)) }),
            new ContactPeriod(new DateOnly(2025, 11, 5),
                new List<TimeRange> { new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)) }),
            new ContactPeriod(new DateOnly(2025, 11, 6),
                new List<TimeRange> { new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)) }),
            new ContactPeriod(new DateOnly(2025, 11, 7),
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

    private static Service CreateStudentensecretariaatMercator()
    {
        var service = new Service(
            "Studentensecretariaat Mercator",
            new ServiceCategory("Administratief")
        );

        service.DescribeService(
            "Het studentensecretariaat helpt je met administratieve vragen over je inschrijving, studiebewijzen, en studiefinanciering.");

        var address = new StructuredAddress("Henleykaai", 84, 9000, "Gent", "");
        var location = new ServiceLocation(address, "Mercator");
        service.ChangeLocation(location);

        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 3), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(new DateOnly(2025, 11, 4), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(new DateOnly(2025, 11, 5), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(new DateOnly(2025, 11, 6), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            }),
            new ContactPeriod(new DateOnly(2025, 11, 7), new List<TimeRange>
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

    private static Service CreateBibliotheekMercator()
    {
        var service = new Service(
            "Bibliotheek Mercator",
            new ServiceCategory("Ondersteunend")
        );

        service.DescribeService(
            "De bibliotheek biedt een uitgebreide collectie boeken, tijdschriften en online bronnen.");

        var address = new StructuredAddress("Henleykaai", 84, 9000, "Gent", "");
        var location = new ServiceLocation(address, "Mercator");
        service.ChangeLocation(location);

        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 3),
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(new DateOnly(2025, 11, 4),
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(new DateOnly(2025, 11, 5),
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(new DateOnly(2025, 11, 6),
                new List<TimeRange> { new TimeRange(new TimeOnly(8, 0), new TimeOnly(21, 45)) }),
            new ContactPeriod(new DateOnly(2025, 11, 7),
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

    private static Service CreateStandaardStudentenShopGent()
    {
        var service = new Service(
            "Standaard Studenten Shop Gent",
            new ServiceCategory("Ondersteunend")
        );

        service.DescribeService("Bij de Standaard kan je terecht voor studiemateriaal, boeken en veel meer.");

        var address = new StructuredAddress("Bagattenstraat", 51, 9000, "Gent", "");
        var location = new ServiceLocation(address, "Gent Campus");
        service.ChangeLocation(location);

        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 3),
                new List<TimeRange> { new TimeRange(new TimeOnly(11, 0), new TimeOnly(18, 0)) }),
            new ContactPeriod(new DateOnly(2025, 11, 4),
                new List<TimeRange> { new TimeRange(new TimeOnly(11, 0), new TimeOnly(15, 0)) }),
            new ContactPeriod(new DateOnly(2025, 11, 5),
                new List<TimeRange> { new TimeRange(new TimeOnly(11, 0), new TimeOnly(18, 0)) }),
            new ContactPeriod(new DateOnly(2025, 11, 6),
                new List<TimeRange> { new TimeRange(new TimeOnly(11, 0), new TimeOnly(15, 0)) }),
            new ContactPeriod(new DateOnly(2025, 11, 7),
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


    private static Service CreateIBaMaFlex()
    {
        var service = new Service(
            "iBaMaFlex!",
            new ServiceCategory("Administratief")
        );

        service.DescribeService("Online studentenplatform voor studievoortgang, punten en examens.");

        service.AddRemark("24/7 online beschikbaar");

        service.AddCommunicationChannel(new CommunicationChannel("Platform", "https://ibamaflex.hogent.be/Main.aspx",
            CommunicationTypes.Form));

        return service;
    }

    private static Service CreateStakingWatNu()
    {
        var service = new Service(
            "Staking: Wat nu?",
            new ServiceCategory("Administratief")
        );

        service.DescribeService(
            "Info over wat te doen bij stakingen van openbaar vervoer. Check hier of je lessen doorgaan en wat je rechten zijn.");


        service.AddCommunicationChannel(new CommunicationChannel("Staking info pagina",
            "https://www.hogent.be/student/praktische-info/staking/", CommunicationTypes.Form));
        service.AddCommunicationChannel(new CommunicationChannel("Instagram updates",
            "https://www.instagram.com/p/CmMefR_K3zh/?hl=en", CommunicationTypes.SocialMedia));

        return service;
    }

    private static Service CreatePsychosocialeOndersteuning()
    {
        var service = new Service(
            "Psychosociale ondersteuning",
            new ServiceCategory("Veiligheid en welzijn")
        );

        service.DescribeService(
            "Vertrouwelijke gesprekken met professionele begeleiders voor studiestress en persoonlijke problemen.");

        var address = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location = new ServiceLocation(address, "Schoonmeersen");
        service.ChangeLocation(location);


        service.AddRemark("Maak een afspraak met het zorgteam.");

        service.AddCommunicationChannel(new CommunicationChannel("Email", "mailto:zorg@hogent.be",
            CommunicationTypes.Email));
        service.AddCommunicationChannel(new CommunicationChannel("Telefoon", "tel:09 243 37 38",
            CommunicationTypes.Phone));

        return service;
    }

    private static Service CreateOngevalMelden()
    {
        var service = new Service(
            "Ongeval melden",
            new ServiceCategory("Veiligheid en welzijn")
        );

        service.DescribeService(
            "Ben je getuige of slachtoffer van een ongeval?\u2028Meld dit altijd aan de Dienst Preventie en Welzijn");

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

    private static Service CreateOverigeVragen()
    {
        var service = new Service(
            "Overige vragen",
            new ServiceCategory("Onbekend")
        );

        service.DescribeService(
            "Voor alle andere vragen die niet in bovenstaande categorieën passen, kan je hier terecht.");

        service.AddRemark("Vragen over studentenkaart");
        service.AddRemark("Vragen over afwezigheden");
        service.AddRemark("Vragen over studiekosten");

        service.AddCommunicationChannel(new CommunicationChannel("", "", CommunicationTypes.None));


        return service;
    }
}