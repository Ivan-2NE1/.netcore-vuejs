using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VSAND.Backend.Policies.Requirements;

namespace VSAND.Backend.Policies.Handlers
{
    public class PermissionHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach(var requirement in pendingRequirements)
            {
                if (requirement is AnyRoleRequirement)
                {
                    var roles = (requirement as AnyRoleRequirement).Roles;

                    if (InAnyRole(context.User, roles))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }

            return Task.CompletedTask;
        }

        private bool InAnyRole(ClaimsPrincipal user, List<string> roles)
        {
            return roles.Any(r => user.Claims.Any(c => c.Type == r && c.Value == "true"));
        }
    }
}
