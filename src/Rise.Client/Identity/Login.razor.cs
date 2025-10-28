using Microsoft.AspNetCore.Components;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Identity;

public partial class Login
{
    [SupplyParameterFromQuery] private string? ReturnUrl { get; set; }

    private AccountRequest.Login Model = new();
    [Inject] public required NavigationManager Navigation { get; set; }
    private int randomNumber = new Random().Next(1, 9);

    public void HandleLogin()
    {
        try
        {
            var returnUrl = ReturnUrl ?? "/kalender";

            Navigation.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(returnUrl)}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Could not start Microsoft login");
        }
    }

}