using System.Security.Claims;
using Rise.Client.Layout.Nav.Desktop;
using Rise.Client.Layout.Nav.Mobile;
using Rise.Shared.Identity;

namespace Rise.Client.Layout.Nav;

public class NavigationService
{
    public static List<NavItem> GetNavItemsDesktop(ClaimsPrincipal user)
    {
        INavigationSource navigationSource = new BaseNavigationSourceDesktop();

        if (user is { Identity.IsAuthenticated: true })
            navigationSource = new AuthorizedDesktopNavDecorator(navigationSource);
        
        return navigationSource.GetItems();
    }
    
    public static List<NavItem> GetNavItemsMobile(ClaimsPrincipal user)
    {
        INavigationSource navigationSource = new BaseNavigationSourceMobile();

        if (user.IsInRole(nameof(AppRoles.RegularStudent)))
            navigationSource = new RegularStudentMobileNavDecorator(navigationSource);
        else if (user.IsInRole(nameof(AppRoles.DistanceStudent)))
            navigationSource = new DistanceStudentMobileNavDecorator(navigationSource);
        else
            navigationSource = new PublicMobileNavDecorator(navigationSource);
        
        return navigationSource.GetItems();
    }
}