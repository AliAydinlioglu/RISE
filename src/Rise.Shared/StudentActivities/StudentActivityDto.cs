using Rise.Shared.Locations;
using Rise.Shared.StudentClubs;

namespace Rise.Shared.StudentActivities;

public class StudentActivityDto
{
    public class Index
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required DateTime Date { get; set; }
        public required LocationDto Location { get; set; }
        public required StudentClubDto StudentClub { get; set; }
    }
}