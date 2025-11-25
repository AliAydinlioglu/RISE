namespace Rise.Domain.Tests.Menu;

using Rise.Domain.Menu;

public class MenuCategoryShould
{
    [Fact]
    public void Create_When_Valid_Name()
    {
        var category = new MenuCategory("Main Courses");
        Assert.Equal("Main Courses", category.Name);
    }

    [Fact]
    public void Throw_When_Name_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new MenuCategory(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Throw_When_Name_Is_Empty_Or_Whitespace(string name)
    {
        Assert.Throws<ArgumentException>(() => new MenuCategory(name));
    }
}