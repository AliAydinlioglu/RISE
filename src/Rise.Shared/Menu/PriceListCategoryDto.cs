namespace Rise.Shared.Menu;

public class PriceListCategoryDto
{
    public string CategoryName { get; set; } = null!;
    public PriceListItemDto[] PriceListItems { get; set; } = [];
}