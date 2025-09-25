using Rise.Shared.Products;
using Xunit.Abstractions;
using Shouldly;
using NSubstitute;
using System.Threading.Tasks;
using System.Linq;
using Ardalis.Result;
using Rise.Shared.Common;

namespace Rise.Client.Products;

/// <summary>
/// Same as <see cref="IndexShould"/> using mocking instead of faking.
/// https://nsubstitute.github.io
/// </summary>
public class IndexWithMockExampleShould : TestContext
{
    public IndexWithMockExampleShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
    }

    [Fact]
    public void ShowsProducts()
    {
        // Authenticate as a test user for this specific test.
        this.AddTestAuthorization().SetAuthorized("TEST USER");
        
        // Mock
        var products = Enumerable.Range(1, 5)
                         .Select(i => new ProductDto.Index() { Id = i, Name = $"Product {i}", Description = $"Description {i}"  });
    
        var wrapper = new ProductResponse.Index
        {
            Products = products,
            TotalCount = 5,
        };
        
        var productServiceMock = Substitute.For<IProductService>();
        // Any is that we don't care about the incoming parameters. We can specify them for a specific case, but this is fine for this test.
        productServiceMock.GetIndexAsync(Arg.Any<QueryRequest.SkipTake>()).Returns(Task.FromResult(Result.Success(wrapper)));

        Services.AddScoped(provider => productServiceMock);

        var cut = RenderComponent<Index>();
        cut.FindAll("table tbody tr").Count.ShouldBe(5);
    }
}
