//using Microsoft.AspNetCore.Authorization.Policy;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Authorization;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using System.Web.WebPages;
//using Uniware_PandoIntegration.APIs;
//using System.Diagnostics;
//using Uniware_PandoIntegration.Entities;


//namespace UniWare_PandoIntegration.ActionFilters
//{
//    public class ActionFilterExample : ActionFilterAttribute
//    {
//        //void IActionFilter.OnActionExecuted(ActionExecutedContext context)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        //void IActionFilter.OnActionExecuting(ActionExecutingContext context)
//        //{
//        //    //var data = context.HttpContext.Request.Headers["LoginId"].ToString();
//        //    if (context.HttpContext.Request.Headers["LoginId"].ToString() != null)
//        //    {
//        //        return;
//        //    }
//        //    else
//        //    {
//        //        context.Result = new RedirectToRouteResult(
//        //          new RouteValueDictionary { { "Controller", "Home" }, { "Action", "SessionExpire" }, { "Area", "" } });
//        //    }
//        //    if (context.HttpContext.Request.Headers["X-Requested-With"]== "XMLHttpRequest")
//        //    {
//        //        context.Result = new RedirectToRouteResult(
//        //        new RouteValueDictionary { { "Controller", "Home" }, { "Action", "SessionExpire" }, { "Area", "" } });
//        //    }
//        //    else
//        //    {
//        //        RouteValueDictionary redirectTarget =
//        //        new RouteValueDictionary { { "Controller", "Home" }, { "Action", "SessionExpire" }, { "Area", "" } };
//        //        context.Result = new RedirectToRouteResult(redirectTarget);
//        //    }
//        //    throw new NotImplementedException();
//        //}
//        //public override void OnActionExecuting(ActionExecutingContext filterContext)
//        //{
//        //    base.OnActionExecuting(filterContext);
//        //    if (filterContext.HttpContext == null || filterContext.HttpContext.Session.GetString("LoginId") == null || filterContext.HttpContext.Session.GetString("Environment") ==null)
//        //    {
//        //        //return RedirectToAction("Index", "Login");
//        //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
//        //        {
//        //            controller = "Home",
//        //            action = "Login"
//        //        }));
//        //    }
//        //}
//    }
//    public class RequestAuthenticationFilter : IActionFilter
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        public RequestAuthenticationFilter(IHttpContextAccessor httpContextAccessor)
//        {
//            _httpContextAccessor = httpContextAccessor;
//        }
//        /// 

//        /// OnActionExecuting
//        /// 

//        /// 
//        public void OnActionExecuting(ActionExecutingContext context)
//        {
//            //var session = context.HttpContext.Session;
//            //if (session.Keys.Contains("LoginId") == null)
//            //{
//            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "SessionExpire", controller = "Home" }));
//            //}
//            //else
//            //{
//            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Home" }));

//            //}
//        }
//        /// 

//        /// OnActionExecuted
//        /// 

//        /// 
//        public void OnActionExecuted(ActionExecutedContext context)
//        {
//            //var session = context.HttpContext.Session;
//            //if (session.Keys.Contains("LoginId") == null)
//            //{
//            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "SessionExpire", controller = "Home" }));
//            //}
//            //else
//            //{

//            //}
//        }
//    }
//}
