using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Contact;

namespace Rise.Persistence.Configurations.Contact;

internal class ServiceConfiguration : EntityConfiguration<Facility>
{
    public override void Configure(EntityTypeBuilder<Facility> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

        builder.OwnsOne(x => x.ServiceCategory, sc =>
        {
            sc.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("ServiceCategoryName");
        });

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.OwnsOne(x => x.Location, loc =>
        {
            loc.OwnsOne(l => l.FaclitityAddress, sa =>
            {
                sa.Property(a => a.Street).HasMaxLength(250);
                sa.Property(a => a.HouseNumber);
                sa.Property(a => a.BusNumber).HasMaxLength(20);
                sa.Property(a => a.Postcode);
                sa.Property(a => a.City).HasMaxLength(100);
            });

            loc.Property(l => l.LocationName).HasMaxLength(250);
        });

        builder.OwnsMany(x => x.OpeningHours, oh =>
        {
            oh.WithOwner().HasForeignKey("Id");
            oh.OwnsMany(c => c.ContactHours, ch =>
            {
                ch.Property(t => t.StartTime).IsRequired();
                ch.Property(t => t.EndTime).IsRequired();
            });
            oh.ToTable("ServiceContactPeriods");
        });

        builder.Property(x => x.Remarks);

        builder.OwnsMany(x => x.CommunicationChannels, cc =>
        {
            cc.WithOwner().HasForeignKey("Id");
            cc.ToTable("ServiceCommunicationChannels");
        });
    }
}