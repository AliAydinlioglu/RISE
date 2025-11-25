using Ardalis.Result;
using NSubstitute;
using Rise.Shared.Menu;
using Rise.Server.Endpoints.Menu;

namespace Rise.Server.Tests.Menu;

public class DayMenuIndexShould
{
    [Fact]
    public async Task CallMenuServiceAndReturnResult()
    {
        // Arrange
        var menuService = Substitute.For<IMenuService>();
        var request = new MenuRequest.DayMenu { RestoId = 1, Date = DateTimeOffset.Now };
        var expectedResult = Result.Success(new MenuResponse.DayMenu());

        menuService.GetDayMenuAsync(request, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedResult));

        var endpoint = new DayMenu.Index(menuService);

        // Act
        var result = await endpoint.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult, result);
        await menuService.Received(1).GetDayMenuAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PassCancellationTokenToService()
    {
        var menuService = Substitute.For<IMenuService>();
        var request = new MenuRequest.DayMenu { RestoId = 1, Date = System.DateTimeOffset.Now };
        var expectedResult = Result.Success(new MenuResponse.DayMenu());

        var cts = new CancellationTokenSource();

        menuService.GetDayMenuAsync(request, cts.Token)
            .Returns(Task.FromResult(expectedResult));

        var endpoint = new DayMenu.Index(menuService);

        var result = await endpoint.ExecuteAsync(request, cts.Token);

        Assert.Equal(expectedResult, result);
        await menuService.Received(1).GetDayMenuAsync(request, cts.Token);
    }
}