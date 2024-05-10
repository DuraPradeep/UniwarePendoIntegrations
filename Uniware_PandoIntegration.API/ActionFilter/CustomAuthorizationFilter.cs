using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Web.WebPages;
using System.Diagnostics;
using Uniware_PandoIntegration.BusinessLayer;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.API.ActionFilter
{
   
    public class ActionFilterExample : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (token != "")
            {
                //var Real = token.Split(" ")[1].ToString();
                var jwthandler = new JwtSecurityTokenHandler();
                var jwttoken = jwthandler.ReadToken(token.Split(" ")[1].ToString());
                var Username = (new ICollectionDebugView<System.Security.Claims.Claim>(((JwtSecurityToken)jwttoken).Claims.ToList()).Items[0]).Value;
                using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetTempPath(), "SaveFile.txt")))
                {
                    sw.WriteLine(Username);
                }


                //ObjBusinessLayer.InsertUsername(Username);

                //value = jwttoken.ToString();
            }

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // our code after action executes
        }
    }
    public class AsyncActionFilterExample : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // execute any code before the action executes
            var result = await next();
            // execute any code after the action executes
        }
    }

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
      

    internal sealed class ICollectionDebugView<T>
    {
        private readonly ICollection<T> _collection;

        public ICollectionDebugView(ICollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            _collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                T[] items = new T[_collection.Count];
                _collection.CopyTo(items, 0);
                return items;
            }
        }
    }
}
