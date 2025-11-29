namespace Rise.Domain.Menu;

public abstract class FoodRestriction : Entity
{
    public string Name { get; set; } = null!;
    public string Symbol { get; set; } = null!;

    private FoodRestriction() { }

    protected FoodRestriction(string name, string symbol)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Symbol = Guard.Against.NullOrWhiteSpace(symbol);
    }
}