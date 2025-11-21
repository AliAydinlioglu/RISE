namespace Rise.Domain.Menu;

public class PriceListItem : Entity
{
    public string Name { get; set; } = null!;
    public decimal StudentPrice { get; set; }
    public decimal? ExternPrice { get; set; }
    public bool IsHighlighted { get; set; }
    public bool HasCategoryRemark { get; set; }

    public PriceListCategory PriceListCategory { get; init; } = null!;
    
    private PriceListItem() { }

    public PriceListItem(
        string name, 
        decimal studentPrice, 
        decimal? externPrice, 
        PriceListCategory priceListCategory,
        bool isHighlighted = false, 
        bool hasCategoryRemark = false)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        StudentPrice = Guard.Against.NegativeOrZero(studentPrice);
        ExternPrice = externPrice != null ? Guard.Against.NegativeOrZero(externPrice.Value) : null;
        PriceListCategory = Guard.Against.Null(priceListCategory);
        IsHighlighted = isHighlighted;
        HasCategoryRemark = hasCategoryRemark;
    }
}