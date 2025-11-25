using Rise.Domain.Common;
using Rise.Domain.Menu;

namespace Rise.Domain.Tests.Menu;

public class PriceListShould
{
    [Fact]
    public void Create_When_ValidName()
    {
        var priceList = new PriceList("Lunch Menu");
        Assert.Equal("Lunch Menu", priceList.Name);
        Assert.Empty(priceList.PriceListItems);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ThrowArgumentNullOrException_When_Name_Is_Invalid(string? name)
    {
        if (name == null)
            Assert.Throws<ArgumentNullException>(() => new PriceList(name!));
        else
            Assert.Throws<ArgumentException>(() => new PriceList(name));
    }

    [Fact]
    public void AddPriceListItems_Should_Add_Items()
    {
        var priceList = new PriceList("Lunch Menu");
        var category = new PriceListCategory("Drinks", null);
        var item = new PriceListItem("Cola", Price.ForPriceListItem(2m, 3m), category);

        priceList.AddPriceListItems(new[] { item });

        Assert.Single(priceList.PriceListItems);
        Assert.Contains(item, priceList.PriceListItems);
    }

    [Fact]
    public void AddPriceListItems_Should_Throw_When_Item_Is_Null()
    {
        var priceList = new PriceList("Lunch Menu");
        Assert.Throws<ArgumentNullException>(() => priceList.AddPriceListItems(new PriceListItem[] { null! }));
    }
}