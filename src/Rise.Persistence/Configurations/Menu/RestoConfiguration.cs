using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Menu;

namespace Rise.Persistence.Configurations.Menu;

internal class RestoConfiguration : EntityConfiguration<Resto>
{
    public override void Configure(EntityTypeBuilder<Resto> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        
        builder.OwnsMany(x => x.OpeningHours, oh =>
        {
            oh.WithOwner().HasForeignKey("Id");
            oh.OwnsMany(c => c.ContactHours, ch =>
            {
                ch.Property(t => t.StartTime).IsRequired();
                ch.Property(t => t.EndTime).IsRequired();
            });
            oh.ToTable("RestoContactPeriods");
        });
    }
}