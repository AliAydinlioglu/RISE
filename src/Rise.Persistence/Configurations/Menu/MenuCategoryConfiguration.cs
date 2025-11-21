using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Menu;

namespace Rise.Persistence.Configurations.Menu;

internal class MenuCategoryConfiguration : EntityConfiguration<MenuCategory>
{
    public override void Configure(EntityTypeBuilder<MenuCategory> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
    }
}