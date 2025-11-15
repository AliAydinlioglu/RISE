using Rise.Domain.Common;
using Rise.Domain.Contact;

namespace Rise.Domain.Tests.Contact;

public class ServiceLocationShould
{
    private readonly StructuredAddress _address = new("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
    private const string LocationName = "Schoonmeersen";
    
    [Fact]
    public void BeCreated_WithValidParameters()
    {
        var location = new FacilityLocation(_address, LocationName);

        location.ShouldNotBeNull();
        location.FaclitityAddress.ShouldBe(_address);
        location.LocationName.ShouldBe(LocationName);
    }

    [Fact]
    public void BeEqual_WhenAddressAndLocationNameAreTheSame()
    {
        var location1 = new FacilityLocation(_address, LocationName);
        var location2 = new FacilityLocation(_address, LocationName);

        location1.ShouldBe(location2);
    }

    [Fact]
    public void NotBeEqual_WhenLocationNamesAreDifferent()
    {
        var location1 = new FacilityLocation(_address, "Schoonmeersen");
        var location2 = new FacilityLocation(_address, "Mercator");

        location1.ShouldNotBe(location2);
    }

    [Fact]
    public void NotBeEqual_WhenAddressesAreDifferent()
    {
        var address1 = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var address2 = new StructuredAddress("Henleykaai", 84, 9000, "Gent", "");
        var location1 = new FacilityLocation(address1, LocationName);
        var location2 = new FacilityLocation(address2, LocationName);

        location1.ShouldNotBe(location2);
    }

    [Theory]
    [InlineData("Schoonmeersen", "Valentin Vaerwyckweg", 1, 9000, "Gent")]
    [InlineData("Mercator", "Henleykaai", 84, 9000, "Gent")]
    [InlineData("Vesalius", "Keramiekstraat", 80, 9000, "Gent")]
    public void BeCreated_WithDifferentCampuses(string campus, string street, int houseNumber, int postcode, string city)
    {
        var address = new StructuredAddress(street, houseNumber, postcode, city, "");

        var location = new FacilityLocation(address, campus);

        location.ShouldNotBeNull();
        location.LocationName.ShouldBe(campus);
        location.FaclitityAddress.Street.ShouldBe(street);
        location.FaclitityAddress.HouseNumber.ShouldBe(houseNumber);
        location.FaclitityAddress.Postcode.ShouldBe(postcode);
        location.FaclitityAddress.City.ShouldBe(city);
    }
}

