using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.Entities;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Net.Security;
 
namespace UniWare_PandoIntegration.Controllers
{
    public class HomeController : Controller
    {
        
        ApiOperation ApiControl;


		[HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApiControl = new ApiOperation();
					ServiceResponse<UserLogin> serviceResponseg = ApiControl.Get<ServiceResponse<UserLogin>, string, string>(UserName, Password, "UserName", "Password", "Api/Login/GetUserNamePassword");
                    if (serviceResponseg.ObjectParam.UserName == UserName && serviceResponseg.ObjectParam.Password == Password)
                    {

						HttpContext.Session.SetString("UserName", serviceResponseg.ObjectParam.UserName);
                        HttpContext.Session.SetString("NotificationCount", "1");
                        ViewBag.Message = "Welcome "+ HttpContext.Session.GetString("UserName")+ " to the Dashboard!!";
                        return View("Dashboard");
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Credential!!";
                        return View("Login");
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }
        [HttpGet]
        public ActionResult ErrorList()
        {
            //ViewBag.response = msg;
            //ViewData["response"] = msg;
            //if (msg != null)
            //{
            //    HttpContext.Session.SetString("response", msg);
            //}
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/GetErrorCodes");
            for (int i = 0; i < response.ObjectParam.Count; i++)
            {
                if (!response.ObjectParam[i].Triggerid.Equals("NA"))
                {
                    HttpContext.Session.SetString("Saletriggerid", response.ObjectParam[i].Triggerid);
                }               
            }
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
			}
            return View("~/Views/Home/Pv_ErrorList.cshtml", response.ObjectParam);
        }
        public JsonResult ErrorListDataObject()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/GetErrorCodes");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return Json(response.ObjectParam);
        }

        [HttpGet]
        public JsonResult Retrigger()
        {
            string msg;
            ApiControl = new ApiOperation();
            ServiceResponse<List<PostErrorDetails>> triggerid = new ServiceResponse<List<PostErrorDetails>>();
            ServiceResponse<IActionResult> responses = new ServiceResponse<IActionResult>();
            //triggerid = ApiControl.Get<ServiceResponse<List<PostErrorDetails>>>("api/UniwarePando/SendRecordStatus");
            var triggerids=HttpContext.Session.GetString("Saletriggerid");
            if (triggerids!=null)
            {
                var postres = ApiControl.Get<ServiceResponse<string>>("api/UniwarePando/RetriggerPushData");
                responses = ApiControl.Get<ServiceResponse<IActionResult>>("api/UniwarePando/Retrigger");
                //msg = "Posted Failed Records";
                msg = responses.ObjectParam.ToString();
            }
            else
            {
                responses = ApiControl.Get<ServiceResponse<IActionResult>>("api/UniwarePando/Retrigger");
                msg = "Failed Record Triggered Successfully";
                //msg= responses.ObjectParam.ToString();
                
            }
            return Json(new { Message = msg });
            //return RedirectToAction("ErrorList","Home",new { msg });
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            //ViewBag.Message = "Welcome to the Dashboard!!";
            return View();
        }

