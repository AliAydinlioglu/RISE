using Rise.Shared.Common;

namespace Rise.Client.Restaurant.Components;

public class WeekmenuService(HttpClient httpClient) : IWeekmenuService
{
    // Simulates sending the Request DTO and getting the Response DTO
    public async Task<WeekMenuResponse.DayMenu> GetDayMenuAsync(WeekMenuRequest.DayMenu request)
    {
        // Simulate network delay
        await Task.Delay(500);

        // LOGIC SIMULATION:
        // 1. Determine which Resto name to show based on the ID sent
        string restoName = request.RestoID switch
        {
            1 => "Resto Campus (Requested)",
            2 => "Resto City Center (Requested)",
            _ => "Default/Favorite Resto" // Logic for when RestoID is null
        };

        // 2. Return a mock response matching the requested Date
        return new WeekMenuResponse.DayMenu
        {
            RestoName = restoName,
            StartDate = request.Date, // Echo back the requested date
            DayOfWeek = request.Date.DayOfWeek,
            MenuItemCategories = new[]
            {
                new MenuItemCategoryDTO
                {
                    CategoryName = "Dagschotel",
                    MenuItems = new[]
                    {
                        new MenuItemDTO
                        {
                            MenuItemName = $"Schotel van {request.Date.DayOfWeek}",
                            StudentPrice = 4.50,
                            Allergens = new HashSet<AllergenDTO>
                            {
                                new AllergenDTO { Name = "Gluten", Symbol = "🌾" },
                                new AllergenDTO { Name = "Crustaceans", Symbol = "🦐" },
                                new AllergenDTO { Name = "Eggs", Symbol = "🥚" },
                                new AllergenDTO { Name = "Fish", Symbol = "🐟" },
                                new AllergenDTO { Name = "Peanuts", Symbol = "🥜" },
                                new AllergenDTO { Name = "Soybeans", Symbol = "🫘" },
                                new AllergenDTO { Name = "Milk", Symbol = "🥛" },
                                new AllergenDTO { Name = "Nuts", Symbol = "🌰" },
                                new AllergenDTO { Name = "Celery", Symbol = "🥬" },
                                new AllergenDTO { Name = "Mustard", Symbol = "🌭" },
                                new AllergenDTO { Name = "Sesame", Symbol = "🫘" },
                                new AllergenDTO { Name = "Sulphites", Symbol = "🧪" },
                                new AllergenDTO { Name = "Lupin", Symbol = "🌸" },
                                new AllergenDTO { Name = "Molluscs", Symbol = "🐚" }
                            }
                        }
                    }
                }
            }
        };
    }

    public async Task<Result<List<WeekMenuResponse.DayMenu>>> GetIndexAsync(QueryRequest.SkipTake request,
        CancellationToken ctx)
    {
        return await Task.FromResult(Result.Success(GetWeeklyMenu()));
    }

    public class MenuService
    {
        // Simulates sending the Request DTO and getting the Response DTO
        public async Task<WeekMenuResponse.DayMenu> GetDayMenuAsync(WeekMenuRequest.DayMenu request)
        {
            // Simulate network delay
            await Task.Delay(500);

            // LOGIC SIMULATION:
            // 1. Determine which Resto name to show based on the ID sent
            string restoName = request.RestoID switch
            {
                1 => "Resto Campus (Requested)",
                2 => "Resto City Center (Requested)",
                _ => "Default/Favorite Resto" // Logic for when RestoID is null
            };

            // 2. Return a mock response matching the requested Date
            return new WeekMenuResponse.DayMenu
            {
                RestoName = restoName,
                StartDate = request.Date, // Echo back the requested date
                DayOfWeek = request.Date.DayOfWeek,
                MenuItemCategories = new[]
                {
                    new MenuItemCategoryDTO
                    {
                        CategoryName = "Dagschotel",
                        MenuItems = new[]
                        {
                            new MenuItemDTO
                            {
                                MenuItemName = $"Schotel van {request.Date.DayOfWeek}",
                                StudentPrice = 4.50
                            }
                        }
                    }
                }
            };
        }
    }

    public static class WeekMenuRequest
    {
        public class DayMenu
        {
            //when NULL => look at default or favorite resto    
            public int? RestoID { get; set; }

            // initially the date of today, but may be different when user chooses a specific date       
            public DateTimeOffset Date { get; set; }
        }

        // used for getting the pricelist of a resto + favoritizing a resto
        public class Resto
        {
            public int ID { get; set; }
        }
    }

    public Task<Result<ISet<RestoOverviewDTO>>> getRestoOverviews()
    {
        return Task.FromResult(Result.Success<ISet<RestoOverviewDTO>>(new HashSet<RestoOverviewDTO>
        {
            new RestoOverviewDTO
            {
                Name = "Schoonmeersen B",
                IsFavorite = true,
            },
            new RestoOverviewDTO
            {
                Name = "Schoonmeersen P",
                IsFavorite = false,
            }
        }));
    }

