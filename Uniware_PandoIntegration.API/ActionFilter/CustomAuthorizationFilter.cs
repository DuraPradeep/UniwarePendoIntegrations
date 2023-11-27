using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Uniware_PandoIntegration.API.ActionFilter
{
    //public class CustomAuthorizationFilter : ActionFilterAttribute,IAsyncAuthorizationFilter
    //{
    //    public AuthorizationPolicy Policy { get; }

    //    public CustomAuthorizationFilter()
    //    {
    //        Policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    //    }

    //    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    //    {
    //        if (context == null)
    //        {
    //            throw new ArgumentNullException(nameof(context));
    //        }        
    //        if (context.Filters.Any(item => item is IAllowAnonymousFilter))
    //        {
    //            return;
    //        }
    //        var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
    //        var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
    //        var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

    //        if (authorizeResult.Challenged)
    //        {
    //            // Return custom 401 result
    //            context.Result = new JsonResult(new
    //            {
    //                Reason = "Unauthorized",
    //                status = "FAILED",
    //                message = "Resource requires authentication. Please check your authorization token."
    //                //status: "FAILED",
    //                //reason: "Unauthorized",
    //                //message: "Resource requires authentication. Please check your authorization token."
    //            })
    //            {
    //                StatusCode = StatusCodes.Status401Unauthorized
    //            };
    //        }
    //    }
    //}
}
