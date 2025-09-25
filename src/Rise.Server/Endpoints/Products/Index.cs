using Rise.Shared.Common;
using Rise.Shared.Products;
namespace Rise.Server.Endpoints.Products;

/// <summary>
/// List all products.
/// See https://fast-endpoints.com/
/// </summary>
/// <param name="productService"></param>
public class Index(IProductService productService) : Endpoint<QueryRequest.SkipTake, Result<ProductResponse.Index>>
{
    public override void Configure()
    {
        Get("/api/products");
        AllowAnonymous(); 
    }

    public override Task<Result<ProductResponse.Index>> ExecuteAsync(QueryRequest.SkipTake req, CancellationToken ct)
    {
        return productService.GetIndexAsync(req, ct);
    }
}