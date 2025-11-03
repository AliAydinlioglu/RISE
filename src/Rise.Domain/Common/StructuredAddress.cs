namespace Rise.Domain.Common;
/// <summary>
/// Address informatio in a structured way
/// </summary>
/// <param name="name"></param>
/// <param name="street"></param>
/// <param name="houseNumber"></param>
/// <param name="postcode"></param>
/// <param name="city"></param>
/// <param name="busNumber"></param>
public class StructuredAddress(
    string name, string street, int houseNumber, int postcode, string city, string busNumber)
{
    public string? Name { get; private set; } = name;
    public string Street { get; private set; } = Guard.Against.NullOrWhiteSpace(street);
    public int HouseNumber { get; private set; } = Guard.Against.NegativeOrZero(houseNumber);
    public string? BusNumber { get; private set; } = busNumber;
    public int Postcode { get; private set; } = Guard.Against.BetweenMinAndMax(postcode, 1000, 9999);
    public string City { get; private set; } = Guard.Against.NullOrWhiteSpace(city);
}
