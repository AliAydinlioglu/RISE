using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Identity;

namespace Rise.Persistence.Configurations.Identity;

internal class RoleConfiguration : EntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired() .HasMaxLength(200);

        builder.HasMany(r => r.RoleNavigationItems)
            .WithOne(rn => rn.Role)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}