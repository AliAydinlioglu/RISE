using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Client.Identity
{
    public class FakeAuthenticationService
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; } = "Test User";
        public string Email { get; set; } = "test@hogent.be";

        public ClaimsPrincipal GetUser()
        {
            if (!IsAuthenticated)
                return new ClaimsPrincipal(new ClaimsIdentity());

            var claims = new[]
            {
            new Claim("name", UserName),
            new Claim("email", Email),
            new Claim("preferred_username", Email)
        };

            var identity = new ClaimsIdentity(claims, "Test");
            return new ClaimsPrincipal(identity);
        }
    }
}