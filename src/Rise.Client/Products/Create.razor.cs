using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;


public partial class Create
{
    [Inject] public required IProductService ProductService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    private ProductRequest.Create Model { get; set; } = new();
    private Result<ProductResponse.Create>? _result;
    private async Task CreateProductAsync()
    {
        _result = await ProductService.CreateAsync(Model);
        if (_result.IsSuccess)
        {
            NavigationManager.NavigateTo("/product");
        }
    }
}