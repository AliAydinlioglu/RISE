namespace Rise.Shared.Menu;

public class MenuItemDto
{
    public string MenuItemName { get; set; } = null!;
    public double? StudentPrice { get; set; }
    public double? ExternalPrice { get; set; } 
    public FoodRestrictionDto[] Allergens { get; set; } = [];
    public FoodRestrictionDto[] DietaryRestrictions { get; set; } = [];     
}
