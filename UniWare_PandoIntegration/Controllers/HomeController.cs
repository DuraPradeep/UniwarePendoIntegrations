using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.Entities;

namespace UniWare_PandoIntegration.Controllers
{
    public class HomeController : Controller
    {
        ApiOperation ApiControl;
        [HttpPost]
        public async Task<ActionResult> Login(UserLogin login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (login.UserName == "admin" && login.Password == "admin")
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("https://localhost:7128");

                        //HttpResponseMessage htp = await client.GetAsync("api/UniwarePando/GetErrorCodes");
                        //var codes = htp.Content.ReadAsStringAsync().Result;
                        ////response = codes;

                        //return RedirectToAction("ErrorList");
                        return View("DashBoard");
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
           
            return View();
        }

        public ActionResult ErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/GetErrorCodes");
            if(response.ObjectParam.Count> 0)
            {                
                ViewData["UserName"] = 1;
            }
            return View(response.ObjectParam);
        }

        [HttpGet]
        public JsonResult Retrigger()
        {
            string msg;
            ApiControl = new ApiOperation();
            ServiceResponse<List<PostErrorDetails>> triggerid = new ServiceResponse<List<PostErrorDetails>>();
            triggerid = ApiControl.Get<ServiceResponse<List<PostErrorDetails>>>("api/UniwarePando/SendRecordStatus");
            if (triggerid.ObjectParam.Count > 0)
            {
                var postres = ApiControl.Get("api/UniwarePando/RetriggerPushData");
                var responses = ApiControl.Get("api/UniwarePando/Retrigger");
                msg = "Posted Failed Records";
            }
            else
            {
                var responses = ApiControl.Get("api/UniwarePando/Retrigger");
                msg = "Failed Record Triggered Successfully";
            }
            return Json(new {Message=msg});
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


    }
}
