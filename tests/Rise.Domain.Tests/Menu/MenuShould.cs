using Rise.Domain.Common;

namespace Rise.Domain.Tests.Menu;

using Rise.Domain.Menu;

public class MenuShould
{
    [Fact]
    public void Create_When_Valid_Date()
    {
        var date = DateTimeOffset.Now;
        var menu = new Menu(date);

        Assert.Equal(date, menu.Date);
    }

    [Fact]
    public void Throw_When_Date_Is_Default()
    {
        Assert.Throws<ArgumentException>(() => new Menu(default));
    }

    [Fact]
    public void AddWeekMenuItems_When_Valid_Items()
    {
        var menu = new Menu(DateTimeOffset.Now);
        var item = new MenuItem("Test", new MenuCategory("Cat"), Price.ForMenuItem(5m,null));

        menu.AddWeekMenuItems([item]);

        Assert.Single(menu.MenuItems);
    }

    [Fact]
    public void Throw_When_AddWeekMenuItems_Includes_Null()
    {
        var menu = new Menu(DateTimeOffset.Now);

        Assert.Throws<ArgumentNullException>(() =>
            menu.AddWeekMenuItems([null!]));
    }
}