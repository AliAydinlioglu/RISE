using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.StudentActivities;

namespace Rise.Persistence.Configurations.StudentActivities;

internal class StudentClubConfiguration : EntityConfiguration<StudentClub>
{
    public override void Configure(EntityTypeBuilder<StudentClub> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(250);
        builder.Property(x => x.Description);
        builder.Property(x => x.LogoUrl).HasMaxLength(2048);
    }
}