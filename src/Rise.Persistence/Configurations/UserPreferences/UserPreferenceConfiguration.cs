using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Persistence.Models.Identity;
using Rise.Domain.UserPreferences;

namespace Rise.Persistence.Configurations.UserPreferences
{
    public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
    {
        public void Configure(EntityTypeBuilder<UserPreference> builder)
        {
            builder.ToTable("UserPreferences");

            builder.HasKey(up => up.UserId);

            builder.Property(up => up.UserId)
                .IsRequired();

            builder.Property(up => up.PreferencesJson)
                .IsRequired()
                .HasColumnType("LONGTEXT");

            builder.Property(up => up.UpdatedAt)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}