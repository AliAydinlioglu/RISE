namespace Rise.Shared.Menu;

public class PriceListItemDto
{
    public string Name { get; set; } = null!;
    public double StudentPrice { get; set; }
    public double? ExternalPrice { get; set; } 
}