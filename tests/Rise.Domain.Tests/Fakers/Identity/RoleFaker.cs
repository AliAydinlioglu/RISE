using Rise.Domain.Identity;
using Rise.Domain.Tests.Fakers.Common;

namespace Rise.Domain.Tests.Fakers.Identity;

public class RoleFaker : EntityFaker<Role,Guid>
{
    public override Role Generate()
    {
        return new Role(Guid.NewGuid(),"TestRole");
    }
}