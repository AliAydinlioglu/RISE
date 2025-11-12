using Microsoft.AspNetCore.Components;
using Rise.Client.Components;
using Rise.Client.Courses.Components;
using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Shared.Courses;
using Shouldly;

namespace Rise.Client.Courses;

public class GivenACourseDetail : TestContext
{
    private const int TestCourseId = 1;
    private readonly DateOnly _testDate = new(2024, 11, 13);

    public GivenACourseDetail()
    {
        var pageTitleService = new FakePageTitleService();
        Services.AddScoped<IPageTitleService>(_ => pageTitleService);
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

        var gridItems = cut.FindAll(".mud-grid-item");

        gridItems[0].ClassList.ShouldContain("mud-grid-item-xs-6");
        gridItems[1].ClassList.ShouldContain("mud-grid-item-xs-6");
    }

    private IRenderedComponent<Detail> RenderedComponent(bool isLoading = false)
    {
        var fakeCourseService = new FakeCourseService(isLoading);
        Services.AddScoped<ICourseService>(_ => fakeCourseService);

        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
        var uri = navigationManager.GetUriWithQueryParameter("datum", _testDate);
        navigationManager.NavigateTo(uri);

        return RenderComponent<Detail>(parameters => parameters
            .Add(param => param.CourseId, TestCourseId.ToString())
        );
    }
}