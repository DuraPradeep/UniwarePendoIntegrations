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
            _logger.LogInformation("Login time at {DT}", DateTime.Now.ToLongTimeString());
            return ObjBusinessLayer.CheckLoginCredentials(UserName, Password);
        }
    }
}
