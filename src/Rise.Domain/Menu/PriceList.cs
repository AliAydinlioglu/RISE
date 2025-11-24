namespace Rise.Domain.Menu;

public class PriceList : Entity
{
    public string Name { get; set; } = null!;
    
    private readonly List<PriceListItem> _priceListItems = [];
    public IReadOnlyList<PriceListItem> PriceListItems => _priceListItems.AsReadOnly();
    
    private PriceList() { }

    public PriceList(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }

    public void AddPriceListItems(IEnumerable<PriceListItem> priceListItems)
    {
        foreach (var priceListItem in priceListItems)
        {
            _priceListItems.Add(Guard.Against.Null(priceListItem));
        }
    }
}