namespace Rise.Domain.WeekMenu;

public class PriceListCategory : Entity
{
    public string Name { get; set; } = null!;
    
    private PriceListCategory() { }

    public PriceListCategory(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
}