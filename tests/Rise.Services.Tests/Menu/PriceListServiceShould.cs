using Microsoft.EntityFrameworkCore;
using Rise.Domain.Common;
using Rise.Persistence;
using Rise.Domain.Menu;
using Rise.Services.Menu;
using Rise.Shared.Menu;
using Rise.TestDoubles;

namespace Rise.Services.Tests.Menu;

public class PriceListServiceShould
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
    [InlineData(-1)]
    public async Task ReturnError_When_IdIsInvalid(int id)
    {
        var db = GetDbContext(nameof(ReturnError_When_IdIsInvalid));
        var service = new PriceListService(db);
        var req = new MenuRequest.Resto { Id = id };

        var result = await service.GetForRestoAsync(req, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Id has an invalid value.", result.Errors.First());
    }

    [Fact]
    public async Task ReturnNotFound_When_RestoDoesNotExist()
    {
        var db = GetDbContext(nameof(ReturnNotFound_When_RestoDoesNotExist));
        var service = new PriceListService(db);
        var req = new MenuRequest.Resto { Id = 1 };

        var result = await service.GetForRestoAsync(req, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Resto was not found.", result.Errors.First());
    }

    [Fact]
    public async Task ReturnEmptyPricelist_When_RestoHasNoPriceList()
    {
        var db = GetDbContext(nameof(ReturnEmptyPricelist_When_RestoHasNoPriceList));
        var resto = new Resto("Resto1", LocationTestDataFactory.CreateDefaultLocation(), null);
        db.Restos.Add(resto);
        await db.SaveChangesAsync();

        var service = new PriceListService(db);
        var req = new MenuRequest.Resto { Id = resto.Id };

        var result = await service.GetForRestoAsync(req, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.PricelistCategories);
    }

    [Fact]
    public async Task ReturnPricelist_When_RestoHasPriceListWithItems()
    {
        var db = GetDbContext(nameof(ReturnPricelist_When_RestoHasPriceListWithItems));

        // Setup PriceList
        var category1 = new PriceListCategory("Drinks", null);
        var category2 = new PriceListCategory("Food", null);

        var item1 = new PriceListItem("Cola", Price.ForPriceListItem(2m, 3m), category1);
        var item2 = new PriceListItem("Burger", Price.ForPriceListItem(5m, 6m), category2);

        var priceList = new PriceList("Lunch Menu");
        priceList.AddPriceListItems(new[] { item1, item2 });

        // Setup Resto
        var resto = new Resto("Resto1", LocationTestDataFactory.CreateDefaultLocation(), priceList);
        db.Restos.Add(resto);
        await db.SaveChangesAsync();

        var service = new PriceListService(db);
        var req = new MenuRequest.Resto { Id = resto.Id };

        var result = await service.GetForRestoAsync(req, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.PricelistCategories.Length);

        Assert.Contains(result.Value.PricelistCategories, c => c.CategoryName == "Drinks" && c.PriceListItems.Any(i => i.Name == "Cola"));
        Assert.Contains(result.Value.PricelistCategories, c => c.CategoryName == "Food" && c.PriceListItems.Any(i => i.Name == "Burger"));
    }

    [Fact]
    public async Task UseDefaultId_When_IdIsNull()
    {
        var db = GetDbContext(nameof(UseDefaultId_When_IdIsNull));
        var defaultResto = new Resto("DefaultResto", LocationTestDataFactory.CreateDefaultLocation(), null);
        db.Restos.Add(defaultResto);
        await db.SaveChangesAsync();

        var service = new PriceListService(db);
        var req = new MenuRequest.Resto { Id = null };

        var result = await service.GetForRestoAsync(req, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
