using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Menu;

namespace Rise.Persistence.Configurations.Menu;

internal class MenuItemConfiguration : EntityConfiguration<MenuItem>
{
    public override void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.StudentPrice).HasPrecision(5,2);
        builder.Property(x => x.ExternPrice).HasPrecision(5,2);
    }
}