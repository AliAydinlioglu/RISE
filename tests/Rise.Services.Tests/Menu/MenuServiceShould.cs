using Microsoft.EntityFrameworkCore;
using Rise.Domain.Common;
using Rise.Persistence;
using Rise.Domain.Menu;
using Rise.Services.Menu;
using Rise.Shared.Menu;
using Rise.TestDoubles;

namespace Rise.Services.Tests.Menu;

public class MenuServiceShould
{
    private ApplicationDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new ApplicationDbContext(options);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task ReturnError_When_RestoIdIsInvalid(int id)
    {
        var db = GetDbContext(nameof(ReturnError_When_RestoIdIsInvalid));
        var service = new MenuService(db);

        var req = new MenuRequest.DayMenu
        {
            Date = DateTimeOffset.Now,
            RestoId = id
        };

        var result = await service.GetDayMenuAsync(req, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Id has an invalid value.", result.Errors.First());
    }

    [Fact]
    public async Task ReturnNotFound_When_NoMenuExists()
    {
        var db = GetDbContext(nameof(ReturnNotFound_When_NoMenuExists));
        var service = new MenuService(db);

        var req = new MenuRequest.DayMenu
        {
            Date = DateTimeOffset.Now,
            RestoId = 1
        };

        var result = await service.GetDayMenuAsync(req, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("DayMenu for this resto and this date was not found.", result.Errors.First());
    }

    [Fact]
    public async Task ReturnMenu_When_MenuExists()
    {
        var now = DateTimeOffset.Now;
        
        var db = GetDbContext(nameof(ReturnMenu_When_MenuExists));

        // Arrange: create resto, menu, category, menuitems
        var resto = new Resto("TestResto", LocationTestDataFactory.CreateDefaultLocation(), null);
        db.Restos.Add(resto);

        var category = new MenuCategory("Starters");
        var menuItem = new MenuItem("Soup", category, Price.ForMenuItem(5m, 6m));
        var menu = new Domain.Menu.Menu(now) { Resto = resto };
        menu.AddWeekMenuItems([menuItem]);

        db.Menus.Add(menu);
        await db.SaveChangesAsync();

        var service = new MenuService(db);

        var req = new MenuRequest.DayMenu
        {
            Date = now,
            RestoId = resto.Id
        };

        var result = await service.GetDayMenuAsync(req, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.MenuItemCategories);
        Assert.Equal("Starters", result.Value.MenuItemCategories.First().CategoryName);
        Assert.Single(result.Value.MenuItemCategories.First().MenuItems);
        Assert.Equal("Soup", result.Value.MenuItemCategories.First().MenuItems.First().Name);
    }

    [Fact]
    public async Task UseDefaultRestoId_When_RestoIdIsNull()
    {
        var db = GetDbContext(nameof(UseDefaultRestoId_When_RestoIdIsNull));

        var now = DateTimeOffset.Now;
        
        var defaultResto = new Resto("DefaultResto", LocationTestDataFactory.CreateDefaultLocation(), null);
        db.Restos.Add(defaultResto);

        var menu = new Domain.Menu.Menu(now) { Resto = defaultResto };
        db.Menus.Add(menu);
        await db.SaveChangesAsync();

        var service = new MenuService(db);

        var req = new MenuRequest.DayMenu
        {
            Date = now,
            RestoId = null
        };

        var result = await service.GetDayMenuAsync(req, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
