using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Common;
using Rise.Domain.Contact;
using Rise.Persistence;
using Rise.Services.Contact;
using Rise.Shared.Common;

namespace Rise.Services.Tests.Contact;

public class ContactServiceShould
{
    [Theory]
    [InlineData("Studentensecretariaat", 2)]
    [InlineData("Bibliotheek", 2)]
    [InlineData("administratieve vragen", 2)]
    [InlineData("studiemateriaal", 2)]
    public async Task GetIndexAsync_SearchInNameOrDescription(string searchTerm, int expectedCount)
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_SearchInNameOrDescription) + searchTerm);
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake { Skip = 0, Take = 20, SearchTerm = searchTerm };

        var result = await service.GetIndexAsync(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Facilities.Count().ShouldBe(expectedCount);
    }

    [Fact]
    public async Task GetIndexAsync_ReturnAllServicesWithCorrectSorting_WhenNoSearchTerm()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_ReturnAllServicesWithCorrectSorting_WhenNoSearchTerm));
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake { Skip = 0, Take = 20 };

        var result = await service.GetIndexAsync(request, CancellationToken.None);
        var resultServices = result.Value.Facilities.ToList();

        result.IsSuccess.ShouldBeTrue();
        result.Value.Facilities.Count().ShouldBe(11);
        result.Value.TotalCount.ShouldBe(11);
        resultServices.First().FacilityCategoryName.ShouldBe("Administratief");
    }

    [Fact]
    public async Task GetIndexAsync_ApplySkipAndTake()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_ApplySkipAndTake));
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake { Skip = 2, Take = 3 };

        var result = await service.GetIndexAsync(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Facilities.Count().ShouldBe(3);
        result.Value.TotalCount.ShouldBe(11);
    }

    [Theory]
    [InlineData("Name", false, "Bibliotheek Mercator")]
    [InlineData("Name", true, "Studentensecretariaat Schoonmeersen")]
    public async Task GetIndexAsync_OrderBy(string orderBy, bool descending, string expectedFirst)
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_OrderBy) + orderBy + descending);
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 20,
            OrderBy = orderBy,
            OrderDescending = descending
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Facilities.First().Name.ShouldBe(expectedFirst);
    }

    [Fact]
    public async Task GetDetailByIdAsync_ReturnCorrectDto_WhenIdExists()
    {
        var options = GetDbContextOptions(nameof(GetDetailByIdAsync_ReturnCorrectDto_WhenIdExists));
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);
        var existingService = await dbContext.Services.FirstAsync();

        var result = await service.GetDetailByIdAsync(existingService.Id, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Service.Id.ShouldBe(existingService.Id);
        result.Value.Service.Name.ShouldBe(existingService.Name);
        result.Value.Service.FacilityCategoryName.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetDetailByIdAsync_ReturnNotFound_WhenIdDoesNotExist()
    {
        var options = GetDbContextOptions(nameof(GetDetailByIdAsync_ReturnNotFound_WhenIdDoesNotExist));
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);

        var result = await service.GetDetailByIdAsync(999, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
    }

    [Theory]
    [InlineData("Administratief", 3)]
    [InlineData("Ondersteunend", 5)]
    [InlineData("Veiligheid en welzijn", 2)]
    [InlineData("Onbekend", 1)]
    public async Task GetByCategoryAsync_ReturnCorrectServices_WhenCategoryExists(string category, int expectedCount)
    {
        var options =
            GetDbContextOptions(nameof(GetByCategoryAsync_ReturnCorrectServices_WhenCategoryExists) + category);
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);

        var result = await service.GetByCategoryAsync(category, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Facilities.ShouldAllBe(s => s.FacilityCategoryName == category);
        result.Value.Facilities.Count().ShouldBe(expectedCount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetByCategoryAsync_ReturnError_WhenCategoryNameIsNullOrEmpty(string? categoryName)
    {
        var options = GetDbContextOptions(nameof(GetByCategoryAsync_ReturnError_WhenCategoryNameIsNullOrEmpty));
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);

        var result = await service.GetByCategoryAsync(categoryName!, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
    }

    [Theory]
    [InlineData("Schoonmeersen", 4)]
    [InlineData("Mercator", 2)]
    [InlineData("Gent Campus", 1)]
    public async Task GetByCampusAsync_ReturnCorrectServices_WhenCampusExists(string campus, int expectedCount)
    {
        var options = GetDbContextOptions(nameof(GetByCampusAsync_ReturnCorrectServices_WhenCampusExists) + campus);
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);

        var result = await service.GetByCampusAsync(campus, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Facilities.ShouldAllBe(s => s.Location != null && s.Location.LocationName == campus);
        result.Value.Facilities.Count().ShouldBe(expectedCount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetByCampusAsync_ReturnError_WhenCampusNameIsNullOrEmpty(string? campusName)
    {
        var options = GetDbContextOptions(nameof(GetByCampusAsync_ReturnError_WhenCampusNameIsNullOrEmpty));
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);

        var result = await service.GetByCampusAsync(campusName!, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
    }

    [Fact]
    public async Task GetStaticServicesAsync_ReturnOnlyServicesWithoutLocation()
    {
        var options = GetDbContextOptions(nameof(GetStaticServicesAsync_ReturnOnlyServicesWithoutLocation));
        using var dbContext = new ApplicationDbContext(options);

        var services = CreateTestServices();
        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync();

        var service = new ContactService(dbContext);

        var result = await service.GetStaticServicesAsync(CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Facilities.ShouldAllBe(s => s.Location == null);
        result.Value.Facilities.Count().ShouldBe(4);
    }

    private DbContextOptions<ApplicationDbContext> GetDbContextOptions(string dbName)
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    private List<Facility> CreateTestServices()
    {
        var service1 = new Facility("Studentensecretariaat Schoonmeersen", new FacilityCategory("Administratief"));
        service1.DescribeService(
            "Het studentensecretariaat helpt je met administratieve vragen over je inschrijving, studiebewijzen, en studiefinanciering.");
        var address1 = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location1 = new FacilityLocation(address1, "Schoonmeersen");
        service1.ChangeLocation(location1);
        var openingHours1 = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 3), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(8, 30), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(16, 0))
            })
        };
        service1.ChangeOpeningsHours(openingHours1);
        service1.AddRemark("Aangepaste openingsuren tijdens de examens.");
        service1.AddCommunicationChannel(new CommunicationChannel("E-mail",
            "mailto:studentensecretariaat.dit@hogent.be", CommunicationTypes.Email));

        var service2 = new Facility("Bibliotheek Schoonmeersen", new FacilityCategory("Ondersteunend"));
        service2.DescribeService(
            "De bibliotheek biedt een uitgebreide collectie boeken, tijdschriften en online bronnen.");
        var address2 = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location2 = new FacilityLocation(address2, "Schoonmeersen");
        service2.ChangeLocation(location2);
        service2.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:bibschoonmeersen@hogent.be",
            CommunicationTypes.Email));

        var service3 = new Facility("Standaard Studenten Shop Schoonmeersen", new FacilityCategory("Ondersteunend"));
        service3.DescribeService("Bij de Standaard kan je terecht voor studiemateriaal, boeken en veel meer.");
        var address3 = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location3 = new FacilityLocation(address3, "Schoonmeersen");
        service3.ChangeLocation(location3);
        service3.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:ks.hogent@standaardboekhandel.be",
            CommunicationTypes.Email));

        var service4 = new Facility("Studentensecretariaat Mercator", new FacilityCategory("Administratief"));
        service4.DescribeService("Het studentensecretariaat helpt je met administratieve vragen.");
        var address4 = new StructuredAddress("Henleykaai", 84, 9000, "Gent", "");
        var location4 = new FacilityLocation(address4, "Mercator");
        service4.ChangeLocation(location4);
        service4.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:info@hogent.be",
            CommunicationTypes.Email));

        var service5 = new Facility("Bibliotheek Mercator", new FacilityCategory("Ondersteunend"));
        service5.DescribeService(
            "De bibliotheek biedt een uitgebreide collectie boeken, tijdschriften en online bronnen.");
        var address5 = new StructuredAddress("Henleykaai", 84, 9000, "Gent", "");
        var location5 = new FacilityLocation(address5, "Mercator");
        service5.ChangeLocation(location5);
        service5.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:bibmercator@hogent.be",
            CommunicationTypes.Email));

        var service6 = new Facility("Standaard Studenten Shop Gent", new FacilityCategory("Ondersteunend"));
        service6.DescribeService("Bij de Standaard kan je terecht voor studiemateriaal, boeken en veel meer.");
        var address6 = new StructuredAddress("Bagattenstraat", 51, 9000, "Gent", "");
        var location6 = new FacilityLocation(address6, "Gent Campus");
        service6.ChangeLocation(location6);
        service6.AddCommunicationChannel(new CommunicationChannel("E-mail", "ks.hogent@standaardboekhandel.be",
            CommunicationTypes.Email));

        var service7 = new Facility("iBaMaFlex!", new FacilityCategory("Administratief"));
        service7.DescribeService("Online studentenplatform voor studievoortgang, punten en examens.");
        service7.AddCommunicationChannel(new CommunicationChannel("Platform", "https://ibamaflex.hogent.be/Main.aspx",
            CommunicationTypes.Form));

        var service8 = new Facility("Staking: Wat nu?", new FacilityCategory("Ondersteunend"));
        service8.DescribeService("Info over wat te doen bij stakingen van openbaar vervoer.");
        service8.AddCommunicationChannel(new CommunicationChannel("Staking info pagina",
            "https://www.hogent.be/student/praktische-info/staking/", CommunicationTypes.Form));

        var service9 = new Facility("Psychosociale ondersteuning", new FacilityCategory("Veiligheid en welzijn"));
        service9.DescribeService(
            "Vertrouwelijke gesprekken met professionele begeleiders voor studiestress en persoonlijke problemen.");
        var address9 = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location9 = new FacilityLocation(address9, "Schoonmeersen");
        service9.ChangeLocation(location9);
        service9.AddCommunicationChannel(new CommunicationChannel("Email", "mailto:zorg@hogent.be",
            CommunicationTypes.Email));

        var service10 = new Facility("Ongeval melden", new FacilityCategory("Veiligheid en welzijn"));
        service10.DescribeService(
            "Ben je getuige of slachtoffer van een ongeval? Meld dit altijd aan de Dienst Preventie en Welzijn");
        service10.AddCommunicationChannel(new CommunicationChannel("Formulier",
            "https://forms.office.com/Pages/ResponsePage.aspx", CommunicationTypes.Form));
        service10.AddCommunicationChannel(new CommunicationChannel("Ongeval melden", "tel:09 243 33 20",
            CommunicationTypes.Phone));
        service10.AddCommunicationChannel(new CommunicationChannel("Noodgevallen", "tel:+32 9 248 88 88",
            CommunicationTypes.Phone));

        var service11 = new Facility("Overige vragen", new FacilityCategory("Onbekend"));
        service11.DescribeService("Voor alle andere vragen die niet in bovenstaande categorieën passen.");
        service11.AddRemark("Vragen over studentenkaart");
        service11.AddRemark("Vragen over afwezigheden");
        service11.AddRemark("Vragen over studiekosten");

        return new List<Facility>
        {
            service1, service2, service3, service4, service5, service6, service7, service8, service9, service10,
            service11
        };
    }
}