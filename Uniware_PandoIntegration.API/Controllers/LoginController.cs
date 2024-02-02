using Microsoft.AspNetCore.Mvc;
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
        }
        private UniwareBL ObjBusinessLayer = new();
        [HttpGet]
        public ServiceResponse<UserLogin> GetUserNamePassword(string UserName, string Password)
        {
            ObjBusinessLayer = new UniwareBL();
            _logger.LogInformation("Check Credentials {DT}", DateTime.Now.ToLongTimeString());
            return ObjBusinessLayer.CheckLoginCredentials(UserName, Password);
        }
        [HttpGet]
        public ServiceResponse<MenusAccess> GetRoleMenuAccess(int UserId,string Enviornment)
        {
            ObjBusinessLayer = new UniwareBL();
            _logger.LogInformation("Get Role Menu Access {DT}", DateTime.Now.ToLongTimeString());
            return ObjBusinessLayer.GetRoleMenuAccess(UserId,Enviornment);
        }
        [HttpGet]
        public IEnumerable<UserProfile> GetRoleMaster(string Environment)
        {
            ObjBusinessLayer = new UniwareBL();
            _logger.LogInformation("Get Role Master at {DT}", DateTime.Now.ToLongTimeString());
            return ObjBusinessLayer.GetRoleMaster(Environment);
        }
        [HttpPost]
        public int SaveUser(UserProfile userLogin)
        {
            ObjBusinessLayer=new UniwareBL();
            return ObjBusinessLayer.SaveUser(userLogin);
        }
    }
}
