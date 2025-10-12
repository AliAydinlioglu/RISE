using System.Text.Json.Serialization;
using Rise.Shared.Locations;
using Rise.Shared.StudentClubs;

namespace Rise.Shared.StudentActivities;

public static class StudentActivityDto
{
    public abstract class Base
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required DateTimeOffset Date { get; set; }
        
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        
        public string? ImageUrl { get; set; }
        public required LocationDto.Index Location { get; set; }
    }
    public class Index : Base
    {
        [JsonPropertyOrder(Int32.MaxValue)]
        public required StudentClubDto.Summary StudentClub { get; set; }
    }
    
    public class Detail : Base
    { 
        [JsonPropertyOrder(Int32.MaxValue)]
        public required StudentClubDto.Index StudentClub { get; set; }
    }
}