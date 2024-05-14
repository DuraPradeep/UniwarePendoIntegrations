using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.DataAccessLayer;
using Uniware_PandoIntegration.Entities;
using System.Configuration;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Formatters;
using Uniware_PandoIntegration.BusinessLayer;
using System.Xml.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Net;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Uniware_PandoIntegration.API.Folder;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Primitives;
using Uniware_PandoIntegration.API.ActionFilter;
using Azure;
using System.Collections;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.Xml;
using Microsoft.OpenApi.Validations.Rules;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml.Office2019.Presentation;
using NuGet.Protocol;
using DocumentFormat.OpenXml.Bibliography;
using RepoDb.Extensions.QueryFields;
using static Uniware_PandoIntegration.API.DelegateCalling;
 //using static Uniware_PandoIntegration.API.ActionFilter.CustomAuthorizationFilter;

namespace Uniware_PandoIntegration.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UniwarePandoController : ControllerBase
    {
        private readonly ILogger<UniwarePandoController> _logger;
        private readonly IUniwarePando _jWTManager;
        private readonly string Configuration;
        private readonly IConfiguration iconfiguration;

        public UniwarePandoController(ILogger<UniwarePandoController> logger, IUniwarePando uniwarePando, IConfiguration configuration)
        {
            _logger = logger;
            _jWTManager = uniwarePando;
            iconfiguration = configuration;
        }
        BearerToken _Token = new BearerToken();
        private UniwareBL ObjBusinessLayer = new();
        MethodWrapper _MethodWrapper = new MethodWrapper();
        DelegateCalling obj = new DelegateCalling();

        
        [HttpPost]
        public IActionResult authToken(TokenEntity tokenEntity)
        {
            //GenerateToken generateToken=new GenerateToken(null) ;
            var token = _jWTManager.GenerateJWTTokens(tokenEntity, out tokenEntity);
            var result = "";
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Token Request:- {JsonConvert.SerializeObject(token)}");
            try
            {
                if (token == null)
                {
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error Object {JsonConvert.SerializeObject(token)}");
                    return Unauthorized(new { status = "INVALID_CREDENTIALS", token = "Invalid credentials" });
                }
                else
                {
                    result = JsonConvert.SerializeObject(new { status = "SUCCESS", token = token });
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()},Success Token:-  {JsonConvert.SerializeObject(token)}");
                }
            }
            catch (Exception ex) { _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Token Error {JsonConvert.SerializeObject(ex)}"); }
            return Ok(result);
        }
        //[CustomAuthorizationFilter]

        //[ServiceFilter(typeof(ActionFilterExample))]
        [Authorize]
        [HttpPost]
        public IActionResult waybill(OmsToPandoRoot Records)
        {
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Waybill Get Data From Pando {JsonConvert.SerializeObject(Records)} ,{DateTime.Now.ToLongTimeString()}");

            ServiceResponse<parentList> parentList = new ServiceResponse<parentList>();
            ErrorResponse errorResponse = new ErrorResponse();

            try
            {
                //Thread.Sleep(5000);
                //string Username = string.Empty;
                //using (StreamReader sr = new StreamReader(Path.Combine(Path.GetTempPath(), "SaveFile.txt")))
                //{
                //    Username = sr.ReadLine();
                //    sr.Close();
                //}

                //using (FileStream stream = System.IO.File.Open(Path.Combine(Path.GetTempPath(), "SaveFile.txt"), FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                //{
                //    //StreamReader streamReader = new StreamReader(stream);
                //    //Username = streamReader.ReadLine();
                //    //stream.Close();

                //    byte[] buffer = new byte[stream.Length];
                //    int bytesread = stream.Read(buffer, 0, buffer.Length);
                //    Username = Encoding.ASCII.GetString(buffer, 0, bytesread).Trim();
                //    stream.Close();
                //}
                HttpContext httpContext = HttpContext;
                var jwthandler = new JwtSecurityTokenHandler();

                var token = httpContext.Request.Headers["Authorization"].ToString();
                var jwttoken = jwthandler.ReadToken(token.Split(" ")[1].ToString());
                var Username = (new ICollectionDebugView<System.Security.Claims.Claim>(((JwtSecurityToken)jwttoken).Claims.ToList()).Items[0]).Value;
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()},Waybill Instance Name. {Username}");


                Task.Run(() =>
                {
                    obj.CallingWaybill(Records, Username);
                });

                if (Records != null)
                {
                    errorResponse.status = "FAILED";
                    errorResponse.reason = "AWB not generated";
                    errorResponse.message = "AWB generation is in queue, please check after a few mins";
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, WayBill response {JsonConvert.SerializeObject(errorResponse)}");
                }
                else
                {
                    errorResponse.status = "FAILED";
                    errorResponse.reason = "Data Not came From Uniware";
                    errorResponse.message = "Resource requires authentication. Please check your authorization token.";
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(errorResponse)}");
                }

                //var jsoncodes = JsonConvert.SerializeObject(new { code = Records.Shipment.SaleOrderCode });
                //string Instance = string.Empty;
                //for (int x = 0; x < Records.Shipment.customField.Count; x++)
                //{
                //    if (Records.Shipment.customField[x].name == "INDENTID_DFX")
                //        Instance = "DFX";
                //    else if (Records.Shipment.customField[x].name == "INDENTID_SH")
                //        Instance = "SH";
                //}
                //var resu = _Token.GetTokens(Servertype, Instance).Result;
                //var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                //string token = deres.access_token.ToString();
                //parentList = _MethodWrapper.PassCodeer(jsoncodes, token, "", 0, Servertype, Instance);
                //string FacilityCode = string.Empty;
                //for (int i = 0; i < parentList.ObjectParam.saleOrderItems.Count; i++)
                //{
                //    if (parentList.ObjectParam.saleOrderItems[i].shippingPackageCode == Records.Shipment.code)
                //    {
                //        FacilityCode = parentList.ObjectParam.saleOrderItems[i].facilityCode;
                //    }
                //}


                //RootResponse rootResponse = new RootResponse();
                //string primaryid = ObjBusinessLayer.insertWaybillMain(Records, Servertype);
                //ObjBusinessLayer.insertWaybillshipment(Records, primaryid, FacilityCode, Servertype);
                //List<Item> items = new List<Item>();
                //List<CustomField> customfields = new List<CustomField>();
                //for (int i = 0; i < Records.Shipment.items.Count; i++)
                //{
                //    Item item = new Item();
                //    item.name = Records.Shipment.items[i].name;
                //    item.description = Records.Shipment.items[i].description;
                //    item.quantity = Records.Shipment.items[i].quantity;
                //    item.skuCode = Records.Shipment.items[i].skuCode;
                //    item.itemPrice = Records.Shipment.items[i].itemPrice;
                //    item.imageURL = Records.Shipment.items[i].imageURL;
                //    item.hsnCode = Records.Shipment.items[i].hsnCode;
                //    item.tags = Records.Shipment.items[i].tags;
                //    items.Add(item);
                //}
                //for (int i = 0; i < Records.Shipment.customField.Count; i++)
                //{
                //    CustomField customFieldValue = new CustomField();
                //    customFieldValue.name = Records.Shipment.customField[i].name;
                //    customFieldValue.value = Records.Shipment.customField[i].value;
                //    customfields.Add(customFieldValue);
                //}
                //ObjBusinessLayer.insertWaybilldeliveryaddress(Records.deliveryAddressDetails, primaryid, Servertype);
                //ObjBusinessLayer.insertWaybillpickupadres(Records.pickupAddressDetails, primaryid, Servertype);
                //ObjBusinessLayer.insertWaybillReturnaddress(Records.returnAddressDetails, primaryid, Servertype);
                //ObjBusinessLayer.InsertCustomfieldWaybill(customfields, primaryid, Records.Shipment.code, Servertype);
                //ObjBusinessLayer.InsertitemWaybill(items, primaryid, Records.Shipment.code, Servertype);

                //var sendwaybilldata = ObjBusinessLayer.GetWaybillAllRecrdstosend(Instance, Servertype);
                //_logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, WayBill Data Get From Database:- {JsonConvert.SerializeObject(sendwaybilldata)}");
                //if (sendwaybilldata.Count > 0)
                //{
                //    var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendwaybilldata, Servertype, Instance);
                //    var postres = _MethodWrapper.WaybillGenerationPostData(sendwaybilldata, 0, triggerid, Servertype);
                //    if (postres.IsSuccess)
                //    {
                //        errorResponse.status = "FAILED";
                //        errorResponse.reason = "AWB not generated";
                //        errorResponse.message = "AWB generation is in queue, please check after a few mins";
                //        _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, WayBill response {JsonConvert.SerializeObject(errorResponse)}");
                //    }
                //    else
                //    {
                //        errorResponse.status = "FAILED";
                //        errorResponse.reason = postres.ObjectParam;
                //        errorResponse.message = "Resource requires authentication. Please check your authorization token.";
                //        _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(errorResponse)}");
                //    }
                //    //_logger.LogInformation($"Reason:-  {postres.ObjectParam},{DateTime.Now.ToLongTimeString()}");
                //    //return Accepted(postres.Result.ObjectParam);


                //    //return new JsonResult(errorResponse);
                //}
                //else
                //{

                //}

            }
            catch (Exception ex)
            {
                //ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.status = "FAILED";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Resource requires authentication. Please check your authorization token.";
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(errorResponse)}");
                throw;
            }
            return new JsonResult(errorResponse);

        }

       
        //[ServiceFilter(typeof(ActionFilterExample))]
        [HttpPost]
        [Authorize]
        public IActionResult UpdateShippingPackage(List<UpdateShippingpackage> shippingPackages)
        {
            List<UpdateShippingpackagedb> updatelist = new List<UpdateShippingpackagedb>();
            List<ShippingBoxdb> shipbox = new List<ShippingBoxdb>();
            List<addCustomFieldValue> customFields = new List<addCustomFieldValue>();
            _logger.LogInformation($"DateTime:- {DateTime.Now.ToLongTimeString()}, UpdateShippingPackage Request {JsonConvert.SerializeObject(shippingPackages)}");
            try
            {
                HttpContext httpContext = HttpContext;
                var jwthandler = new JwtSecurityTokenHandler();

                var tokens = httpContext.Request.Headers["Authorization"].ToString();
                var jwttoken = jwthandler.ReadToken(tokens.Split(" ")[1].ToString());
                var Username = (new ICollectionDebugView<System.Security.Claims.Claim>(((JwtSecurityToken)jwttoken).Claims.ToList()).Items[0]).Value;
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()},UpdateShipping Instance Name. {Username}");

                string Servertype = ObjBusinessLayer.GetEnviroment(Username);
                //string Servertype = iconfiguration["ServerType:type"];

                for (int i = 0; i < shippingPackages.Count; i++)
                {
                    UpdateShippingpackagedb updateShippingpackage = new UpdateShippingpackagedb();
                    var randomid = ObjBusinessLayer.GenerateNumeric();
                    updateShippingpackage.id = randomid;
                    updateShippingpackage.shippingPackageCode = shippingPackages[i].shippingPackageCode.ToString();
                    updatelist.Add(updateShippingpackage);
                    for (int k = 0; k < shippingPackages[i].customFieldValues.Count; k++)
                    {
                        addCustomFieldValue customFieldValue = new addCustomFieldValue();
                        customFieldValue.Id = randomid;
                        customFieldValue.name = shippingPackages[i].customFieldValues[k].name;
                        customFieldValue.value = shippingPackages[i].customFieldValues[k].value;
                        customFields.Add(customFieldValue);
                    }
                }
                ObjBusinessLayer.InsertUpdateShippingpackage(updatelist, Servertype);
                //ObjBusinessLayer.InsertUpdateShippingpackageBox(shipbox);
                ObjBusinessLayer.InsertCustomFields(customFields, Servertype);
                ////Data Pushing to Pando
                //var resu = _Token.GetTokens(Servertype).Result;
                SuccessResponse successResponse = new SuccessResponse();
                List<string> ErrorList = new List<string>();
                var lists = ObjBusinessLayer.UpdateShipingPck(Servertype);
                var triggerid = ObjBusinessLayer.UpdateShippingDataPost(lists, Servertype);

                //var facilitycode = "";
                if (lists.Count > 0)
                {
                    for (int i = 0; i < lists.Count; i++)
                    {
                        string Instance = string.Empty;
                        UpdateShippingpackage updateShippingpackage = new UpdateShippingpackage();
                        updateShippingpackage.customFieldValues = new List<CustomFieldValue>();
                        updateShippingpackage.shippingPackageCode = lists[i].shippingPackageCode;
                        var facilitycode = lists[i].FacilityCode;
                        for (int k = 0; k < lists[i].customFieldValues.Count; k++)
                        {
                            CustomFieldValue customFieldValue = new CustomFieldValue();
                            customFieldValue.name = lists[i].customFieldValues[k].name;
                            customFieldValue.value = lists[i].customFieldValues[k].value;
                            updateShippingpackage.customFieldValues.Add(customFieldValue);
                            if (lists[i].customFieldValues[k].name == "INDENTID_SH")
                            {
                                Instance = "SH";
                            }
                            else if (lists[i].customFieldValues[k].name == "INDENTID_DFX")
                            {
                                Instance = "DFX";
                            }
                            //var res = JsonConvert.SerializeObject(new { customFieldValues = new { name = customFieldValue.name, value = customFieldValue.value } });
                            //var dres=JsonConvert.DeserializeObject<CustomFieldValue>(res);
                        }
                        //var triggerid = ObjBusinessLayer.UpdateShippingDataPost(updateShippingpackage, facilitycode, Servertype);
                        //string Instance = "SH";
                        var resu = _Token.GetTokens(Servertype, Instance).Result;
                        var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                        string token = accesstoken.access_token;
                        if (token != null)
                        {
                            //var response = _Token.PostUpdateShippingpckg(updateShippingpackage);
                            var response = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, updateShippingpackage.shippingPackageCode, token, facilitycode, Servertype, Instance);
                            //var response = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, triggerid, token, facilitycode, Servertype, Instance);
                            if (response.IsSuccess)
                            {
                                successResponse.status = "Success";
                                successResponse.waybill = "";
                                successResponse.shippingLabel = "";
                                //successResponse.courierName = Records.courierName;
                                _logger.LogInformation($"DateTime:- {DateTime.Now.ToLongTimeString()}, UpdateShippingPackage response {JsonConvert.SerializeObject(successResponse)}");
                            }
                            else
                            {
                                ErrorList.Add("ShippingPackageCode:- " + updateShippingpackage.shippingPackageCode + ", Reason" + response.ObjectParam);
                                successResponse.status = "False";
                                successResponse.waybill = response.ObjectParam;
                                successResponse.shippingLabel = "";
                                //ErrorResponse errorResponse = new ErrorResponse();
                                //errorResponse.status = "Error";
                                //errorResponse.reason = response.ObjectParam;
                                //errorResponse.message = "";

                                _logger.LogInformation($"DateTime:- {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(successResponse)}");
                                //return new JsonResult(errorResponse);
                            }
                        }
                        //return new JsonResult(successResponse);

                    }
                    if (ErrorList.Count > 0)
                    {
                        var serilizelist = JsonConvert.SerializeObject(ErrorList);
                        Emailtrigger.SendEmailToAdmin("Update Shipping Package", JsonConvert.SerializeObject(ErrorList));

                    }
                    //return Accepted("All Records Pushed Successfully");
                    return new JsonResult(successResponse);

                }
                else
                {
                    ErrorResponse errorResponse = new ErrorResponse();
                    errorResponse.status = "Error";
                    errorResponse.reason = "There Is no Data For Trigger";
                    errorResponse.message = "Please Retrigger";
                    _logger.LogInformation($"DateTime:- {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(errorResponse)}");
                    return new JsonResult(errorResponse);
                }

            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.status = "Error";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Please Retrigger";
                _logger.LogInformation($"DateTime:- {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(errorResponse)}");
                return new JsonResult(errorResponse);

            }

        }
       
       
        //[ServiceFilter(typeof(ActionFilterExample))]
        [Authorize]
        [HttpPost]
        public IActionResult AllocateShipping(List<AllocateshippingPando> allocateshippings)
        {
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Request Allocate Shipping {JsonConvert.SerializeObject(allocateshippings)}");
            SuccessResponse successResponse = new SuccessResponse();
            try
            {
                HttpContext httpContext = HttpContext;
                var jwthandler = new JwtSecurityTokenHandler();
                var token = httpContext.Request.Headers["Authorization"].ToString();
                var jwttoken = jwthandler.ReadToken(token.Split(" ")[1].ToString());
                var JwtSecurity = jwttoken as JwtSecurityToken;
                string Servertype = JwtSecurity.Claims.First(m => m.Type == "Environment").Value;
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Instance Name. {Servertype}");


                //string Servertype = ObjBusinessLayer.GetEnviroment(Username);
                bool insertstatus = ObjBusinessLayer.InsertAllocate_Shipping(allocateshippings, Servertype);
                Task.Run(() =>
                {
                    obj.CallingAllocateShipping(Servertype, allocateshippings);
                });
                if (insertstatus)
                {
                    successResponse.status = "Success";
                    successResponse.waybill = "";
                    successResponse.shippingLabel = "";
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping Data Received From Pando {JsonConvert.SerializeObject(successResponse)}");
                    return Ok(successResponse);
                }
                else
                {
                    ErrorResponse errorResponse = new ErrorResponse();
                    errorResponse.status = "Error";
                    errorResponse.reason = "No Data For Transaction";
                    errorResponse.message = "Please Retrigger";
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping response Error from Pando{JsonConvert.SerializeObject(errorResponse)}");
                    // return new JsonResult(errorResponse);
                    //_logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Success to Pando {JsonConvert.SerializeObject(reversePickupResponse)}");
                    return Problem("No Data Received", null, 204, "Not received", null);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.status = "Error";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Please Retrigger";
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()} , Error: {JsonConvert.SerializeObject(errorResponse)}");
                //return new JsonResult(errorResponse);
                return Problem(ex.Message, null, 204, "Not received", null);

                //throw ex;
            }
            // return Accepted();

        }

        //[ServiceFilter(typeof(ActionFilterExample))]
        [Authorize]
        [HttpPost]
        public IActionResult TrackingStatus(List<TrackingStatusDb> TrackingDetails)
        {
            try
            {
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Details. {JsonConvert.SerializeObject(TrackingDetails)}");
                HttpContext httpContext = HttpContext;
                var jwthandler = new JwtSecurityTokenHandler();
                var token = httpContext.Request.Headers["Authorization"].ToString();
                var jwttoken = jwthandler.ReadToken(token.Split(" ")[1].ToString());
                var JwtSecurity = jwttoken as JwtSecurityToken;
                string Servertype = JwtSecurity.Claims.First(m => m.Type == "Environment").Value;
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Instance Name. {Servertype}");
                var details = ObjBusinessLayer.BLinsertTrackingDetails(TrackingDetails, Servertype);
                Task.Run(() =>
                {
                    obj.CallingTrackingStatus(Servertype, TrackingDetails);
                });
                if (details)
                {
                    TrackingResponse reversePickupResponse = new TrackingResponse();
                    reversePickupResponse.successful = true;
                    reversePickupResponse.message = "Data Received from Pando";
                    reversePickupResponse.errors = "";
                    reversePickupResponse.warnings = "";
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Success to Pando {JsonConvert.SerializeObject(reversePickupResponse)}");
                    return Ok(reversePickupResponse);
                }
                else
                {
                    TrackingResponse reversePickupResponse = new TrackingResponse();
                    reversePickupResponse.successful = true;
                    reversePickupResponse.message = "No Data Received";
                    reversePickupResponse.errors = "";
                    reversePickupResponse.warnings = "";
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Error to Pando {JsonConvert.SerializeObject(reversePickupResponse)}");
                    return Problem("No Data Received", null, 204, "Not received", null);
                }
                

            }
            catch (Exception ex)
            {
                TrackingResponse reversePickupResponse = new TrackingResponse();
                reversePickupResponse.successful = false;
                reversePickupResponse.message = ex.Message;
                reversePickupResponse.errors = "";
                reversePickupResponse.warnings = "";
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Details. {JsonConvert.SerializeObject(reversePickupResponse)}");
                _logger.LogError($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Details. {JsonConvert.SerializeObject(reversePickupResponse)}");
                return Problem(ex.Message, null, 204, "Not received", null);
                //return new JsonResult(reversePickupResponse);
                throw;
            }
        }

        [ServiceFilter(typeof(ActionFilterExample))]
        [Authorize]
        [HttpPost]
        public IActionResult cancel(calcelwaybill waybill)
        {
            try
            {
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Cancel Waybill: {JsonConvert.SerializeObject(waybill.waybill)}");
                Thread.Sleep(5000);
                //string Servertype = iconfiguration["ServerType:type"];
                //string myTempFile = Path.Combine(Path.GetTempPath(), "SaveFile.txt");
                string Username = string.Empty;
                //string Username = System.IO.File.ReadAllText(myTempFile).Remove(System.IO.File.ReadAllText(myTempFile).Length - 2);
                using (var stream = System.IO.File.Open(Path.Combine(Path.GetTempPath(), "SaveFile.txt"), FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    //StreamReader streamReader = new StreamReader(stream);
                    //Username = streamReader.ReadLine();
                    //stream.Close();

                    byte[] buffer = new byte[stream.Length];
                    int bytesread = stream.Read(buffer, 0, buffer.Length);
                    Username = Encoding.ASCII.GetString(buffer, 0, bytesread).Trim();
                    stream.Close();
                }
                string Servertype = ObjBusinessLayer.GetEnviroment(Username);
                ObjBusinessLayer.WaybillCancel(waybill.waybill, Servertype);
                var canceldata = ObjBusinessLayer.GetWaybillCancelData(Servertype);
                CancelResponse response = new CancelResponse();

                //var sendwaybilldata = ObjBusinessLayer.GetWaybillAllRecrdstosend();
                if (canceldata.Count > 0)
                {
                    //var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendwaybilldata);
                    var postres = _MethodWrapper.WaybillCancelPostData(canceldata, 0);
                    //return postres;
                    if (postres.Result.IsSuccess)
                    {
                        //_logger.LogInformation($"Reason:-  {postres.Result.ObjectParam},{DateTime.Now.ToLongTimeString()}");
                        //return Accepted(postres.Result.ObjectParam);
                        response.status = "SUCCESS";
                        response.waybill = waybill.waybill;
                        response.errorMessage = "Order has been canceled";
                        _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Reason:-  {response}");

                        return new JsonResult(response);
                    }
                    else
                    {
                        response.status = "Failed";
                        response.waybill = waybill.waybill;
                        response.errorMessage = postres.Result.ObjectParam;
                        _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Failed Reason:-  {response}");

                        return new JsonResult(response);
                    }
                }
                else
                {
                    response.status = "FAILED";
                    response.waybill = waybill.waybill;
                    response.errorMessage = "There is No Data For Cancelation";
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error Reason:-  {response}");

                    return new JsonResult(response);
                }

                //CancelResponse response = new CancelResponse();
                //response.status = "SUCCESS";
                //response.waybill = waybill.waybill;
                //response.errorMessage = "Order has been canceled";
                //return new JsonResult(response);
            }
            catch (Exception ex)
            {
                CancelResponse response = new CancelResponse();
                response.status = "FAILED";
                response.waybill = waybill.waybill;
                response.errorMessage = ex.Message;
                _logger.LogInformation($"Error Reason:-  {response},{DateTime.Now.ToLongTimeString()}");

                return new JsonResult(response);
                //throw;
            }

        }
        
    }
}
