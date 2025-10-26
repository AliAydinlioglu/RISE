using MudBlazor;
using Rise.Client.Components.Dropdown;
using Shouldly;

namespace Rise.Client.Components.DropdownComponent;

public abstract class GivenADropdownSetupTest<TRiseDropdownComponent, TItem> : MudBlazorTestSetup
    where TRiseDropdownComponent : RiseDropdownBase<TItem>
{
    [Fact]
    public void WhenDropdownIsRendered_ThenAllItemsShouldBeInTheMenu()
    {
        var items = GetDefaultItems();
        var cut = RenderDropdownWithItems(items);

        var instance = cut.Instance;
        instance.Items.Count().ShouldBe(items.Count);
    }
    
    [Fact]
    public void WhenPlaceholderIsNotProvided_ThenDefaultPlaceholderShouldBeDisplayed()
    {
        var cut = RenderDropdownWithItems(GetDefaultItems());

        cut.Markup.ShouldContain("Selecteer...");
    }


    [Fact]
    public void WhenPlacecholderIsProvided_ThenItShouldBeDisplayed()
    {
        const string customPlacecholder = "Navigatie Menu";

        var cut = RenderDropdownWithItems(GetDefaultItems(), parameters => parameters
            .Add(param => param.Placeholder, customPlacecholder)
        );

        cut.Markup.ShouldContain(customPlacecholder);
    }

    [Fact]
    public void WhenActivatorContentIsNull_ThenActivatorBarShouldBeRendered()
    {
        var items = GetDefaultItems();

        var cut = RenderDropdownWithItems(items);

        cut.Markup.ShouldContain("rise-dropdown-bar");
        cut.FindComponent<MudIcon>().ShouldNotBeNull();
    }
    
    protected IRenderedComponent<TRiseDropdownComponent> RenderDropdownWithItems(
        IEnumerable<TItem> items,
        Action<ComponentParameterCollectionBuilder<TRiseDropdownComponent>>? additionalParameters = null
    )
    {
        return RenderComponent<TRiseDropdownComponent>(parameters =>
        {
            parameters.Add(param => param.Items, items);
            additionalParameters?.Invoke(parameters);
        });
    }


    protected abstract List<TItem> GetDefaultItems();
    protected abstract Func<TItem, string> GetDefaultItemTextFunc();
}