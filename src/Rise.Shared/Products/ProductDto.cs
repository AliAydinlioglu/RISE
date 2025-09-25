namespace Rise.Shared.Products;

/// <summary>
/// Contains data transfer objects (DTOs) used for product-related operations.
/// </summary>
public static class ProductDto
{
    /// <summary>
    /// Represents a product index containing minimalistic product details.
    /// </summary>
    public class Index
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}