using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Menu;

namespace Rise.Persistence.Configurations.Menu;

internal class MenuItemConfiguration : EntityConfiguration<MenuItem>
{
    public override void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
       
        builder.OwnsOne(x => x.Price, price =>
        {
            price.Property(p => p.Student)
                .HasColumnName("StudentPrice")
                .HasPrecision(5, 2); 

            price.Property(p => p.Extern)
                .HasColumnName("ExternPrice")
                .HasPrecision(5, 2); 
        });
        
        builder.HasOne(mi => mi.MenuCategory)
            .WithMany()
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasMany(mi => mi.Allergens)
            .WithMany()
            .UsingEntity("MenuItemAllergen");
        
        builder.HasMany(mi => mi.DietaryRestrictions)
            .WithMany()
            .UsingEntity("MenuItemDietaryRestriction");
    }
}