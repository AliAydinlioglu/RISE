using Rise.Shared.Locations;

namespace Rise.Shared.SchoolEvents
{
    public static class SchoolEventDto
    {
        public abstract class Base
        {
            public required int Id { get; set; }
            public required string Title { get; set; }
            public string? Description { get; set; }
            public required DateTimeOffset Date { get; set; }
            public required TimeOnly StartTime { get; set; }
            public required TimeOnly EndTime { get; set; }
            public required decimal Price { get; set; }
            public required string RegisterLink { get; set; }
            public required int Capacity { get; set; }
            public required bool Registrable { get; set; }
            public required string Publicity { get; set; }
            public required string Category { get; set; }
            public required string? ImageUrl { get; set; }
            public required LocationDto.Index Location { get; set; }
            
            public string TimeString => $"{StartTime:HH:mm} - {EndTime:HH:mm}";
            public string LocalDateString => Location.GetAddressWithName();
            public bool IsFree() => Price == 0;
        }
        public class Index : Base { }
        public class Detail : Base { }
    }
}
