namespace Rise.Domain.Menu;

public class PriceListCategory : Entity
{
    public string Name { get; init; } = null!;
    public string? Remark { get; init; } 
    
    private PriceListCategory() { }

    public PriceListCategory(string name, string? remark)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Remark = remark;
    }
}