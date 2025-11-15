namespace Rise.Domain.Locations;

public class Location : Entity
{
    public string? Name { get; private set; }
    public string Street { get; private set; }
    public int HouseNumber { get; private set; }
    public string? BusNumber { get; private set; } = String.Empty;
    public int Postcode { get; private set; }
    public string City { get; private set; }
    
    public Location() { }
    public Location(string name, string street, int houseNumber, int postcode, string city, string busNumber)
    {
        Name = name;
        Street = Guard.Against.NullOrWhiteSpace(street);
        HouseNumber = Guard.Against.NegativeOrZero(houseNumber);
        Postcode = Guard.Against.BetweenMinAndMax(postcode, 1000, 9999);
        City =  Guard.Against.NullOrWhiteSpace(city);
        BusNumber = busNumber;
    }
}