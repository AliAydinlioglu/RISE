using Rise.Domain.Identity;
using Rise.Domain.Navigation;
using Rise.Domain.Tests.Fakers.Identity;
using Rise.Domain.Tests.Fakers.Navigation;

namespace Rise.Domain.Tests.Navigation;

public class RoleNavigationItemContentLocationShould
{
    // Arrange
    private const string RoleIdString = "f66d0202-62cb-42f2-ac72-fc93ea9f0646";
    private const int NavigationItemId = 1;
    private const int ContentLocationId = 1;
    private const int SequenceNr = 1;
    
    private readonly NavigationItem _navItem = new NavigationItemFaker().Generate();
    private readonly Role _role = new RoleFaker().Generate();
    private readonly ContentLocation _contentLocation = new ContentLocationFaker().Generate();
    
    [Fact]
    public void Be_Created_WhenValidIds()
    {
        // Arrange
        var roleId = GetRoleIdAsGuid();
        
        // Act
        var roleNavContentLocation = new RoleNavigationItemContentLocation(roleId, NavigationItemId, ContentLocationId, SequenceNr);

        // Assert
        roleNavContentLocation.ShouldNotBeNull();
        roleNavContentLocation.Id.RoleId.ShouldBe(roleId);
        roleNavContentLocation.Id.NavigationItemId.ShouldBe(NavigationItemId);
        roleNavContentLocation.Id.ContentLocationId.ShouldBe(ContentLocationId);
        roleNavContentLocation.SequenceNr.ShouldBe(SequenceNr);
    }
    
    [Fact]
    public void Be_Created_WhenValidObjects()
    {
        // Act
        var roleNavContentLocation = new RoleNavigationItemContentLocation(_role,_navItem, _contentLocation, SequenceNr);

        // Assert
        roleNavContentLocation.ShouldNotBeNull();
        roleNavContentLocation.Role.ShouldNotBeNull();
        roleNavContentLocation.NavigationItem.ShouldNotBeNull();
        roleNavContentLocation.ContentLocation.ShouldNotBeNull();
        roleNavContentLocation.SequenceNr.ShouldBe(SequenceNr);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(-1, -1, -1)]
    [InlineData(NavigationItemId, -1, -1)]
    [InlineData(NavigationItemId, 0, 0)]
    [InlineData(NavigationItemId, ContentLocationId, -1)]
    [InlineData(NavigationItemId, ContentLocationId, 0)]
    [InlineData(NavigationItemId, -1, SequenceNr)]
    [InlineData(NavigationItemId, 0, SequenceNr)]
    [InlineData(0, ContentLocationId, SequenceNr)]
    [InlineData(-1, ContentLocationId, SequenceNr)]
    public void Throw_WhenInvalidParamsExceptGuid(int navigationItemId, int contentLocationId, int sequenceNr)
    {   
        // Assert
        Should.Throw<ArgumentException>(() 
            => new RoleNavigationItemContentLocation(GetRoleIdAsGuid(), navigationItemId, contentLocationId, sequenceNr));
    }
    
    [Fact]
    public void Throw_WhenValidParamsExceptGuid()
    {   
        // Assert
        Should.Throw<ArgumentException>(() 
            => new RoleNavigationItemContentLocation(Guid.Empty, NavigationItemId, ContentLocationId, SequenceNr));
    }
    
    [Fact]
    public void Throw_WhenNavItemNull()
    {
        // Arrange
        NavigationItem invalidNavItem = null;
        
        // Act && Assert
        Should.Throw<ArgumentNullException>(()
            => new RoleNavigationItemContentLocation(_role, invalidNavItem, _contentLocation, SequenceNr));
    }
    
    [Fact]
    public void Throw_WhenContentLocationNull()
    {
        // Arrange
        ContentLocation contentLocation = null;
        
        // Act && Assert
        Should.Throw<ArgumentNullException>(()
            => new RoleNavigationItemContentLocation(_role, _navItem, contentLocation, SequenceNr));
    }
    
    [Fact]
    public void Throw_WhenRoleNull()
    {
        // Arrange
        Role role = null;
        
        // Act && Assert
        Should.Throw<ArgumentNullException>(()
            => new RoleNavigationItemContentLocation(role, _navItem, _contentLocation, SequenceNr));
    }

    private Guid GetRoleIdAsGuid()
    {
        return new Guid(RoleIdString);
    }
}