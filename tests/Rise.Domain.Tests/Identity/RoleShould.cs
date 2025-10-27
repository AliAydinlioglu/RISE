using Rise.Domain.Identity;

namespace Rise.Domain.Tests.Identity;

public class RoleShould
{
    private const string RoleName = "TestRole";
    private const string RoleIdString = "f66d0202-62cb-42f2-ac72-fc93ea9f0646";
    
    [Fact]
    public void Be_Created_WhenValid_WithoutId()
    {
        // Act
        var role = new Role(RoleName);   

        // Assert  
        role.ShouldNotBeNull();
        role.Name.ShouldBe(RoleName);
    }
    
    [Fact]
    public void Be_Created_WhenValid_WithId()
    {
        // Arrange
        var roleId = GetRoleIdAsGuid();
        
        // Act
        var role = new Role(roleId,RoleName);   

        // Assert  
        role.ShouldNotBeNull();
        role.Id.ShouldBe(roleId);
        role.Name.ShouldBe(RoleName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Throw_WhenInvalidName_WithoutId(string name)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => new Role(name));
    }
    
    [Fact]
    public void Throw_WhenValidNameWithInvalidId()
    {   
        // Act & Assert
        Should.Throw<ArgumentException>(() => new Role(Guid.Empty,RoleName));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Throw_WhenInvalidNameWithValidId(string name)
    {   
        // Act & Assert
        Should.Throw<ArgumentException>(() => new Role(GetRoleIdAsGuid(),name));
    }
    
    private Guid GetRoleIdAsGuid()
    {
        return new Guid(RoleIdString);
    }
}