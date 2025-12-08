namespace Rise.Client.Layout.Nav;

public interface INavigationSource
{
    HashSet<NavItem> GetItems();
}