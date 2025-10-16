using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.StudentActivities;

namespace Rise.Persistence.Configurations.Locations;

internal class LocationsConfiguration : EntityConfiguration<Location>
{
    public override void Configure(EntityTypeBuilder<Location> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(250);
        builder.Property(x => x.Street).IsRequired().HasMaxLength(250);
        builder.Property(x => x.HouseNumber).IsRequired();
        builder.Property(x => x.Postcode).IsRequired();
        builder.Property(x => x.City).IsRequired().HasMaxLength(100);
    }
}
