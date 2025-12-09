using Rise.Client.Components;
using Rise.Client.Faker;
using Rise.Client.Restaurant.Components;
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

public class PricelistShould : MudBlazorTestSetup
{
    public PricelistShould(ITestOutputHelper output)
    {
        CultureInfo.CurrentCulture = new CultureInfo("nl-BE");
        CultureInfo.CurrentUICulture = new CultureInfo("nl-BE");
        
        Services.AddXunitLogger(output);
        Services.AddScoped<IPageTitleService, FakePageTitleService>();

        Services.AddScoped<IPriceListService, FakePriceListService>();
        Services.AddScoped<IRestaurantSelectionService, FakeRestaurantSelectionService>();
        Services.AddScoped<IRestoService, FakeRestoService>();
        Services.AddScoped<IFavouriteRestoService, FakeFavouriteRestoService>();
        Services.AddScoped<IThemingService, FakeThemingService>();
        Services.AddScoped<IUserPreferenceStateService, FakeUserPreferenceStateService>();
    }

    [Fact]
    public void ShowNoPricelistMessage_WhenCategoriesAreNull()
    {
        // arrange
        FakePriceListService.Mode = FakePriceListService.ModeType.SuccessWithNullCategories;

        // act
        var cut = RenderComponent<Pricelist>();

        // assert
        cut.Find("p[data-bunit='no-pricelist-message']").TextContent
            .ShouldBe("Er zijn momenteel geen prijslijsten beschikbaar voor dit restaurant.");
    }

    [Fact]
    public void ShowNoPricelistMessage_WhenResultIsNotFound()
    {
        // arrange
        FakePriceListService.Mode = FakePriceListService.ModeType.NotFound;

        // act
        var cut = RenderComponent<Pricelist>();

        // assert
        cut.Find("p[data-bunit='no-pricelist-message']").TextContent
            .ShouldBe("Er zijn momenteel geen prijslijsten beschikbaar voor dit restaurant.");
    }

    [Fact]
    public void ShowErrorMessage_WhenRequestIsError()
    {
        // arrange
        FakePriceListService.Mode = FakePriceListService.ModeType.Error;

        // act
        var cut = RenderComponent<Pricelist>();

        // assert
        cut.Find("div[data-bunit='error-message'] p").TextContent
            .ShouldBe("Er ging iets mis met het ophalen van deze informatie! Probeer later opnieuw");
    }

    [Fact]
    public void ShowsPricelist_WhenDataIsAvailable()
    {
        // arrange
        FakePriceListService.Mode = FakePriceListService.ModeType.SuccessWithData;

        // act
        var cut = RenderComponent<Pricelist>();

        // assert
        cut.Find("div[data-bunit='pricelist-panels']").ShouldNotBeNull();
        
        var categoryNames = cut.FindAll("span[data-bunit='category-name']");
        categoryNames.Count.ShouldBe(2);
        categoryNames[0].TextContent.Trim().ShouldBe("Soepen");
        categoryNames[1].TextContent.Trim().ShouldBe("Hoofdgerechten");

        var soepenPanel = cut.FindAll(".mud-expand-panel-header").First();
        soepenPanel.Click();

        var items = cut.FindAll("td[data-bunit='item-name']");
        items[0].TextContent.Trim().ShouldContain("Tomatensoep");
        items[1].TextContent.Trim().ShouldContain("Kippensoep");
        
        var studentPrices = cut.FindAll("td[data-bunit='student-price']");
        studentPrices[0].TextContent.Trim().ShouldBe("2,50");
        studentPrices[1].TextContent.Trim().ShouldBe("2,75");
        
        var externalPrices = cut.FindAll("td[data-bunit='external-price']");
        externalPrices[0].TextContent.Trim().ShouldBe("3,00");
        externalPrices[1].TextContent.Trim().ShouldBe("3,25");
        
        var remark = cut.Find("td[data-bunit='category-remark']");
        remark.TextContent.ShouldContain("Prijs per kom");

        var hoofdgerechtPanel = cut.FindAll(".mud-expand-panel-header")[1];
        hoofdgerechtPanel.Click();

        var allItems = cut.FindAll("td[data-bunit='item-name']");
        allItems[2].TextContent.Trim().ShouldContain("Spaghetti Bolognese");
        allItems[3].TextContent.Trim().ShouldContain("Vegetarische Lasagne");
        
        var allStudentPrices = cut.FindAll("td[data-bunit='student-price']");
        allStudentPrices[2].TextContent.Trim().ShouldBe("5,50");
        allStudentPrices[3].TextContent.Trim().ShouldBe("6,00");
        
        var allExternalPrices = cut.FindAll("td[data-bunit='external-price']");
        allExternalPrices[2].TextContent.Trim().ShouldBe("7,00");
        allExternalPrices[3].TextContent.Trim().ShouldBe("7,50");

        cut.FindAll("div[data-bunit='error-message']").ShouldBeEmpty();
    }

    [Fact]
    public void ShowsTableHeaders_WhenDataIsAvailable()
    {
        // arrange
        FakePriceListService.Mode = FakePriceListService.ModeType.SuccessWithData;

        // act
        var cut = RenderComponent<Pricelist>();
        var firstPanel = cut.FindAll(".mud-expand-panel-header").First();
        firstPanel.Click();

        // assert
        cut.Find("th[data-bunit='student-header']").TextContent.Trim().ShouldBe("Studenten");
        cut.Find("th[data-bunit='external-header']").TextContent.Trim().ShouldBe("Externen");
    }

    [Fact]
    public void UpdatesPricelist_WhenRestaurantIsSelected()
    {
        // arrange
        FakePriceListService.Mode = FakePriceListService.ModeType.SuccessWithData;
        var cut = RenderComponent<Pricelist>();
        
        var restoButtonWrapper = cut.Find("div[data-bunit='restaurant-selector-button']");
        restoButtonWrapper.TextContent.Trim().ShouldContain("Test Resto");

        // act
        var restoButton = restoButtonWrapper.QuerySelector("button");
        restoButton!.Click();
        
        var restaurantOptions = cut.FindAll("div[data-bunit='restaurant-option']");
        var newRestoOption = restaurantOptions.First(div => div.TextContent.Contains("New Test Resto"));
        var newRestoButton = newRestoOption.QuerySelector("button");
        newRestoButton!.Click();

        // assert
        var restaurantSelectionService = Services.GetService<IRestaurantSelectionService>();
        var selectedResto = restaurantSelectionService!.GetSelectedRestoAsync().Result;
        selectedResto.Id.ShouldBe(2);
        selectedResto.Name.ShouldBe("New Test Resto");
        
        var updatedRestoButtonWrapper = cut.Find("div[data-bunit='restaurant-selector-button']");
        updatedRestoButtonWrapper.TextContent.Trim().ShouldContain("New Test Resto");
        
        var categoryNames = cut.FindAll("span[data-bunit='category-name']");
        categoryNames.Count.ShouldBe(2);
        categoryNames[0].TextContent.Trim().ShouldBe("Soepen");
        categoryNames[1].TextContent.Trim().ShouldBe("Hoofdgerechten");
    }
}
