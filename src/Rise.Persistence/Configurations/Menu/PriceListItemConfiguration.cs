using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Menu;

namespace Rise.Persistence.Configurations.Menu;

internal class PriceListItemConfiguration : EntityConfiguration<PriceListItem>
{
    public override void Configure(EntityTypeBuilder<PriceListItem> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IsHighlighted).IsRequired();
        builder.Property(x => x.HasCategoryRemark).IsRequired();

        builder.OwnsOne(x => x.Price, price =>
        {
            price.Property(p => p.Student)
                .HasColumnName("StudentPrice")
                .HasPrecision(5, 2)
                .IsRequired(); 

            price.Property(p => p.Extern)
                .HasColumnName("ExternPrice")
                .HasPrecision(5, 2); 
        });
        
        builder.HasOne(pli => pli.PriceListCategory)
            .WithMany()
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}