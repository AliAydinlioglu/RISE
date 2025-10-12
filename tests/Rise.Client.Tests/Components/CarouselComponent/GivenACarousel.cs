
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.Components.CarouselComponent;

public class GivenACarousel : TestContext
{
    public GivenACarousel(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
    }

    [Fact]
    public void WhenRendering_ThenAllItemsShouldBeDisplayed()
    {
        var items = CreateRenderFragments("Slide 1", "Slide 2", "Slide 3");
        
        var cut = CreateCarousel(items);

        cut.FindAll("div").ShouldNotBeEmpty();
        cut.Markup.ShouldContain("Slide 1");
        cut.Markup.ShouldContain("Slide 2");
        cut.Markup.ShouldContain("Slide 3");
    }

    [Fact]
    public async Task WhenSwipingLeft_ThenIndexShouldIncrement()
    {
        var items = CreateRenderFragments("Slide 1", "Slide 2", "Slide 3");
        
        var cut = CreateCarousel(items);
        var container = cut.Find(".carousel-container");

        await container.TriggerEventAsync("ontouchstart", CreateTouchEvent(100));
        await container.TriggerEventAsync("ontouchmove", CreateTouchEvent(20));
        await container.TriggerEventAsync("ontouchend", new TouchEventArgs());

        cut.Instance.CurrentIndex.ShouldBe(1);
    }

    [Fact]
    public async Task WhenSwipingRight_ThenIndexShouldDecrement()
    {
        var items = CreateRenderFragments("Slide 1", "Slide 2", "Slide 3");

        var cut = CreateCarousel(items, currentIndex: 1);
        var container = cut.Find(".carousel-container");

        await container.TriggerEventAsync("ontouchstart", CreateTouchEvent(20));
        await container.TriggerEventAsync("ontouchmove", CreateTouchEvent(120));
        await container.TriggerEventAsync("ontouchend", new TouchEventArgs());

        cut.Instance.CurrentIndex.ShouldBe(0);
    }

    private IRenderedComponent<Carousel> CreateCarousel(
        List<RenderFragment> items,
        int? currentIndex = null)
    {
        return RenderComponent<Carousel>(parameters =>
        {
            parameters.Add(p => p.Items, items);
            if (currentIndex.HasValue)
                parameters.Add(p => p.CurrentIndex, currentIndex.Value);
        });
    }

    private static List<RenderFragment> CreateRenderFragments(params string[] contents) =>
        contents.Select(text => (RenderFragment)(builder =>
        {
            builder.AddContent(0, text);
        })).ToList();
    
    private static TouchEventArgs CreateTouchEvent(double clientX) =>
        new()
        {
            Touches = new[]
            {
                new TouchPoint
                {
                    ClientX = clientX,
                    ClientY = 0
                }
            }
        };
}
