using Microsoft.AspNetCore.Components;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Rise.Client.Identity
{
    public class LoginShould : TestContext
    {
        public LoginShould(ITestOutputHelper outputHelper)
        {
            Services.AddXunitLogger(outputHelper);

        }

        [Fact]
        public void ShowLoginButton_WhenNotAuthenticated()
        {
            var authContext = this.AddTestAuthorization();
            authContext.SetNotAuthorized();

            var cut = RenderComponent<Login>();

            var button = cut.Find("button");
            button.TextContent.ShouldContain("AANMELDEN");
        }

        [Fact]
        public void NavigateToMicrosoftAuth_WhenLoginClicked()
        {
            var authContext = this.AddTestAuthorization();
            authContext.SetNotAuthorized();

            var navMan = Services.GetRequiredService<NavigationManager>();
            var cut = RenderComponent<Login>();

            var button = cut.Find("button");
            button.Click();

            navMan.Uri.ShouldContain("authentication/login");
        }
        
    }
}