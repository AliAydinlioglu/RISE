using Rise.Domain.Products;

namespace Rise.Domain.Tests.Products;

/// <summary>
/// Example Domain Tests using xUnit and Shouldly
/// https://xunit.net
/// <see cref="https://docs.shouldly.org"/>
///<see cref="https://xunit.net/?tabs=cs"/>
/// </summary>
public class ProductShould
{
    [Fact]
    public void BeCreated()
    {
        var p = new Product
        {
            Name = "iPhone 16",
            Description = "A smartphone"
        };

        p.Name.ShouldBe("iPhone 16");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("")]
    public void NotBeCreatedWithAnInvalidName(string? name)
    {
        Action act = () =>
        {
            Product product = new()
            {
                Name = name!,
                Description = "A smartphone"
            };
        };
        act.ShouldThrow<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("")]
    public void NotBeChangedToHaveAnInvalidName(string? name)
    {
        Action act = () =>
        {
            Product p = new() { Name = "iPhone 16", Description = "A smartphone" };
            p.Name = name!;
        };

        act.ShouldThrow<ArgumentException>();
    }
}