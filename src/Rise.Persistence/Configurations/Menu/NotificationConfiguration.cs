using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Notifications;

namespace Rise.Persistence.Configurations.Projects;

/// <summary>
/// Specific configuration for <see cref="Notification"/>.
/// </summary>
internal class NotificationConfiguration : EntityConfiguration<Notification>
{
    public override void Configure(EntityTypeBuilder<Notification> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.NotificationLevel).IsRequired();
        builder.Property(x => x.TypeOfNotification).IsRequired();

        builder.OwnsMany(x => x.Acknowledgements, ack => {
            ack.Property(x => x.UserName).HasMaxLength(150).IsRequired();
            ack.Property(x => x.ReadOn).IsRequired();
        });

        builder.OwnsOne(x => x.MsgDetail, msg =>
        {
            msg.Property(x => x.Title).IsRequired().HasMaxLength(50).HasColumnName(nameof(Message.Title));
            msg.Property(x => x.Description).IsRequired().HasMaxLength(255).HasColumnName(nameof(Message.Description));
            msg.Property(x => x.UrlDetailPage).HasMaxLength(255).HasColumnName(nameof(Message.UrlDetailPage));
        })
        .Navigation(x => x.MsgDetail)
        .IsRequired();
    }
}