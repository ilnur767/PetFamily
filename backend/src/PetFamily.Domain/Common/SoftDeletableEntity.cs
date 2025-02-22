using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Common;

public abstract class SoftDeletableEntity<TId> : Entity<TId>, ISoftDeletableEntity where TId : IComparable<TId>
{
    public SoftDeletableEntity(TId id) : base(id)
    {
    }

    public SoftDeletableEntity()
    {
    }

    /// <summary>
    ///     Признак удаления сущности.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    ///     Временная метка удаления сущности.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    public virtual void SoftDelete(DateTime deletedAt)
    {
        DeletedAt = deletedAt;
        IsDeleted = true;
    }

    public virtual void Restore()
    {
        DeletedAt = null;
        IsDeleted = false;
    }
}