    public List<WeekMenuResponse.DayMenu> GetWeeklyMenu()
    {
        // 1. Define reusable Restrictions
        var gluten = new FoodRestrictionDTO { Name = "Gluten", Symbol = "🌾" };
        var lactose = new FoodRestrictionDTO { Name = "Lactose", Symbol = "🥛" };
        var vegan = new FoodRestrictionDTO { Name = "Vegan", Symbol = "🌿" };
        var veggie = new FoodRestrictionDTO { Name = "Vegetarian", Symbol = "🥕" };

        // 2. Create Monday Data
        var mondayMenu = new WeekMenuResponse.DayMenu
        {
            RestoName = "Resto Campus",
            StartDate = DateTimeOffset.Now,
            DayOfWeek = DayOfWeek.Monday,
            MenuItemCategories = new[]
            {
                new MenuItemCategoryDTO
                {
                    CategoryName = "Soep",
                    MenuItems = new[]
                    {
                        new MenuItemDTO
                        {
                            MenuItemName = "Pompoensoep",
                            StudentPrice = 1.20,
                            ExternalPrice = 2.50,
                            DayOfWeek = DayOfWeek.Monday,
                            DietaryRestrictions = new[] { vegan, veggie }
                        }
                    }
                },
                new MenuItemCategoryDTO
                {
                    CategoryName = "Hoofdgerecht",
                    MenuItems = new[]
                    {
                        new MenuItemDTO
                        {
                            MenuItemName = "Vol-au-vent met frietjes",
                            StudentPrice = 5.60,
                            ExternalPrice = 8.20,
                            DayOfWeek = DayOfWeek.Monday,
                            //  Allergens = new[] { gluten, lactose }
                        },
                        new MenuItemDTO
                        {
                            MenuItemName = "Wok met Tofu (Vegan)",
                            StudentPrice = 4.50,
                            ExternalPrice = 7.00,
                            DayOfWeek = DayOfWeek.Monday,
                            DietaryRestrictions = new[] { vegan, veggie },
                            // Allergens = new[] { gluten } // Soy sauce often has gluten
                        }
                    }
                }
            }
        };

        // 3. Create Tuesday Data
        var tuesdayMenu = new WeekMenuResponse.DayMenu
        {
            RestoName = "Resto Campus",
            StartDate = DateTimeOffset.Now.AddDays(1),
            DayOfWeek = DayOfWeek.Tuesday,
            MenuItemCategories = new[]
            {
                new MenuItemCategoryDTO
                {
                    CategoryName = "Soep",
                    MenuItems = new[]
                    {
                        new MenuItemDTO
                        {
                            MenuItemName = "Kervelsoep",
                            StudentPrice = 1.20,
                            ExternalPrice = 2.50,
                            DietaryRestrictions = new[] { veggie }
                        }
                    }
                },
                new MenuItemCategoryDTO
                {
                    CategoryName = "Hoofdgerecht",
                    MenuItems = new[]
                    {
                        new MenuItemDTO
                        {
                            MenuItemName = "Spaghetti Bolognese",
                            StudentPrice = 5.00,
                            ExternalPrice = 7.50,
                            //   Allergens = new[] { gluten }
                        }
                    }
                }
            }
        };

        return new List<WeekMenuResponse.DayMenu> { mondayMenu, tuesdayMenu };
    }
}

//temp tot backend in orde komt
public interface IWeekmenuService
{
    Task<Result<List<WeekMenuResponse.DayMenu>>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx);
    Task<WeekMenuResponse.DayMenu> GetDayMenuAsync(WeekmenuService.WeekMenuRequest.DayMenu request);
    Task<Result<ISet<RestoOverviewDTO>>> getRestoOverviews();
}

public static class WeekMenuResponse
{
    public class DayMenu
    {
        public string RestoName { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public MenuItemCategoryDTO[] MenuItemCategories { get; set; } = [];
    }

    public class RestoOverview
    {
        public RestoOverviewDTO[] Restos { get; set; } = [];
    }

    public class Pricelist
    {
        public MenuItemCategoryDTO[] PricelistCategories { get; set; }
    }
}

public class MenuItemCategoryDTO
{
    public string CategoryName { get; set; }
    public MenuItemDTO[] MenuItems { get; set; } = [];
}

public class MenuItemDTO
{
    public string MenuItemName { get; set; }
    public double? StudentPrice { get; set; }
    public double? ExternalPrice { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public ISet<AllergenDTO> Allergens { get; set; } = new HashSet<AllergenDTO>();
    public FoodRestrictionDTO[] DietaryRestrictions { get; set; } = [];
}

public class FoodRestrictionDTO
{
    public string Name { get; set; }
    public string Symbol { get; set; } // e.g., "🥜", "V"
}

public class RestoOverviewDTO
{
    public int RestoId { get; set; }

    public string Name { get; set; }

    // public LocationDTO Location { get; set; } // Commented out as I don't have this class definition
    public RestoOpeningHourDTO[] OpeningHours { get; set; } = []; // Added property Name
    public bool IsFavorite { get; set; }
}

public class RestoOpeningHourDTO
{
    public TimeSpan From { get; set; }
    public TimeSpan To { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}

public class AllergenDTO
{
    public string Name { get; set; }
    public string Symbol { get; set; } // e.g., "🥜"
}