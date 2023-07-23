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

        public ActionResult ErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/GetErrorCodes");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
			}
            return View("~/Views/Home/Pv_ErrorList.cshtml", response.ObjectParam);
        }
        //public ActionResult STOPAPiErrorDetail()
        //{
        //    ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
        //    ApiControl = new ApiOperation();
        //    response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/STOApiErrorDetails");
        //    if (response.ObjectParam.Count > 0)
        //    {
        //        ViewData["UserName"] = 1;
        //    }
        //    return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml", response.ObjectParam);
        //}

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
            return Json(new { Message = msg });
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "Welcome to the Dashboard!!";
            return View();
        }

        public ActionResult Logout()
        {
            ViewBag.Message = "Logout Sucessfuly!!";
           
            return RedirectToAction("Login");
        }
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
            //return View("~/Views/Home/Pv_WaybillErrorList.cshtml");
        }
        public ActionResult ReturnOrderErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/ReturnOrderDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return View("~/Views/Home/Pv_ReturnOrderErrorList.cshtml", response.ObjectParam);
        }
        public ActionResult STOWaybillErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/STOWaybillErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return View("~/Views/Home/pv_STOWaybill.cshtml", response.ObjectParam);
            //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        }
        public ActionResult STOAPIErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/UniwarePando/STOApiErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["UserName"] = 1;
            }
            return View("~/Views/Home/Pv_STOAPIErrorList.cshtml", response.ObjectParam);
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
