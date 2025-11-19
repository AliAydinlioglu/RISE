namespace Rise.Domain.WeekMenu;

public class Allergen : Entity, IFoodRestriction
{
    public string Name { get; set; } = null!;
    public string Symbol { get; set; } = null!;

    private Allergen() { }

    public Allergen(string name, string symbol)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Symbol = Guard.Against.NullOrWhiteSpace(symbol);
    }
}