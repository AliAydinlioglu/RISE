using MudBlazor;
using Rise.Client.Restaurant.Enums;

namespace Rise.Client.Restaurant.Helpers;

public static class DietaryRestrictionHelper
{
    public static string GetDietaryRestrictionIcon(string dietaryRestriction)
    {
        return dietaryRestriction switch
        {
            "Veganistisch" => @Icons.Material.Filled.ExpandCircleDown,
            "Vegetarisch" => @Icons.Material.Filled.ExpandCircleDown,
            _ => ""
        };
    }
    
}