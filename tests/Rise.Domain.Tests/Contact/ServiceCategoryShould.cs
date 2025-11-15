using Rise.Domain.Contact;

namespace Rise.Domain.Tests.Contact;

public class ServiceCategoryShould
{
    private const string CategoryName = "Administratief";
    
    [Fact]
    public void BeCreated_WithValidName()
    {
        var category = new FacilityCategory(CategoryName);

        category.ShouldNotBeNull();
        category.Name.ShouldBe(CategoryName);
    }

    [Fact]
    public void BeEqual_WhenNamesAreTheSame()
    {
        var category1 = new FacilityCategory(CategoryName);
        var category2 = new FacilityCategory(CategoryName);

        category1.ShouldBe(category2);
    }

    [Fact]
    public void NotBeEqual_WhenNamesAreDifferent()
    {
        var category1 = new FacilityCategory("Administratief");
        var category2 = new FacilityCategory("Ondersteunend");

        category1.ShouldNotBe(category2);
    }

    [Theory]
    [InlineData("Administratief", "Administratief", true)]
    [InlineData("Administratief", "Ondersteunend", false)]
    [InlineData("Veiligheid en welzijn", "Veiligheid en welzijn", true)]
    public void CompareEquality_Correctly(string name1, string name2, bool expectedEqual)
    {
        var category1 = new FacilityCategory(name1);
        var category2 = new FacilityCategory(name2);

        var areEqual = category1.Equals(category2);

        areEqual.ShouldBe(expectedEqual);
    }
}

