using Ardalis.Result;
using NSubstitute;
using Rise.Shared.Menu;
using Rise.Shared.Common;
using Rise.Server.Endpoints.Menu;

namespace Rise.Server.Tests.Menu;

public class RestoOverviewShould
{
    [Fact]
    public async Task CallRestoServiceAndReturnResult()
    {
        // Arrange
        var restoService = Substitute.For<IRestoService>();
        var request = new QueryRequest.SkipTake { Skip = 0, Take = 10 };
        var expectedResult = Result.Success(new MenuResponse.RestoOverview());

        restoService.GetOverviewAsync(request, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedResult));

        var endpoint = new RestoOverview.Index(restoService);

        // Act
        var result = await endpoint.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult, result);
        await restoService.Received(1).GetOverviewAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PassCancellationTokenToService()
    {
        // Arrange
        var restoService = Substitute.For<IRestoService>();
        var request = new QueryRequest.SkipTake { Skip = 0, Take = 10 };
        var expectedResult = Result.Success(new MenuResponse.RestoOverview());
        var cts = new CancellationTokenSource();

        restoService.GetOverviewAsync(request, cts.Token)
            .Returns(Task.FromResult(expectedResult));

        var endpoint = new RestoOverview.Index(restoService);

        // Act
        var result = await endpoint.ExecuteAsync(request, cts.Token);

        // Assert
        Assert.Equal(expectedResult, result);
        await restoService.Received(1).GetOverviewAsync(request, cts.Token);
    }
}