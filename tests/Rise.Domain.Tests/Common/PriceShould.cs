using System.Globalization;
using Rise.Domain.Common;

namespace Rise.Domain.Tests.Common;

public class PriceShould
{
    [Fact]
    public void Create_ForMenuItem_When_Valid()
    {
        var price = Price.ForMenuItem(5.5m, 10.0m);

        Assert.Equal(5.5m, price.Student);
        Assert.Equal(10.0m, price.Extern);
    }

    [Fact]
    public void Create_ForPriceListItem_When_Valid()
    {
        var price = Price.ForPriceListItem(4.0m, 6.0m);

        Assert.Equal(4.0m, price.Student);
        Assert.Equal(6.0m, price.Extern);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Throw_When_Student_Is_NegativeOrZero(decimal student)
    {
        Assert.Throws<ArgumentException>(() => Price.ForMenuItem(student, 5m));
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    public void Throw_When_Extern_Is_NegativeOrZero(decimal externPrice)
    {
        Assert.Throws<ArgumentException>(() => Price.ForMenuItem(5m, externPrice));
    }

    [Fact]
    public void Throw_When_StudentPrice_Exceeds_ExternPrice()
    {
        Assert.Throws<ArgumentException>(() => Price.ForMenuItem(10m, 5m));
    }

    [Fact]
    public void Throw_When_Decimals_MoreThanTwo()
    {
        Assert.Throws<ArgumentException>(() => Price.ForMenuItem(1.123m, 5m));
        Assert.Throws<ArgumentException>(() => Price.ForMenuItem(1m, 2.345m));
    }

    [Fact]
    public void Allow_Null_Prices()
    {
        var price = Price.ForMenuItem(null, null);
        Assert.Null(price.Student);
        Assert.Null(price.Extern);
    }

    [Fact]
    public void ToString_Should_Format_Correctly()
    {
        var culture = CultureInfo.GetCultureInfo("nl-NL");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        
        var price = Price.ForMenuItem(1.5m, 3m);
        var str = price.ToString();
        Assert.Contains("1,50", str);
        Assert.Contains("3,00", str);
    }
}