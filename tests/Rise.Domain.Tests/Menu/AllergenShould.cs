using Rise.Domain.Menu;

namespace Rise.Domain.Tests.Menu;

public class AllergenShould
{
    [Fact]
    public void Create_When_ValidArguments()
    {
        var allergen = new Allergen("Gluten", "G");

        Assert.Equal("Gluten", allergen.Name);
        Assert.Equal("G", allergen.Symbol);
    }

    [Fact]
    public void ThrowArgumentNull_When_Name_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new Allergen(null!, "G"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ThrowArgumentException_When_Name_Is_EmptyOrWhitespace(string invalidName)
    {
        Assert.Throws<ArgumentException>(() => new Allergen(invalidName, "G"));
    }

    [Fact]
    public void ThrowArgumentNull_When_Symbol_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new Allergen("Gluten", null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ThrowArgumentException_When_Symbol_Is_EmptyOrWhitespace(string invalidSymbol)
    {
        Assert.Throws<ArgumentException>(() => new Allergen("Gluten", invalidSymbol));
    }
}
