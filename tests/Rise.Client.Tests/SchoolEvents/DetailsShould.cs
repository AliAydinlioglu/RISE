using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Client.Theme;
using Rise.Client.Theme.Fakers;
using Rise.Shared.SchoolEvents;
using Rise.TestDoubles;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.SchoolEvents;

public class DetailsShould : TestContext
{
    private const int ValidId = 1;
    
    public DetailsShould(ITestOutputHelper output)
    {
        Services.AddXunitLogger(output);
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        Services.AddScoped<IPageTitleService>(_ => new FakePageTitleService());
        Services.AddScoped<NavigationManager, FakeNavigationManager>();
        Services.AddMudServices();
        Services.AddScoped<IThemingService, FakeThemingService>();
    }
    
    [Fact]
    public void ShowError_WhenIdIsInvalid()
    {
        // arrange
        var nav = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;
        
        RenderComponent<Details>(p => p.Add(x => x.Id,-1));

        // assert
        nav!.Uri.ShouldEndWith("/notfound");
    }

    [Fact]
    public void ShowSchoolEventDetails_WhenEventIsLoadedSuccessfully()
    {
        // arrange
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        var schoolEventService = Services.GetService<ISchoolEventService>() as FakeSchoolEventService;
        schoolEventService!.SetSchoolEventForDetails(SchoolEventTestDataFactory.CreateDefaultSchoolEvent());
        var evt = schoolEventService!.GetSchoolEventForDetails();
        var cut = RenderComponent<Details>(p => p.Add(x => x.Id, ValidId));

        // assert
        var container = cut.Find("[data-bunit='se-detail']");
        container.ToMarkup().ShouldContain(evt!.Title);
        container.ToMarkup().ShouldContain(evt!.Category);
        container.ToMarkup().ShouldContain(evt!.Location!.Name!);
        container.ToMarkup().ShouldContain(evt!.Description!);
    }

    [Fact]
    public void ShowGratis_WhenEventIsFree()
    {
        // arrange
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        var schoolEventService = Services.GetService<ISchoolEventService>() as FakeSchoolEventService;
        var location = SchoolEventTestDataFactory.CreateDefaultLocation();
        schoolEventService!.SetSchoolEventForDetails(SchoolEventTestDataFactory.CreateSchoolEvent(
            "test", 
            "test beschrijving", 
            DateTimeOffset.Now, 
            location,
            0));
        
        var cut = RenderComponent<Details>(p => p.Add(x => x.Id, ValidId));

        // assert
        cut.Find("[data-bunit='se-detail-price']").InnerHtml.ShouldContain("Gratis");
    }

    [Fact]
    public void NavigateToRegisterLink_WhenRegisterButtonClicked()
    {
        // Arrange
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        var schoolEventService = Services.GetService<ISchoolEventService>() as FakeSchoolEventService;
        schoolEventService!.SetSchoolEventForDetails(SchoolEventTestDataFactory.CreateDefaultSchoolEvent());
        var evt = schoolEventService!.GetSchoolEventForDetails();
        var nav = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;

        var cut = RenderComponent<Details>(p => p.Add(x => x.Id, ValidId));

        cut.WaitForAssertion(() =>
        {
            var buttons = cut.FindAll("button");
            buttons.ShouldNotBeEmpty();

            // Act
            buttons.Last().Click();
        });

        // Assert
        nav!.Uri.ShouldContain(evt.RegisterLink);
    }
}
