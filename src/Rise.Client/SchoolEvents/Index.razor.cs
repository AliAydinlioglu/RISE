using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Locations;

namespace Rise.Client.SchoolEvents;

[HomeBlock(icon:@Icons.Material.Outlined.EventNote, label:"Evenementen", route:"/events")]
public partial class Index 
{
    private IEnumerable<SchoolEventDto.Index>? schoolEvents;
    
    private int currentPage = 1;
    private int pageSize = 8;
    private int totalCount = 0;
    private int totalPages => (int)Math.Ceiling((double)totalCount / pageSize);
    
    private async Task LoadStudentActivitiesAsync()
    {
        /*
        var request = new QueryRequest.SkipTake
        {
            Skip = (currentPage - 1) * pageSize,
            Take = pageSize,
        };
        */

        var schoonmeersen = new LocationDto.Index()
        {
            Id = 1,
            Name = "Schoonmeersen",
            Street = "teststraat",
            HouseNumber = 123,
            City = "Gent",
            Postcode = 9000,
        };

        IEnumerable<SchoolEventDto.Index> events = [
            new()
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 10, 20),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },
            new()
            {
                Id = 2,
                Title = "Move & Groove",
                Date = new DateTime(2025, 10, 20),
                Category = "Sport",
                StartTime = new TimeOnly(18, 00),
                EndTime = new TimeOnly(20, 00),
                Location = schoonmeersen
            },
            new()
            {
                Id = 3,
                Title = "HOGENT CUP",
                Date = new DateTime(2025, 10, 23),
                Category = "Feest",
                StartTime = new TimeOnly(19, 00),
                EndTime = new TimeOnly(23, 00),
                Location = schoonmeersen
            },
            new()
            {
                Id = 4,
                Title = "Uitstelgedrag aanpakken",
                Date = new DateTime(2025, 10, 27),
                Category = "Goed in je vel",
                StartTime = new TimeOnly(18, 30),
                EndTime = new TimeOnly(20, 30),
                Location = null, //TODO: check
            },
            new()
            {
                Id = 5,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 10, 20),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },
            new()
            {
                Id = 6,
                Title = "Move & Groove",
                Date = new DateTime(2025, 10, 20),
                Category = "Sport",
                StartTime = new TimeOnly(18, 00),
                EndTime = new TimeOnly(20, 00),
                Location = schoonmeersen
            },
            new()
            {
                Id = 7,
                Title = "HOGENT CUP",
                Date = new DateTime(2025, 10, 23),
                Category = "Feest",
                StartTime = new TimeOnly(19, 00),
                EndTime = new TimeOnly(23, 00),
                Location = schoonmeersen
            },
            new()
            {
                Id = 8,
                Title = "Uitstelgedrag aanpakken",
                Date = new DateTime(2025, 10, 27),
                Category = "Goed in je vel",
                StartTime = new TimeOnly(18, 30),
                EndTime = new TimeOnly(20, 30),
                Location = null, //TODO: check
            }];
        
        var result = Result.Success(new SchoolEventResponse.Index
        {
            SchoolEvents = events,
            TotalCount = events.Count(),
        });
    }

    private async Task OnPageChangedAsync(int page)
    {
        currentPage = page;
        await LoadStudentActivitiesAsync();
    }
}

public static class SchoolEventDto
{
    public class Index
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Category { get; set; }
        public string? Description { get; set; }
        public required DateTimeOffset Date { get; set; }
        
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        
        public string? ImageUrl { get; set; }
        public required LocationDto.Index Location { get; set; }
    }
}

public static class SchoolEventResponse
{
    public class Index
    {
        public IEnumerable<SchoolEventDto.Index> SchoolEvents { get; set; } = [];
        public int TotalCount { get; set; }
    }
}