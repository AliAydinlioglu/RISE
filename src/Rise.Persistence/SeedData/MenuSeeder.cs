using Rise.Domain.Locations;
using Rise.Domain.Menu;

namespace Rise.Persistence.SeedData;

public static class MenuSeeder
{
    public static async Task Seed(ApplicationDbContext dbContext)
    {
        if (dbContext.Restos.Any(r => r.Name.Contains("Schoonmeersen")))
            return;

        // --- Allergens ---
        var gluten = new Allergen("Gluten", "G");
        var lactose = new Allergen("Lactose", "L");
        var nuts = new Allergen("Noten", "N");
        dbContext.Allergens.AddRange(gluten, lactose, nuts);

        // --- Dietary Restrictions ---
        var vegetarian = new DietaryRestriction("Vegetarisch", "V");
        var vegan = new DietaryRestriction("Veganistisch", "VG");
        var glutenFree = new DietaryRestriction("Glutenvrij", "GF");
        dbContext.DietaryRestrictions.AddRange(vegetarian, vegan, glutenFree);

        // --- PriceList ---
        var drinks = new PriceListCategory("Dranken", null);
        var snacks = new PriceListCategory("Snacks", null);
        var mains = new PriceListCategory("Hoofdgerechten", null);

        var priceList = new PriceList("Schoonmeersen Prijslijst");
        priceList.AddPriceListItems(new List<PriceListItem>
        {
            new PriceListItem("Coca-Cola", 1.5m, 2.0m, drinks),
            new PriceListItem("Broodje Kaas", 2.5m, 3.0m, snacks),
            new PriceListItem("Pasta Bolognese", 5.0m, 6.0m, mains)
        });
        dbContext.PriceLists.Add(priceList);
        await dbContext.SaveChangesAsync();

        // --- Locations ---
        var locB = new Location("Schoonmeersen B", "Valentin Vaerwyckweg", 1, 9000, "Gent", "B");
        var locD = new Location("Schoonmeersen D", "Valentin Vaerwyckweg", 1, 9000, "Gent", "D");
        var locP = new Location("Schoonmeersen P", "Valentin Vaerwyckweg", 1, 9000, "Gent", "P");

        // --- Restos ---
        var restos = new List<Resto>
        {
            new Resto("Schoonmeersen B", locB, priceList),
            new Resto("Schoonmeersen D", locD, priceList),
            new Resto("Schoonmeersen P", locP, priceList)
        };
        dbContext.Restos.AddRange(restos);
        await dbContext.SaveChangesAsync();

        // --- Menus for 2 weeks ---
        var rnd = new Random(42);
        foreach (var resto in restos)
        {
            for (int dayOffset = 0; dayOffset < 14; dayOffset++)
            {
                var menuDate = DateTimeOffset.Now.Date.AddDays(dayOffset);
                var menu = new Menu(menuDate, resto);

                // 3 menu items per menu
                var menuItems = new List<MenuItem>
                {
                    new MenuItem($"Soep dag {dayOffset+1}", new MenuCategory("Soepen"), 2.50m, 3.50m),
                    new MenuItem($"Hoofdgerecht dag {dayOffset+1}", new MenuCategory("Hoofdgerechten"), 5.00m, 6.50m),
                    new MenuItem($"Dessert dag {dayOffset+1}", new MenuCategory("Desserts"), 3.00m, 4.00m)
                };

                // Random allergen & dietary restriction
                foreach (var item in menuItems)
                {
                    if (rnd.Next(2) == 0) item.AddAllergens(new List<Allergen> { gluten });
                    if (rnd.Next(2) == 0) item.AddAllergens(new List<Allergen> { lactose });
                    if (rnd.Next(2) == 0) item.AddAllergens(new List<Allergen> { nuts });

                    if (rnd.Next(2) == 0) item.AddDietaryRestrictions(new List<DietaryRestriction> { vegetarian });
                    if (rnd.Next(2) == 0) item.AddDietaryRestrictions(new List<DietaryRestriction> { vegan });
                    if (rnd.Next(2) == 0) item.AddDietaryRestrictions(new List<DietaryRestriction> { glutenFree });
                }

                menu.AddWeekMenuItems(menuItems);
                resto.AddMenus(new List<Menu> { menu });
            }
        }

        await dbContext.SaveChangesAsync();
    }
}