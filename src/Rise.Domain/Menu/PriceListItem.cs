namespace Rise.Domain.Menu;

public class PriceListItem : Entity
{
    public string Name { get; set; } = null!;
    public bool IsHighlighted { get; set; }
    public bool HasCategoryRemark { get; set; }

    public Price Price { get; set; } = null!;
    public PriceListCategory PriceListCategory { get; init; } = null!;
    
    private PriceListItem() { }

    public PriceListItem(
        string name, 
        Price price, 
        PriceListCategory priceListCategory,
        bool isHighlighted = false, 
        bool hasCategoryRemark = false)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Price = Guard.Against.Null(price);
        PriceListCategory = Guard.Against.Null(priceListCategory);
        IsHighlighted = isHighlighted;
        HasCategoryRemark = hasCategoryRemark;
    }
}