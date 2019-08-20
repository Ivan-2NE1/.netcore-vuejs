using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace VSAND.Backend.Policies.Requirements
{
    public class AnyRoleRequirement : IAuthorizationRequirement
    {
        public List<string> Roles;

        public AnyRoleRequirement(List<string> roles)
        {
            Roles = roles;
        }
    }
}
