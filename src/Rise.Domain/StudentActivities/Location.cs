namespace Rise.Domain.StudentActivities;

public class Location : Entity
{
    public string Name { get; init; }
    public string Street { get; init; }
    public int HouseNumber { get; init; }
    public int Postcode { get; init; }
    public string City { get; init; }

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