using Rise.Client.Products;
using Rise.Shared.Products;
using Rise.Shared.StudentActivities;
using Shouldly;
using Xunit.Abstractions;
using Index = Rise.Client.StudentActivities.Index;

namespace Xunit.StudentActivities;

public class IndexShould: TestContext
{
    public IndexShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IStudentActivityService, FakeStudentActivitiesService>(); 
    }

    [Fact]
    public void ShowsStudentActivities()
    {
        var cut = RenderComponent<Index>();
        cut.FindAll("[data-bunit='sa-index']").Count.ShouldBe(5);
    }
}