﻿using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Framework.Authorization;

public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement
{
    public PermissionAttribute(string code) : base(code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
