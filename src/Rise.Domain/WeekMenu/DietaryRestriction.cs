namespace Rise.Domain.WeekMenu;

public class DietaryRestriction : Entity, IFoodRestriction
{
    public string Name { get; set; } = null!;
    public string Symbol { get; set; } = null!;

    private DietaryRestriction() { }
    
    public DietaryRestriction(string name, string symbol)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Symbol = Guard.Against.NullOrWhiteSpace(symbol);
    }
}