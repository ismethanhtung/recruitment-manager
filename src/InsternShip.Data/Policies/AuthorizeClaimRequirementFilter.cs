using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace InsternShip.Data.Policies
{
    public class AuthorizeClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;
        public AuthorizeClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var check = context.HttpContext.User.Claims.ToList()[3].Value;
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Value == _claim.Value);
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
