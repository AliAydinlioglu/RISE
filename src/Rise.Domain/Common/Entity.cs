namespace Rise.Domain.Common;

/// <summary>
/// Entity Base Class, all entities should inherit from this. (read: Entity = Row in SQL terms)
/// <para>
/// TId is provided to make this base class more flexible with the type of Primary Key.
/// It makes it possible to define the PK as whatever type needed, even combined keys.
/// </para>
/// </summary>
public abstract class Entity<TId> : EntityBase
{
    /// <summary>
    /// Primary Key of the <see cref="Entity"/>
    /// </summary>
    public TId Id { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (EqualityComparer<TId>.Default.Equals(Id, default) ||
            EqualityComparer<TId>.Default.Equals(other.Id, default))
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b) => !(a == b);

    public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
}