using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rise.Persistence.Configurations.Menu;

internal class MenuConfiguration : EntityConfiguration<Domain.Menu.Menu>
{
    public override void Configure(EntityTypeBuilder<Domain.Menu.Menu> builder)
    {
        base.Configure(builder);
        builder.Property(m => m.Date).IsRequired();
        
        builder.HasMany(m => m.MenuItems)
            .WithOne()
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}