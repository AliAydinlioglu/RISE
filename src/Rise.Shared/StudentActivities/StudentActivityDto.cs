using Rise.Shared.Locations;
using Rise.Shared.StudentClubs;

namespace Rise.Shared.StudentActivities;

public static class StudentActivityDto
{
    public class Index
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required DateTime Date { get; set; }
        
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        
        public string? ImageUrl { get; set; }
        public required LocationDto.Index Location { get; set; }
        public required StudentClubDto.Summary StudentClub { get; set; }
    }
}