using Ardalis.Result;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

public class FakeRestaurantService : IMenuService
{
    public enum ModeType
    {
        SuccessWithData,
        SuccessWithNullMenuItems,
        NotFound,
        Error
    }

    public static ModeType Mode { get; set; } = ModeType.SuccessWithData;



    public async Task<Result<MenuResponse.DayMenu>> GetDayMenuAsync(MenuRequest.DayMenu req, CancellationToken ct)
    {
        return Mode switch
        {
            ModeType.SuccessWithData => Result<MenuResponse.DayMenu>.Success(new MenuResponse.DayMenu
            {
                MenuItemCategories = GetDayMenu()
            }),
            ModeType.SuccessWithNullMenuItems => Result<MenuResponse.DayMenu>.Success(new MenuResponse.DayMenu
            {
                MenuItemCategories = null!
            }),
            ModeType.NotFound => throw new HttpRequestException("Not Found", null, System.Net.HttpStatusCode.NotFound),
            ModeType.Error => throw new HttpRequestException("Internal Server Error", null, System.Net.HttpStatusCode.InternalServerError),
            _ => Result<MenuResponse.DayMenu>.Success(new MenuResponse.DayMenu
            {
                MenuItemCategories = GetDayMenu()
            })
        };
    }


    private static MenuItemCategoryDto[] GetDayMenu()
    {
        return new MenuItemCategoryDto[]
        {
            new MenuItemCategoryDto
            {
                CategoryName = "Soepen",
                MenuItems = new MenuItemDto[]
                {
                    new MenuItemDto
                    {
                        Name = "test soep 1",
                        ExternalPrice = 5.49m,
                        StudentPrice = 4.99m,
                        Allergens = GetAllergens(),
                        DietaryRestrictions = new FoodRestrictionDto[]
                        {
                            new FoodRestrictionDto { Name = "Vegetarisch", Symbol = "V" },
                        }
                    },
                    new MenuItemDto
                    {
                        Name = "test soep 2",
                        ExternalPrice = 5.49m,
                        StudentPrice = 4.99m,
                        Allergens = GetAllergens(),
                        DietaryRestrictions = new FoodRestrictionDto[]
                        {
                            new FoodRestrictionDto { Name = "Veganistisch", Symbol = "VG" },
                        }
                    }
                }
            },
            new MenuItemCategoryDto
            {
                CategoryName = "Hoofdgerechten",
                MenuItems = new MenuItemDto[]
                {
                    new MenuItemDto
                    {
                        Name = "Hoofdgerecht 1",
                        ExternalPrice = 5.49m,
                        StudentPrice = 4.99m,
                        Allergens = GetAllergens(),
                        DietaryRestrictions = new FoodRestrictionDto[]
                        {
                            new FoodRestrictionDto { Name = "Vegetarisch", Symbol = "V" },
                        }
                    },
                    new MenuItemDto
                    {
                        Name = "Hoofdgerecht 2",
                        ExternalPrice = 5.49m,
                        StudentPrice = 4.99m,
                        Allergens = GetAllergens(),
                        DietaryRestrictions = new FoodRestrictionDto[]
                        {
                            new FoodRestrictionDto { Name = "Veganistisch", Symbol = "VG" },
                        }
                    }
                }
            }
        };
    }

    private static FoodRestrictionDto[] GetAllergens()
    {
        return new FoodRestrictionDto[]
        {
            new FoodRestrictionDto { Name = "Gluten", Symbol = "G" },
            new FoodRestrictionDto { Name = "Lactose", Symbol = "L" },
            new FoodRestrictionDto { Name = "Noten", Symbol = "N" }
        };
    }
}