using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Rise.Client.Components.Docs;

public partial class Index
{
    private int CurrentCarouselIndex = 0;
    private List<RenderFragment> CarouselItems = new();

    protected override void OnInitialized()
    {
        var slides = new[]
        {
            ("Slide 1", "#667eea 0%, #764ba2 100%"),
            ("Slide 2", "#f093fb 0%, #f5576c 100%"),
            ("Slide 3", "#4facfe 0%, #00f2fe 100%"),
            ("Slide 4", "#43e97b 0%, #38f9d7 100%")
        };

        CarouselItems = slides.Select(slide => CreateSlide(slide.Item1, slide.Item2)).ToList();
    }

    private RenderFragment CreateSlide(string title, string gradient)
    {
        return builder =>
        {
            var style =
                $"background: linear-gradient(135deg, {gradient}); color: var(—mud-palette-text-secondary); text-align: center; min-height: 300px; display: flex; align-items: center; justify-content: center;";
            builder.OpenComponent<MudPaper>(0);
            builder.AddAttribute(1, "Class", "pa-6");
            builder.AddAttribute(2, "Style", style);
            builder.AddMarkupContent(3,
                $"<div><h4 class=\"mud-typography mud-typography-h4\">{title}</h4><p class=\"mud-typography mud-typography-subtitle1 mt-2\">Dit is de {(title == "Slide 1" ? "eerste" : title == "Slide 2" ? "tweede" : title == "Slide 3" ? "derde" : "vierde")} slide</p></div>");
            builder.CloseComponent();
        };
    }

    public class DummyModel
    {
    }
}