using Rise.Client.Components.Card;
using Rise.Client.Theme;
using Rise.Client.Theme.Fakers;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.Components.CardComponent;

public class GivenACard : TestContext
{
    public GivenACard(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IThemingService, FakeThemingService>();
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

        cut.Find(".title-label").TextContent.ShouldBe(title);
        cut.Find(".description-label").TextContent.ShouldBe(description ?? "");
        cut.Find(".header-label").TextContent.ShouldBe(cardHeader ?? "");
    }

    [Theory]
    [MemberData(nameof(CardTestData.DatesData), MemberType = typeof(CardTestData))]

    public void WhenRenderingTheCard_ThenTheDateShouldBeShownCorrectly(DateTime date, string expectedDay, string expectedMonth)
    {
        var cut = RenderComponentCardComponent(dateInput: date);

        cut.Find(".day-of-month-label").TextContent.ShouldBe(expectedDay);
        cut.Find(".month-abbr-label").TextContent.ShouldBe(expectedMonth);
    }
    
    [Fact]
    public void WhenRenderingTheCard_ThenMonthShouldNotEndWithPoint()
    {
        DateTime date = new DateTime(2024, 11, 13);

        var cut = RenderComponentCardComponent(dateInput: date);

        cut.Find(".month-abbr-label").TextContent.ShouldNotContain(".");
    }

    [Theory]
    [MemberData(nameof(CardTestData.BackgroundTitleData), MemberType = typeof(CardTestData))]
    public void WhenRenderingTheCard_ThenTheDateBackgroundShouldBeGeneratedBasedOnTitleUppercaseLetters(string actual, string expected)
    {
        var cut = RenderComponentCardComponent(title: actual);

        cut.Find(".stacking-container-background p").TextContent.ShouldBe(expected);
    }

    private IRenderedComponent<RiseCard> RenderComponentCardComponent(
        string? title = "Test Course",
        string? description = "B. Alice",
        string? cardHeader = "14:00 | GSCHB.1.001",
        DateTime? dateInput = null
    )
    {
        return RenderComponent<RiseCard>(parameters =>
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