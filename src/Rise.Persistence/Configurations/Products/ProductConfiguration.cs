using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Products;

namespace Rise.Persistence.Configurations.Products;

/// <summary>
/// Specific configuration for <see cref="Product"/>.
/// </summary>
internal class ProductConfiguration : EntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(250);
        // Other Product configuration here.
    }
}