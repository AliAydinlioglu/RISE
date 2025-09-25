using Rise.Shared.Products;
using Xunit.Abstractions;
using Shouldly;
using System.Collections.Generic;

namespace Rise.Client.Products;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class IndexShould : TestContext
{
    public IndexShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
		Services.AddScoped<IProductService, FakeProductService>(); // Use our own Faker
		// Authenticate as a test user for all the tests in this class
		// Can also be done on the test level.
		this.AddTestAuthorization().SetAuthorized("TEST USER");
    }

    [Fact]
	public void ShowsProducts()
	{

		
		var cut = RenderComponent<Index>();
		cut.FindAll("table tbody tr").Count.ShouldBe(5);
	}
}
