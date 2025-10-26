using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Shared;
using Rise.Shared.StudentActivities;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.StudentActivities;

public class IndexShould: TestContext
{
    public IndexShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IStudentActivityService, FakeStudentActivitiesService>();
        
        var pageTitleService = new FakePageTitleService();
        Services.AddScoped<IPageTitleService>(_ => pageTitleService);
    }

    [Fact]
    public void ShowsStudentActivities()
    {
        var cut = RenderComponent<Index>();
        cut.FindAll("[data-bunit='sa-index']").Count.ShouldBe(5);
    }
}