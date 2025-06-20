﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.Entities;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Net.Security;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UniWare_PandoIntegration.Controllers
{
    public class HomeController : Controller
    {

        ApiOperation ApiControl;
        private readonly IConfiguration iconfiguration;
        private readonly string Apibase;
        public HomeController(IConfiguration configuration)
        {
            Apibase = configuration.GetSection("baseaddress:Url").Value;
        }
        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApiControl = new ApiOperation(Apibase);
                    ServiceResponse<UserLogin> serviceResponseg = ApiControl.Get<ServiceResponse<UserLogin>, string, string>(UserName, Password, "UserName", "Password", "Api/Login/GetUserNamePassword");
                    if (serviceResponseg.ObjectParam.UserName == UserName && serviceResponseg.ObjectParam.Password == Password)
                    {
                        HttpContext.Session.SetString("UserName", serviceResponseg.ObjectParam.UserName);
                        HttpContext.Session.SetString("Password", serviceResponseg.ObjectParam.Password);
                        HttpContext.Session.SetString("LoginId", serviceResponseg.ObjectParam.LoginID);
                        HttpContext.Session.SetString("Role", serviceResponseg.ObjectParam.RoleId);
                        HttpContext.Session.SetString("Environment", serviceResponseg.ObjectParam.Environment);
                        HttpContext.Session.SetString("NotificationCount", "1");
                        //TempData["Success"] = "Welcome "+ HttpContext.Session.GetString("UserName")+ " to the Dashboard!!";
                        //TempData["Success"] = "Welcome " + HttpContext.Session.GetString("UserName")+ " to the Dashboard!!";
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        TempData["Success"] = "Invalid Credential!!";
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
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/GetSaleOrderErrorCodes");

            for (int i = 0; i < response.ObjectParam.Count; i++)
            {
                if (!response.ObjectParam[i].Triggerid.Equals("NA"))
                {
                    HttpContext.Session.SetString("Saletriggerid", response.ObjectParam[i].Triggerid);
                }
                if (response.ObjectParam[i].Reason!= "INVALID_SALE_ORDER_CODE")
                {                    
                        ViewData["FailedStatus"] = 1;
                }
            }
            
            return View("~/Views/Home/Pv_ErrorList.cshtml", response.ObjectParam);
        }
        public JsonResult ErrorListDataObject()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/GetSaleOrderErrorCodes");

            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return Json(response.ObjectParam);
        }

        [HttpGet]
        public JsonResult Retrigger()
        {
            string msg;
            ApiControl = new ApiOperation(Apibase);
            //ServiceResponse<List<PostErrorDetails>> triggerid = new ServiceResponse<List<PostErrorDetails>>();
            UserProfile Enviornment = new UserProfile();
            Enviornment.Environment = HttpContext.Session.GetString("Environment").ToString();

            //ServiceResponse<IActionResult> responses = new ServiceResponse<IActionResult>();
            //var responses = "";
            //triggerid = ApiControl.Get<ServiceResponse<List<PostErrorDetails>>>("api/Calling/SendRecordStatus");
            var triggerids = HttpContext.Session.GetString("Saletriggerid");
            if (triggerids != null)
            {
                var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/RetriggerPushData");
                //responses = ApiControl.Get<string>(Enviornment, "api/Calling/Retrigger");
                //responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/Retrigger");
                msg = responses.Remove(0, 1).Remove(responses.Length - 2, 1);
            }
            else
            {
                //responses = ApiControl.Get("api/Calling/Retrigger");
                var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/Retrigger");
                msg = responses.Remove(0, 1).Remove(responses.Length - 2, 1); ;

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
            ServiceResponse<MenusAccess> serviceResponse1 = new ServiceResponse<MenusAccess>();

            int LoginId = Convert.ToInt32(HttpContext.Session.GetString("LoginId"));
            ApiControl = new ApiOperation(Apibase);
            var roleid = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(roleid))
            {
                // Handle missing Role, e.g. redirect to login page
                return RedirectToAction("Login");
            }

            //var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            if (TempData["Success"] == null)
            {
                TempData["Success"] = "Welcome " + HttpContext.Session.GetString("UserName") + " to the Dashboard!!";
            }
            if(roleid=="3")
            {
                return View("~/Views/TrackingDashboard/Dashboard.cshtml");
            }
            //serviceResponse1 = ApiControl.Get<ServiceResponse<MenusAccess>, int, string>(LoginId, Enviornment, "UserId", "Enviornment", "Api/Login/GetRoleMenuAccess");
            //if (serviceResponse1 == null)
            //{
            //    TempData["Success"] = "Menus Not Assigned to " + HttpContext.Session.GetString("UserName");
            //    return View(new MenusAccess());
            //}


            //return View(serviceResponse1.ObjectParam);
            return View();
        }

        public ActionResult Logout()
        {
            TempData["Success"] = "Logout Sucessfuly!!";

            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult WaybillErrorList()
        {
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            ServiceResponse<List<EndpointErrorDetails>> response = new ServiceResponse<List<EndpointErrorDetails>>();//ApiControl = new ApiOperation();
            response = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/waybillErrorDetails");
             if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return View("~/Views/Home/Pv_WaybillErrorList.cshtml", response.ObjectParam);

        }
        [HttpGet]
        public JsonResult WaybillRetrigger()
        {
            string msg;
            ApiControl = new ApiOperation(Apibase);
            //var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            UserProfile Enviornment = new UserProfile();
            Enviornment.Environment= HttpContext.Session.GetString("Environment").ToString();
            //var postres = ApiControl.Get<string>(Enviornment, "api/Calling/PostWaybillGeneration");
            var response = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/Retriggerwaybill");
            msg = response.Remove(0, 1).Remove(response.Length - 2, 1);

            return Json(new { Message = msg });
        }
        public JsonResult WaybillErrorListDataObject()
        {
            ServiceResponse<List<EndpointErrorDetails>> response = new ServiceResponse<List<EndpointErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            response = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/waybillErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return Json(response.ObjectParam);
            //return View("~/Views/Home/Pv_WaybillErrorList.cshtml");
        }
        public ActionResult ReturnOrderErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/ReturnOrderDetails");
            for (int i = 0; i < response.ObjectParam.Count; i++)
            {
                if (!response.ObjectParam[i].Triggerid.Equals("NA"))
                {
                    HttpContext.Session.SetString("ReturnTriId", response.ObjectParam[i].Triggerid);
                }

            }
            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return View("~/Views/Home/Pv_ReturnOrderErrorList.cshtml", response.ObjectParam);
        }
        public JsonResult ReturnOrderErrorListDataObject()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            //response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/Calling/ReturnOrderDetails");
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/ReturnOrderDetails");

            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return Json(response.ObjectParam);
        }
        [HttpGet]
        public JsonResult ReturnOrderRetrigger()
        {
            string msg;
            ApiControl = new ApiOperation(Apibase);
            //var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            UserProfile Enviornment = new UserProfile();
            Enviornment.Environment= HttpContext.Session.GetString("Environment").ToString();
            var triggerids = HttpContext.Session.GetString("ReturnTriId");
            if (triggerids != null)
            {
                //var postres = ApiControl.Get("api/Calling/ReturnorderFinalData");
                //var responses = ApiControl.Get("api/Calling/ReturnOrderAPIRetrigger");
                var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/ReturnorderFinalData");
                //var postres = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/ReturnOrderAPIRetrigger");

                msg = responses.ToString();
            }
            else
            {
                var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/ReturnOrderAPIRetrigger");
                msg = responses.ToString();
                //msg = "Failed Record Triggered Successfully";
            }
            return Json(new { Message = msg });
        }
        [HttpGet]
        public ActionResult STOWaybillErrorList()
        {
            HttpContext.Session.Remove("StowaybillError");
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/STOWaybillErrorDetails");
            for (int i = 0; i < response.ObjectParam.Count; i++)
            {
                if (!response.ObjectParam[i].Triggerid.Equals("NA"))
                {
                    HttpContext.Session.SetString("STOWaybillTriId", response.ObjectParam[i].Triggerid);
                }
            }
            if (response.ObjectParam.Count > 0)
            {
                HttpContext.Session.SetString("StowaybillError", "1");
            }
            else
            {
                HttpContext.Session.SetString("StowaybillError", "0");
            }
            return View("~/Views/Home/pv_STOWaybill.cshtml", response.ObjectParam);
        }
        public JsonResult STOWaybillErrorListDataObject()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            //response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/Calling/STOWaybillErrorDetails");
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/STOWaybillErrorDetails");

            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return Json(response.ObjectParam);

        }
        [HttpGet]
        public JsonResult STOWaybillRetrigger()
        {
            string msg;
            ApiControl = new ApiOperation(Apibase);
            UserProfile Enviornment = new UserProfile();
            Enviornment.Environment= HttpContext.Session.GetString("Environment").ToString();

            var triggerids = HttpContext.Session.GetString("STOWaybillTriId");
            if (triggerids != null)
            {
                //var postres = ApiControl.Get<string>(Enviornment, "api/Calling/STOwaybillFinalData");
                //var responses = ApiControl.Get<string>(Enviornment, "api/Calling/STOWaybillRetrigger");
                //var responses = ApiControl.Get("api/Calling/STOWaybillRetrigger");
                var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/STOwaybillFinalData");

                msg = responses;
                //msg = "Posted Failed Records";
            }
            else
            {
                //var responses = ApiControl.Get("api/Calling/STOWaybillRetrigger");
                var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/STOWaybillRetrigger");

                msg = responses;
                //msg = "Failed Record Triggered Successfully";
            }
            return Json(new { Message = msg });
        }
        [HttpGet]
        public ActionResult STOAPIErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/STOApiErrorDetails");
            for (int i = 0; i < response.ObjectParam.Count; i++)
            {
                if (!response.ObjectParam[i].Triggerid.Equals("NA"))
                {
                    HttpContext.Session.SetString("STOAPITriId", response.ObjectParam[i].Triggerid);
                }

            }
            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return View("~/Views/Home/Pv_STOAPIErrorList.cshtml", response.ObjectParam);
            //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        }

        public JsonResult STOAPIErrorListData()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            //response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>>("api/Calling/STOApiErrorDetails");
            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/STOApiErrorDetails");

            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return Json(response.ObjectParam);
            //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        }

        [HttpGet]
        public JsonResult STOAPIRetrigger()
        {
            string msg;
            ApiControl = new ApiOperation(Apibase);
            UserProfile Enviornment = new UserProfile();
            Enviornment.Environment= HttpContext.Session.GetString("Environment").ToString();

            var triggerids = HttpContext.Session.GetString("STOAPITriId");
            if (triggerids != null)
            {
                //    var postres = ApiControl.Get<string>(Enviornment, "api/Calling/STOAPIFinaldata");
                //    var responses = ApiControl.Get<string>(Enviornment, "api/Calling/STOAPIRetrigger");
                var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/STOAPIFinaldata");
                //var responses = ApiControl.Get("api/Calling/STOAPIRetrigger");
                msg = responses.ToString();
            }
            else
            {
                var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/STOAPIRetrigger");
                msg = responses.ToString();
            }
            return Json(new { Message = msg });
        }

        public ActionResult NotificationErrorListCount()
        {
            //sale order
            //ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);

            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var countDetailsresponse = ApiControl.Get<ServiceResponse<ErrorCountEntitys>, string>(Enviornment, "Enviornment", "api/Calling/GetErrorCountDetails");

            //response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/GetSaleOrderErrorCodes");
            int count = 0;
            List<string> strings = new List<string>();
            string name;
            if (countDetailsresponse.ObjectParam.SaleorderDetails.Count> 0)
            {
                name = "Sale Order API";
                count += 1;
                strings.Add(name);
            }
            if (countDetailsresponse.ObjectParam.WaybillError.Count > 0)
            {
                name = "Waybill generation";
                count += 1;
                ViewData["FailedStatus"] = 1;
                strings.Add(name);
            }

            if (countDetailsresponse.ObjectParam.STOWaybill.Count > 0)
            {
                name = "STO Waybill";
                count += 1;
                ViewData["FailedStatus"] = 1;
                strings.Add(name);
            }
            if (countDetailsresponse.ObjectParam.STOAPI.Count > 0)
            {
                name = "STO API";
                count += 1;
                ViewData["FailedStatus"] = 1;
                strings.Add(name);
            }
            if (countDetailsresponse.ObjectParam.UpdateShippingError.Count > 0)
            {
                name = "Update Shipping";
                count += 1;
                ViewData["FailedStatus"] = 1;
                strings.Add(name);
            }
            if (countDetailsresponse.ObjectParam.AllocateShippingError.Count > 0)
            {
                name = "Allocate Shipping";
                count += 1;
                ViewData["FailedStatus"] = 1;
                strings.Add(name);
            }

            //if (response.ObjectParam.Count > 0)
            //{
            //    name = "Sale Order API";
            //    count += 1;
            //    strings.Add(name);
            //}
            ////waybill
            //ServiceResponse<List<EndpointErrorDetails>> waybill = new ServiceResponse<List<EndpointErrorDetails>>();
            ////ApiControl = new ApiOperation();
            //waybill = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/waybillErrorDetails");
            //if (waybill.ObjectParam.Count > 0)
            //{
            //    name = "Waybill generation";
            //    count += 1;
            //    ViewData["FailedStatus"]  = 1;
            //    strings.Add(name);
            //}
            ////return Order
            ////ServiceResponse<List<CodesErrorDetails>> resturnorer = new ServiceResponse<List<CodesErrorDetails>>();
            //////ApiControl = new ApiOperation();
            ////resturnorer = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/ReturnOrderDetails");
            ////if (resturnorer.ObjectParam.Count > 0)
            ////{
            ////    name = "return Order";
            ////    count += 1;
            ////    ViewData["FailedStatus"]  = 1;
            ////    strings.Add(name);
            ////}
            ////STO Waybill
            //ServiceResponse<List<CodesErrorDetails>> STOwaybill = new ServiceResponse<List<CodesErrorDetails>>();
            ////ApiControl = new ApiOperation();
            //STOwaybill = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/STOWaybillErrorDetails");
            //if (STOwaybill.ObjectParam.Count > 0)
            //{
            //    name = "STO Waybill";
            //    count += 1;
            //    ViewData["FailedStatus"]  = 1;
            //    strings.Add(name);
            //}
            ////STO API
            //ServiceResponse<List<CodesErrorDetails>> STOAPI = new ServiceResponse<List<CodesErrorDetails>>();
            ////ApiControl = new ApiOperation();
            //STOAPI = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/STOApiErrorDetails");
            //if (STOAPI.ObjectParam.Count > 0)
            //{
            //    name = "STO API";
            //    count += 1;
            //    ViewData["FailedStatus"]  = 1;
            //    strings.Add(name);
            //}
            //ServiceResponse<List<EndpointErrorDetails>> UpdateShiping = new ServiceResponse<List<EndpointErrorDetails>>();
            //UpdateShiping = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/UpdateShippingErrorDetails");
            //if (UpdateShiping.ObjectParam.Count > 0)
            //{
            //    name = "Update Shipping";
            //    count += 1;
            //    ViewData["FailedStatus"]  = 1;
            //    strings.Add(name);
            //}
            //ServiceResponse<List<EndpointErrorDetails>> AlocateShiping = new ServiceResponse<List<EndpointErrorDetails>>();
            //AlocateShiping = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/AloateShippingErrorDetails");
            //if (AlocateShiping.ObjectParam.Count > 0)
            //{
            //    name = "Allocate Shipping";
            //    count += 1;
            //    ViewData["FailedStatus"]  = 1;
            //    strings.Add(name);
            //}

            var result = new { name = strings, ID = count };
            //return Json(count,strings);
            return Json(result);
        }

        [HttpGet]
        public ActionResult UpdateShippingErrorList()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/UpdateShippingErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return View("~/Views/Home/pv_UpdateShipping.cshtml", response.ObjectParam);

        }

        public JsonResult UpdateShippingErrorListData()
        {
            ServiceResponse<List<CodesErrorDetails>> response = new ServiceResponse<List<CodesErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            response = ApiControl.Get<ServiceResponse<List<CodesErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/UpdateShippingErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return Json(response.ObjectParam);
            //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        }
        [HttpGet]
        public JsonResult UpdateShippingRetrigger()
        {
            string msg;
            ApiControl = new ApiOperation(Apibase);
            UserProfile Enviornment = new UserProfile();
            Enviornment.Environment = HttpContext.Session.GetString("Environment").ToString();

            ServiceResponse<List<PostErrorDetails>> triggerid = new ServiceResponse<List<PostErrorDetails>>();
            //var responses = ApiControl.Get<string>(Enviornment, "api/Calling/RetriggerUpdateShipping");
            var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/RetriggerUpdateShipping");


            msg = responses;
            //msg = "Trigger successfully";

            return Json(new { Message = msg });
        }
        [HttpGet]
        public ActionResult AllocateShippingErrorList()
        {
            ServiceResponse<List<EndpointErrorDetails>> response = new ServiceResponse<List<EndpointErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            response = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/AloateShippingErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return View("~/Views/Home/Pv_AlocateShipping.cshtml", response.ObjectParam);
            //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        }
        [HttpGet]
        public JsonResult AlocateShippingRetrigger()
        {
            string msg;
            ApiControl = new ApiOperation(Apibase);
            UserProfile Enviornment = new UserProfile();
            Enviornment.Environment= HttpContext.Session.GetString("Environment").ToString();

            //var responses = ApiControl.Get<string>(Enviornment, "api/Calling/RetriggerAllocateShipping");
            var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/RetriggerAllocateShipping");
            msg = responses;
            return Json(new { Message = msg });
        }
        public JsonResult AlocateShippingErrorListData()
        {
            ServiceResponse<List<EndpointErrorDetails>> response = new ServiceResponse<List<EndpointErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            //response = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>>("api/Calling/AloateShippingErrorDetails");
            response = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/AloateShippingErrorDetails");

            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return Json(response.ObjectParam);
            //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        }
        [HttpGet]
        public ActionResult ReversePickUpErrorList()
        {
            ServiceResponse<List<EndpointErrorDetails>> response = new ServiceResponse<List<EndpointErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            response = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/ReversePickupErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return View("~/Views/Home/pv_ReversePickup.cshtml", response.ObjectParam);
        }
        [HttpGet]
        public JsonResult ReversePickupRetrigger()
        {
            string msg;
            ApiControl = new ApiOperation(Apibase);
            UserProfile Enviornment = new UserProfile();
            Enviornment.Environment= HttpContext.Session.GetString("Environment").ToString();

            //var responses = ApiControl.Get<string>(Enviornment, "api/Calling/RetriggerreversePickup");
            var responses = ApiControl.Post1<ServiceResponse<string>, UserProfile>(Enviornment, "api/Calling/RetriggerreversePickup");

            //var responses = ApiControl.Get("api/Calling/RetriggerreversePickup");
            msg = responses;
            return Json(new { Message = msg });
        }
        public JsonResult ReversePickupErrorListData()
        {
            ServiceResponse<List<EndpointErrorDetails>> response = new ServiceResponse<List<EndpointErrorDetails>>();
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            response = ApiControl.Get<ServiceResponse<List<EndpointErrorDetails>>, string>(Enviornment, "Enviornment", "api/Calling/ReversePickupErrorDetails");
            if (response.ObjectParam.Count > 0)
            {
                ViewData["FailedStatus"]  = 1;
            }
            return Json(response.ObjectParam);
            //return View("~/Views/Home/Pv_STOWaybillErrorList.cshtml");
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            ApiControl = new ApiOperation(Apibase);
            UserProfile userProfile = new UserProfile();            
            var Environment = HttpContext.Session.GetString("Environment").ToString(); 
            var serviceResponse = ApiControl.Get<List<UserProfile>, string>(Environment, "Environment", "Api/Login/GetRoleMaster");
            userProfile.RoleNameList = new SelectList(serviceResponse, "Roleid", "RoleName");
            return View(userProfile);
        }
        [HttpPost]
        public IActionResult SaveUser(UserProfile userProfile)
        {
            userProfile.Environment= HttpContext.Session.GetString("Environment").ToString();

            //var response = ApiControl.Post1<ServiceResponse<string>, UserProfile>(userProfile, "Api/Login/SaveUser");
            ApiControl = new ApiOperation(Apibase);

            var response = ApiControl.Post1<ServiceResponse<string>, UserProfile>(userProfile, "Api/Login/SaveUser");
            if (response == "0")
            {
                TempData["Success"] = "Something Went Wrong!";
            }
            else
            {
                TempData["Success"] = "Uase Added Successfully !!";
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            UserProfile userProfile = new UserProfile();

            return View();
        }
        [HttpPost]
        public IActionResult Reset(UserProfile userProfile)
        {
            ApiControl = new ApiOperation(Apibase);
            userProfile.Environment = HttpContext.Session.GetString("Environment").ToString();
            var response = ApiControl.Post1<ServiceResponse<string>, UserProfile>(userProfile, "Api/Calling/ResetPassword").Trim();
            TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            return RedirectToAction("Dashboard");
        }
    }
}
