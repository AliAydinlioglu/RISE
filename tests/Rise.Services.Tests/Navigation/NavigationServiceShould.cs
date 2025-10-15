using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Navigation;
using Rise.Persistence;
using Rise.Persistence.Configurations.Identity;
using Rise.Services.Navigation;
using Rise.Shared.Identity;
using Rise.Shared.Navigation;

namespace Rise.Services.Tests.Navigation;

public class NavigationServiceShould
{
    [Theory]
    [InlineData(AppRoles.Public, ContentLocations.Header, 2)]
    [InlineData(AppRoles.Public, ContentLocations.Body, 3)]
    [InlineData(AppRoles.Public, ContentLocations.Footer, 1)]
    [InlineData(AppRoles.Regular, ContentLocations.Header, 4)]
    [InlineData(AppRoles.Regular, ContentLocations.Body, 1)]
    [InlineData(AppRoles.Regular, ContentLocations.Footer, 2)]
    [InlineData(AppRoles.Distance, ContentLocations.Header, 1)]
    [InlineData(AppRoles.Distance, ContentLocations.Body, 2)]
    [InlineData(AppRoles.Distance, ContentLocations.Footer, 3)]
    public async Task ReturnNavItems_WhenRoleAndContentLocationAreSpecific(string roleIdString, string contentLocation, int expectedCount)
    {
        // Arrange
        var options = GetDbContextOptions(nameof(ReturnNavItems_WhenRoleAndContentLocationAreSpecific));
        
        using var dbContext = new ApplicationDbContext(options);

        await SeedDbAsync(dbContext);
        
        var service = new NavigationService(dbContext);

        var request = new NavigationRequest.Get
        {
            ContentLocation = contentLocation
        };

        var roleId = new Guid(roleIdString);
        
        // Act
        var result = await service.GetAsync(request, roleId, CancellationToken.None);
        
        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.NavigationItems.Count().ShouldBe(expectedCount);
    }

    [Theory]
    [InlineData("d298458a-8635-443d-a78e-da0bae3dde0a", ContentLocations.Header)]
    [InlineData(AppRoles.Public, "Unknown")]
    public async Task ReturnEmptyList_WhenRoleOrContentLocationIsUnknown(string roleIdString, string contentLocation)
    {
        // Arrange
        var options = GetDbContextOptions(nameof(ReturnEmptyList_WhenRoleOrContentLocationIsUnknown));
        
        using var dbContext = new ApplicationDbContext(options);

        await SeedDbAsync(dbContext);
        
        var service = new NavigationService(dbContext);

        var request = new NavigationRequest.Get
        {
            ContentLocation = contentLocation
        };

        var roleId = new Guid(roleIdString);
        
        // Act
        var result = await service.GetAsync(request, roleId, CancellationToken.None);
        
        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.NavigationItems.Count().ShouldBe(0);
    }

    [Fact]
    public async Task ReturnItemsOrderedBySequenceNr()
    {
        // Arrange
        var options = GetDbContextOptions(nameof(ReturnItemsOrderedBySequenceNr));
        
        using var dbContext = new ApplicationDbContext(options);

        await SeedDbAsync(dbContext);
        
        var service = new NavigationService(dbContext);

        var request = new NavigationRequest.Get
        {
            ContentLocation = ContentLocations.Header
        };

        var roleId = new Guid(AppRoles.Regular);
        
        // Act
        var result = await service.GetAsync(request, roleId, CancellationToken.None);
        var items = result.Value.NavigationItems.ToList();
        
        // Assert
        result.IsSuccess.ShouldBeTrue();
        items.Select(i => i.Label).ShouldBe(new []{ "nav4", "nav2", "nav1", "nav3" });
    }
    
