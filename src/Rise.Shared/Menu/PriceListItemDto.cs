namespace Rise.Shared.Menu;

public class PriceListItemDto
{
    public string Name { get; set; } = null!;
    public decimal StudentPrice { get; set; }
    public decimal? ExternalPrice { get; set; } 
}