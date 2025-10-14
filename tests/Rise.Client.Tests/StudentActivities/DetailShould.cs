using Rise.Client.StudentActivities;
using Rise.Shared.StudentActivities;
using Shouldly;
using Xunit.Abstractions;

namespace Xunit.StudentActivities;

public class DetailShould: TestContext
{
    public DetailShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IStudentActivityService, FakeStudentActivitiesService>(); 
    }

    [Fact]
    public void ShowsStudentActivities()
    {

		
        var cut = RenderComponent<Detail>(ComponentParameter.CreateParameter("Id","1"));
        
        // Assert: organisator (first p inside the organisator field)
        var organisatorText = cut.Find("[data-bunit='sa-detail-organisator'] p").TextContent.Trim();
        organisatorText.ShouldBe("Club A");

        // Assert: location name and address (two p elements under location)
        var locationPs = cut.FindAll("[data-bunit='sa-detail-location'] p");
        locationPs[0].TextContent.Trim().ShouldBe("Test Location");
        locationPs[1].TextContent.Trim().ShouldBe("Test street 42 B, 1234 Test City");

        // Assert: description text
        var descriptionText = cut.Find("[data-bunit='sa-detail-description'] .content p").TextContent.Trim();
        descriptionText.ShouldBe("DescTest1");
    }
}