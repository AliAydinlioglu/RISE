namespace Rise.Domain.Menu;

public class PriceListCategory : Entity
{
    public string Name { get; set; } = null!;
    public string? Remark { get; set; } 
    
    private PriceListCategory() { }

    public PriceListCategory(string name, string? remark)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Remark = remark;
    }
}