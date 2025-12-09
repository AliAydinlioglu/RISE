using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Rise.Client.Components;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Identity;

public partial class Login
{
    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    private AccountRequest.Login Model = new();

    [Inject]
    public required NavigationManager Navigation { get; set; }

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private int randomNumber = new Random().Next(1, 9);
    private RiseButton? loginButtonRef;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                // Zet focus op login button
                await JSRuntime.InvokeVoidAsync("eval",
                    "document.querySelector('.button-field button')?.focus()");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error setting focus on login button");
            }
        }
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await HandleLogin();
        }
    }

    public async Task HandleLogin()
    {
        try
        {
            var returnUrl = ReturnUrl;
            
            if (!string.IsNullOrWhiteSpace(returnUrl))
                await JSRuntime.InvokeVoidAsync("localStorage.setItem", "loginReturnUrl", returnUrl);

            Navigation.NavigateTo("authentication/login");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Could not start Microsoft login");
        }
    }
}