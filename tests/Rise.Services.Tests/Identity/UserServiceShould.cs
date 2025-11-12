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
    private const string Email = "example@example.com";
    
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
        var result =  await userService.GetOrCreateUserAsync(Oid);
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
        result.Errors
            .ShouldHaveSingleItem()
            .ShouldBe("User is not authenticated");
    }
    
    [Fact]
    public async Task ReturnUnauthorizedResult_WhenUserIsNotAuthenticated()
    {
        // arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate();
        var sessionProvider = new FakeSessionContextProvider(FakeClaimsPrincipal.Unauthenticated());
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync(Oid);
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
        result.Errors
            .ShouldHaveSingleItem()
            .ShouldBe("User is not authenticated");
    }
    
    [Fact]
    public async Task ReturnUnauthorizedResult_WhenOidNotGiven()
    {
        // arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate(); 
        var sessionProvider = new FakeSessionContextProvider(FakeClaimsPrincipal.WithClaimsForLoginCallback("", Email, AppRoles.RegularStudent));
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync(null!);
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
        result.Errors
            .ShouldHaveSingleItem()
            .ShouldBe("Oid is required");
    }
    
    [Fact]
    public async Task ReturnErrorResult_WhenFormatOfOidInClaimInvalid()
    {
        // arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate();
        var sessionProvider = new FakeSessionContextProvider(FakeClaimsPrincipal.WithClaimsForLoginCallback("abc", Email, AppRoles.RegularStudent));
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync("abc");
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
        result.Errors
            .ShouldHaveSingleItem()
            .ShouldBe("Oid is in a wrong format");
    }
    
    [Fact]
    public async Task ReturnErrorResult_WhenEmailNotProvided()
    {
        // arrange
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate();
        var sessionProvider = new FakeSessionContextProvider(FakeClaimsPrincipal.WithOidOnly(Oid));
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync(Oid);
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
        result.Errors
            .ShouldHaveSingleItem()
            .ShouldBe("Email is required");
    }
    
    [Fact]
    public async Task ReturnSuccessResult_WhenUserIsFoundAndUpdateSucceeds()
    {
        // arrange
        await using var dbContext = new ApplicationDbContext(GetDbContextOptions(nameof(ReturnSuccessResult_WhenUserIsFoundAndUpdateSucceeds)));

        var oid = new Guid(Oid);
        const string role = nameof(AppRoles.RegularStudent);
        var claimsPrincipal = FakeClaimsPrincipal.WithClaimsForLoginCallback(Oid, Email, role);
        var sessionProvider = new FakeSessionContextProvider(claimsPrincipal);
        
        var roleManager = FakeRoleManager.Generate(dbContext);
        await roleManager.CreateAsync(new ApplicationRole(AppRoles.RegularStudent, role));
        
        var userManager = FakeUserManager.Generate(dbContext);

        var fakeApplicationUser = FakeApplicationUser.New(oid);
        await userManager.CreateAsync(fakeApplicationUser);
        await userManager.AddToRoleAsync(fakeApplicationUser, role);
        
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync(Oid);
        
        //assert
        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.Email.ShouldBe(Email);
        result.Value.Roles.ShouldHaveSingleItem().ShouldBe(role);
    }
    
    [Fact]
    public async Task ReturnErrorResult_WhenUserIsFoundAndUpdateFails()
    {
        // arrange
        await using var dbContext = new ApplicationDbContext(GetDbContextOptions(nameof(ReturnErrorResult_WhenUserIsFoundAndUpdateFails)));

        var oid = new Guid(Oid);
        const string role = nameof(AppRoles.RegularStudent);
        var claimsPrincipal = FakeClaimsPrincipal.WithClaimsForLoginCallback(Oid, Email, role);
        var sessionProvider = new FakeSessionContextProvider(claimsPrincipal);
        
        var roleManager = FakeRoleManager.Generate(dbContext);
        await roleManager.CreateAsync(new ApplicationRole(AppRoles.RegularStudent, role));
        
        var userManager = FakeUserManager.Generate(dbContext, exceptionWhenUpdating: new Exception("store exception"));
        
        var fakeApplicationUser = FakeApplicationUser.New(oid);
        await userManager.CreateAsync(fakeApplicationUser);
        await userManager.AddToRoleAsync(fakeApplicationUser, role);
        
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync(Oid);
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
        result.Errors
            .ShouldHaveSingleItem()
            .ShouldBe("Something went wrong when updating user");
    }
    
    [Fact]
    public async Task ReturnCreatedResult_WhenUserIsNotFoundAndCreateSucceeds()
    {
        // arrange
        await using var dbContext = new ApplicationDbContext(GetDbContextOptions(nameof(ReturnCreatedResult_WhenUserIsNotFoundAndCreateSucceeds)));
        
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate(dbContext);

        var claimsPrincipal = FakeClaimsPrincipal.WithClaimsForLoginCallback(Oid, Email, "test"); 
        var sessionProvider = new FakeSessionContextProvider(claimsPrincipal);
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync(Oid);
        
        //assert
        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Created);
        result.Value.Email.ShouldBe(Email);
        result.Value.Roles.ShouldBeEmpty();
    }
    
    [Fact]
    public async Task ReturnCreatedResult_WhenUserIsNotFoundAndCreateFailsAndUserStillNotFound()
    {
        // arrange
        await using var dbContext = new ApplicationDbContext(GetDbContextOptions(nameof(ReturnCreatedResult_WhenUserIsNotFoundAndCreateFailsAndUserStillNotFound)));
        
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate(dbContext, exceptionWhenCreating: new DbUpdateException("unique constraint violation"));

        var claimsPrincipal = FakeClaimsPrincipal.WithClaimsForLoginCallback(Oid, Email, "test"); 
        var sessionProvider = new FakeSessionContextProvider(claimsPrincipal);
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync(Oid);
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
        result.Errors
            .ShouldHaveSingleItem()
            .ShouldBe("User with this info is already present, but for some reason not found.");
    }
    
    [Fact]
    public async Task ReturnCreatedResult_WhenUserIsNotFoundAndCreateFailsUnexpected()
    {
        // arrange
        await using var dbContext = new ApplicationDbContext(GetDbContextOptions(nameof(ReturnCreatedResult_WhenUserIsNotFoundAndCreateFailsAndUserStillNotFound)));
        
        var roleManager = FakeRoleManager.Generate();
        var userManager = FakeUserManager.Generate(dbContext, exceptionWhenCreating: new Exception("store create fails"));

        var claimsPrincipal = FakeClaimsPrincipal.WithClaimsForLoginCallback(Oid, Email, "test"); 
        var sessionProvider = new FakeSessionContextProvider(claimsPrincipal);
        var userService = new UserService(roleManager, userManager, sessionProvider);
        
        //act
        var result =  await userService.GetOrCreateUserAsync(Oid);
        
        //assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
        result.Errors
            .ShouldHaveSingleItem()
            .ShouldBe("Something went wrong when creating user");
    }
    
    private static DbContextOptions<ApplicationDbContext> GetDbContextOptions(string dbName)
    {
        ArgumentNullException.ThrowIfNull(dbName);
        
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName) // Do NOT use InMemoryDatabase... it's not reliable. Use a real database and come up with a strategy to clean up the database between tests.
            .Options;
    }
}