using Microsoft.AspNetCore.Components;

namespace Rise.Client.Restaurant.Components;

public partial class AllergenList : ComponentBase
{
    [Parameter] public required ISet<AllergenDTO> Allergens { get; set;  }
    
    private string getAllergenImage(Allergen allergen) =>
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
    
    public enum Allergen
    {
        Gluten,
        Crustaceans,
        Eggs,
        Fish,
        Peanuts,
        Soybeans,
        Milk,
        Nuts,
        Celery,
        Mustard,
        Sesame,
        Sulphites,
        Lupin,
        Molluscs
    }
}