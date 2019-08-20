using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Backend.Controllers;

namespace VSAND.Backend.Filters
{
    public class SchoolMasterAccountFilter : IAsyncActionFilter
    {
        private static readonly List<Type> _controllers = new List<Type>
        {
            typeof(SelfProvisionController),
            typeof(AccountController)
        };

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var isSchoolMasterAccount = resultContext.HttpContext.User.HasClaim(c => c.Type == "UserFunction.SchoolMasterAccount" && c.Value == "true");
            if (!isSchoolMasterAccount)
            {
                return;
            }

            if (!_controllers.Any(c => c.Equals(resultContext.Controller.GetType())))
            {
                resultContext.Result = new RedirectResult("/SelfProvision");
            }
        }
    }
}
