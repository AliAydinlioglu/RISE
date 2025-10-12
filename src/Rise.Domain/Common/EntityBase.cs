namespace Rise.Domain.Common;

/// <summary>
/// Base class for providing auditing and soft-delete support.
/// <para>
/// This class supports non-generic infrastructure components such as triggers or generic configurations,
/// which cannot easily handle open generic types like Entity&lt;TId&gt;
/// </para>
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Date of the initial creation.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Date of the last update.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    /// <summary>
    /// Soft Delete indicator, instead of deleting rows, we flag them as deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}