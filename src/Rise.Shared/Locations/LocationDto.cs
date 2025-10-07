namespace Rise.Shared.Locations;

public static class LocationDto
{
    public class Index
    {
        public required int Id { get; set; } 
        public string? Name { get; set; }
        public required string Street { get; set; }
        public required int HouseNumber { get; set; }
        public required string City { get; set; }
        public required int Postcode { get; set; }

    }
}