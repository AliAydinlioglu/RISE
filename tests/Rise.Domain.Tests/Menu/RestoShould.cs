namespace Rise.Domain.Tests.Menu;

using Rise.Domain.Menu;
using Rise.Domain.Contact;
using Rise.Domain.Locations;

public class RestoShould
{
    [Fact]
    public void Create_When_Valid()
    {
        var resto = new Resto("Test", CreateValidLocation(), null);

        Assert.Equal("Test", resto.Name);
    }

    [Fact]
    public void Throw_When_Name_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Resto(null!, CreateValidLocation(), null));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Throw_When_Name_Is_Empty_Or_Whitespace(string name)
    {
        Assert.Throws<ArgumentException>(() =>
            new Resto(name, CreateValidLocation(), null));
    }

    [Fact]
    public void AddContactPeriods_When_Valid()
    {
        var now = DateOnly.FromDateTime(DateTime.Now);
        var resto = CreateValidResto();
        var cp = new ContactPeriod(now, []);

        resto.AddContactPeriods([cp]);

        Assert.Single(resto.OpeningHours);
    }

    [Fact]
    public void Throw_When_AddContactPeriods_Includes_Null()
    {
        var resto = CreateValidResto();

        Assert.Throws<ArgumentNullException>(() =>
            resto.AddContactPeriods([null!]));
    }

    [Fact]
    public void AddMenus_When_Valid()
    {
        var resto = CreateValidResto();
        var menu = new Menu(DateTimeOffset.Now);

        resto.AddMenus([menu]);

        Assert.Single(resto.Menus);
    }

    private Location CreateValidLocation()
    {
        return new Location("Test Name", "Test street", 123, 5678, "Test City", "A");
    }
    
    private Resto CreateValidResto()
    {
        return new Resto("Test", CreateValidLocation(), null);
    }
}