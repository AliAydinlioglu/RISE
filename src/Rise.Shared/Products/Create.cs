namespace Rise.Shared.Products;

/// <summary>
/// Represents a static utility class containing request-related structures for products.
/// </summary>
public static partial class ProductRequest
{
    public class Create
    {
        /// <summary>
        /// The name of the product.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// A short description of the product.
        /// </summary>
        public string? Description { get; set; }
        
        public class Validator : AbstractValidator<Create>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(250); 
                RuleFor(x => x.Description).NotEmpty().MaximumLength(4_000);
            }
        }
    }
}

public static partial  class ProductResponse
{
    public class Create
    {
        public int ProductId { get; set; }
    }
}