using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Calendar;

namespace Rise.Persistence.Configurations.Calendar;

internal class CourseConfiguration: EntityConfiguration<Course>
{
    public override void Configure(EntityTypeBuilder<Course> builder)
    {
        base.Configure(builder);

        builder.Property(it => it.Title).IsRequired().HasMaxLength(250);
        builder.Property(it => it.Lecturer).IsRequired().HasMaxLength(250);
        builder.Property(it => it.ClassGroup).IsRequired().HasMaxLength(50);
        
        builder.HasMany(it => it.Lessons)
            .WithOne(it => it.Course)
            .HasForeignKey(it => it.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(it => it.Deadlines)
            .WithOne(it => it.Course)
            .HasForeignKey(it => it.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(it => it.Exams)
            .WithOne(it => it.Course)
            .HasForeignKey(it => it.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}