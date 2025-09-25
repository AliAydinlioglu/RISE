using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Products;
using Rise.Domain.Projects;

namespace Rise.Persistence.Configurations.Projects;

/// <summary>
/// Specific configuration for <see cref="Product"/>.
/// </summary>
internal class TechnicianConfiguration : EntityConfiguration<Technician>
{
    public override void Configure(EntityTypeBuilder<Technician> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.AccountId).IsRequired().HasMaxLength(36);
        builder.HasIndex(x => x.AccountId).IsUnique();
        
        // See https://hogent-web.github.io/csharp/chapters/09/slides/index.html#1 for more information.
        // Other Technician configuration here.
    }
}