using Rise.Domain.Navigation;
using Rise.Domain.Tests.Fakers.Common;

namespace Rise.Domain.Tests.Fakers.Navigation;

public class ContentLocationFaker : EntityFaker<ContentLocation,int>
{
    public override ContentLocation Generate()
    {
        return new ContentLocation(1,"TestContentLocation");
    }
}