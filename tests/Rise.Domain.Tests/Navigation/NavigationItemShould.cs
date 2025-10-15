using Rise.Domain.Navigation;

namespace Rise.Domain.Tests.Navigation;

public class NavigationItemShould
{
    private const int Id = 1;
    private const string Label = "label";
    private const string Icon = "icon";
    private const string Url = "url";
    
    [Fact]
    public void Be_Created_WhenValid_WithoutId()
    {
        // Act
        var navigationItem = new NavigationItem(Label, Icon, Url);

        // Assert
        navigationItem.ShouldNotBeNull();
        navigationItem.Label.ShouldBe(Label);
        navigationItem.Icon.ShouldBe(Icon);
        navigationItem.Url.ShouldBe(Url);
        navigationItem.RoleNavigationItems.ShouldBeEmpty();
    }
    
    [Fact]
    public void Be_Created_WhenValid_WithId()
    {
        // Act
        var navigationItem = new NavigationItem(Id,Label, Icon, Url);

        // Assert
        navigationItem.ShouldNotBeNull();
        navigationItem.Id.ShouldBe(Id);
        navigationItem.Label.ShouldBe(Label);
        navigationItem.Icon.ShouldBe(Icon);
        navigationItem.Url.ShouldBe(Url);
        navigationItem.RoleNavigationItems.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(Label, Icon,"")]
    [InlineData(Label, Icon," ")]
    [InlineData(Label, Icon, null)]
    [InlineData(Label, "",Url)]
    [InlineData(Label, " ",Url)]
    [InlineData(Label, null,Url)]
    [InlineData("", Icon,Url)]
    [InlineData(" ", Icon,Url)]
    [InlineData(null, Icon,Url)]
    public void Throw_WhenInvalid_WithoutId(string label, string icon, string url)
    {
        Should.Throw<ArgumentException>(() => new NavigationItem(label, icon, url));
    }
    
    [Theory]
    [InlineData(Id,Label, Icon,"")]
    [InlineData(Id,Label, Icon," ")]
    [InlineData(Id,Label, Icon, null)]
    [InlineData(Id,Label, "",Url)]
    [InlineData(Id,Label, " ",Url)]
    [InlineData(Id,Label, null,Url)]
    [InlineData(Id,"", Icon,Url)]
    [InlineData(Id," ", Icon,Url)]
    [InlineData(Id,null, Icon,Url)]
    [InlineData(0,Label, Icon,Url)]
    [InlineData(-1,Label, Icon,Url)]
    public void Throw_WhenInvalid_withId(int id,string label, string icon, string url)
    {
        Should.Throw<ArgumentException>(() => new NavigationItem(id,label, icon, url));
    }
}