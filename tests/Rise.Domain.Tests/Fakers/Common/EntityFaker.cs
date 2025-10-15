using Rise.Domain.Common;

namespace Rise.Domain.Tests.Fakers.Common;

public abstract class EntityFaker<TEntity,TId> where TEntity : Entity<TId>
{
    /// <summary>
    /// Maakt één entity aan.
    /// </summary>
    public abstract TEntity Generate();
}