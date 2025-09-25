namespace Rise.Shared.Products;

/// <summary>
/// Represents the response structure for product-related operations.
/// </summary>
public static partial class ProductResponse
{
    public class Index
    {
        public IEnumerable<ProductDto.Index> Products { get; set; } = [];
        public int TotalCount { get; set; }
    }
}

