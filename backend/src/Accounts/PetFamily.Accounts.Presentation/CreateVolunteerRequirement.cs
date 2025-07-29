namespace PetFamily.Accounts.Presentation;

// public class PermissionRequirement: IAuthorizationRequirement
// {
//     public PermissionRequirement(string code)
//     {
//         Code = code;
//     }
//
//     public string Code { get; }
// }
//
// public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
// {
//     protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
//     {
//         var permission = context.User.Claims.FirstOrDefault(c => c.Type == "Permission");
//         if (permission == null)
//         {
//             return Task.CompletedTask;
//         }
//
//         if (permission.Value == requirement.Code)
//         {
//             context.Succeed(requirement);
//         }
//         return Task.CompletedTask;
//     }
// }
