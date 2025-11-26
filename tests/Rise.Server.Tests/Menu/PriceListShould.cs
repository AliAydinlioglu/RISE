using Ardalis.Result;
using NSubstitute;
using Rise.Shared.Menu;
using Rise.Server.Endpoints.Menu;

namespace Rise.Server.Tests.Menu;

public class PriceListShould
{
    [Fact]
    public async Task CallPriceListServiceAndReturnResult()
    {
        // Arrange
        var priceListService = Substitute.For<IPriceListService>();
        var request = new MenuRequest.Resto { Id = 1 };
        var expectedResult = Result.Success(new MenuResponse.Pricelist());

        priceListService.GetForRestoAsync(request, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedResult));

        var endpoint = new PriceList.Index(priceListService);

        // Act
        var result = await endpoint.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult, result);
        await priceListService.Received(1).GetForRestoAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PassCancellationTokenToService()
    {
        // Arrange
        var priceListService = Substitute.For<IPriceListService>();
        var request = new MenuRequest.Resto { Id = 1 };
        var expectedResult = Result.Success(new MenuResponse.Pricelist());
        var cts = new CancellationTokenSource();

        priceListService.GetForRestoAsync(request, cts.Token)
            .Returns(Task.FromResult(expectedResult));

        var endpoint = new PriceList.Index(priceListService);

        // Act
        var result = await endpoint.ExecuteAsync(request, cts.Token);

        // Assert
        Assert.Equal(expectedResult, result);
        await priceListService.Received(1).GetForRestoAsync(request, cts.Token);
    }
}