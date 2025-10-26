using System.Security.Claims;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Persistence.Models.Identity;
using Rise.Services.Identity;
using Rise.Shared.Identity;
using Rise.TestDoubles.Fakers;

namespace Rise.Services.Tests.Identity;

public class UserServiceShould
{
    private const string Oid = "62f44ebb-1305-40ed-8c51-1d8858289cb2";
    
    [Fact]
    public async Task ReturnPublicRole_WhenUserIsUnknown()
    {
        // Arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate();
        var userService = new UserService(roleManager, userManager, new FakeSessionContextProvider());

        // Act
        var result = await userService.GetRoleIdAsync(null);

        // Assert
        result.ShouldBe(new Guid(AppRoles.Public));
    }
    
    [Fact]
    public async Task ReturnPublicRole_WhenUserHasNoRole()
    {
        // Arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate();
        var userService = new UserService(roleManager, userManager, new FakeSessionContextProvider());

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([], "mock"));

        // Act
        var result = await userService.GetRoleIdAsync(claimsPrincipal);

        // Assert
        result.ShouldBe(new Guid(AppRoles.Public));
    }
    
    [Fact]
    public async Task ReturnSpecificRole_WhenUserHasRole()
    {
        // Arrange
        await using var dbContext = new ApplicationDbContext(GetDbContextOptions(nameof(ReturnSpecificRole_WhenUserHasRole)));
        
        var roleManager = FakeRoleManager.Generate(dbContext);
        var userManager = FakeUserManager.Generate();

        var role = new ApplicationRole(AppRoles.RegularStudent, "Regular");
        await roleManager.CreateAsync(role);
        
        var userService = new UserService(roleManager, userManager, new FakeSessionContextProvider());

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, "Regular")], "mock"));

        // Act
        var result = await userService.GetRoleIdAsync(claimsPrincipal);

        // Assert
        result.ShouldBe(new Guid(AppRoles.RegularStudent));
    }

    [Fact]
    public async Task ReturnUnauthorizedResult_WhenUserIsNull()
    {
        // arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate();
        var userService = new UserService(roleManager, userManager, new FakeSessionContextProvider());
        
        //act
        var result =  await userService.GetOrCreateUserAsync();
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
        result.Errors.ShouldHaveSingleItem("User is not authenticated");
    }
    
    [Fact]
    public async Task ReturnUnauthorizedResult_WhenUserIsNotAuthenticated()
    {
        // arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate();
        var identity = new  ClaimsIdentity(); 
        var sessionProvider = new FakeSessionContextProvider(new ClaimsPrincipal(identity));
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync();
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
        result.Errors.ShouldHaveSingleItem("User is not authenticated");
    }
    
    [Fact]
    public async Task ReturnUnauthorizedResult_WhenUserHasNoOidClaim()
    {
        // arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate();
        var identity = new  ClaimsIdentity(new List<Claim>(), "SomeAuthType"); 
        var sessionProvider = new FakeSessionContextProvider(new ClaimsPrincipal(identity));
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync();
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
        result.Errors.ShouldHaveSingleItem("Oid of user is not known and therefore not authenticated");
    }
    
    [Fact]
    public async Task ReturnErrorResult_WhenFormatOfOidInClaimInvalid()
    {
        // arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate();
        var identity = new  ClaimsIdentity(new List<Claim>(){ new("oid","abc") }, "SomeAuthType"); 
        var sessionProvider = new FakeSessionContextProvider(new ClaimsPrincipal(identity));
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync();
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
        result.Errors.ShouldHaveSingleItem("Oid is in a wrong format");
    }
    
    [Fact]
    public async Task ReturnCreatedResult_WhenUserIsNotFound()
    {
        // arrange
        await using var dbContext = new ApplicationDbContext(GetDbContextOptions(nameof(ReturnCreatedResult_WhenUserIsNotFound)));

        var email = "example@example.com";
        
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate(dbContext);
        
        var identity = new  ClaimsIdentity([
            new Claim("oid",Oid), 
            new Claim(ClaimTypes.Email, email)
        ], "SomeAuthType"); 
        var sessionProvider = new FakeSessionContextProvider(new ClaimsPrincipal(identity));
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync();
        
        //assert
        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Created);
        result.Value.Email.ShouldBe(email);
        result.Value.Roles.ShouldBeEmpty();
    }
    
    [Fact]
    public async Task ReturnSuccessResult_WhenUserIsFound()
    {
        // arrange
        await using var dbContext = new ApplicationDbContext(GetDbContextOptions(nameof(ReturnSuccessResult_WhenUserIsFound)));

        var oid = new Guid(Oid);
        var email = "example@example.com";
        var role = nameof(AppRoles.RegularStudent);
        
        var identity = new  ClaimsIdentity([
            new Claim("oid",Oid), 
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        ], "SomeAuthType"); 
        var sessionProvider = new FakeSessionContextProvider(new ClaimsPrincipal(identity));
        
        var roleManager = FakeRoleManager.Generate(dbContext);
        await roleManager.CreateAsync(new ApplicationRole(AppRoles.RegularStudent, role));
        
        var userManager = FakeUserManager.Generate(dbContext);

        var fakeApplicationUser = FakeApplicationUser.Generate(oid);
        await userManager.CreateAsync(fakeApplicationUser);
        await userManager.AddToRoleAsync(fakeApplicationUser, role);
        
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync();
        
        //assert
        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.Email.ShouldBe(email);
        result.Value.Roles.ShouldHaveSingleItem(role);
    }
    
    private static DbContextOptions<ApplicationDbContext> GetDbContextOptions(string dbName)
    {
        ArgumentNullException.ThrowIfNull(dbName);
        
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName) // Do NOT use InMemoryDatabase... it's not reliable. Use a real database and come up with a strategy to clean up the database between tests.
            .Options;
    }
}