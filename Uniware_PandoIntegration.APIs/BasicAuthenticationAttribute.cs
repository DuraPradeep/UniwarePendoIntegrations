//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Http.Controllers;
//using System.Web.Http.Filters;
//using System.Security.Principal;
//using System.Threading;
//using System.Web;
//using System.Net.Http.Headers;
//using System.Runtime.Remoting.Contexts;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authentication;
//using Umbraco.Core.Services;
//using Microsoft.Extensions.Options;
//using Microsoft.Owin.Logging;
//using System.Text.Encodings.Web;
//using Umbraco.Core.Persistence.Repositories;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNetCore.Http;
//using Umbraco.Core.Models.Membership;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System.Web.Mvc;
//using Microsoft.AspNetCore.Mvc;

//namespace Uniware_PandoIntegration.APIs
//{

//    public class BasicAuthenticationFilterAttribute : Attribute, IAsyncAuthorizationFilter
//    {
//        public string Realm { get; set; }
//        public const string AuthTypeName = "Basic ";
//        private const string _authHeaderName = "Authorization";

//        public BasicAuthenticationFilterAttribute(string realm = null)
//        {
//            Realm = realm;
//        }

//        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//        {
//            try
//            {
//                var request = context?.HttpContext?.Request;
//                var authHeader = request.Headers.Keys.Contains(_authHeaderName) ? request.Headers[_authHeaderName].First() : null;
//                string encodedAuth = (authHeader != null && authHeader.StartsWith(AuthTypeName)) ? authHeader.Substring(AuthTypeName.Length).Trim() : null;
//                if (string.IsNullOrEmpty(encodedAuth))
//                {
//                    context.Result = new BasicAuthChallengeResult(Realm);
//                    return;
//                }

//                var (username, password) = DecodeUserIdAndPassword(encodedAuth);

//                // Authenticate credentials against database
//                var db = (ApplicationDbContext)context.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));
//                var userManager = (UserManager<User>)context.HttpContext.RequestServices.GetService(typeof(UserManager<User>));
//                var founduser = await db.Users.Where(u => u.Email == username).FirstOrDefaultAsync();
//                if (!await userManager.CheckPasswordAsync(founduser, password))
//                {
//                    // writing to the Result property aborts rest of the pipeline
//                    // see https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-3.0#cancellation-and-short-circuiting
//                    context.Result = new StatusCodeOnlyResult(StatusCodes.Status401Unauthorized);
//                }

//                // Populate user: adjust claims as needed
//                var claims = new[] { new Claim(ClaimTypes.Name, username, ClaimValueTypes.String, AuthTypeName) };
//                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthTypeName));
//                context.HttpContext.User = principal;
//            }
//            catch
//            {
//                // log and reject
//                context.Result = new StatusCodeOnlyResult(StatusCodes.Status401Unauthorized);
//            }
//        }

//        private static (string userid, string password) DecodeUserIdAndPassword(string encodedAuth)
//        {
//            var userpass = Encoding.UTF8.GetString(Convert.FromBase64String(encodedAuth));
//            var separator = userpass.IndexOf(':');
//            if (separator == -1)
//                return (null, null);

//            return (userpass.Substring(0, separator), userpass.Substring(separator + 1));
//        }
//    }

//    public class StatusCodeOnlyResult : ActionResult
//    {
//        protected int StatusCode;

//        public StatusCodeOnlyResult(int statusCode)
//        {
//            StatusCode = statusCode;
//        }

//        public override Task ExecuteResultAsync(ActionContext context)
//        {
//            context.HttpContext.Response.StatusCode = StatusCode;
//            return base.ExecuteResultAsync(context);
//        }
//    }

//    public class BasicAuthChallengeResult : StatusCodeOnlyResult
//    {
//        private string _realm;

//        public BasicAuthChallengeResult(string realm = "") : base(StatusCodes.Status401Unauthorized)
//        {
//            _realm = realm;
//        }

//        public override Task ExecuteResultAsync(ActionContext context)
//        {
//            context.HttpContext.Response.StatusCode = StatusCode;
//            context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"{BasicAuthenticationFilterAttribute.AuthTypeName} Realm=\"{_realm}\"");
//            return base.ExecuteResultAsync(context);
//        }
//    }


















//    //public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//    //{
//    //    private readonly IUserService _userService;




//    //public BasicAuthenticationHandler(
//    //    IOptionsMonitor<AuthenticationSchemeOptions> options,
//    //    ILoggerFactory logger,
//    //    UrlEncoder encoder,
//    //    ISystemClock clock,
//    //    IUserService userService) : base(options, logger, encoder, clock)
//    //{
//    //    _userService = userService;
//    //}

//    //    protected async override Task HandleAuthenticateAsync()
//    //    {
//    //        var authorizationHeader = Request.Headers["Authorization"].ToString();
//    //        if (authorizationHeader != null && authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
//    //        {
//    //            var token = authorizationHeader.Substring("Basic ".Length).Trim();
//    //            var credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
//    //            var credentials = credentialsAsEncodedString.Split(':');
//    //            if (await _userRepository.Authenticate(credentials[0], credentials[1]))
//    //            {
//    //                var claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "Admin") };
//    //                var identity = new ClaimsIdentity(claims, "Basic");
//    //                var claimsPrincipal = new ClaimsPrincipal(identity);
//    //                return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
//    //            }
//    //        }
//    //        Response.StatusCode = 401;
//    //        Response.Headers.Add("WWW-Authenticate", "Basic realm=\"joydipkanjilal.com\"");
//    //        return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
//    //    }


//    //}
//    //public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
//    //{
//    //    private const string Realm = "My Realm";
//    //    public override void OnAuthorization(HttpActionContext actionContext)
//    //    {
//    //        //If the Authorization header is empty or null
//    //        //then return Unauthorized
//    //        if (actionContext.Request.Headers.Authorization == null)
//    //        {
//    //            actionContext.Response = actionContext.Request
//    //                .CreateResponse(HttpStatusCode.Unauthorized);
//    //            // If the request was unauthorized, add the WWW-Authenticate header 
//    //            // to the response which indicates that it require basic authentication
//    //            if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
//    //            {
//    //                actionContext.Response.Headers.Add("WWW-Authenticate",
//    //                    string.Format("Basic realm=\"{0}\"", Realm));
//    //            }
//    //        }
//    //        else
//    //        {
//    //            //Get the authentication token from the request header
//    //            string authenticationToken = actionContext.Request.Headers
//    //                .Authorization.Parameter;
//    //            //Decode the string
//    //            string decodedAuthenticationToken = Encoding.UTF8.GetString(
//    //                Convert.FromBase64String(authenticationToken));
//    //            //Convert the string into an string array
//    //            string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
//    //            //First element of the array is the username
//    //            string username = usernamePasswordArray[0];
//    //            //Second element of the array is the password
//    //            string password = usernamePasswordArray[1];
//    //            //call the login method to check the username and password
//    //            if (username == "Uniware" && password == "Uniware@123")
//    //            {
//    //                var identity = new GenericIdentity(username);
//    //                IPrincipal principal = new GenericPrincipal(identity, null);
//    //                Thread.CurrentPrincipal = principal;
//    //                if (HttpContext.Current != null)
//    //                {
//    //                    HttpContext.Current.User = principal;
//    //                }
//    //            }
//    //            else
//    //            {
//    //                actionContext.Response = actionContext.Request
//    //                    .CreateResponse(HttpStatusCode.Unauthorized);
//    //            }
//    //        }
//    //    }
//    //}
//}
