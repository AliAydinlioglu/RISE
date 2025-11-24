namespace Rise.Shared.Menu;

public class MenuItemDto
{
    public string Name { get; set; } = null!;
    public decimal? StudentPrice { get; set; }
    public decimal? ExternalPrice { get; set; } 
    public FoodRestrictionDto[] Allergens { get; set; } = [];
    public FoodRestrictionDto[] DietaryRestrictions { get; set; } = [];     
}
