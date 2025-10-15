using Rise.Domain.Navigation;
using Rise.Domain.Tests.Fakers.Common;

namespace Rise.Domain.Tests.Fakers.Navigation;

public class NavigationItemFaker : EntityFaker<NavigationItem,int>
{
    public override NavigationItem Generate()
    {
        return new NavigationItem(1,"label", "icon", "url");
    }
}