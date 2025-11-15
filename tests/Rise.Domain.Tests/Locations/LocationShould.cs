using Rise.Domain.Locations;
using Rise.Domain.StudentActivities;

namespace Rise.Domain.Tests.Locations;

public class LocationShould
{
    [Fact]
    public void CreateLocation_WhenValidParameters()
    {
        // Arrange
        var name = "Test Name";
        var street = "Test street";
        var houseNumber = 123;
        var postcode = 5678;
        var city = "Test City";
        var busNumber = "A";

        // Act
        var location = new Location(name, street, houseNumber, postcode, city,busNumber);

        // Assert
        location.Name.ShouldBe(name);
        location.Street.ShouldBe(street);
        location.HouseNumber.ShouldBe(houseNumber);
        location.Postcode.ShouldBe(postcode);
        location.City.ShouldBe(city);
        location.BusNumber.ShouldBe(busNumber);
    }

    [Theory]
    [InlineData(null, 123, 5678, "Test City")]
    [InlineData("", 123, 5678, "Test City")]
    [InlineData("   ", 123, 5678, "Test City")]
    [InlineData("Test street", 0, 5678, "Test City")]
    [InlineData("Test street", -1, 5678, "Test City")]
    [InlineData("Test street", 123, 999, "Test City")]
    [InlineData("Test street", 123, 10000, "Test City")]
    [InlineData("Test street", 123, 50, "Test City")]
    [InlineData("Test street", 123, 30000, "Test City")]
    [InlineData("Test street", 123, 5678, null)]
    [InlineData("Test street", 123, 5678, "")]
    [InlineData("Test street", 123, 5678, "   ")]
    public void CreateLocation_ExceptionWhenInvalidRequiredFields(string street, int houseNumber, int postcode,
        string city)
    {
        
        // Assert
        Should.Throw<ArgumentException>(() =>  new Location("", street, houseNumber, postcode, city, ""));
    }
    
  
}