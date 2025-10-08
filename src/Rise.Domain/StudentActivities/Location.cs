namespace Rise.Domain.StudentActivities;

public class Location : Entity
{
    public string Name { get; private set; }
    public string Street { get; private set; }
    public int HouseNumber { get; private set; }
    public int Postcode { get; private set; }
    public string City { get; private set; }
    
    public Location() { }
    public Location(string name, string street, int houseNumber, int postcode, string city)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NullOrWhiteSpace(street);
        Guard.Against.NegativeOrZero(houseNumber);
        Guard.Against.BetweenMinAndMax(postcode, 1000, 9999);
        Guard.Against.NullOrWhiteSpace(city);

        Name = name;
        Street = street;
        HouseNumber = houseNumber;
        Postcode = postcode;
        City = city;
    }
}