using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.News;

namespace Rise.Persistence.Configurations.News;

internal class NewsItemConfiguration : EntityConfiguration<NewsItem>
{
    public override void Configure(EntityTypeBuilder<NewsItem> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Summary).IsRequired().HasMaxLength(500);
        
        builder.Property("_contentSections")
            .HasColumnName("ContentSections")
            .HasColumnType("json")
            .IsRequired();
        
        builder.Property("_imageUrls")
            .HasColumnName("ImageUrls")
            .HasColumnType("json")
            .IsRequired(false);
        
        builder.Property(x => x.PublishedAt).IsRequired().HasColumnType("datetime");
    }
}

