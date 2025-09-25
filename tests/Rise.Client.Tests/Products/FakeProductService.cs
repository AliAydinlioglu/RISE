using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using Rise.Shared.Common;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public class FakeProductService : IProductService
{
    public Task<Result<ProductResponse.Create>> CreateAsync(ProductRequest.Create request, CancellationToken ctx = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ProductResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default)
    {
        var products = Enumerable.Range(1, 5)
            .Select(i => new ProductDto.Index() { Id = i, Name = $"Product {i}", Description = $"Description {i}" });

        var wrapper = new ProductResponse.Index
        {
            Products = products,
            TotalCount = 5,
        };
        
        return Task.FromResult(Result.Success(wrapper));
    }
}

