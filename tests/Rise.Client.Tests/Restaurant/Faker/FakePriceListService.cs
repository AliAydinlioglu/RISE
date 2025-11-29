using Ardalis.Result;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

public class FakePriceListService : IPriceListService
{
    public enum ModeType
    {
        SuccessWithData,
        SuccessWithNullCategories,
        NotFound,
        Error
    }

    public static ModeType Mode { get; set; } = ModeType.SuccessWithData;

    public async Task<Result<MenuResponse.Pricelist>> GetForRestoAsync(MenuRequest.Resto req, CancellationToken ct)
    {
        return Mode switch
        {
            ModeType.SuccessWithData => Result<MenuResponse.Pricelist>.Success(new MenuResponse.Pricelist
            {
                PricelistCategories = GetPricelistCategories()
            }),
            ModeType.SuccessWithNullCategories => Result<MenuResponse.Pricelist>.Success(new MenuResponse.Pricelist
            {
                PricelistCategories = null!
            }),
            ModeType.NotFound => throw new HttpRequestException("Not Found", null, System.Net.HttpStatusCode.NotFound),
            ModeType.Error => throw new HttpRequestException("Internal Server Error", null, System.Net.HttpStatusCode.InternalServerError),
            _ => Result<MenuResponse.Pricelist>.Success(new MenuResponse.Pricelist
            {
                PricelistCategories = GetPricelistCategories()
            })
        };
    }

    private static PriceListCategoryDto[] GetPricelistCategories()
    {
        return new PriceListCategoryDto[]
        {
            new PriceListCategoryDto
            {
                CategoryName = "Soepen",
                Remark = "Prijs per kom",
                PriceListItems = new PriceListItemDto[]
                {
                    new PriceListItemDto
                    {
                        Name = "Tomatensoep",
                        StudentPrice = 2.50m,
                        ExternalPrice = 3.00m,
                        IsHighlighted = false,
                        HasCategoryRemark = false
                    },
                    new PriceListItemDto
                    {
                        Name = "Kippensoep",
                        StudentPrice = 2.75m,
                        ExternalPrice = 3.25m,
                        IsHighlighted = true,
                        HasCategoryRemark = true
                    }
                }
            },
            new PriceListCategoryDto
            {
                CategoryName = "Hoofdgerechten",
                Remark = null,
                PriceListItems = new PriceListItemDto[]
                {
                    new PriceListItemDto
                    {
                        Name = "Spaghetti Bolognese",
                        StudentPrice = 5.50m,
                        ExternalPrice = 7.00m,
                        IsHighlighted = false,
                        HasCategoryRemark = false
                    },
                    new PriceListItemDto
                    {
                        Name = "Vegetarische Lasagne",
                        StudentPrice = 6.00m,
                        ExternalPrice = 7.50m,
                        IsHighlighted = false,
                        HasCategoryRemark = false
                    }
                }
            }
        };
    }
}

