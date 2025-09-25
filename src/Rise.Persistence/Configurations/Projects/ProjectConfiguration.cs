using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Products;
using Rise.Domain.Projects;

namespace Rise.Persistence.Configurations.Projects;

/// <summary>
/// Specific configuration for <see cref="Product"/>.
/// </summary>
internal class ProjectConfiguration : EntityConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        
        // 1 to Many relationship with a cascade restrict behavio
        builder.HasOne(x => x.Technician)
            .WithMany(x => x.Projects)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Value Object Mapping and rename of columns
        builder.OwnsOne(x => x.Location, loc =>
        {
            loc.Property(x => x.Addressline1).IsRequired().HasMaxLength(250).HasColumnName(nameof(Address.Addressline1));
            loc.Property(x => x.Addressline2).IsRequired().HasMaxLength(250).HasColumnName(nameof(Address.Addressline2));
            loc.Property(x => x.City).IsRequired().HasMaxLength(50).HasColumnName(nameof(Address.City));
            loc.Property(x => x.PostalCode).IsRequired().HasMaxLength(20).HasColumnName(nameof(Address.PostalCode));
        }).Navigation(x => x.Location).IsRequired();
        
        // See /// See https://hogent-web.github.io/csharp/chapters/09/slides/index.html#1 for more information.
        // Other Project configuration here.
    }
}