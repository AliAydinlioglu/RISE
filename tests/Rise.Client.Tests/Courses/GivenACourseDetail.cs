using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Components;
using Rise.Client.Components.Card;
using Rise.Client.Courses.Components;
using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Client.Theme;
using Rise.Client.Theme.Fakers;
using Rise.Shared.Courses;
using Shouldly;

namespace Rise.Client.Courses;

public class GivenACourseDetail : MudBlazorTestSetup
{
    private const int TestCourseId = 1;
    private readonly DateOnly _testDate = new(2024, 11, 13);

    public GivenACourseDetail()
    {
        var pageTitleService = new FakePageTitleService();
        Services.AddScoped<IPageTitleService>(_ => pageTitleService);
        Services.AddScoped<IThemingService, FakeThemingService>();
    }

    [Fact(DisplayName = "When rendering course detail but data not yet fetched, then loader should be shown")]
    public void LoaderTest()
    {
        var cut = RenderedComponent(isLoading: true);

        cut.FindComponent<RiseLoader>().ShouldNotBeNull();
    }

    [Fact(DisplayName = "When rendering course detail, then metadata should be shown")]
    public void MetadataTest()
    {
        var cut = RenderedComponent();

        var metaDataLabels = cut.FindComponents<MetaDataLabel>();
        
        metaDataLabels.Count.ShouldBe(4);
        metaDataLabels.Any(l => l.Instance.LabelText == "Datum").ShouldBeTrue();
        metaDataLabels.Any(l => l.Instance.LabelText == "Uur").ShouldBeTrue();
        metaDataLabels.Any(l => l.Instance.LabelText == "Locatie").ShouldBeTrue();
        metaDataLabels.Any(l => l.Instance.LabelText == "Docent").ShouldBeTrue();

    }

    [Fact(DisplayName = "When rendering course detail, then date and hour should share same row")]
    public void HourAndDateTest()
    {
        var cut = RenderedComponent();

        var generalInfo = cut.FindComponent<CourseDetailGeneralInfo>();
        var gridItems = generalInfo.FindAll(".mud-grid-item");

        gridItems[0].ClassList.ShouldContain("mud-grid-item");
        gridItems[1].ClassList.ShouldContain("mud-grid-item");
    }
    
    [Fact(DisplayName = "When course has no announcements, then notification should be shown")]
    public void NoAnnouncementsTest()
    {
        var cut = RenderedComponent(hasAnnouncements: false);

        var notification = cut.FindComponent<RiseNotification>();
        notification.ShouldNotBeNull();
        notification.Instance.Title.ShouldBe("Geen vaknieuws beschikbaar");
        notification.Instance.Severity.ShouldBe(RiseNotification.NotificationSeverity.Info);
    }
    
    [Fact(DisplayName = "When course has announcements, then each should display title, sender and message")]
    public void AnnouncementContentTest()
    {
        var cut = RenderedComponent();

        var papers = cut.FindComponents<MudPaper>();
        var firstPaper = papers.ElementAt(1);

        var title = firstPaper.Find(".mud-typography-h4");
        title.ShouldNotBeNull();
        title.TextContent.ShouldNotBeNullOrEmpty();

        var sender = firstPaper.Find(".mud-typography-caption");
        sender.ShouldNotBeNull();
        sender.TextContent.ShouldNotBeNullOrEmpty();

        var divider = firstPaper.FindComponent<MudDivider>();
        divider.ShouldNotBeNull();

        var message = firstPaper.Find(".mud-typography-body2");
        message.ShouldNotBeNull();
        message.TextContent.ShouldNotBeNullOrEmpty();
    }
    
    [Fact(DisplayName = "When rendering course detail, then headers should be shown")]
    public void DeadlinesHeadingTest()
    {
        var cut = RenderedComponent();

        var headings = cut.FindAll("h2");
        
        headings.First().TextContent.ShouldContain("Lesinfo");
        headings.ElementAt(1).TextContent.ShouldContain("Vaknieuws");
        headings.ElementAt(2).TextContent.ShouldContain("Deadlines");
    }
    
    [Fact(DisplayName = "When course has no deadlines, then notification should be shown")]
    public void NoDeadlinesTest()
    {
        var cut = RenderedComponent(hasDeadlines: false);

        var notification = cut.FindComponent<RiseNotification>();
        notification.ShouldNotBeNull();
        notification.Instance.Title.ShouldBe("Geen deadlines beschikbaar");
        notification.Instance.Severity.ShouldBe(RiseNotification.NotificationSeverity.Info);
    }
    
    [Fact(DisplayName = "When course has deadlines, cards should be rendered")]
    public void DeadlineContentTest()
    {
        var cut = RenderedComponent();

        var card = cut.FindComponent<RiseCard>();
        
        card.ShouldNotBeNull();
    }
    
    [Fact(DisplayName = "When course no announcements nor deadlines, then two notifications should be shown")]
    public void NotificationsTest()
    {
        var cut = RenderedComponent(hasAnnouncements: false, hasDeadlines: false);

        var notifications = cut.FindComponents<RiseNotification>();
        
        notifications.Count.ShouldBe(2);
        notifications[0].Instance.Title.ShouldBe("Geen vaknieuws beschikbaar");
        notifications[1].Instance.Title.ShouldBe("Geen deadlines beschikbaar");
    }

     private IRenderedComponent<Detail> RenderedComponent(
         bool isLoading = false, 
         bool hasAnnouncements = true, 
         bool hasDeadlines = true)
    {
        var fakeCourseService = new FakeCourseService(isLoading, hasAnnouncements, hasDeadlines);
        Services.AddScoped<ICourseService>(_ => fakeCourseService);

        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        var uri = navigationManager.GetUriWithQueryParameter("datum", _testDate);
        navigationManager.NavigateTo(uri);

        return RenderComponent<Detail>(parameters => parameters
            .Add(param => param.CourseId, TestCourseId.ToString())
        );
    }
}