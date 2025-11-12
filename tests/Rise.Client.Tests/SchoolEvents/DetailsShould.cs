using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Shared.SchoolEvents;
using Rise.TestDoubles;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.SchoolEvents;

public class DetailsShould : TestContext
{
    public DetailsShould(ITestOutputHelper output)
    {
        Services.AddXunitLogger(output);
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        Services.AddScoped<IPageTitleService>(_ => new FakePageTitleService());
        Services.AddScoped<NavigationManager, FakeNavigationManager>();
        Services.AddMudServices();
    }

    [Fact]
    public void ShowError_WhenIdIsNullOrEmpty()
    {
        // arrange
        var cut = RenderComponent<Details>(p => p.Add(x => x.Id, null));

        // assert
        cut.Markup.ShouldContain("Er liep iets fout bij het ophalen van het evenement.");
    }
    
    [Fact]
    public void ShowError_WhenIdIsInvalid()
    {
        // arrange
        var cut = RenderComponent<Details>(p => p.Add(x => x.Id,"abc"));

        // assert
        cut.Markup.ShouldContain("Er liep iets fout bij het ophalen van het evenement.");
    }

    [Fact]
    public void ShowSchoolEventDetails_WhenEventIsLoadedSuccessfully()
    {
        // arrange
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        var schoolEventService = Services.GetService<ISchoolEventService>() as FakeSchoolEventService;
        schoolEventService!.SetSchoolEventForDetails(SchoolEventTestDataFactory.CreateDefaultSchoolEvent());
        var evt = schoolEventService!.GetSchoolEventForDetails();
        var cut = RenderComponent<Details>(p => p.Add(x => x.Id, evt!.Id.ToString()));

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
        schoolEventService!.SetSchoolEventForDetails(SchoolEventTestDataFactory.CreateDefaultSchoolEvent());
        var location = SchoolEventTestDataFactory.CreateDefaultLocation();
        schoolEventService.SetSchoolEventForDetails(SchoolEventTestDataFactory.CreateSchoolEvent(
            "test", 
            "test beschrijving", 
            DateTimeOffset.Now, 
            location,
            0));
        
        var evt = schoolEventService!.GetSchoolEventForDetails();
        var cut = RenderComponent<Details>(p => p.Add(x => x.Id, evt!.Id.ToString()));

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

        var cut = RenderComponent<Details>(p => p.Add(x => x.Id, evt.Id.ToString()));

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
