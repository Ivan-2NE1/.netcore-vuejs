using Hangfire.Dashboard;

namespace VSAND.Backend.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.HasClaim("UserFunction.Admin", "true");
        }
    }
}
