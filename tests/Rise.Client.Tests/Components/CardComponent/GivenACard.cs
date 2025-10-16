using System.Globalization;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.Components.CardComponent;

public class GivenACard : TestContext
{
    public GivenACard(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
    }

    [Theory]
    [MemberData(nameof(CardTestData.CourseInfoData), MemberType = typeof(CardTestData))]
    public void WhenRenderingTheCard_ThenTheCourseInfoShouldBeRenderedCorrectly(string title, string? description, string? cardHeader)
    {
        var cut = RenderComponentCardComponent(
            title: title,
            description: description,
            cardHeader: cardHeader
        );

        cut.Find(".course-info .is-size-4").TextContent.ShouldBe(title);
        cut.Find(".course-info .is-size-6").TextContent.ShouldBe(description ?? "");
        cut.Find(".course-info .is-size-7").TextContent.ShouldBe(cardHeader ?? "");
    }

    [Theory]
    [MemberData(nameof(CardTestData.DatesData), MemberType = typeof(CardTestData))]

    public void WhenRenderingTheCard_ThenTheDateShouldBeShownCorrectly(DateTime date, string expectedDay, string expectedMonth)
    {
        var cut = RenderComponentCardComponent(dateInput: date);

        cut.Find(".inner-date-square .is-size-1").TextContent.ShouldBe(expectedDay);
        cut.Find(".inner-date-square .is-size-3").TextContent.ShouldBe(expectedMonth);
    }
    
    [Fact]
    public void WhenRenderingTheCard_ThenMonthShouldNotEndWithPoint()
    {
        DateTime date = new DateTime(2024, 11, 13);

        var cut = RenderComponentCardComponent(dateInput: date);

        cut.Find(".inner-date-square .is-size-3").TextContent.ShouldNotContain(".");

    }


    [Theory]
    [MemberData(nameof(CardTestData.BackgroundTitleData), MemberType = typeof(CardTestData))]
    public void WhenRenderingTheCard_ThenTheDateBackgroundShouldBeGeneratedBasedOnTitleUppercaseLetters(string actual, string expected)
    {
        var cut = RenderComponentCardComponent(title: actual);

        cut.Find(".stacking-container-background p").TextContent.ShouldBe(expected);
    }

    private IRenderedComponent<Card> RenderComponentCardComponent(
        string? title = "Test Course",
        string? description = "B. Alice",
        string? cardHeader = "14:00 | GSCHB.1.001",
        DateTime? dateInput = null
    )
    {
        return RenderComponent<Card>(parameters =>
        {
            var date = dateInput ?? new DateTime(2025, 11, 12);
            parameters
                .Add(param => param.Title, title)
                .Add(param => param.Description, description)
                .Add(param => param.CardHeader, cardHeader)
                .Add(param => param.Date, date);
        });
    }
}