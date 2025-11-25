using Rise.Domain.Common;

namespace Rise.Domain.Tests.Menu;

using Rise.Domain.Menu;

public class PriceListItemShould
{
    [Fact]
    public void Create_When_Valid()
    {
        var category = new PriceListCategory("Food", null);
        var price = Price.ForPriceListItem(12m,null);

        var item = new PriceListItem("Burger", price, category, true, false);

        Assert.Equal("Burger", item.Name);
        Assert.Equal(price, item.Price);
        Assert.Equal(category, item.PriceListCategory);
    }

    [Fact]
    public void Throw_When_Name_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PriceListItem(null!, Price.ForPriceListItem(5m,null), new PriceListCategory("Food", null)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Throw_When_Name_Is_Empty_Or_Whitespace(string name)
    {
        Assert.Throws<ArgumentException>(() =>
            new PriceListItem(name, Price.ForPriceListItem(5m,null), new PriceListCategory("Food", null)));
    }

    [Fact]
    public void Throw_When_Price_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PriceListItem("Cola", null!, new PriceListCategory("Drinks", null)));
    }
}