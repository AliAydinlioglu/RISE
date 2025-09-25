using Rise.Shared.Identity;
using Rise.Shared.Products;
namespace Rise.Server.Endpoints.Products;

/// <summary>
/// Creation of a <see cref="Product"/>
/// See https://fast-endpoints.com/
/// </summary>
/// <param name="productService"></param>
public class Create(IProductService productService) : Endpoint<ProductRequest.Create, Result<ProductResponse.Create>>
{
    public override void Configure()
    {
        Post("/api/products");
        Roles(AppRoles.Administrator); // Only Administrators can create products.
    }

    public override Task<Result<ProductResponse.Create>> ExecuteAsync(ProductRequest.Create req, CancellationToken ctx)
    {
        return productService.CreateAsync(req, ctx);
    }
}