using System.Security.Claims;

namespace Rise.Shared.Identity.Accounts;

public static partial class AccountRequest
{
    public class LoginCallback
    {
        public required string Oid { get; set; }
    }
}

public static partial class AccountResponse
{
    public class LoginCallback
    {
        public required string Email { get; set; }
        /// <summary>
        /// Only used for conditional rendering in frontend! Should not be the only source for security checks!
        /// </summary>
        public string[] Roles { get; set; } = [];
    }
}