namespace Rise.Domain.Tests.Menu;

using Rise.Domain.Menu;

public class PriceListCategoryShould
{
    [Fact]
    public void Create_When_Valid()
    {
        var cat = new PriceListCategory("Drinks", "Cold only");

        Assert.Equal("Drinks", cat.Name);
        Assert.Equal("Cold only", cat.Remark);
    }

    [Fact]
    public void Throw_When_Name_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new PriceListCategory(null!, null));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Throw_When_Name_Is_Empty_Or_Whitespace(string name)
    {
        Assert.Throws<ArgumentException>(() => new PriceListCategory(name, null));
    }
}