using Rise.Domain.Common;
using Rise.Domain.Locations;
using Rise.Domain.Menu;

namespace Rise.Persistence.SeedData;

public static class MenuSeeder
{
    public static async Task Seed(ApplicationDbContext dbContext)
    {
        if (dbContext.Restos.Any(r => r.Name.Contains("Ledeganck") || r.Name.Contains("Mercator") || r.Name.Contains("Schoonmeersen") || r.Name.Contains("Bijloke")))
            return;

        // --- Allergens ---
        var gluten = new Allergen("Gluten", "G");
        var schaaldieren = new Allergen("Schaaldieren", "S");
        var eieren = new Allergen("Eieren", "E");
        var vis = new Allergen("Vis", "V");
        var pinda = new Allergen("Pinda", "P");
        var soja = new Allergen("Soja", "So");
        var melk = new Allergen("Melk", "M");
        var lactose = new Allergen("Lactose", "L");
        var noten = new Allergen("Noten", "N");
        var selderij = new Allergen("Selderij", "Se");
        var mosterd = new Allergen("Mosterd", "Mo");
        var sesam = new Allergen("Sesam", "Ss");
        var sulfiet = new Allergen("Sulfiet", "Su");
        var lupine = new Allergen("Lupine", "Lu");
        var weekdieren = new Allergen("Weekdieren", "W");
        dbContext.Allergens.AddRange(gluten, schaaldieren, eieren, vis, pinda, soja, melk, lactose, noten, selderij, mosterd, sesam, sulfiet, lupine, weekdieren);

        // --- Dietary Restrictions ---
        var vegetarian = new DietaryRestriction("Vegetarisch", "V");
        var vegan = new DietaryRestriction("Veganistisch", "VG");
        dbContext.DietaryRestrictions.AddRange(vegetarian, vegan);

        // --- PriceList ---
        // Breakfast and Snacks
        var ontbijtEnSnacks = new PriceListCategory("Ontbijt en Snacks", null);
        var ontbijtItems = new List<PriceListItem>
        {
            new PriceListItem("Ontbijtkoek", Price.ForPriceListItem(1.20m, 1.65m), ontbijtEnSnacks),
            new PriceListItem("Stokbrood belegd", Price.ForPriceListItem(2.40m, 3.30m), ontbijtEnSnacks),
            new PriceListItem("Piccolo belegd", Price.ForPriceListItem(1.70m, 2.25m), ontbijtEnSnacks),
            new PriceListItem("Sandwich belegd", Price.ForPriceListItem(1.70m, 2.25m), ontbijtEnSnacks),
            new PriceListItem("Stokbroodje onbelegd", Price.ForPriceListItem(1.00m, 1.25m), ontbijtEnSnacks),
            new PriceListItem("Piccolo onbelegd", Price.ForPriceListItem(0.50m, 0.70m), ontbijtEnSnacks),
            new PriceListItem("Beleg: kleine portie", Price.ForPriceListItem(1.30m, 1.80m), ontbijtEnSnacks),
            new PriceListItem("Beleg: grote portie", Price.ForPriceListItem(1.60m, 2.10m), ontbijtEnSnacks)
        };

        // Warm Meals
        var warmeMaaltijden = new PriceListCategory("Warme Maaltijden", 
            "*Maaltijd omvat:\n- portie vlees, vis, veggie of vegan\n- portie zetmeel\n- portie warme groenten of rauwkost");
        var maaltijdItems = new List<PriceListItem>
        {
            new PriceListItem("Soep", Price.ForPriceListItem(1.00m, 2.15m), warmeMaaltijden),
            new PriceListItem("Warme maaltijd veggie/vegan", Price.ForPriceListItem(4.99m, 11.00m), warmeMaaltijden, false, true),
            new PriceListItem("Warme maaltijd vlees/vis", Price.ForPriceListItem(5.30m, 11.70m), warmeMaaltijden, false, true),
            new PriceListItem("Maaltijdsalade veggie/vegan", Price.ForPriceListItem(4.99m, 11.00m), warmeMaaltijden, false, true),
            new PriceListItem("Maaltijdsalade vlees/vis", Price.ForPriceListItem(5.30m, 11.70m), warmeMaaltijden, false, true),
            new PriceListItem("Picadeli salade per 100gr", Price.ForPriceListItem(1.30m, 1.80m), warmeMaaltijden, true),
            new PriceListItem("Portie warme groenten", Price.ForPriceListItem(1.50m, 3.30m), warmeMaaltijden),
            new PriceListItem("Portie rauwkost (100 gr)", Price.ForPriceListItem(1.50m, 3.30m), warmeMaaltijden),
            new PriceListItem("Sausje (mayonaise, ketchup,...)", Price.ForPriceListItem(0.50m, 1.10m), warmeMaaltijden),
            new PriceListItem("Portie aardappelen, rijst,...", Price.ForPriceListItem(1.80m, 2.65m), warmeMaaltijden),
            new PriceListItem("Portie friet", Price.ForPriceListItem(2.40m, 3.60m), warmeMaaltijden)
        };

        // Desserts
        var desserts = new PriceListCategory("Desserts", null);
        var dessertItems = new List<PriceListItem>
        {
            new PriceListItem("Stuk fruit", Price.ForPriceListItem(0.60m, 0.80m), desserts),
            new PriceListItem("Fruitsla", Price.ForPriceListItem(1.20m, 1.65m), desserts),
            new PriceListItem("Yoghurt, pudding, rijstpap", Price.ForPriceListItem(1.00m, 1.30m), desserts),
            new PriceListItem("Tiramisu, chocomousse, speculoosmousse", Price.ForPriceListItem(1.80m, 2.45m), desserts),
            new PriceListItem("Muffin, donut, suikerwafel", Price.ForPriceListItem(1.60m, 2.10m), desserts),
            new PriceListItem("Cookie", Price.ForPriceListItem(1.20m, 1.65m), desserts),
            new PriceListItem("Snoep", Price.ForPriceListItem(1.25m, 1.70m), desserts)
        };

        // Drinks
        var dranken = new PriceListCategory("Dranken", null);
        var drankenItems = new List<PriceListItem>
        {
            new PriceListItem("Water 50 cl", Price.ForPriceListItem(1.20m, 1.20m), dranken),
            new PriceListItem("Water 1 lit", Price.ForPriceListItem(1.70m, 1.70m), dranken),
            new PriceListItem("Gearomatiseerd water 50 cl", Price.ForPriceListItem(1.80m, 1.80m), dranken),
            new PriceListItem("Frisdrank gesuikerd 50cl", Price.ForPriceListItem(1.95m, 1.95m), dranken),
            new PriceListItem("Frisdrank zero 50cl", Price.ForPriceListItem(1.85m, 1.85m), dranken),
            new PriceListItem("Frisdrank Tönissteiner 33cl", Price.ForPriceListItem(1.65m, 1.65m), dranken),
            new PriceListItem("Fruitsap Tönissteiner 50cl", Price.ForPriceListItem(2.20m, 2.20m), dranken),
            new PriceListItem("TAO", Price.ForPriceListItem(1.80m, 1.80m), dranken),
            new PriceListItem("Bionina", Price.ForPriceListItem(2.10m, 2.10m), dranken),
            new PriceListItem("Nalu of Red Bull", Price.ForPriceListItem(2.20m, 2.20m), dranken),
            new PriceListItem("Fristi", Price.ForPriceListItem(1.70m, 1.70m), dranken),
            new PriceListItem("Koude chocomelk", Price.ForPriceListItem(1.20m, 1.20m), dranken),
            new PriceListItem("Warme chocomelk", Price.ForPriceListItem(1.60m, 1.60m), dranken),
            new PriceListItem("Koffie/thee", Price.ForPriceListItem(1.50m, 1.50m), dranken)
        };

        var priceList = new PriceList("HOGENT Resto Prijslijst");
        var allItems = new List<PriceListItem>();
        allItems.AddRange(ontbijtItems);
        allItems.AddRange(maaltijdItems);
        allItems.AddRange(dessertItems);
        allItems.AddRange(drankenItems);
        priceList.AddPriceListItems(allItems);
        
        dbContext.PriceLists.Add(priceList);
        await dbContext.SaveChangesAsync();

        // --- Locations ---
        var locLedeganck = new Location("Ledeganck", "K.L. Ledeganckstraat", 35, 9000, "Gent", "Ledeganck");
        var locMercator = new Location("Mercator", "Galglaan", 2, 9000, "Gent", "Mercator");
        var locSchoonmeersenB = new Location("Schoonmeersen B", "Valentin Vaerwyckweg", 1, 9000, "Gent", "B");
        var locBijloke = new Location("Bijloke", "Jozef Kluyskensstraat", 2, 9000, "Gent", "Bijloke");
        var locSchoonmeersenP = new Location("Schoonmeersen P", "Valentin Vaerwyckweg", 1, 9000, "Gent", "P");
        var locSchoonmeersenD = new Location("Schoonmeersen D", "Valentin Vaerwyckweg", 1, 9000, "Gent", "D");

        // --- Restos ---
        var restos = new List<Resto>
        {
            new Resto("Ledeganck", locLedeganck, priceList),
            new Resto("Mercator", locMercator, priceList),
            new Resto("Schoonmeersen B", locSchoonmeersenB, priceList),
            new Resto("Bijloke", locBijloke, priceList),
            new Resto("Schoonmeersen P", locSchoonmeersenP, priceList),
            new Resto("Schoonmeersen D", locSchoonmeersenD, priceList)
        };
        dbContext.Restos.AddRange(restos);
        await dbContext.SaveChangesAsync();

        // --- Menu Categories ---
        var soepen = new MenuCategory("Soepen");
        var hoofdgerechten = new MenuCategory("Hoofdgerechten");
        var sauzen = new MenuCategory("Sauzen");
        var groenten = new MenuCategory("Groenten");
        var zetmeel = new MenuCategory("Zetmeel");
        var weekschotel = new MenuCategory("Weekschotel");
        var pastas = new MenuCategory("Pasta's");
        var snacksMenu = new MenuCategory("Snacks");
        dbContext.MenuCategories.AddRange(soepen, hoofdgerechten, sauzen, groenten, zetmeel, weekschotel, pastas, snacksMenu);
        await dbContext.SaveChangesAsync();

        // --- Menus for 2 weeks ---
        
        // Helper method to create menu items for a specific day
        List<MenuItem> CreateMondayMenu() => new()
        {
            CreateMenuItem("Groentensoep", soepen, new[] { selderij }, new[] { vegan }),
            CreateMenuItem("Paprikasoep", soepen, new[] { selderij }, new[] { vegan }),
            CreateMenuItem("Gebakken noedels met omeletreepjes", hoofdgerechten, new[] { gluten, eieren, soja }, new[] { vegetarian }),
            CreateMenuItem("Ardeense burger", hoofdgerechten, new[] { gluten, sesam }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Gepaneerd kalkoenlapje", hoofdgerechten, new[] { gluten, eieren }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Uiensaus", sauzen, new[] { sulfiet }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Gegratineerde broccoli", groenten, new[] { lactose }, new[] { vegetarian }),
            CreateMenuItem("Gestoofde rode kool", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Wortelen met tijm", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Gebakken aardappelen met cajun kruiden", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Natuuraardappelen", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Couscous", zetmeel, new[] { gluten }, new[] { vegan }),
            CreateMenuItem("Vlaamse varkensstoverij met bruin bier", weekschotel, new[] { gluten, selderij, sulfiet }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Lasagne bolognaise", pastas, new[] { gluten, lactose, eieren }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Worstenbroodje", snacksMenu, new[] { gluten, mosterd }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Panini pesto en mozzarella", snacksMenu, new[] { gluten, lactose, noten }, new[] { vegetarian })
        };

        List<MenuItem> CreateTuesdayMenu() => new()
        {
            CreateMenuItem("Pastinaak- en pompoensoep", soepen, new[] { selderij }, new[] { vegan }),
            CreateMenuItem("Witloofsoep", soepen, new[] { selderij, melk }, new[] { vegetarian }),
            CreateMenuItem("Reepjes met pesto", hoofdgerechten, new[] { noten, lactose }, new[] { vegetarian }),
            CreateMenuItem("Kipfilet", hoofdgerechten, Array.Empty<Allergen>(), Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Gegratineerd witloof met ham", hoofdgerechten, new[] { lactose }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Kokoscurrysaus", sauzen, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Ananas", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Gebakken champignons met knoflook", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Gestoofde boterbonen", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Witte rijst", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Aardappelpuree", zetmeel, new[] { lactose }, new[] { vegetarian }),
            CreateMenuItem("Gestoomde krieltjes", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Vlaamse varkensstoverij met bruin bier", weekschotel, new[] { gluten, selderij, sulfiet }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Lasagne bolognaise", pastas, new[] { gluten, lactose, eieren }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Worstenbroodje", snacksMenu, new[] { gluten, mosterd }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Panini pesto en mozzarella", snacksMenu, new[] { gluten, lactose, noten }, new[] { vegetarian })
        };

        List<MenuItem> CreateWednesdayMenu() => new()
        {
            CreateMenuItem("Kippenroomsoep", soepen, new[] { selderij, lactose }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Tomatensoep", soepen, new[] { selderij }, new[] { vegan }),
            CreateMenuItem("Quorn op provencaalse wijze met paprika", hoofdgerechten, new[] { eieren }, new[] { vegetarian }),
            CreateMenuItem("Pitta reepjes", hoofdgerechten, new[] { gluten }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Varkenslapje", hoofdgerechten, Array.Empty<Allergen>(), Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Stroganoffsaus", sauzen, new[] { lactose }, new[] { vegetarian }),
            CreateMenuItem("Paprikareepjes", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Knolselder in room met bieslook", groenten, new[] { lactose }, new[] { vegetarian }),
            CreateMenuItem("Broccoli", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Bulgur", zetmeel, new[] { gluten }, new[] { vegan }),
            CreateMenuItem("Peterselieaardappelen", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Gebakken aardappelblokjes", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Vlaamse varkensstoverij met bruin bier", weekschotel, new[] { gluten, selderij, sulfiet }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Lasagne bolognaise", pastas, new[] { gluten, lactose, eieren }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Worstenbroodje", snacksMenu, new[] { gluten, mosterd }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Panini pesto en mozzarella", snacksMenu, new[] { gluten, lactose, noten }, new[] { vegetarian })
        };

        List<MenuItem> CreateThursdayMenu() => new()
        {
            CreateMenuItem("Champignonsoep", soepen, new[] { selderij, melk }, new[] { vegetarian }),
            CreateMenuItem("Broccolisoep", soepen, new[] { selderij, melk }, new[] { vegetarian }),
            CreateMenuItem("Sojanuggets", hoofdgerechten, new[] { soja, gluten }, new[] { vegan }),
            CreateMenuItem("Visburger met tartaarsaus", hoofdgerechten, new[] { vis, gluten, eieren, mosterd }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Orloffgebraad", hoofdgerechten, new[] { lactose }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Champignonsaus", sauzen, new[] { lactose }, new[] { vegetarian }),
            CreateMenuItem("Venkel met bechamel", groenten, new[] { lactose, gluten }, new[] { vegetarian }),
            CreateMenuItem("Roergebakken groenten", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Spinazie in room", groenten, new[] { lactose }, new[] { vegetarian }),
            CreateMenuItem("Potato wedges", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Gratin dauphinois", zetmeel, new[] { lactose }, new[] { vegetarian }),
            CreateMenuItem("Krieltjes uit de oven", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Vlaamse varkensstoverij met bruin bier", weekschotel, new[] { gluten, selderij, sulfiet }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Lasagne bolognaise", pastas, new[] { gluten, lactose, eieren }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Worstenbroodje", snacksMenu, new[] { gluten, mosterd }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Panini pesto en mozzarella", snacksMenu, new[] { gluten, lactose, noten }, new[] { vegetarian })
        };

        List<MenuItem> CreateFridayMenu() => new()
        {
            CreateMenuItem("Wortelsoep", soepen, new[] { selderij }, new[] { vegan }),
            CreateMenuItem("Waterkersoep", soepen, new[] { selderij, melk }, new[] { vegetarian }),
            CreateMenuItem("Schnitzel", hoofdgerechten, new[] { gluten, eieren }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Vleesbrood", hoofdgerechten, new[] { gluten, eieren }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Wienerschnitzel", hoofdgerechten, new[] { gluten, eieren }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Bruine saus", sauzen, new[] { gluten, selderij }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Appelmoes", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Bloemkool mornay", groenten, new[] { lactose, gluten }, new[] { vegetarian }),
            CreateMenuItem("Gegrilde tomaat", groenten, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Gebakken krieltjes", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Gestoomde aardappelblokjes", zetmeel, Array.Empty<Allergen>(), new[] { vegan }),
            CreateMenuItem("Tarwe", zetmeel, new[] { gluten }, new[] { vegan }),
            CreateMenuItem("Vlaamse varkensstoverij met bruin bier", weekschotel, new[] { gluten, selderij, sulfiet }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Lasagne bolognaise", pastas, new[] { gluten, lactose, eieren }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Worstenbroodje", snacksMenu, new[] { gluten, mosterd }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Panini pesto en mozzarella", snacksMenu, new[] { gluten, lactose, noten }, new[] { vegetarian })
        };

        // Ledeganck menu - simpler structure with generic items
        List<MenuItem> CreateLedeganckMenu() => new()
        {
            CreateMenuItem("Soep van de dag", soepen, new[] { selderij }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Bruin brood met divers beleg", snacksMenu, new[] { gluten }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Wit brood met divers beleg", snacksMenu, new[] { gluten }, Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Vers bereide salades", hoofdgerechten, Array.Empty<Allergen>(), Array.Empty<DietaryRestriction>()),
            CreateMenuItem("Suggesties", hoofdgerechten, Array.Empty<Allergen>(), Array.Empty<DietaryRestriction>())
        };

        MenuItem CreateMenuItem(string name, MenuCategory category, Allergen[] allergens, DietaryRestriction[] restrictions)
        {
            var item = new MenuItem(name, category, Price.ForMenuItem(null, null));
            if (allergens.Length > 0)
                item.AddAllergens(allergens);
            if (restrictions.Length > 0)
                item.AddDietaryRestrictions(restrictions);
            return item;
        }

        var menuFactories = new[]
        {
            CreateMondayMenu,    
            CreateTuesdayMenu,  
            CreateWednesdayMenu, 
            CreateThursdayMenu,  
            CreateFridayMenu     
        };

        foreach (var resto in restos)
        {
            // Find the Monday of the current week, or next week if it's the weekend
            var today = DateTimeOffset.Now.Date;
            var currentDayOfWeek = (int)today.DayOfWeek;
            
            DateTimeOffset startDate;
            if (currentDayOfWeek == 0 || currentDayOfWeek == 6) // Sunday or Saturday
            {
                // It's the weekend, start from next Monday
                var daysUntilMonday = ((int)DayOfWeek.Monday - currentDayOfWeek + 7) % 7;
                startDate = today.AddDays(daysUntilMonday);
            }
            else
            {
                // It's a weekday, go back to Monday of this week
                var daysSinceMonday = currentDayOfWeek - (int)DayOfWeek.Monday;
                startDate = today.AddDays(-daysSinceMonday);
            }
            
            for (int dayOffset = 0; dayOffset < 14; dayOffset++)
            {
                var menuDate = startDate.AddDays(dayOffset);
                
                // Get day of week (0 = Sunday, 1 = Monday, ..., 6 = Saturday)
                var dayOfWeek = (int)menuDate.DayOfWeek;
                
                // Skip weekends (we only have Monday-Friday menus)
                if (dayOfWeek == 0 || dayOfWeek == 6)
                    continue;

                // Map DayOfWeek to array index: Monday(1) -> 0, Tuesday(2) -> 1, etc.
                var menuIndex = dayOfWeek - 1;
                
                // Create fresh menu items for this specific menu
                // Ledeganck has a simpler menu structure, other restos have the detailed menu
                var menuItems = resto.Name == "Ledeganck" 
                    ? CreateLedeganckMenu() 
                    : menuFactories[menuIndex]();
                
                var menu = new Menu(menuDate);
                menu.AddWeekMenuItems(menuItems);
                resto.AddMenus(new List<Menu> { menu });
            }
        }

        await dbContext.SaveChangesAsync();
    }
}