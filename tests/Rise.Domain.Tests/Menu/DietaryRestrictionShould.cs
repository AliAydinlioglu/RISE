using Rise.Domain.Menu;

namespace Rise.Domain.Tests.Menu;

public class DietaryRestrictionShould
{
    [Fact]
    public void Create_When_ValidArguments()
    {
        var dr = new DietaryRestriction("Vegan", "V");

        Assert.Equal("Vegan", dr.Name);
        Assert.Equal("V", dr.Symbol);
    }

    // Name checks
    [Fact]
    public void ThrowArgumentNull_When_Name_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new DietaryRestriction(null!, "V"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ThrowArgumentException_When_Name_Is_EmptyOrWhitespace(string invalidName)
    {
        Assert.Throws<ArgumentException>(() => new DietaryRestriction(invalidName, "V"));
    }

    // Symbol checks (je vroeg specifiek tests op symbol)
    [Fact]
    public void ThrowArgumentNull_When_Symbol_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new DietaryRestriction("Vegan", null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ThrowArgumentException_When_Symbol_Is_EmptyOrWhitespace(string invalidSymbol)
    {
        Assert.Throws<ArgumentException>(() => new DietaryRestriction("Vegan", invalidSymbol));
    }
}
