using Rise.Domain.Common;

namespace Rise.Domain.Tests.Menu;

using Rise.Domain.Menu;

public class MenuItemShould
{
    [Fact]
    public void Create_When_Valid_Arguments()
    {
        var item = CreateValidItem();

        Assert.Equal("Dish", item.Name);
    }

    [Fact]
    public void Throw_When_Name_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new MenuItem(null!, new MenuCategory("Cat"), Price.ForMenuItem(5m,null)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Throw_When_Name_Is_Empty_Or_Whitespace(string name)
    {
        Assert.Throws<ArgumentException>(() =>
            new MenuItem(name, new MenuCategory("Cat"), Price.ForMenuItem(5m,null)));
    }

    [Fact]
    public void AddAllergens_When_Valid_Allergens()
    {
        var item = CreateValidItem();
        var allergen = new Allergen("Gluten", "G");

        item.AddAllergens([allergen]);

        Assert.Single(item.Allergens);
    }

    [Fact]
    public void Throw_When_AddAllergens_Includes_Null()
    {
        var item = CreateValidItem();

        Assert.Throws<ArgumentNullException>(() =>
            item.AddAllergens([null!]));
    }

    [Fact]
    public void AddDietaryRestrictions_When_Valid_Restrictions()
    {
        var item = CreateValidItem();
        var dr = new DietaryRestriction("Vegan", "V");

        item.AddDietaryRestrictions([dr]);

        Assert.Single(item.DietaryRestrictions);
    }

    [Fact]
    public void Throw_When_AddDietaryRestrictions_Includes_Null()
    {
        var item = CreateValidItem();

        Assert.Throws<ArgumentNullException>(() =>
            item.AddDietaryRestrictions([null!]));
    }

    private MenuItem CreateValidItem()
    {
        return new MenuItem("Dish", new MenuCategory("Cat"), Price.ForMenuItem(10m,null));
    }
}