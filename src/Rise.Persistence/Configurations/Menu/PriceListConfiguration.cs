using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Menu;

namespace Rise.Persistence.Configurations.Menu;

internal class PriceListConfiguration : EntityConfiguration<PriceList>
{
    public override void Configure(EntityTypeBuilder<PriceList> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.HasMany(p => p.PriceListItems)
            .WithOne()
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}