using Microsoft.AspNetCore.Components;
using Rise.Client.Identity;

namespace Rise.Client.Components.Main
{
    //todo: verwijderen?
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
            //TODO: logout
            NavigationManager.NavigateTo("authentication/logout?returnUrl=/", forceLoad: true);
            
}


        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

    }
}
