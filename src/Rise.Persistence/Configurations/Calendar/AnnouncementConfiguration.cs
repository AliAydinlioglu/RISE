using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Calendar;

namespace Rise.Persistence.Configurations.Calendar;

internal class AnnouncementConfiguration : EntityConfiguration<Announcement>
{
    public override void Configure(EntityTypeBuilder<Announcement> builder)
    {
        base.Configure(builder);

        builder.Property(it => it.Title).IsRequired();
        builder.Property(it => it.Message).IsRequired();
        builder.Property(it => it.Timestamp).IsRequired();
        
        builder.HasOne(it => it.Sender)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}