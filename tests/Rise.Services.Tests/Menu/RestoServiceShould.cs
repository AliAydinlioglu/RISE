using Microsoft.EntityFrameworkCore;
using Rise.Domain.Common;
using Rise.Domain.Contact;
using Rise.Persistence;
using Rise.Domain.Menu;
using Rise.Services.Menu;
using Rise.Shared.Menu;
using Rise.Shared.Common;
using Rise.TestDoubles;

namespace Rise.Services.Tests.Menu;

public class RestoServiceShould
{
    private ApplicationDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task ReturnOverview_When_RestosExist()
    {
        var db = GetDbContext(nameof(ReturnOverview_When_RestosExist));
        db.Restos.AddRange(
            new Resto("Resto1", LocationTestDataFactory.CreateDefaultLocation(), null),
            new Resto("Resto2", LocationTestDataFactory.CreateDefaultLocation(), null)
        );
        await db.SaveChangesAsync();

        var service = new RestoService(db);
        var req = new QueryRequest.SkipTake { Skip = 0, Take = 10 };

        var result = await service.GetOverviewAsync(req, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Restos.Length);
        Assert.Contains(result.Value.Restos, r => r.Name == "Resto1");
        Assert.Contains(result.Value.Restos, r => r.Name == "Resto2");
    }

    [Fact]
    public async Task ReturnEmptyOverview_When_NoRestos()
    {
        var db = GetDbContext(nameof(ReturnEmptyOverview_When_NoRestos));
        var service = new RestoService(db);
        var req = new QueryRequest.SkipTake { Skip = 0, Take = 10 };

        var result = await service.GetOverviewAsync(req, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Restos);
    }

    [Fact]
    public async Task ReturnDetail_When_RestoExists()
    {
        var db = GetDbContext(nameof(ReturnDetail_When_RestoExists));
        var location = LocationTestDataFactory.CreateDefaultLocation();
        var resto = new Resto("TestResto", location, null);

        // Optional: Add opening hours
        resto.AddContactPeriods([
            new ContactPeriod(DateOnly.FromDateTime(DateTime.Now),  [ 
            
                new TimeRange(TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)),TimeOnly.FromTimeSpan(TimeSpan.FromHours(16))) 
            ])
        ]);

        db.Restos.Add(resto);
        await db.SaveChangesAsync();

        var service = new RestoService(db);
        var req = new MenuRequest.Resto { Id = resto.Id };

        var result = await service.GetDetailAsync(req, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("TestResto", result.Value.Resto.Name);
        Assert.Equal("Test Straat", result.Value.Resto.Location.Street);
        Assert.Single(result.Value.Resto.OpeningHours);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task ReturnError_When_RestoIdInvalid(int id)
    {
        var db = GetDbContext(nameof(ReturnError_When_RestoIdInvalid));
        var service = new RestoService(db);
        var req = new MenuRequest.Resto { Id = id };

        var result = await service.GetDetailAsync(req, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Id has an invalid value.", result.Errors.First());
    }

    [Fact]
    public async Task ReturnNotFound_When_RestoDoesNotExist()
    {
        var db = GetDbContext(nameof(ReturnNotFound_When_RestoDoesNotExist));
        var service = new RestoService(db);
        var req = new MenuRequest.Resto { Id = 1 };

        var result = await service.GetDetailAsync(req, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Resto is not found.", result.Errors.First());
    }

    [Fact]
    public async Task ThrowNotImplemented_When_SetFavoriteRestoCalled()
    {
        var db = GetDbContext(nameof(ThrowNotImplemented_When_SetFavoriteRestoCalled));
        var service = new RestoService(db);
        var req = new MenuRequest.Resto { Id = 1 };

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            service.SetFavoriteResto(req, CancellationToken.None));
    }
}
