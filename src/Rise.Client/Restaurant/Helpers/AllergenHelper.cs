using Rise.Client.Restaurant.Enums;

namespace Rise.Client.Restaurant.Helpers;


public static class AllergenHelper
{
    public static string GetAllergenImage(Allergen allergen) =>
        allergen switch
        {
            Allergen.Gluten => "/Images/allergens/gluten.webp",
            Allergen.Crustaceans => "/Images/allergens/crustaceans.webp",
            Allergen.Eggs => "/Images/allergens/egg.webp",
            Allergen.Fish => "/Images/allergens/fish.webp",
            Allergen.Peanuts => "/Images/allergens/peanuts.webp",
            Allergen.Soybeans => "/Images/allergens/soyabeans.webp",
            Allergen.Milk => "/Images/allergens/milk.webp",
            Allergen.Nuts => "/Images/allergens/treenuts.webp",
            Allergen.Celery => "/Images/allergens/celery.webp",
            Allergen.Mustard => "/Images/allergens/mustard.webp",
            Allergen.Sesame => "/Images/allergens/sesame.webp",
            Allergen.Sulphites => "/Images/allergens/sulphites.webp",
            Allergen.Lupin => "/Images/allergens/lupin.webp",
            Allergen.Molluscs => "/Images/allergens/molluscs.webp",
            _ => "/Images/allergens/default.webp"
        };
    
    public static string GetAllergenName(Allergen allergen) =>
        allergen switch
        {
            Allergen.Gluten => "Gluten",
            Allergen.Crustaceans => "Schaaldieren",
            Allergen.Eggs => "Eieren",
            Allergen.Fish => "Vis",
            Allergen.Peanuts => "Pinda's",
            Allergen.Soybeans => "Soja",
            Allergen.Milk => "Melk",
            Allergen.Nuts => "Noten",
            Allergen.Celery => "Selderij",
            Allergen.Mustard => "Mosterd",
            Allergen.Sesame => "Sesam",
            Allergen.Sulphites => "Sulfieten",
            Allergen.Lupin => "Lupine",
            Allergen.Molluscs => "Weekdieren",
            _ => allergen.ToString()
        };
}