﻿namespace PetFamily.Accounts.Infrastructure;

public class AdminOptions
{
    public const string Admin = nameof(Admin);

    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
