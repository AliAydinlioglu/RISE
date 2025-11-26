namespace Rise.Shared.Menu;

public class PriceListCategoryDto
{
    public string CategoryName { get; set; } = null!;
    public string? Remark { get; set; }
    public PriceListItemDto[] PriceListItems { get; set; } = [];
}