using Microsoft.AspNetCore.Components;
using Rise.Client.Identity;

namespace Rise.Client.Components.Main
{
    public partial class Header
    {
        private bool isDropdownOpen = false;
        void GoToLogin() { NavigationManager.NavigateTo("/login"); }

        private void ToggleDropdown(){isDropdownOpen = !isDropdownOpen;}

        private void CloseDropdown(){isDropdownOpen = false;}

        private void GoToAccountSettings(){
            NavigationManager.NavigateTo("/account");
            isDropdownOpen = false;}

        private async Task LogoutAsync(){
            await AccountManager.LogoutAsync();
            NavigationManager.NavigateTo("/login");
            isDropdownOpen = false;}


        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public required IAccountManager AccountManager { get; set; }

    }
}
