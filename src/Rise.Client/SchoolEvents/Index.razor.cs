using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Calendar;
using Rise.Shared.Locations;

namespace Rise.Client.SchoolEvents;

[HomeBlock(icon:@Icons.Material.Outlined.CalendarToday, label:"Evenementen", route:"/school-events")]
public partial class Index 
{
    private IEnumerable<SchoolEventDto.Index>? _schoolEvents;

    private DateTime _selectedDate = DateTime.Now;
    
    private const int PageSize = 8;
    private int TotalCount { get; set; }
    private int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    
    private async Task LoadSchoolEventsAsync()
    {
        Log.Information("{0}: selected date={1}", nameof(LoadSchoolEventsAsync), _selectedDate);
        
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

        _schoolEvents = [
            new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },new SchoolEventDto.Index
            {
                Id = 1,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 11, 08),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },
            new SchoolEventDto.Index
            {
                Id = 2,
                Title = "Move & Groove",
                Date = new DateTime(2025, 10, 20),
                Category = "Sport",
                StartTime = new TimeOnly(18, 00),
                EndTime = new TimeOnly(20, 00),
                Location = schoonmeersen
            },
            new SchoolEventDto.Index
            {
                Id = 3,
                Title = "HOGENT CUP",
                Date = new DateTime(2025, 10, 23),
                Category = "Feest",
                StartTime = new TimeOnly(19, 00),
                EndTime = new TimeOnly(23, 00),
                Location = schoonmeersen
            },
            new SchoolEventDto.Index
            {
                Id = 4,
                Title = "Uitstelgedrag aanpakken",
                Date = new DateTime(2025, 10, 27),
                Category = "Goed in je vel",
                StartTime = new TimeOnly(18, 30),
                EndTime = new TimeOnly(20, 30),
                Location = null, //TODO: check
            },
            new SchoolEventDto.Index
            {
                Id = 5,
                Title = "Hersftwandeling",
                Date = new DateTime(2025, 10, 20),
                Category = "Biodiversiteit",
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(13, 30),
                Location = schoonmeersen
            },
            new SchoolEventDto.Index
            {
                Id = 6,
                Title = "Move & Groove",
                Date = new DateTime(2025, 10, 20),
                Category = "Sport",
                StartTime = new TimeOnly(18, 00),
                EndTime = new TimeOnly(20, 00),
                Location = schoonmeersen
            },
            new SchoolEventDto.Index
            {
                Id = 7,
                Title = "HOGENT CUP",
                Date = new DateTime(2025, 10, 23),
                Category = "Feest",
                StartTime = new TimeOnly(19, 00),
                EndTime = new TimeOnly(23, 00),
                Location = schoonmeersen
            },
            new SchoolEventDto.Index
            {
                Id = 8,
                Title = "Uitstelgedrag aanpakken",
                Date = new DateTime(2025, 10, 27),
                Category = "Goed in je vel",
                StartTime = new TimeOnly(18, 30),
                EndTime = new TimeOnly(20, 30),
                Location = null, //TODO: check
            }
        ];

        TotalCount = _schoolEvents.Count();
        _schoolEvents = _schoolEvents.Where(x => x.Date.Date == _selectedDate.Date);
        Log.Information("{0}: events={1}", nameof(LoadSchoolEventsAsync), _schoolEvents);
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadSchoolEventsAsync();
    }

    private async Task OnDateChangedAsync(DateTime date)
    {
        Log.Information("{0}: {1}", nameof(OnDateChangedAsync), $"{date:dd/MM/yyyy}");
        _selectedDate = date;
        await LoadSchoolEventsAsync();
    }
    
    private void SetDateRelativeToCurrentDate(int days)
    {
        Log.Information("{0}: {1}", nameof(SetDateRelativeToCurrentDate), days);
        _selectedDate = _selectedDate.AddDays(days);
        _selectedDate = CalendarHelpers.GetMondayOfWeek(_selectedDate);
    }
}

public static class SchoolEventDto
{
    public class Index
    {
        public required int Id { get; init; }
        public required string Title { get; init; }
        public required string Category { get; init; }
        public required DateTimeOffset Date { get; init; }
        
        public required TimeOnly StartTime { get; init; }
        public required TimeOnly EndTime { get; init; }
        
        public string? ImageUrl { get; set; }
        public LocationDto.Index? Location { get; init; }
    }
}