        public ActionResult Logout()
        {
            ViewBag.Message = "Logout Sucessfuly!!";
           
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult WaybillErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/waybillErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return View("~/Views/Home/Pv_WaybillErrorList.cshtml", response.ObjectParam);
            
        }
        [HttpGet]
        public JsonResult WaybillRetrigger()
        {
            string msg;
            ApiControl = new ApiOperation();
            ServiceResponse<List<PostErrorDetails>> triggerid = new ServiceResponse<List<PostErrorDetails>>();
            
            
                var responses = ApiControl.Get("api/UniwarePando/PostWaybillGeneration");
                
                msg = responses;
          
            return Json(new { Message = msg });
        }
        public JsonResult WaybillErrorListDataObject()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/waybillErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return Json(response.ObjectParam);
            //return View("~/Views/Home/Pv_WaybillErrorList.cshtml");
        }
        public ActionResult ReturnOrderErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/ReturnOrderDetails");
            for (int i = 0; i < response.ObjectParam.Count; i++)
            {
                if (!response.ObjectParam[i].Triggerid.Equals("NA"))
                {
                    HttpContext.Session.SetString("ReturnTriId", response.ObjectParam[i].Triggerid);
                }

            }
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return View("~/Views/Home/Pv_ReturnOrderErrorList.cshtml", response.ObjectParam);
        }
        public JsonResult ReturnOrderErrorListDataObject()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/ReturnOrderDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return Json(response.ObjectParam);
        }
        [HttpGet]
        public JsonResult ReturnOrderRetrigger()
        {
            string msg;
            ApiControl = new ApiOperation();
            var triggerids = HttpContext.Session.GetString("ReturnTriId");
            if (triggerids == null)
            {
                var postres = ApiControl.Get("api/UniwarePando/ReturnorderFinalData");
                var responses = ApiControl.Get("api/UniwarePando/ReturnOrderAPIRetrigger");
                msg = responses;
            }
            else
            {
                var responses = ApiControl.Get("api/UniwarePando/ReturnOrderAPIRetrigger");
                msg = responses;
            }
            return Json(new { Message = msg });
        }
        [HttpGet]
        public ActionResult STOWaybillErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/STOWaybillErrorDetails");
            for (int i = 0; i < response.ObjectParam.Count; i++)
            {
                if (!response.ObjectParam[i].Triggerid.Equals("NA"))
                {
                    HttpContext.Session.SetString("STOWaybillTriId", response.ObjectParam[i].Triggerid);
                }

            }
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return View("~/Views/Home/pv_STOWaybill.cshtml", response.ObjectParam);            
        }
        public JsonResult STOWaybillErrorListDataObject()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/STOWaybillErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return Json(response.ObjectParam);
           
        }
        [HttpPost]
        public JsonResult STOWaybillRetrigger()
        {
            string msg;
            ApiControl= new ApiOperation();
            var triggerids = HttpContext.Session.GetString("STOWaybillTriId");
            if (triggerids == null)
            {
                var postres = ApiControl.Get("api/UniwarePando/STOwaybillFinalData");
                var responses = ApiControl.Get("api/UniwarePando/STOWaybillRetrigger");
                msg = responses;
            }
            else
            {
                var responses = ApiControl.Get("api/UniwarePando/STOWaybillRetrigger");
                msg = responses;
            }
            return Json(new { Message = msg });
        }
        [HttpGet]
        public ActionResult STOAPIErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/STOApiErrorDetails");
            for (int i = 0; i < response.ObjectParam.Count; i++)
            {
                if (!response.ObjectParam[i].Triggerid.Equals("NA"))
                {
                    HttpContext.Session.SetString("STOAPITriId", response.ObjectParam[i].Triggerid);
                }

            }
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return View("~/Views/Home/Pv_STOAPIErrorList.cshtml", response.ObjectParam);
            //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        }

        public JsonResult STOAPIErrorListData()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/STOApiErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return Json(response.ObjectParam);
            //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        }
        //public ActionResult WaybillErrorList()
        //{
        //    ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
        //    ApiControl = new ApiOperation();
        //    response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/aybillErrorDetails");
        //    if (response.ObjectParam.Count > 0)
        //    {
        //        ViewData["UserName"] = 1;
        //    }
        //    return View("~/Views/Home/Pv_STOAPIErrorList.cshtml", response.ObjectParam);
        //    //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        //}
        [HttpGet]
        public JsonResult STOAPIRetrigger()
        {
            string msg;
            var triggerids = HttpContext.Session.GetString("STOAPITriId");
            if (triggerids == null)
            {
                var postres = ApiControl.Get("api/UniwarePando/STOAPIFinaldata");
                var responses = ApiControl.Get("api/UniwarePando/STOAPIRetrigger");
                msg = responses;
            }
            else
            {
                var responses = ApiControl.Get("api/UniwarePando/STOAPIRetrigger");
                msg = responses;
            }
            return Json(new { Message = msg });
        }

        public ActionResult NotificationErrorListCount()
        {
            //sale order
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/GetErrorCodes");
            int count = 0;
            List<string> strings = new List<string>();
            string name;
            if (response.ObjectParam.Count > 0)
            {
                name = "Sale Order API";
                count += 1;
                strings.Add(name);
            }
            //waybill
            ServiceResponse<List<CodesErrorDetails>> waybill = new ServiceResponse<List<CodesErrorDetails>>();
            //ApiControl = new ApiOperation();
            waybill = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/waybillErrorDetails");
            if (waybill.ObjectParam.Count > 0)
            {
                name = "Waybill generation";
                count += 1;
                ViewData["UserName"] = 1;
                strings.Add(name);
            }
            //return Order
            ServiceResponse<List<CodesErrorDetails>> resturnorer = new ServiceResponse<List<CodesErrorDetails>>();
            //ApiControl = new ApiOperation();
            resturnorer = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/ReturnOrderDetails");
            if (resturnorer.ObjectParam.Count > 0)
            {
                name = "return Order";
                count += 1;
                ViewData["UserName"] = 1;
                strings.Add(name);
            }
            //STO Waybill
            ServiceResponse<List<CodesErrorDetails>> STOwaybill = new ServiceResponse<List<CodesErrorDetails>>();
            //ApiControl = new ApiOperation();
            STOwaybill = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/STOWaybillErrorDetails");
            if (STOwaybill.ObjectParam.Count > 0)
            {
                name = "STO Waybill";
                count += 1;
                ViewData["UserName"] = 1;
                strings.Add(name);
            }
            //STO API
            ServiceResponse<List<CodesErrorDetails>> STOAPI = new ServiceResponse<List<CodesErrorDetails>>();
            //ApiControl = new ApiOperation();
            STOAPI = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/STOApiErrorDetails");
            if (STOAPI.ObjectParam.Count > 0)
            {
                name = "STO API";
                count += 1;
                ViewData["UserName"] = 1;
                strings.Add(name);
            }
            var result = new { name = strings, ID = count };
            //return Json(count,strings);
            return Json(result);           
        }
    }
}
