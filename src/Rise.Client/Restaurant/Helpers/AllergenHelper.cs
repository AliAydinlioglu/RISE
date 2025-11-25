using Rise.Client.Restaurant.Enums;

namespace Rise.Client.Restaurant.Helpers;

public static class AllergenHelper
{
    public static string GetAllergenImage(Allergen allergen) =>
        allergen switch
        {
            Allergen.Gluten => "/Images/allergens/gluten.webp",
            Allergen.Schaaldieren => "/Images/allergens/crustaceans.webp",
            Allergen.Eieren => "/Images/allergens/egg.webp",
            Allergen.Vis => "/Images/allergens/fish.webp",
            Allergen.Pinda => "/Images/allergens/peanuts.webp",
            Allergen.Soja => "/Images/allergens/soyabeans.webp",
            Allergen.Melk => "/Images/allergens/milk.webp",
            Allergen.Lactose => "/Images/allergens/milk.webp",
            Allergen.Noten => "/Images/allergens/treenuts.webp",
            Allergen.Selderij => "/Images/allergens/celery.webp",
            Allergen.Mosterd => "/Images/allergens/mustard.webp",
            Allergen.Sesam => "/Images/allergens/sesame.webp",
            Allergen.Sulfiet => "/Images/allergens/sulphites.webp",
            Allergen.Lupine => "/Images/allergens/lupin.webp",
            Allergen.Weekdieren => "/Images/allergens/molluscs.webp",
            _ => "/Images/allergens/default.webp"
        };

    public static string GetAllergenName(Allergen allergen) =>
        allergen switch
        {
            Allergen.Gluten => "Gluten",
            Allergen.Schaaldieren => "Schaaldieren",
            Allergen.Eieren => "Eieren",
            Allergen.Vis => "Vis",
            Allergen.Pinda => "Pinda's",
            Allergen.Soja => "Soja",
            Allergen.Melk => "Melk",
            Allergen.Lactose => "Lactose",
            Allergen.Noten => "Noten",
            Allergen.Selderij => "Selderij",
            Allergen.Mosterd => "Mosterd",
            Allergen.Sesam => "Sesam",
            Allergen.Sulfiet => "Sulfieten",
            Allergen.Lupine => "Lupine",
            Allergen.Weekdieren => "Weekdieren",
            _ => allergen.ToString()
        };
}