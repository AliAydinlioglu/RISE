using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Navigation;

namespace Rise.Persistence.Configurations.Navigation;

internal class RoleNavigationItemContentLocationConfiguration : EntityConfiguration<RoleNavigationItemContentLocation>
{
    public override void Configure(EntityTypeBuilder<RoleNavigationItemContentLocation> builder)
    {
        builder.OwnsOne(x => x.Id, id =>
        {
            id.Property(i => i.RoleId).HasColumnName("RoleId");
            id.Property(i => i.NavigationItemId).HasColumnName("NavigationItemId");
            id.Property(i => i.ContentLocationId).HasColumnName("ContentLocationId");
        });
        
        builder.HasKey("RoleId", "NavigationItemId", "ContentLocationId");

        builder.HasOne(x => x.Role)
            .WithMany(r => r.RoleNavigationItems)
            .HasForeignKey("RoleId");

        builder.HasOne(x => x.NavigationItem)
            .WithMany(n => n.RoleNavigationItems)
            .HasForeignKey("NavigationItemId");

        builder.HasOne(x => x.ContentLocation)
            .WithMany(c => c.RoleNavigationItems)
            .HasForeignKey("ContentLocationId");
    }
}