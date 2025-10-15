using Rise.Domain.Navigation;

namespace Rise.Domain.Tests.Navigation;

public class ContentLocationShould
{
    // Arrange
    private const int ContentLocationId = 1;
    private const string ContentLocationName = "TestContentLocation";
    
    [Fact]
    public void Be_Created_WhenValid_WithoutId()
    {
        // Act
        var contentLocation = new ContentLocation(ContentLocationName);

        // Assert
        contentLocation.ShouldNotBeNull();
        contentLocation.Name.ShouldBe(ContentLocationName);
        contentLocation.RoleNavigationItems.ShouldBeEmpty();
    }
    
    [Fact]
    public void Be_Created_WhenValid_WithId()
    {
        // Act
        var contentLocation = new ContentLocation(ContentLocationId,ContentLocationName);

        // Assert
        contentLocation.ShouldNotBeNull();
        contentLocation.Id.ShouldBe(ContentLocationId);
        contentLocation.Name.ShouldBe(ContentLocationName);
        contentLocation.RoleNavigationItems.ShouldBeEmpty();
    }
    
    [Theory]
    [InlineData(ContentLocationId,"")]
    [InlineData(ContentLocationId," ")]
    [InlineData(ContentLocationId,null)]
    [InlineData(-1,ContentLocationName)]
    [InlineData(0,ContentLocationName)]
    public void Throw_WhenInvalidParams(int id, string name)
    {
        // Assert
        Should.Throw<ArgumentException>(() => new ContentLocation(id,name));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Throw_WhenInvalidName(string name)
    {
        // Assert
        Should.Throw<ArgumentException>(() => new ContentLocation(name));
    }
}