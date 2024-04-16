using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Uniware_PandoIntegration.API.Folder;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.BusinessLayer;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            //_logger.LogInformation("execute");
        }
        private UniwareBL ObjBusinessLayer = new();
        [HttpGet]
        public ServiceResponse<UserLogin> GetUserNamePassword(string UserName, string Password)
        {
            ObjBusinessLayer = new UniwareBL();
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Check Credentials Username: "+UserName+" Password:- "+Password);
            return ObjBusinessLayer.CheckLoginCredentials(UserName, Password);
        }
        [HttpGet]
        public ServiceResponse<MenusAccess> GetRoleMenuAccess(int UserId,string Enviornment)
        {
            ObjBusinessLayer = new UniwareBL();
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()},Get Role Menu Access ");
            return ObjBusinessLayer.GetRoleMenuAccess(UserId,Enviornment);
        }
        [HttpGet]
        public IEnumerable<UserProfile> GetRoleMaster(string Environment)
        {
            ObjBusinessLayer = new UniwareBL();
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Get Role Master");
            return ObjBusinessLayer.GetRoleMaster(Environment);
        }
        [HttpPost]
        public int SaveUser(UserProfile userLogin)
        {
            ObjBusinessLayer=new UniwareBL();
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, User Create, User Details:- {JsonConvert.SerializeObject(userLogin)}");

            return ObjBusinessLayer.SaveUser(userLogin);
        }
    }
}
