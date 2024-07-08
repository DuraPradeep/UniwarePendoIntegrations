using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.Entities;
using Uniware_PandoIntegration.BusinessLayer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Uniware_PandoIntegration.API.Folder;
using Uniware_PandoIntegration.API.ActionFilter;
using System.Text;

namespace Uniware_PandoIntegration.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UniwarePandoController : ControllerBase
    {
        private readonly ILogger<UniwarePandoController> _logger;
        private readonly IUniwarePando _jWTManager;
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

        [Authorize]
        [HttpPost]
        public IActionResult waybill(OmsToPandoRoot Records)
        {
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Waybill Get Data From Pando {JsonConvert.SerializeObject(Records)} ,{DateTime.Now.ToLongTimeString()}");

            ServiceResponse<parentList> parentList = new ServiceResponse<parentList>();
            ErrorResponse errorResponse = new ErrorResponse();
            try
            {
                HttpContext httpContext = HttpContext;
                var token = httpContext.Request.Headers["Authorization"].ToString();
                var JwtSecurity = new JwtSecurityTokenHandler().ReadToken(token.Split(" ")[1].ToString()) as JwtSecurityToken;
                //string Username = JwtSecurity.Claims.First(m => m.Type == "name").Value;
                string Servertype = JwtSecurity.Claims.First(m => m.Type == "Environment").Value;

                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()},Waybill Instance Name. {Servertype}");
                Task.Run(() =>
                {
                    obj.CallingWaybill(Records, Servertype);
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


            }
            catch (Exception ex)
            {
                errorResponse.status = "FAILED";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Resource requires authentication. Please check your authorization token.";
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(errorResponse)}");
                throw;
            }
            return new JsonResult(errorResponse);

        }

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
                var tokens = httpContext.Request.Headers["Authorization"].ToString();
                var JwtSecurity = new JwtSecurityTokenHandler().ReadToken(tokens.Split(" ")[1].ToString()) as JwtSecurityToken;
                string Servertype = JwtSecurity.Claims.First(m => m.Type == "Environment").Value;
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()},UpdateShipping Instance Name. {Servertype}");

                for (int i = 0; i < shippingPackages.Count; i++)
                {
                    UpdateShippingpackagedb updateShippingpackage = new UpdateShippingpackagedb();
                    var randomid = ObjBusinessLayer.GenerateNumeric();
                    updateShippingpackage.id = shippingPackages[i].shippingPackageCode.ToString();
                    updateShippingpackage.shippingPackageCode = shippingPackages[i].shippingPackageCode.ToString();
                    updatelist.Add(updateShippingpackage);
                    for (int k = 0; k < shippingPackages[i].customFieldValues.Count; k++)
                    {
                        addCustomFieldValue customFieldValue = new addCustomFieldValue();
                        customFieldValue.Id = shippingPackages[i].shippingPackageCode.ToString();
                        customFieldValue.name = shippingPackages[i].customFieldValues[k].name;
                        customFieldValue.value = shippingPackages[i].customFieldValues[k].value;
                        customFields.Add(customFieldValue);
                    }
                }
                ObjBusinessLayer.InsertUpdateShippingpackage(updatelist, Servertype);
                ObjBusinessLayer.InsertCustomFields(customFields, Servertype);

                SuccessResponse successResponse = new SuccessResponse();
                List<string> ErrorList = new List<string>();
                var lists = ObjBusinessLayer.UpdateShipingPck(Servertype);
                var triggerid = ObjBusinessLayer.UpdateShippingDataPost(lists, Servertype);

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

                        }

                        var resu = _Token.GetTokens(Servertype, Instance).Result;
                        var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                        string token = accesstoken.access_token;
                        if (token != null)
                        {
                            var response = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, updateShippingpackage.shippingPackageCode, token, facilitycode, Servertype, Instance);
                            if (response.IsSuccess)
                            {
                                successResponse.status = true;
                                successResponse.waybill = "";
                                successResponse.shippingLabel = "";
                                _logger.LogInformation($"DateTime:- {DateTime.Now.ToLongTimeString()}, UpdateShippingPackage response {JsonConvert.SerializeObject(successResponse)}");
                            }
                            else
                            {
                                ErrorList.Add("ShippingPackageCode:- " + updateShippingpackage.shippingPackageCode + ", Reason" + response.ObjectParam);
                                successResponse.status = false;
                                successResponse.waybill = response.ObjectParam;
                                successResponse.shippingLabel = "";


                                _logger.LogInformation($"DateTime:- {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(successResponse)}");
                            }
                        }

                    }
                    if (ErrorList.Count > 0)
                    {
                        var serilizelist = JsonConvert.SerializeObject(ErrorList);
                        Emailtrigger.SendEmailToAdmin("Update Shipping Package", JsonConvert.SerializeObject(ErrorList));

                    }
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AllocateShipping(List<AllocateshippingPando> allocateshippings)
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
                Task<SuccessResponse> Call1 = ObjBusinessLayer.InsertAllocate_Shipping(allocateshippings, Servertype);

                //List<AllocateshippingPando> allocateshippingss = ObjBusinessLayer.GetAllcoaetData(Servertype);
                //for (int i = 0; i < allocateshippingss.Count; i++)
                //{
                //    List<AllocateshippingPando> demos = new List<AllocateshippingPando>();

                //    AllocateshippingPando allocateshippingPando = new AllocateshippingPando();
                //    allocateshippingPando.shippingPackageCode = allocateshippingss[i].shippingPackageCode;
                //    allocateshippingPando.shippingLabelMandatory = allocateshippingss[i].shippingLabelMandatory;
                //    allocateshippingPando.shippingProviderCode = allocateshippingss[i].shippingProviderCode;
                //    allocateshippingPando.trackingNumber = allocateshippingss[i].trackingNumber;
                //    allocateshippingPando.shippingCourier = allocateshippingss[i].shippingCourier;
                //    allocateshippingPando.tracking_link_url = allocateshippingss[i].tracking_link_url;
                //    demos.Add(allocateshippingPando);

                //obj.CallingAllocateShippings(Servertype, demos);
                YourMethod(Servertype, allocateshippings);

                //}


                //Task<bool> Call2 = obj.CallingAllocateShipping(Servertype, allocateshippings);
                SuccessResponse result1 = await Call1;
                //bool result2 = await Call2;
                await Task.WhenAll(Call1);
                return Ok(result1);
                //Task.Run(() =>
                //{
                //    obj.CallingAllocateShipping(Servertype, allocateshippings);
                //});


                //if (insertstatus)
                //{
                //    successResponse.status = "Success";
                //    successResponse.waybill = "";
                //    successResponse.shippingLabel = "";
                //    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping Data Received From Pando {JsonConvert.SerializeObject(successResponse)}");
                //    return Ok(successResponse);
                //}
                //else
                //{
                //    ErrorResponse errorResponse = new ErrorResponse();
                //    errorResponse.status = "Error";
                //    errorResponse.reason = "No Data For Transaction";
                //    errorResponse.message = "Please Retrigger";
                //    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping response Error from Pando{JsonConvert.SerializeObject(errorResponse)}");
                //    return Problem("No Data Received", null, 204, "Not received", null);
                //}
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.status = "Error";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Please Retrigger";
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()} , Error: {JsonConvert.SerializeObject(errorResponse)}");
                return Problem(ex.Message, null, 204, "Not received", null);

            }

        }
        async Task YourMethod(string Servertype, List<AllocateshippingPando> allocateshippings)
        {

            await Task.Run(() =>
            {
                obj.CallingAllocateShipping(Servertype, allocateshippings);
            });


        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TrackingStatus(List<TrackingStatusDb> TrackingDetails)
        {
            try
            {

               
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Details. {JsonConvert.SerializeObject(TrackingDetails)}");
                HttpContext httpContext = HttpContext;
                var token = httpContext.Request.Headers["Authorization"].ToString();
                var JwtSecurity = new JwtSecurityTokenHandler().ReadToken(token.Split(" ")[1].ToString()) as JwtSecurityToken;
                string Servertype = JwtSecurity.Claims.First(m => m.Type == "Environment").Value;
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Instance Name. {Servertype}");
                Task<TrackingResponse> Call1 = ObjBusinessLayer.BLinsertTrackingDetails(TrackingDetails, Servertype);

                Task.Run(() => obj.CallingTrackingStatus(Servertype, TrackingDetails));
                //Task<bool> Call2 = 
                TrackingResponse result1 = await Call1;
                //bool result2 = await Call2;
                await Task.WhenAll(Call1);
                return Ok(result1);

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

        //[ServiceFilter(typeof(ActionFilterExample))]
        [Authorize]
        [HttpPost]
        public IActionResult cancel(calcelwaybill waybill)
        {
            try
            {
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Cancel Waybill: {JsonConvert.SerializeObject(waybill.waybill)}");
                //Thread.Sleep(5000);

                HttpContext httpContext = HttpContext;
                var tokens = httpContext.Request.Headers["Authorization"].ToString();
                var JwtSecurity = new JwtSecurityTokenHandler().ReadToken(tokens.Split(" ")[1].ToString()) as JwtSecurityToken;
                string Servertype = JwtSecurity.Claims.First(m => m.Type == "Environment").Value;
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()},CancelWaybill Instance Name. {Servertype}");
                //string Servertype = ObjBusinessLayer.GetEnviroment(Username);
                ObjBusinessLayer.WaybillCancel(waybill.waybill, Servertype);
                var canceldata = ObjBusinessLayer.GetWaybillCancelData(Servertype);
                CancelResponse response = new CancelResponse();

                if (canceldata.Count > 0)
                {
                    var postres = _MethodWrapper.WaybillCancelPostData(canceldata, 0);
                    if (postres.Result.IsSuccess)
                    {

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

            }
            catch (Exception ex)
            {
                CancelResponse response = new CancelResponse();
                response.status = "FAILED";
                response.waybill = waybill.waybill;
                response.errorMessage = ex.Message;
                _logger.LogInformation($"Error Reason:-  {response},{DateTime.Now.ToLongTimeString()}");

                return new JsonResult(response);
            }

        }


        [Authorize]
        [HttpPost]
        public IActionResult FTLShipmentStatus(FTLShipment FTLRecordsa)
        {
            try
            {
                _logger.LogInformation($"FTL ShipmentStaus Data From Pando. {JsonConvert.SerializeObject(FTLRecordsa)}");

                FTLShipmentResponse fTLShipmentResponse = new FTLShipmentResponse();

                HttpContext httpContext = HttpContext;
                var token = httpContext.Request.Headers["Authorization"].ToString();
                var JwtSecurity = new JwtSecurityTokenHandler().ReadToken(token.Split(" ")[1].ToString()) as JwtSecurityToken;
                string Servertype = JwtSecurity.Claims.First(m => m.Type == "Environment").Value;
                _logger.LogInformation($"Instance Name. {Servertype}");

                var FTLMain = ObjBusinessLayer.InsertFTLShipmentMain(FTLRecordsa, Servertype);
                var FTMshipment = ObjBusinessLayer.InsertFTLShipment(FTLRecordsa.shipments, FTLRecordsa.shipment_id, Servertype);
                if(FTMshipment)
                {
                    fTLShipmentResponse.Status = true;
                    fTLShipmentResponse.Message = "Data Received From Pando";
                    fTLShipmentResponse.Reason = "";
                    _logger.LogInformation($"FTL ShipmentStaus Success. {JsonConvert.SerializeObject(fTLShipmentResponse)}");
                    return Ok(fTLShipmentResponse);

                }
                else
                {
                    fTLShipmentResponse.Status = false;
                    fTLShipmentResponse.Message = "There Is some error In Data Structure";
                    fTLShipmentResponse.Reason = "";
                    _logger.LogInformation($"FTL ShipmentStaus Error. {JsonConvert.SerializeObject(fTLShipmentResponse)}");
                    return Problem("There Is some error In Data Structure", null, 204, "Not received", null);

                }

            }
            catch (Exception ex)
            {
                FTLShipmentResponse fTLShipmentResponse = new FTLShipmentResponse();
                fTLShipmentResponse.Status = false;
                fTLShipmentResponse.Message = "There Is some error Is Data Structure";
                fTLShipmentResponse.Reason = "";
                _logger.LogInformation($"FTL ShipmentStaus Error. {JsonConvert.SerializeObject(fTLShipmentResponse)}");

                return Problem(ex.Message, null, 204, "Not received", null);
            }            

        }

        //[HttpGet]
        //public async Task<IActionResult> TrackingStatusDemo()
        //{
        //    try
        //    {
        //        //_logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Details. {JsonConvert.SerializeObject(TrackingDetails)}");
        //        //HttpContext httpContext = HttpContext;
        //        //var token = httpContext.Request.Headers["Authorization"].ToString();
        //        //var JwtSecurity = new JwtSecurityTokenHandler().ReadToken(token.Split(" ")[1].ToString()) as JwtSecurityToken;
        //        //string Servertype = JwtSecurity.Claims.First(m => m.Type == "Environment").Value;
        //        //_logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Instance Name. {Servertype}");
        //        string Servertype = "Prod";

        //        var list = ObjBusinessLayer.GetLast30daysStatus(Servertype);
        //        //Task<TrackingResponse> Call1 = ObjBusinessLayer.BLinsertTrackingDetails(list, Servertype);
        //        Task<bool> Call2 = obj.CallingTrackingStatus(Servertype, list);
        //        //TrackingResponse result1 = await Call1;
        //        ////bool result2 = await Call2;
        //        //await Task.WhenAll(Call1);
        //        return Ok();

        //    }
        //    catch (Exception ex)
        //    {
        //        TrackingResponse reversePickupResponse = new TrackingResponse();
        //        reversePickupResponse.successful = false;
        //        reversePickupResponse.message = ex.Message;
        //        reversePickupResponse.errors = "";
        //        reversePickupResponse.warnings = "";
        //        _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Details. {JsonConvert.SerializeObject(reversePickupResponse)}");
        //        _logger.LogError($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Details. {JsonConvert.SerializeObject(reversePickupResponse)}");
        //        return Problem(ex.Message, null, 204, "Not received", null);
        //        //return new JsonResult(reversePickupResponse);
        //        throw;
        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> TestTrackingStatus()
        {
            try
            {
                string Servertype = "Prod";
                List<TrackingStatusDb> allocateshippingss = ObjBusinessLayer.GetTrackingstatusFailedData(Servertype);
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Details. get from DB {JsonConvert.SerializeObject(allocateshippingss)}");
                for (int i = 0; i < allocateshippingss.Count; i++)
                {
                    List<TrackingStatusDb> demos = new List<TrackingStatusDb>();

                    TrackingStatusDb allocateshippingPando = new TrackingStatusDb();
                    allocateshippingPando.providerCode = allocateshippingss[i].providerCode;
                    allocateshippingPando.trackingStatus = allocateshippingss[i].trackingStatus;
                    allocateshippingPando.trackingNumber = allocateshippingss[i].trackingNumber;
                    allocateshippingPando.shipmentTrackingStatusName = allocateshippingss[i].shipmentTrackingStatusName;
                    allocateshippingPando.statusDate = allocateshippingss[i].statusDate;
                    allocateshippingPando.facilitycode = allocateshippingss[i].facilitycode;
                    demos.Add(allocateshippingPando);

                    obj.failedtrackingstatus(Servertype, demos);

                }
                return Ok();

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


    }
}
