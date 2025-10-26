using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Components.Dropdown;
using Shouldly;

namespace Rise.Client.Components.DropdownComponent;

public class GivenANavigationDropdown : GivenADropdownSetupTest<RiseNavigationDropdown<string>, string>
{
    [Fact]
    public void WhenActivatorContentIsProvided_ThenCustomActivatorWithAvatarAndIconShouldRender()
    {
        var items = GetDefaultItems();

        var cut = RenderDropdownWithItems(items, parameters => parameters
            .Add(p => p.ActivatorContent, (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "flex items-center gap-2 cursor-pointer hover:opacity-80 transition");

                builder.OpenComponent<MudAvatar>(2);
                builder.AddAttribute(3, "Size", Size.Medium);
                builder.AddAttribute(4, "Color", Color.Primary);
                builder.AddContent(5, "AW");
                builder.CloseComponent();

                builder.OpenComponent<MudIcon>(6);
                builder.AddAttribute(7, "Icon", Icons.Material.Filled.ArrowDropDown);
                builder.AddAttribute(8, "Size", Size.Small);
                builder.CloseComponent();

                builder.CloseElement();
            }))
        );

        cut.FindComponents<MudAvatar>().ShouldNotBeEmpty();
        cut.FindComponents<MudIcon>().ShouldNotBeEmpty();
        cut.Markup.ShouldNotContain("rise-dropdown-label");
    }

    private static readonly List<string> DefaultPages = new() { "Home", "About", "Contact" };
    protected override List<string> GetDefaultItems() => DefaultPages;
    protected override Func<string, string> GetDefaultItemTextFunc() => page => page;
}