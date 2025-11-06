using Rise.Domain.Contact;

namespace Rise.Domain.Tests.Contact;

public class ServiceCategoryShould
{
    private const string CategoryName = "Administratief";
    
    [Fact]
    public void BeCreated_WithValidName()
    {
        var category = new ServiceCategory(CategoryName);

        category.ShouldNotBeNull();
        category.Name.ShouldBe(CategoryName);
    }

    [Fact]
    public void BeEqual_WhenNamesAreTheSame()
    {
        var category1 = new ServiceCategory(CategoryName);
        var category2 = new ServiceCategory(CategoryName);

        category1.ShouldBe(category2);
    }

    [Fact]
    public void NotBeEqual_WhenNamesAreDifferent()
    {
        var category1 = new ServiceCategory("Administratief");
        var category2 = new ServiceCategory("Ondersteunend");

        category1.ShouldNotBe(category2);
    }

    [Theory]
    [InlineData("Administratief", "Administratief", true)]
    [InlineData("Administratief", "Ondersteunend", false)]
    [InlineData("Veiligheid en welzijn", "Veiligheid en welzijn", true)]
    public void CompareEquality_Correctly(string name1, string name2, bool expectedEqual)
    {
        var category1 = new ServiceCategory(name1);
        var category2 = new ServiceCategory(name2);

        var areEqual = category1.Equals(category2);

        areEqual.ShouldBe(expectedEqual);
    }
}

