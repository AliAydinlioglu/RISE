using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Navigation;

namespace Rise.Persistence.Configurations.Navigation;

internal class ContentLocationConfiguration : EntityConfiguration<ContentLocation>
{
    public override void Configure(EntityTypeBuilder<ContentLocation> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

        builder.HasMany(cl => cl.RoleNavigationItems)
            .WithOne(ni => ni.ContentLocation)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}