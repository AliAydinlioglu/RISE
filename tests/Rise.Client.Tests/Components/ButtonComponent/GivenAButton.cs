using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shouldly;

namespace Rise.Client.Components.ButtonComponent;

public class GivenAButton : TestContext
{
    [Fact]
    public void WhenButtonIsRenderedWithoutOptionalParams_ThenDefaultParamsShouldBeSet()
    {
        var cut = RenderComponent<RiseButton>(parameters => parameters
            .Add(param => param.OnClick, EventCallback.Empty)
            .AddChildContent("Rise Button")
        );
        
        var mudButton = cut.FindComponent<MudButton>();
        
        mudButton.Instance.Disabled.ShouldBe(false);
        mudButton.Instance.Variant.ShouldBe(Variant.Filled);
        mudButton.Instance.Color.ShouldBe(Color.Primary);
        
        mudButton.Markup.ShouldContain("px-6 py-3"); // Size aftesten
    }

    [Theory]
    [InlineData(RiseButton.RiseButtonType.Primary, Variant.Filled)]
    [InlineData(RiseButton.RiseButtonType.Secondary, Variant.Outlined)]
    public void WhenButtonHasSpecificButtonType_ThenItShouldBeMappedToTheCorrectVariant(
        RiseButton.RiseButtonType type,
        Variant expected
        )
    {
        var cut = RenderComponent<RiseButton>(parameters => parameters
            .Add(param => param.OnClick, EventCallback.Empty)
            .Add(param => param.Type, type)
            .AddChildContent("Rise Button")
        );
        
        var mudButton = cut.FindComponent<MudButton>();
        
        mudButton.Instance.Variant.ShouldBe(expected);
    }

    [Theory]
    [InlineData(RiseButton.RiseButtonSize.Small, "px-4 py-2")]
    [InlineData(RiseButton.RiseButtonSize.Medium, "px-6 py-3")]
    [InlineData(RiseButton.RiseButtonSize.Large, "px-10 py-4")]
    public void WhenButtonHasSpecificButtonSize_ThenItShouldBeMappedToTheCorrectSize(
        RiseButton.RiseButtonSize size,
        string expected
    )
    {
        var cut = RenderComponent<RiseButton>(parameters => parameters
            .Add(param => param.OnClick, EventCallback.Empty)
            .Add(param => param.Size, size)
            .AddChildContent("Rise Button")
        );
        
        var mudButton = cut.FindComponent<MudButton>();
        
        mudButton.Markup.ShouldContain(expected);
    }
}