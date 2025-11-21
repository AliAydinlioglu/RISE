using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Menu;

namespace Rise.Persistence.Configurations.Menu;

internal class PriceListCategoryConfiguration : EntityConfiguration<PriceListCategory>
{
    public override void Configure(EntityTypeBuilder<PriceListCategory> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Remark).HasMaxLength(1000);
    }
}