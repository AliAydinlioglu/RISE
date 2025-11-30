using Rise.Domain.Menu;
using Rise.Shared.Menu;

namespace Rise.Services.Menu;

public static class MenuExtensions
{   
    public static MenuItemDto ToMenuItemDto(this MenuItem mi)
    {
        return new MenuItemDto
        {
            Name = mi.Name,
            StudentPrice = mi.Price?.Student,
            ExternalPrice = mi.Price?.Extern,
            Allergens = mi.Allergens.Select(ToFoodRestrictionDto).ToArray(),
            DietaryRestrictions = mi.DietaryRestrictions.Select(ToFoodRestrictionDto).ToArray()
        };
    }

    public static PriceListItemDto ToPriceListItemDto(this PriceListItem pli)
    {
        return new PriceListItemDto
        {
            Name = pli.Name,
            StudentPrice = pli.Price.Student,
            ExternalPrice = pli.Price.Extern,
            IsHighlighted = pli.IsHighlighted,
            HasCategoryRemark = pli.HasCategoryRemark
        };
    }

    private static FoodRestrictionDto ToFoodRestrictionDto(this FoodRestriction fr)
    {
        return new FoodRestrictionDto
        {
            Name = fr.Name,
            Symbol = fr.Symbol
        };
    }
}