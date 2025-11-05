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
        
        builder.OwnsOne(it => it.Sender, lecturer =>
        {
            lecturer.Property(it => it.FirstName)
                .IsRequired()
                .HasColumnName(nameof(Lecturer.FirstName));
            
            lecturer.Property(it => it.LastName)
                .IsRequired()
                .HasColumnName(nameof(Lecturer.LastName));
        }).Navigation(it => it.Sender).IsRequired();
    }
}