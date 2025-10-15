using Microsoft.AspNetCore.Components;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Identity;

public partial class Login
{
    [SupplyParameterFromQuery] private string? ReturnUrl { get; set; }

    private AccountRequest.Login Model = new();
    private Result _result = new();
    [Inject] public required IAccountManager AccountManager { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }
    private int randomNumber = new Random().Next(1, 9);

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            NavigationManager.NavigateTo("/kalender");
        }
    }
    public async Task LoginUser()
    {
        //_result = await AccountManager.LoginAsync(Model.Email!, Model.Password!);
        _result = await AccountManager.LoginAsync("admin@example.com", "A1b2C3!");


        //if (_result.IsSuccess && !string.IsNullOrEmpty(ReturnUrl))
        //{
        //    Navigation.NavigateTo(ReturnUrl);
        //}
        await Task.Delay(400);
        Navigation.NavigateTo("/kalender");
    }
}