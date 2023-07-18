using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Uniware_PandoIntegration.API
{
    public class BasicAuthenticationFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public string Realm { get; set; }
        public const string AuthTypeName = "Basic ";
        private const string _authHeaderName = "Authorization";
        private static IConfiguration _configuration;
        public BasicAuthenticationFilterAttribute(string realm = null)
        {
            Realm = realm;
            // _configuration = configuration.GetSection("credentials");
        }

        public BasicAuthenticationFilterAttribute(IConfiguration configuration)
        {
             _configuration = configuration;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                string userid = _configuration.GetSection("credentials:userid").Value;
                string pwd = _configuration.GetSection("credentials:password").Value;

                var request = context?.HttpContext?.Request;
                var authHeader = request.Headers.Keys.Contains(_authHeaderName) ? request.Headers[_authHeaderName].First() : null;
                string encodedAuth = (authHeader != null && authHeader.StartsWith(AuthTypeName)) ? authHeader.Substring(AuthTypeName.Length).Trim() : null;
                if (string.IsNullOrEmpty(encodedAuth))
                {
                    context.Result = new BasicAuthChallengeResult(Realm);
                    return;
                }                
                var (username, password) = DecodeUserIdAndPassword(encodedAuth);

                // Authenticate credentials against database
                //var db = (ApplicationDbContext)context.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));
                //var userManager = (UserManager<User>)context.HttpContext.RequestServices.GetService(typeof(UserManager<User>));
                //var founduser = await db.Users.Where(u => u.Email == username).FirstOrDefaultAsync();

                if (!(username == userid && password == pwd))
                {
                    // writing to the Result property aborts rest of the pipeline
                    // see https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-3.0#cancellation-and-short-circuiting
                    context.Result = new StatusCodeOnlyResult(StatusCodes.Status401Unauthorized);
                }

                // Populate user: adjust claims as needed
                var claims = new[] { new Claim(ClaimTypes.Name, username, ClaimValueTypes.String, AuthTypeName) };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthTypeName));
                context.HttpContext.User = principal;
            }
            catch
            {
                // log and reject
                context.Result = new StatusCodeOnlyResult(StatusCodes.Status401Unauthorized);
            }
        }

        private static (string userid, string password) DecodeUserIdAndPassword(string encodedAuth)
        {
            var userpass = Encoding.UTF8.GetString(Convert.FromBase64String(encodedAuth));
            var separator = userpass.IndexOf(':');
            if (separator == -1)
                return (null, null);

            return (userpass.Substring(0, separator), userpass.Substring(separator + 1));
        }
    }

    public class StatusCodeOnlyResult : ActionResult
    {
        protected int StatusCode;

        public StatusCodeOnlyResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            return base.ExecuteResultAsync(context);
        }
    }

    public class BasicAuthChallengeResult : StatusCodeOnlyResult
    {
        private string _realm;

        public BasicAuthChallengeResult(string realm = "") : base(StatusCodes.Status401Unauthorized)
        {
            _realm = realm;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"{BasicAuthenticationFilterAttribute.AuthTypeName} Realm=\"{_realm}\"");
            return base.ExecuteResultAsync(context);
        }
    }
}
