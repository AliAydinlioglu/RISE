using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Calendar;

namespace Rise.Persistence.Configurations.Calendar;

internal class LecturerConfiguration : EntityConfiguration<Lecturer>
{
    public override void Configure(EntityTypeBuilder<Lecturer> builder)
    {
        base.Configure(builder);
        
        builder.Property(it => it.FirstName).IsRequired();
        builder.Property(it => it.LastName).IsRequired();
        
        builder.HasIndex(it => new { it.FirstName, it.LastName }).IsUnique();
    }
}