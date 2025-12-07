using Rise.Client.Restaurant.Enums;

namespace Rise.Client.Restaurant.Helpers;

public static class AllergenHelper
{
    public static string GetAllergenImage(Allergen allergen) =>
        allergen switch
        {
            Allergen.Gluten => "/img/allergens/gluten.webp",
            Allergen.Schaaldieren => "/img/allergens/crustaceans.webp",
            Allergen.Eieren => "/img/allergens/egg.webp",
            Allergen.Vis => "/img/allergens/fish.webp",
            Allergen.Pinda => "/img/allergens/peanuts.webp",
            Allergen.Soja => "/img/allergens/soyabeans.webp",
            Allergen.Melk => "/img/allergens/milk.webp",
            Allergen.Lactose => "/img/allergens/milk.webp",
            Allergen.Noten => "/img/allergens/treenuts.webp",
            Allergen.Selderij => "/img/allergens/celery.webp",
            Allergen.Mosterd => "/img/allergens/mustard.webp",
            Allergen.Sesam => "/img/allergens/sesame.webp",
            Allergen.Sulfiet => "/img/allergens/sulphites.webp",
            Allergen.Lupine => "/img/allergens/lupin.webp",
            Allergen.Weekdieren => "/img/allergens/molluscs.webp",
            _ => "/img/allergens/default.webp"
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