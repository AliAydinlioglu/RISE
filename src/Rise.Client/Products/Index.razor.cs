using Microsoft.AspNetCore.Components;
using Rise.Shared.Common;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class Index
{
    private IEnumerable<ProductDto.Index>? products;

    [Inject] public required IProductService ProductService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 50,
            OrderBy = "Id",
        };

        var result = await ProductService.GetIndexAsync(request);
        products = result.Value.Products;
    }
}

