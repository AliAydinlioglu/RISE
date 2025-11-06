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
    [Fact]
    public async Task GetIndexAsync_ReturnAllServices_WhenNoFiltersApplied()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_ReturnAllServices_WhenNoFiltersApplied));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 10
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Services.Count().ShouldBe(3);
        result.Value.TotalCount.ShouldBe(3);
    }

    [Fact]
    public async Task GetIndexAsync_ReturnFilteredServices_WhenSearchTermProvided()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_ReturnFilteredServices_WhenSearchTermProvided));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 10,
            SearchTerm = "Bibliotheek"
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Services.Count().ShouldBe(1);
        result.Value.Services.First().Name.ShouldContain("Bibliotheek");
    }

    [Fact]
    public async Task GetIndexAsync_ReturnPaginatedResults_WhenSkipTakeProvided()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_ReturnPaginatedResults_WhenSkipTakeProvided));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 1,
            Take = 1
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Services.Count().ShouldBe(1);
        result.Value.TotalCount.ShouldBe(3);
    }

    [Theory]
    [InlineData("Name", false)]
    [InlineData("Name", true)]
    [InlineData("Id", false)]
    [InlineData("Id", true)]
    public async Task GetIndexAsync_ReturnOrderedResults_WhenOrderByProvided(string orderBy, bool descending)
    {
        var options = GetDbContextOptions($"{nameof(GetIndexAsync_ReturnOrderedResults_WhenOrderByProvided)}_{orderBy}_{descending}");
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 10,
            OrderBy = orderBy,
            OrderDescending = descending
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Services.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetIndexAsync_OrderByCategoryByDefault()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_OrderByCategoryByDefault));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 10
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        var services = result.Value.Services.ToList();
        services.First().ServiceCategoryName.ShouldBe("Administratief");
    }

    [Fact]
    public async Task GetDetailByIdAsync_ReturnService_WhenIdExists()
    {
        var options = GetDbContextOptions(nameof(GetDetailByIdAsync_ReturnService_WhenIdExists));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var existingService = await dbContext.Services.FirstAsync();

        var result = await service.GetDetailByIdAsync(existingService.Id, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Service.ShouldNotBeNull();
        result.Value.Service.Id.ShouldBe(existingService.Id);
        result.Value.Service.Name.ShouldBe(existingService.Name);
    }

    [Fact]
    public async Task GetDetailByIdAsync_ReturnNotFound_WhenIdDoesNotExist()
    {
        var options = GetDbContextOptions(nameof(GetDetailByIdAsync_ReturnNotFound_WhenIdDoesNotExist));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);

        var result = await service.GetDetailByIdAsync(999, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
    }

    [Fact]
    public async Task GetByCategoryAsync_ReturnServicesInCategory()
    {
        var options = GetDbContextOptions(nameof(GetByCategoryAsync_ReturnServicesInCategory));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);

        var result = await service.GetByCategoryAsync("Administratief", CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Services.ShouldAllBe(s => s.ServiceCategoryName == "Administratief");
        result.Value.Services.Count().ShouldBe(1);
    }

    [Fact]
    public async Task GetByCategoryAsync_ReturnEmptyList_WhenCategoryHasNoServices()
    {
        var options = GetDbContextOptions(nameof(GetByCategoryAsync_ReturnEmptyList_WhenCategoryHasNoServices));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);

        var result = await service.GetByCategoryAsync("NonExistent", CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Services.ShouldBeEmpty();
        result.Value.TotalCount.ShouldBe(0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetByCategoryAsync_ReturnError_WhenCategoryNameIsNullOrWhitespace(string categoryName)
    {
        var options = GetDbContextOptions($"{nameof(GetByCategoryAsync_ReturnError_WhenCategoryNameIsNullOrWhitespace)}_{categoryName}");
        using var dbContext = new ApplicationDbContext(options);

        var service = new ContactService(dbContext);

        var result = await service.GetByCategoryAsync(categoryName, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
    }

    [Fact]
    public async Task GetByCampusAsync_ReturnServicesForCampus()
    {
        var options = GetDbContextOptions(nameof(GetByCampusAsync_ReturnServicesForCampus));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);

        var result = await service.GetByCampusAsync("Schoonmeersen", CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Services.ShouldAllBe(s => s.Location != null && s.Location.LocationName == "Schoonmeersen");
        result.Value.Services.Count().ShouldBe(1);
    }

    [Fact]
    public async Task GetByCampusAsync_ReturnEmptyList_WhenCampusHasNoServices()
    {
        var options = GetDbContextOptions(nameof(GetByCampusAsync_ReturnEmptyList_WhenCampusHasNoServices));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);

        var result = await service.GetByCampusAsync("NonExistentCampus", CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Services.ShouldBeEmpty();
        result.Value.TotalCount.ShouldBe(0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetByCampusAsync_ReturnError_WhenCampusNameIsNullOrWhitespace(string campusName)
    {
        var options = GetDbContextOptions($"{nameof(GetByCampusAsync_ReturnError_WhenCampusNameIsNullOrWhitespace)}_{campusName}");
        using var dbContext = new ApplicationDbContext(options);

        var service = new ContactService(dbContext);

        var result = await service.GetByCampusAsync(campusName, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
    }

    [Fact]
    public async Task GetStaticServicesAsync_ReturnOnlyServicesWithoutLocation()
    {
        var options = GetDbContextOptions(nameof(GetStaticServicesAsync_ReturnOnlyServicesWithoutLocation));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);

        var result = await service.GetStaticServicesAsync(CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Services.ShouldAllBe(s => s.Location == null);
        result.Value.Services.Count().ShouldBe(2);
    }

    [Fact]
    public async Task GetIndexAsync_MapAllFieldsCorrectly()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_MapAllFieldsCorrectly));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 1
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);
        var serviceDto = result.Value.Services.First();

        result.IsSuccess.ShouldBeTrue();
        serviceDto.Id.ShouldBeGreaterThan(0);
        serviceDto.Name.ShouldNotBeNullOrWhiteSpace();
        serviceDto.ServiceCategoryName.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetIndexAsync_MapLocationCorrectly_WhenLocationExists()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_MapLocationCorrectly_WhenLocationExists));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 10,
            SearchTerm = "Studentensecretariaat"
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);
        var serviceDto = result.Value.Services.First();

        result.IsSuccess.ShouldBeTrue();
        serviceDto.Location.ShouldNotBeNull();
        serviceDto.Location.LocationName.ShouldBe("Schoonmeersen");
        serviceDto.Location.ServiceAddress.ShouldNotBeNull();
        serviceDto.Location.ServiceAddress.Street.ShouldNotBeNullOrWhiteSpace();
        serviceDto.Location.ServiceAddress.Postcode.ShouldBeGreaterThan(1000);
    }

    [Fact]
    public async Task GetIndexAsync_MapOpeningHoursCorrectly_WhenOpeningHoursExist()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_MapOpeningHoursCorrectly_WhenOpeningHoursExist));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 10,
            SearchTerm = "Studentensecretariaat"
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);
        var serviceDto = result.Value.Services.First();

        result.IsSuccess.ShouldBeTrue();
        serviceDto.OpeningHours.ShouldNotBeEmpty();
        var firstPeriod = serviceDto.OpeningHours.First();
        firstPeriod.ContactDate.ShouldNotBe(default);
        firstPeriod.ContactHours.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetIndexAsync_MapCommunicationChannelsCorrectly_WhenChannelsExist()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_MapCommunicationChannelsCorrectly_WhenChannelsExist));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 10,
            SearchTerm = "Studentensecretariaat"
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);
        var serviceDto = result.Value.Services.First();

        result.IsSuccess.ShouldBeTrue();
        serviceDto.CommunicationChannels.ShouldNotBeEmpty();
        var firstChannel = serviceDto.CommunicationChannels.First();
        firstChannel.Name.ShouldNotBeNullOrWhiteSpace();
        firstChannel.Link.ShouldNotBeNullOrWhiteSpace();
        firstChannel.TypeOfCommunication.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetIndexAsync_MapRemarksCorrectly_WhenRemarksExist()
    {
        var options = GetDbContextOptions(nameof(GetIndexAsync_MapRemarksCorrectly_WhenRemarksExist));
        using var dbContext = new ApplicationDbContext(options);
        await SeedDbAsync(dbContext);

        var service = new ContactService(dbContext);
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 10,
            SearchTerm = "Studentensecretariaat"
        };

        var result = await service.GetIndexAsync(request, CancellationToken.None);
        var serviceDto = result.Value.Services.First();

        result.IsSuccess.ShouldBeTrue();
        serviceDto.Remarks.ShouldNotBeEmpty();
        serviceDto.Remarks.First().ShouldNotBeNullOrWhiteSpace();
    }

    private DbContextOptions<ApplicationDbContext> GetDbContextOptions(string dbName)
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    private async Task SeedDbAsync(ApplicationDbContext dbContext)
    {
        var service1 = new Service(
            "Studentensecretariaat Schoonmeersen",
            new ServiceCategory("Administratief")
        );
        service1.DescribeService("Het studentensecretariaat helpt je met administratieve vragen.");
        var address1 = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location1 = new ServiceLocation(address1, "Schoonmeersen");
        service1.ChangeLocation(location1);
        service1.AddRemark("Gesloten tijdens examens");
        service1.AddCommunicationChannel(new CommunicationChannel("E-mail", "mailto:test@hogent.be", CommunicationTypes.Email));
        service1.AddCommunicationChannel(new CommunicationChannel("Telefoon", "tel:09 123 45 67", CommunicationTypes.Phone));

        var openingHours1 = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 6), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(9, 0), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(17, 0))
            })
        };
        service1.ChangeOpeningsHours(openingHours1);

        var service2 = new Service(
            "Bibliotheek",
            new ServiceCategory("Ondersteunend")
        );
        service2.DescribeService("De bibliotheek biedt studieplekken en materialen.");

        var service3 = new Service(
            "iBaMaFlex!",
            new ServiceCategory("Overige")
        );
        service3.DescribeService("Online platform voor flexibel leren.");

        dbContext.Services.AddRange(service1, service2, service3);
        await dbContext.SaveChangesAsync();
    }
}

