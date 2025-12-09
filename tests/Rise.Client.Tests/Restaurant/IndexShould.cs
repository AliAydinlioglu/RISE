using Rise.Client.Components;
using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Client.Theme;
using Rise.Client.Theme.Fakers;
using Rise.Client.UserPreferences.Services;
using Rise.Shared;
using Rise.Shared.Menu;
using Shouldly;
using System.Globalization;
using Xunit.Abstractions;

namespace Rise.Client.Restaurant;

public class IndexShould : MudBlazorTestSetup
{
    public IndexShould(ITestOutputHelper output)
    {
        CultureInfo.CurrentCulture = new CultureInfo("nl-BE");
        CultureInfo.CurrentUICulture = new CultureInfo("nl-BE");
        
        Services.AddXunitLogger(output);
        Services.AddScoped<IPageTitleService, FakePageTitleService>();

        Services.AddScoped<IMenuService, FakeRestaurantService>();
        Services.AddScoped<IRestaurantSelectionService, FakeRestaurantSelectionService>();
        Services.AddScoped<IRestoService, FakeRestoService>();
        Services.AddScoped<IFavouriteRestoService, FakeFavouriteRestoService>();
        Services.AddScoped<IDateTimeService,Rise.Client.Faker.FakeDateTimeService>();
        Services.AddScoped<IThemingService, FakeThemingService>();
        Services.AddScoped<IUserPreferenceStateService, FakeUserPreferenceStateService>();
    }


    [Fact]
    public void ShowNoMenuInfoMessage_WhenMenuItemsAreNull()
    {
        // arrange
        FakeRestaurantService.Mode = FakeRestaurantService.ModeType.SuccessWithNullMenuItems;

        // act
        var cut = RenderComponent<Index>();

        // assert
        cut.Find("p[data-bunit='no-items-message']").TextContent
            .ShouldBe("Nog geen menu beschikbaar voor deze datum. Kom later nog eens terug!");
    }

    [Fact]
    public void ShowNoMenuInfoMessage_WhenResultIsNotFound()
    {
        // arrange
        FakeRestaurantService.Mode = FakeRestaurantService.ModeType.NotFound;

        // act
        var cut = RenderComponent<Index>();

        // assert
        cut.Find("p[data-bunit='no-items-message']").TextContent
            .ShouldBe("Nog geen menu beschikbaar voor deze datum. Kom later nog eens terug!");
    }


    [Fact]
    public void ShowErrorMessage_WhenRequestIsError()
    {
        // arrange
        FakeRestaurantService.Mode = FakeRestaurantService.ModeType.Error;

        // act
        var cut = RenderComponent<Index>();

        // assert
        cut.Find("div[data-bunit='error-message'] p").TextContent
            .ShouldBe("Er ging iets mis met het ophalen van deze informatie! Probeer later opnieuw");
    }
    
    [Fact]
    public void ShowsMenu_WhenCorrect()
    {
        // arrange
        FakeRestaurantService.Mode = FakeRestaurantService.ModeType.SuccessWithData;

        // act
        var cut = RenderComponent<Index>();

        // assert
        cut.Find("div[data-bunit='menu-container']").ShouldNotBeNull();
        
        var categoryNames = cut.FindAll("p[data-bunit='category-name']");
        categoryNames.Count.ShouldBe(2);
        categoryNames[0].TextContent.Trim().ShouldBe("Soepen");
        categoryNames[1].TextContent.Trim().ShouldBe("Hoofdgerechten");
        
        var menuItemNames = cut.FindAll("p[data-bunit='menu-item-name']");
        menuItemNames[0].TextContent.Trim().ShouldBe("test soep 1");
        menuItemNames[1].TextContent.Trim().ShouldBe("test soep 2");
        menuItemNames[2].TextContent.Trim().ShouldBe("Hoofdgerecht 1");
        menuItemNames[3].TextContent.Trim().ShouldBe("Hoofdgerecht 2");
        
        cut.FindAll("p[data-bunit='no-items-message']").ShouldBeEmpty();
        cut.FindAll("div[data-bunit='error-message']").ShouldBeEmpty();
    }

    [Fact]
    public void UpdatesMenu_WhenRestaurantIsSelected()
    {
        // arrange
        FakeRestaurantService.Mode = FakeRestaurantService.ModeType.SuccessWithData;
        var cut = RenderComponent<Index>();
        
        var restoButtonWrapper = cut.Find("span[data-bunit='restaurant-selector-button']");
        restoButtonWrapper.TextContent.Trim().ShouldContain("Test Resto");
        
        // act
        var restoButton = restoButtonWrapper.QuerySelector("button");
        restoButton!.Click();
        
        var restaurantOptions = cut.FindAll("div[data-bunit='restaurant-option']");
        var newRestoOption = restaurantOptions.First(div => div.TextContent.Contains("New Test Resto"));
        var newRestoButton = newRestoOption.QuerySelector("button");
        newRestoButton!.Click();

        // assert
        var preferenceService = Services.GetService<IUserPreferenceStateService>();
        preferenceService!.FavoriteResto.ShouldBe(2);
        
        var updatedRestoButtonWrapper = cut.Find("span[data-bunit='restaurant-selector-button']");
        updatedRestoButtonWrapper.TextContent.Trim().ShouldContain("New Test Resto");
        
        var categoryNames = cut.FindAll("p[data-bunit='category-name']");
        categoryNames.Count.ShouldBe(2);
        categoryNames[0].TextContent.Trim().ShouldBe("Soepen");
        categoryNames[1].TextContent.Trim().ShouldBe("Hoofdgerechten");
    }

   
}