    [Fact]
    public async Task ReturnResultError_WhenRequestIsNull()
    {
        // Arrange
        var options = GetDbContextOptions(nameof(ReturnResultError_WhenRequestIsNull));
        
        using var dbContext = new ApplicationDbContext(options);
        
        var service = new NavigationService(dbContext);
        
        var roleId = new Guid(AppRoles.Public);

        // Act
        var result = await service.GetAsync(null!, roleId, CancellationToken.None);
        
        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ReturnResultError_WhenContentLocationIsNullOrWhitespace(string contentLocation)
    {
        // Arrange
        var options = GetDbContextOptions(nameof(ReturnResultError_WhenContentLocationIsNullOrWhitespace));
        
        using var dbContext = new ApplicationDbContext(options);
        
        var service = new NavigationService(dbContext);
        
        var roleId = new Guid(AppRoles.Public);
        
        var request = new NavigationRequest.Get
        {
            ContentLocation = contentLocation
        };

        // Act
        var result = await service.GetAsync(request, roleId, CancellationToken.None);
        
        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
    }
    
    [Fact]
    public async Task MapAllNavigationFieldsCorrectly()
    {
        // Arrange
        var options = GetDbContextOptions(nameof(MapAllNavigationFieldsCorrectly));
        
        using var dbContext = new ApplicationDbContext(options);
        
        await SeedDbAsync(dbContext);

        var service = new NavigationService(dbContext);
        
        var request = new NavigationRequest.Get { ContentLocation = ContentLocations.Header };
        
        var roleId = new Guid(AppRoles.Regular);

        // Act
        var result = await service.GetAsync(request, roleId, CancellationToken.None);
        var first = result.Value.NavigationItems.First();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        first.Label.ShouldNotBeNullOrWhiteSpace();
        first.Icon.ShouldStartWith("icon");
        first.Url.ShouldStartWith("url");
    }
    
    private DbContextOptions<ApplicationDbContext> GetDbContextOptions(string dbName)
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(dbName)) // Do NOT use InMemoryDatabase... it's not reliable. Use a real database and come up with a strategy to clean up the database between tests.
            .Options;
    }

    private async Task SeedDbAsync(ApplicationDbContext dbContext)
    {
        var identityRoles = new []
        {
            new ApplicationRole(AppRoles.Public, "Public"),
            new ApplicationRole(AppRoles.Regular, "Regular"),
            new ApplicationRole(AppRoles.Distance, "Distance"),
        };
        
        var roles = new[]
        {
            identityRoles[0].MapToDomain(),
            identityRoles[1].MapToDomain(),
            identityRoles[2].MapToDomain()
        };
        
        var contentLocations = new[]
        {
            new ContentLocation(1,ContentLocations.Header),
            new ContentLocation(2,ContentLocations.Body),
            new ContentLocation(3,ContentLocations.Footer)
        };

        var navigationItems = new[]
        {
            new NavigationItem(1,"nav1", "icon1", "url1"),
            new NavigationItem(2,"nav2", "icon2", "url2"),
            new NavigationItem(3,"nav3", "icon3", "url3"),
            new NavigationItem(4,"nav4", "icon4", "url4"),
            new NavigationItem(5,"nav5", "icon5", "url5"),
            new NavigationItem(6,"nav6", "icon6", "url6"),
            new NavigationItem(7,"nav7", "icon7", "url7"),
            new NavigationItem(8,"nav8", "icon8", "url8"),
            new NavigationItem(9,"nav9", "icon9", "url9")
        };

        var roleNavigationItemContentLocations = new[]
        {
            // public + header (2)
            new RoleNavigationItemContentLocation(roles[0], navigationItems[0], contentLocations[0], 1),
            new RoleNavigationItemContentLocation(roles[0], navigationItems[1], contentLocations[0], 2),
            // public + body (3)
            new RoleNavigationItemContentLocation(roles[0], navigationItems[4], contentLocations[1], 1),
            new RoleNavigationItemContentLocation(roles[0], navigationItems[5], contentLocations[1], 2),
            new RoleNavigationItemContentLocation(roles[0], navigationItems[6], contentLocations[1], 3),
            // public + footer (1)
            new RoleNavigationItemContentLocation(roles[0], navigationItems[8], contentLocations[2], 1),
            // regular + header (4)
            new RoleNavigationItemContentLocation(roles[1], navigationItems[0], contentLocations[0], 3),
            new RoleNavigationItemContentLocation(roles[1], navigationItems[1], contentLocations[0], 2),
            new RoleNavigationItemContentLocation(roles[1], navigationItems[2], contentLocations[0], 4),
            new RoleNavigationItemContentLocation(roles[1], navigationItems[3], contentLocations[0], 1),
            // regular + body (1)
            new RoleNavigationItemContentLocation(roles[1], navigationItems[4], contentLocations[1], 1),
            // regular + footer (2)
            new RoleNavigationItemContentLocation(roles[1], navigationItems[7], contentLocations[2], 1),
            new RoleNavigationItemContentLocation(roles[1], navigationItems[8], contentLocations[2], 2),
            // distance + header (1)
            new RoleNavigationItemContentLocation(roles[2], navigationItems[0], contentLocations[0], 1),
            // distance + body (2)
            new RoleNavigationItemContentLocation(roles[2], navigationItems[4], contentLocations[1], 1),
            new RoleNavigationItemContentLocation(roles[2], navigationItems[1], contentLocations[1], 2),
            // distance + footer (3)
            new RoleNavigationItemContentLocation(roles[2], navigationItems[8], contentLocations[2], 1),
            new RoleNavigationItemContentLocation(roles[2], navigationItems[6], contentLocations[2], 2),
            new RoleNavigationItemContentLocation(roles[2], navigationItems[2], contentLocations[2], 3),
        };

        if (!dbContext.Roles.Any())
            await dbContext.Roles.AddRangeAsync(identityRoles);
        
        if(!dbContext.ContentLocations.Any())
            await dbContext.ContentLocations.AddRangeAsync(contentLocations);
        
        if (!dbContext.NavigationItems.Any())
            await dbContext.NavigationItems.AddRangeAsync(navigationItems);

        if (!dbContext.RoleNavigationItems.Any())
            await dbContext.RoleNavigationItems.AddRangeAsync(roleNavigationItemContentLocations);
        
        await dbContext.SaveChangesAsync();
    }
}