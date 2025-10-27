using Rise.Client.Components.Navigation;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.Components.NavigationComponent
{
    public class NavItemShould : TestContext
    {
        public NavItemShould(ITestOutputHelper outputHelper)
        {
            Services.AddXunitLogger(outputHelper);
        }

        [Theory]
        [InlineData("Home", "icons/home.svg", "/home")]
        [InlineData("Settings", "icons/settings.svg", "/settings")]
        public void RenderTheItemCorrectly(string label, string icon, string link)
        {
            // Arrange
            var cut = RenderComponentNavItem(label, icon, link);

            // Act
            var anchor = cut.Find("a");

            // Assert
            anchor.GetAttribute("href").ShouldBe(link);
            anchor.InnerHtml.ShouldContain(label);
            anchor.InnerHtml.ShouldContain(icon);

            var img = cut.Find("i");
            img.GetAttribute("class").ShouldBe(icon);

            var labelElement = cut.Find("label");
            labelElement.TextContent.ShouldBe(label);
        }
        private IRenderedComponent<NavItem> RenderComponentNavItem(
            string label, string icon, string url)
        {
            return RenderComponent<NavItem>(parameters =>
            {
                parameters
                    .Add(param => param.Label, label)
                    .Add(param => param.Icon, icon)
                    .Add(param => param.Link, url);
            });
        }
    }
}

