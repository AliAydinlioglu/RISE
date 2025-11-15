using System.Text;

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
        
        public string? BusNumber { get; set; } = String.Empty;

        public string GetAddressWithName()
        {
            return !string.IsNullOrWhiteSpace(Name) ? Name : GetAddressWithoutName();
        }
        
        public string GetAddressWithoutName()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{Street} {HouseNumber}");

            if (!string.IsNullOrWhiteSpace(BusNumber)) 
                stringBuilder.Append($" {BusNumber}");
                
            stringBuilder.Append($", {Postcode} {City}");
            
            return stringBuilder.ToString();
        }
    }
}