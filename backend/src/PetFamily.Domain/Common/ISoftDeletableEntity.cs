﻿namespace PetFamily.Domain.Common;

public interface ISoftDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}