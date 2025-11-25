using Ardalis.Result;
using NSubstitute;
using Rise.Shared.Menu;
using Rise.Server.Endpoints.Menu;

namespace Rise.Server.Tests.Menu;

public class RestoDetailShould
{
    [Fact]
    public async Task CallRestoServiceAndReturnResult()
    {
        // Arrange
        var restoService = Substitute.For<IRestoService>();
        var request = new MenuRequest.Resto { Id = 1 };
        var expectedResult = Result.Success(new MenuResponse.RestoDetail());

        restoService.GetDetailAsync(request, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedResult));

        var endpoint = new RestoDetail.Index(restoService);

        // Act
        var result = await endpoint.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult, result);
        await restoService.Received(1).GetDetailAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PassCancellationTokenToService()
    {
        // Arrange
        var restoService = Substitute.For<IRestoService>();
        var request = new MenuRequest.Resto { Id = 1 };
        var expectedResult = Result.Success(new MenuResponse.RestoDetail());
        var cts = new CancellationTokenSource();

        restoService.GetDetailAsync(request, cts.Token)
            .Returns(Task.FromResult(expectedResult));

        var endpoint = new RestoDetail.Index(restoService);

        // Act
        var result = await endpoint.ExecuteAsync(request, cts.Token);

        // Assert
        Assert.Equal(expectedResult, result);
        await restoService.Received(1).GetDetailAsync(request, cts.Token);
    }
}