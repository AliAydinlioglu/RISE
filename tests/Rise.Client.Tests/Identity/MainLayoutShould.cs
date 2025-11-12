using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor.Services;
using Rise.Client.Faker;
using Rise.Client.Layout;
using Rise.Client.Shared;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Rise.Client.Identity
{
    public class MainLayoutShould : TestContext
    {
        public MainLayoutShould(ITestOutputHelper outputHelper)
        {
            Services.AddXunitLogger(outputHelper);
            Services.AddMudServices();
            JSInterop.Mode = JSRuntimeMode.Loose;
            var pageTitleService = new FakePageTitleService();
            Services.AddScoped<IPageTitleService>(_ => pageTitleService);
        }

        [Fact]
        public void ShowLoginIcon_WhenNotAuthenticated()
        {
            var authContext = this.AddTestAuthorization();
            authContext.SetNotAuthorized();

            var cut = RenderComponent<MainLayout>();

            var avatar = cut.Find(".mud-avatar");
            avatar.ShouldNotBeNull();
        }

        [Fact]
        public void ShowUserInitials_WhenAuthenticated()
        {
            var authContext = this.AddTestAuthorization();
            authContext.SetAuthorized("Jan Janssens");
            authContext.SetClaims(new[]
            {
            new Claim("name", "Jan Janssens"),
            new Claim("email", "jan.janssens@hogent.be")
        });

            var cut = RenderComponent<MainLayout>();

            cut.Markup.ShouldContain("JJ");
        }


        [Theory]
        [InlineData("Jan Janssens", "JJ")]
        [InlineData("Marie", "MA")]
        [InlineData("john.doe@example.com", "JD")]
        [InlineData("test-user", "TU")]
        public void CalculateCorrectInitials_ForDifferentNames(string name, string expectedInitials)
        {
            var authContext = this.AddTestAuthorization();
            authContext.SetAuthorized(name);
            authContext.SetClaims(new[]
            {
            new Claim("name", name)
        });

            var cut = RenderComponent<MainLayout>();

            cut.Markup.ShouldContain(expectedInitials);
        }
    }

}
