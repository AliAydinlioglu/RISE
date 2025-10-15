using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Navigation;

namespace Rise.Persistence.Configurations.Navigation;

internal class NavigationItemConfiguration : EntityConfiguration<NavigationItem>
{
    public override void Configure(EntityTypeBuilder<NavigationItem> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Label).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Icon).IsRequired().HasMaxLength(15);
        builder.Property(x => x.Url).IsRequired().HasMaxLength(300);

        builder.HasMany(cl => cl.RoleNavigationItems)
            .WithOne(ni => ni.NavigationItem)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}