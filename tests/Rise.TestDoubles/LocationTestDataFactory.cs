using Rise.Domain.Locations;

namespace Rise.TestDoubles;

public class LocationTestDataFactory
{
    public static Location CreateDefaultLocation()
    {
        return new Location("Test Location", "Test Straat", 1, 9000, "Gent", null);
    }
}