using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Campuses;
using Rise.Shared.Locations;

namespace Rise.Client.Campuses;

[HomeBlock(icon: Icons.Material.Filled.Map, label: "Campussen", route: "/campuses")]
public partial class Index
{
    private string _pageTitle = "Campussen";
    private IEnumerable<CampusDto.Index> _campuses = [];

    private bool _isMapDialogOpen = false;
    private string? _selectedMapUrl;

    [Inject] public required IDialogService DialogService { get; set; }

    protected override void OnInitialized()
    {
        _campuses = [
            // 1. Aalst
            new CampusDto.Index
            {
                Description = "<p>Campus Aalst is vlot bereikbaar en centraal gelegen op wandelafstand van station Aalst (850 m), bushalte ‘Vredeplein’ (200 m), parking Hopmarkt (350 m) en parking Keizershallen (350 m). Kijk voor meer informatie over parkeren op <a href=\"https://www.aalst.be/infofiche/parkeren-tarieven-en-parkings\" target=\"_blank\">aalst.be/infofiche/parkeren-tarieven-en-parkings</a>.</p>",
                Location = new LocationDto.Index
                {
                    Id = 1,
                    Name = "Campus Aalst",
                    Street = "Arbeidstraat",
                    HouseNumber = 14,
                    Postcode = 9300,
                    City = "Aalst"
                }
            },
            // 2. Bijloke
            new CampusDto.Index
            {
                Description = "<p>Campus Bijloke bevindt zich ideaal gelegen aan de kleine ring van Gent (R40), op 6 minuutjes fietsen of 15 minuten wandelen van het station Gent Sint-Pieters (1,5 km). Je kan ook de tram nemen tot aan de halte ‘Gent Bijlokehof’.</p>\r\n<p>In de directe omgeving van campus Bijloke kan je betalend parkeren op straat (oranje zone). Aan de overkant van de R40 start de groene zone en heb je een gunstiger parkeertarief. Kijk op <a href=\"https://stad.gent/nl/mobiliteit-openbare-werken/parkeren/parkeren-op-straat/parkeertarieven-op-straat\" target=\"_blank\">stad.gent/parkeren</a> voor meer informatie over de parkeertarieven, zones en parkeerduur.</p>\r\n<p>Campus Bijloke ligt binnen de lage-emissiezone (LEZ). Kijk op <a href=\"http://stad.gent/lez\" target=\"_blank\">stad.gent/lez</a> voor meer informatie over de lage-emissiezone in Gent en om te testen of je voertuig binnen mag in deze zone.</p>",
                Location = new LocationDto.Index
                {
                    Id = 2,
                    Name = "Campus Bijloke",
                    Street = "Louis Pasteurlaan",
                    HouseNumber = 2,
                    Postcode = 9000,
                    City = "Gent"
                }
            },
            // 3. Grote Sikkel
            new CampusDto.Index
            {
                Description = "<p>Campus Grote Sikkel bevindt zich in het centrum van Gent, vlak tussen de Sint-Baafskathedraal en het stadhuis en op 350 meter van bus- en tramhaltes ‘Gent Korenmarkt’ en ‘Gent Duivelsteen’, of op 450 meter van tramhalte ‘Gent Vogelmarkt’.</p>\r\n<p>Betalend parkeren kan in of Parking Vrijdagmarkt (400 m), Parking Reep (450 m) of Parking Kouter (450 m). Kijk op <a href=\"https://stad.gent/nl/mobiliteit-openbare-werken/parkeren/parkings-gent\" target=\"_blank\">stad.gent/parkeren</a> voor meer informatie over parkeertarieven en parkeerduur.</p>\r\n<p>Campus Grote Sikkel ligt binnen de lage-emissiezone (LEZ). Kijk op <a href=\"http://stad.gent/lez\" target=\"_blank\">stad.gent/lez</a> voor meer informatie over de lage-emissiezone in Gent en om te testen of je voertuig binnen mag in deze zone.</p>",
                Location = new LocationDto.Index
                {
                    Id = 3,
                    Name = "Campus Grote Sikkel",
                    Street = "Hoogpoort",
                    HouseNumber = 64,
                    Postcode = 9000,
                    City = "Gent"
                }
            },
            // 4. Ledeganck
            new CampusDto.Index
            {
                Description = "<p>Campus Ledeganck ligt op 5 minuutjes fietsen of 15 minuten wandelen van het station Gent Sint-Pieters, aan de andere kant van het Citadelpark (1 km), vlak aan de kleine ring van Gent (R40) en de Overpoort.</p>\r\n<p>In de Ledeganckstraat kan je betalend parkeren op straat (groene zone). Kijk op <a href=\"https://stad.gent/nl/mobiliteit-openbare-werken/parkeren/parkeren-op-straat/parkeertarieven-op-straat#Groene%20tariefzone%20en%20%22Groene%20Zone%20Uitbreiding%22\" target=\"_blank\">stad.gent/parkeren</a> voor meer informatie over parkeertarieven en parkeerduur.</p>\r\n<p>",
                Location = new LocationDto.Index
                {
                    Id = 4,
                    Name = "Campus Ledeganck",
                    Street = "K.L. Ledeganckstraat",
                    HouseNumber = 8,
                    Postcode = 9000,
                    City = "Gent"
                }
            },
            // 5. Lokeren
            new CampusDto.Index
            {
                Description = "<p>Campus Lokeren ligt vlak bij station Lokeren (200 m) en de gratis stations­parking. Kijk voor meer informatie over parkeren op <a href=\"https://lokeren.be/producten-en-diensten/parkeren\" target=\"_blank\">lokeren.be/producten-en-diensten/parkeren</a>.</p>",
                Location = new LocationDto.Index
                {
                    Id = 5,
                    Name = "Campus Lokeren",
                    Street = "Groendreef",
                    HouseNumber = 31,
                    Postcode = 9160,
                    City = "Lokeren"
                }
            },
            // 6. Melle
            new CampusDto.Index
            {
                Description = "<p>Campus Melle is vlot bereikbaar en goed gelegen aan de Brusselsesteenweg (N9). De bus brengt je op 16 minuten van station Gent Sint-Pieters naar bushalte 'Melle Tuinbouwschool'. Ook met de fiets is het goed te doen: zo’n 7 km vanaf het station.</p>\r\n<p>Aan de Brusselsesteenweg kan je in de directe omgeving van campus Melle gratis parkeren langs de straatkant.</p>\r\n<p>",
                Location = new LocationDto.Index
                {
                    Id = 6,
                    Name = "Campus Melle",
                    Street = "Brusselsesteenweg",
                    HouseNumber = 161,
                    Postcode = 9090,
                    City = "Melle"
                }
            },
            // 7. Mercator
            new CampusDto.Index
            {
                Description = "<p>Campus Mercator bevindt zich ideaal gelegen op 4 minuutjes fietsen of 12 minuten wandelen van het centrum en van station Gent Sint-Pieters (1 km). Je kan ook de tram nemen tot aan de halte ‘Gent Koning Albertbrug’ (200 m) of halte ‘Gent Van Nassaustraat’ (450 m).</p>\r\n<p>In de directe omgeving van campus Mercator kan je betalend parkeren op straat (groene zone). Kijk op <a href=\"https://stad.gent/nl/mobiliteit-openbare-werken/parkeren/parkeren-op-straat/parkeertarieven-op-straat#Groene%20tariefzone%20en%20%22Groene%20Zone%20Uitbreiding%22\" target=\"_blank\">stad.gent/parkeren</a> voor meer informatie over parkeertarieven en parkeerduur.</p>\r\n<p>Campus Mercator ligt buiten de lage-emissiezone (LEZ). Kijk op <a href=\"http://stad.gent/lez\" target=\"_blank\">stad.gent/lez</a> voor meer informatie over de lage-emissiezone in Gent.</p>\r\n<p>",
                Map = new CampusMapDto.Details
                {
                    Url = "/img/campuses/mercator-plan.jpg",
                    Description = "Plan of map plattegrond campus Mercator"
                },
                Location = new LocationDto.Index
                {
                    Id = 7,
                    Name = "Campus Mercator",
                    Street = "Henleykaai",
                    HouseNumber = 84,
                    Postcode = 9000,
                    City = "Gent"
                }
            },
            // 8. Schoonmeersen
            new CampusDto.Index
            {
                Description = "<p>Campus Schoonmeersen is vlot bereikbaar en goed gelegen aan de ring rond Gent (R4) en op wandelafstand van station Gent Sint-Pieters (600 m). De bus- en tramhalte ‘Gent Tuinwijklaan’ heeft een vlotte verbinding met het centrum en de randgemeentes. Gebouw T bevindt zich een halte verder, aan bus- en tramhalte ‘Gent Flamingostraat’.</p>\r\n<p>Als HOGENT-student of CVO-cursist kan je op basis van je studentennummer een parkeervignet aanvragen. Met een geldig parkeervignet kan je tijdens de lessen gratis parkeren op de centrale bovengrondse parking op campus Schoonmeersen. Kijk op <a href=\"/parkeren/\">hogent.be/parkeren</a> voor meer informatie en het parkeerreglement.</p>\r\n<p>Andere bezoekers kunnen terecht in betaalparking Gent Sint-Pieters. Kijk op <a href=\"https://www.belgiantrain.be/nl/station-information/car-or-bike-at-station/b-parking/my-b-parking/gentstpieters\" target=\"_blank\">belgiantrain.be</a> voor meer informatie over parkeertarieven en parkeerduur.</p>\r\n<p>Vanaf de gratis Park &amp; Ride The Loop/Expo is het 9 minuten fietsen tot campus Schoonmeersen (2,4 km). Kijk op <a href=\"https://stad.gent/nl/mobiliteit-openbare-werken/parkeren/park-and-ride-pr/pr-loopexpo\" target=\"_blank\">stad.gent/parkeren</a> voor meer informatie over deze Park &amp; Ride en over de beschikbare deelfietsen ter plekke.</p>\r\n<p>Campus Schoonmeersen ligt buiten de lage-emissiezone (LEZ). Kijk op <a href=\"http://stad.gent/lez\" target=\"_blank\">stad.gent/lez</a> voor meer informatie over de lage-emissiezone in Gent.</p>",
                Map = new CampusMapDto.Details
                {
                    Url = "/img/campuses/240400_Schoonmeersen_plan_detail_nieuw-04.png",
                    Description = "Plan of map plattegrond campus Schoonmeersen"
                },
                Location = new LocationDto.Index
                {
                    Id = 8,
                    Name = "Campus Schoonmeersen",
                    Street = "Valentin Vaerwyckweg",
                    HouseNumber = 1,
                    Postcode = 9000,
                    City = "Gent"
                }
            },
            // 9. Vesalius
            new CampusDto.Index
            {
                Description = "<p>Campus Vesalius bevindt zich vlakbij UZ Gent en heeft een vlotte verbinding met station Gent Sint-Pieters (2,3 km) en met het centrum. De campus is 8 minuutjes wandelen vanaf tramhalte ‘Zwijnaarde Gestichtstraat’ en vanaf bushalte ‘Gent UZ’ (600 m), of 3 minuutjes wandelen van bushalte ‘Gent Roelandt&shy;plein’ (200 m).</p>\r\n<p>In de directe omgeving van campus Vesalius kan je betalend parkeren op straat (groene zone). Kijk op <a href=\"https://stad.gent/nl/mobiliteit-openbare-werken/parkeren/parkeren-op-straat/parkeertarieven-op-straat#Groene%20tariefzone%20en%20%22Groene%20Zone%20Uitbreiding%22\" target=\"_blank\">stad.gent/parkeren</a> voor meer informatie over parkeertarieven en parkeerduur.</p>",
                Location = new LocationDto.Index
                {
                    Id = 9,
                    Name = "Campus Vesalius",
                    Street = "Keramiekstraat",
                    HouseNumber = 80,
                    Postcode = 9000,
                    City = "Gent"
                }
            },
            // 10. Proefhoeve Bottelare
            new CampusDto.Index
            {
                Description = "<p>Proefhoeve Bottelare bevindt zich vlak bij het dorpscentrum van Bottelare, op 400 meter van bushalte ‘Bottelare Dorpsstraat’. Je kan hier ook gratis parkeren.</p>",
                Location = new LocationDto.Index
                {
                    Id = 10,
                    Name = "Proefhoeve Bottelare",
                    Street = "Diepestraat",
                    HouseNumber = 1,
                    Postcode = 9820,
                    City = "Bottelare"
                }
            },
            // 11. Site Geraard de Duivelstraat
            new CampusDto.Index
            {
                Description = "<p>De Wijnaert bevindt zich in het centrum van Gent, vlak tussen de Sint-Baafskathedraal en de Reep en op 50 meter van bus- en tramhalte ‘Gent Duivelsteen’, of op 250 meter van bushalte ‘Gent Reep’.</p>\r\n<p>Betalend parkeren kan in Parking Reep (150 m), Parking Kouter (400 m) of Parking Vrijdagmarkt (700 m). Kijk op <a href=\"https://stad.gent/nl/mobiliteit-openbare-werken/parkeren/parkings-gent\" target=\"_blank\">stad.gent/parkeren</a> voor meer informatie over parkeertarieven en parkeerduur.</p>",
                Location = new LocationDto.Index
                {
                    Id = 11,
                    Name = "Site Geraard de Duivelstraat",
                    Street = "Geraard de Duivelstraat",
                    HouseNumber = 5,
                    Postcode = 9000,
                    City = "Gent"
                }
            },
            // 12. Site Buchtenstraat (FTI Lab)
            new CampusDto.Index
            {
                Description = "<p>Het FTI Lab (Fashion and Textiles Innovation Lab) ligt aan ‘The Loop’, een vlot bereikbare site vlakbij Flanders Expo en op 10 minuutjes fietsen of een halfuurtje wandelen van station Gent Sint-Pieters (2,3 km) of 5 minuten wandelen van bushalte ‘Sint-Denijs-Westrem Kromme Leie’ (450 m).</p>",
                Location = new LocationDto.Index
                {
                    Id = 12,
                    Name = "Site Buchtenstraat (FTI Lab)",
                    Street = "Buchtenstraat",
                    HouseNumber = 11,
                    Postcode = 9000,
                    City = "Gent"
                }
            },
            // 13. Sporthal (Schoonmeersen)
            new CampusDto.Index
            {
                Description = "<p>De sporthal bevindt zich op campus Schoonmeersen aan de toegang langs de Sint-Denijslaan, vlakbij station Gent Sint-Pieters (400 m).</p>",
                Location = new LocationDto.Index
                {
                    Id = 13,
                    Name = "Sporthal",
                    Street = "Sint-Denijslaan",
                    HouseNumber = 251,
                    Postcode = 9000,
                    City = "Gent"
                }
            }
        ];
    }

    private async Task EnlargeMapAsync(CampusDto.Index campus)
    {
        var parameters = new DialogParameters
        {
            ["MapUrl"] = campus.Map!.Url
        };

        await DialogService.ShowAsync<MapDialog>(campus.Location.Name, parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraLarge,
            FullWidth = true,
            CloseButton = true,
            FullScreen = true
        });
    }

}
