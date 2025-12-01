using Rise.Shared.Locations;

namespace Rise.Shared.Campuses
{
    public static class CampusDto
    {
        public class Index
        {
            public required string Description { get; set; }
            public CampusMapDto.Details? Map { get; set; }
            public required LocationDto.Index Location { get; set; }

            public bool HasMap => Map != null && !string.IsNullOrWhiteSpace(Map.Url);
        }
    }
}
