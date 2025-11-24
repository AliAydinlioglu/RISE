using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Menu;

namespace Rise.Persistence.Configurations.Menu;

internal class DietaryRestrictionConfiguration : EntityConfiguration<DietaryRestriction>
{
    public override void Configure(EntityTypeBuilder<DietaryRestriction> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Symbol).HasMaxLength(50).IsRequired();
    }
}