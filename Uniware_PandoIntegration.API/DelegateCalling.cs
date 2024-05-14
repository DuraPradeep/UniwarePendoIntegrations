using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.BusinessLayer;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.API
{
    public class DelegateCalling
    {
        BearerToken _Token = new BearerToken();
        private UniwareBL ObjBusinessLayer = new();
        MethodWrapper _MethodWrapper = new MethodWrapper();

        // public delegate void DelegateTrackingStatus(string Servertype, String Instance, List<TrackingStatusDb> trackingStatusDbs);
        public async Task<bool> CallingTrackingStatus(string Servertype, List<TrackingStatusDb> trackingStatusDbs)
        {
            bool Result=false;
            CreateLog("Execution start");
            try
            {
                //string Getinstance = string.Empty;
                //string Instance = string.Empty;
                string Nameinstance = string.Empty;
                string responsmessage = string.Empty;
                var TrackingList = ObjBusinessLayer.GetTrackingDetails(Servertype, trackingStatusDbs);
                //ObjBusinessLayer.InsertTrackingStatusPostdata(TrackingList, Servertype);

                if (TrackingList.Count > 0)
                {
                    for (int i = 0; i < TrackingList.Count; i++)
                    {

                        TrackingStatus trackingStatus = new TrackingStatus();
                        trackingStatus.providerCode = TrackingList[i].providerCode;
                        trackingStatus.trackingStatus = TrackingList[i].trackingStatus;
                        trackingStatus.trackingNumber = TrackingList[i].trackingNumber;
                        trackingStatus.shipmentTrackingStatusName = TrackingList[i].shipmentTrackingStatusName;
                        trackingStatus.statusDate = TrackingList[i].statusDate;
                        Nameinstance = TrackingList[i].Instance == "INDENTID_SH" ? "SH" : "DFX";

                        var res = _MethodWrapper.TrackingStatus(trackingStatus, 0, TrackingList[i].facilitycode, Servertype, Nameinstance);
                        CreateLog("Execution end");
                        if (res.IsSuccess)
                        {
                            Result = true;
                            responsmessage = res.ObjectParam.ToString();
                        }
                        else
                        {
                            Result = false;

                            responsmessage = res.ObjectParam.ToString();
                        }

                    }
                    TrackingResponse reversePickupResponse = new TrackingResponse();
                    reversePickupResponse.successful = true;
                    reversePickupResponse.message = responsmessage;
                    reversePickupResponse.errors = "";
                    reversePickupResponse.warnings = "";
                    CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Success {JsonConvert.SerializeObject(reversePickupResponse)}");

                    //return new JsonResult(reversePickupResponse);
                }
                else
                {
                    Result = false;

                    TrackingResponse reversePickupResponse = new TrackingResponse();
                    reversePickupResponse.successful = false;
                    reversePickupResponse.message = responsmessage;
                    reversePickupResponse.errors = "There Is not Data For Tacking";
                    reversePickupResponse.warnings = "";
                    CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Error {JsonConvert.SerializeObject(reversePickupResponse)}");

                    //return new JsonResult(reversePickupResponse);
                }

                //}
                //else
                //{
                //    TrackingResponse reversePickupResponse = new TrackingResponse();
                //    reversePickupResponse.successful = false;
                //    reversePickupResponse.message = "Token Not Generated";
                //    reversePickupResponse.errors = "";
                //    reversePickupResponse.warnings = "";
                //    CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Error {JsonConvert.SerializeObject(reversePickupResponse)}");

                //    //return new JsonResult(reversePickupResponse);
                //}
            }
            catch (Exception ex)
            {
                Result = false;

                TrackingResponse reversePickupResponse = new TrackingResponse();
                reversePickupResponse.successful = false;
                reversePickupResponse.message = ex.Message;
                reversePickupResponse.errors = "";
                reversePickupResponse.warnings = "";
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking error Details. {JsonConvert.SerializeObject(reversePickupResponse)}");



                throw;
            }
            return Result;
        }

        public async Task<bool> CallingAllocateShipping(string Servertype, List<AllocateshippingPando> allocateshippings)
        {
            string Instance = string.Empty;
            SuccessResponse successResponse = new SuccessResponse();
            bool res = false;

            List<string> ErrorList = new List<string>();
            List<string> AllocateError = new List<string>();
            try
            {
                var results = ObjBusinessLayer.PostGAllocateShippingData(Servertype, allocateshippings);
                List<UpdateShippingpackagedb> updateShippingpackagedbs = new List<UpdateShippingpackagedb>();
                //List<Allocateshipping> allocatelist = new List<Allocateshipping>();
                //for (int i = 0; i < results.Count; i++)
                //{
                //    UpdateShippingpackagedb updateShippingpackagedb = new UpdateShippingpackagedb();
                //    updateShippingpackagedb.customFieldValues = new List<CustomFieldValue>();
                //    CustomFieldValue customFieldValue1 = new CustomFieldValue();
                //    updateShippingpackagedb.shippingPackageCode = results[i].shippingPackageCode;
                //    customFieldValue1.name = "TrackingLink2";
                //    customFieldValue1.value = results[i].trackingLink;
                //    updateShippingpackagedb.FacilityCode = results[i].FacilityCode;
                //    updateShippingpackagedb.customFieldValues.Add(customFieldValue1);
                //    updateShippingpackagedbs.Add(updateShippingpackagedb);

                //    //#region Allocate looping
                //    //Allocateshipping allocateshipping = new Allocateshipping();
                //    //allocateshipping.shippingPackageCode = results[i].shippingPackageCode;
                //    //allocateshipping.shippingLabelMandatory = results[i].shippingLabelMandatory;
                //    //allocateshipping.shippingProviderCode = results[i].shippingProviderCode;
                //    //allocateshipping.shippingCourier = results[i].shippingCourier;
                //    //allocateshipping.trackingNumber = results[i].trackingNumber;
                //    //allocateshipping.trackingLink = results[i].trackingLink;
                //    //allocatelist.Add(allocateshipping);
                //    //#endregion
                //}
                //var triggerid = ObjBusinessLayer.UpdateShippingDataPost(updateShippingpackagedbs, Servertype);
                //var Triggerid = ObjBusinessLayer.AllocateShippingDataPost(allocateshipping, Servertype);

                if (results.Count > 0)
                {
                    for (int i = 0; i < results.Count; i++)
                    {
                        UpdateShippingpackagedb updateShippingpackagedb = new UpdateShippingpackagedb();
                        updateShippingpackagedb.customFieldValues = new List<CustomFieldValue>();
                        CustomFieldValue customFieldValue1 = new CustomFieldValue();
                        updateShippingpackagedb.shippingPackageCode = results[i].shippingPackageCode;
                        customFieldValue1.name = "TrackingLink2";
                        customFieldValue1.value = results[i].trackingLink;
                        updateShippingpackagedb.FacilityCode = results[i].FacilityCode;
                        updateShippingpackagedb.customFieldValues.Add(customFieldValue1);
                        updateShippingpackagedbs.Add(updateShippingpackagedb);

                        Allocateshipping allocateshipping = new Allocateshipping();
                        allocateshipping.shippingPackageCode = results[i].shippingPackageCode;
                        allocateshipping.shippingLabelMandatory = results[i].shippingLabelMandatory;
                        allocateshipping.shippingProviderCode = results[i].shippingProviderCode;
                        allocateshipping.shippingCourier = results[i].shippingCourier;
                        allocateshipping.trackingNumber = results[i].trackingNumber;
                        allocateshipping.trackingLink = results[i].trackingLink;

                        var reference = results[i].Instance;
                        if (reference == "Duroflex")
                        {
                            Instance = "DFX";
                        }
                        else
                        {
                            Instance = "SH";
                        }
                        var Token = _Token.GetTokens(Servertype, Instance).Result;
                        var _Tokens = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(Token.ObjectParam);
                        var facility = results[i].FacilityCode;

                        //Start Update Shipping Package to Send Data
                        #region Allocate Shipping post data
                        UpdateShippingpackage updateShippingpackage = new UpdateShippingpackage();
                        updateShippingpackage.shippingPackageCode = results[i].shippingPackageCode;
                        updateShippingpackage.customFieldValues = new List<CustomFieldValue>();
                        CustomFieldValue customFieldValue = new CustomFieldValue();

                        customFieldValue.name = "TrackingLink2";
                        customFieldValue.value = results[i].trackingLink;
                        updateShippingpackage.customFieldValues.Add(customFieldValue);
                        //var triggerid = ObjBusinessLayer.UpdateShippingDataPost(updateShippingpackage, facility, Servertype);
                        #endregion



                        //var triggerid = ObjBusinessLayer.UpdateShippingDataPost(lists, Servertype);



                        //var responses = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, triggerid, _Tokens.access_token, facility, Servertype, Instance);
                        if (allocateshippings[0].tracking_link_url == null || allocateshippings[0].tracking_link_url == "https:")
                        {
                            var Triggerid = ObjBusinessLayer.AllocateShippingDataPost(allocateshipping, Servertype);
                            var response = _MethodWrapper.AllocatingShippingPostData(allocateshipping, 0, allocateshipping.shippingPackageCode, _Tokens.access_token, facility, Servertype, Instance);
                            //var response = _MethodWrapper.AllocatingShippingPostData(allocateshipping, 0, Triggerid, _Tokens.access_token, facility, Servertype, Instance);
                            if (response.IsSuccess)
                            {
                                res = true;
                                successResponse.status = true;
                                successResponse.waybill = "";
                                successResponse.shippingLabel = "";
                                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping response {JsonConvert.SerializeObject(successResponse)}");
                                //return new JsonResult(successResponse);
                            }
                            else
                            {
                                res = false;
                                AllocateError.Add("ShippingPackageCode:- " + allocateshipping.shippingPackageCode + ", Reason " + response.ObjectParam);
                                successResponse.status = false;
                                successResponse.waybill = response.ObjectParam;
                                successResponse.shippingLabel = "";
                                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping response Error {JsonConvert.SerializeObject(successResponse)}");
                            }
                        }

                        //Idle for 5sec

                        //Thread.Sleep(5000);
                        var responses = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, updateShippingpackage.shippingPackageCode, _Tokens.access_token, facility, Servertype, Instance);
                        if (responses.IsSuccess==false)
                        {
                            res = false;
                            ErrorList.Add("ShippingPackageCode:- " + updateShippingpackage.shippingPackageCode + ", Reason " + responses.ObjectParam);
                        }
                    }
                    var triggerid = ObjBusinessLayer.UpdateShippingDataPost(updateShippingpackagedbs, Servertype);
                    if (ErrorList.Count > 0)
                    {
                        var serilizelist = JsonConvert.SerializeObject(ErrorList);
                        Emailtrigger.SendEmailToAdmin("Update Shipping Package", JsonConvert.SerializeObject(ErrorList));

                    }
                    if (AllocateError.Count > 0)
                    {
                        Emailtrigger.SendEmailToAdmin("Allocate Shipping", JsonConvert.SerializeObject(AllocateError));

                    }
                    //SuccessResponse successResponse = new SuccessResponse();
                    //successResponse.status = "Success";
                    //successResponse.waybill = "";
                    //successResponse.shippingLabel = "";
                    //_logger.LogInformation($" Allocate Shipping response {JsonConvert.SerializeObject(successResponse)}");
                    //return new JsonResult(successResponse);
                }
                else
                {
                    res = false;
                    ErrorResponse errorResponse = new ErrorResponse();
                    errorResponse.status = "Error";
                    errorResponse.reason = "No Data For Transaction";
                    errorResponse.message = "Please Retrigger";
                    CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping response Error{JsonConvert.SerializeObject(errorResponse)}");
                    //return new JsonResult(errorResponse);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.status = "Error";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Unable to Post Allocate Shipping";
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()} ,Allocate Shipping Error: {JsonConvert.SerializeObject(errorResponse)}");
                //return new JsonResult(errorResponse);

                
            }
            return res;

        }


        public void CallingWaybill(OmsToPandoRoot Records, string Username)
        {
            ServiceResponse<parentList> parentList = new ServiceResponse<parentList>();
            ErrorResponse errorResponse = new ErrorResponse();
            try
            {
                string Servertype = ObjBusinessLayer.GetEnviroment(Username);

                var jsoncodes = JsonConvert.SerializeObject(new { code = Records.Shipment.SaleOrderCode });
                string Instance = string.Empty;
                for (int x = 0; x < Records.Shipment.customField.Count; x++)
                {
                    if (Records.Shipment.customField[x].name == "INDENTID_DFX")
                        Instance = "DFX";
                    else if (Records.Shipment.customField[x].name == "INDENTID_SH")
                        Instance = "SH";
                }
                var resu = _Token.GetTokens(Servertype, Instance).Result;
                var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                string token = deres.access_token.ToString();
                parentList = _MethodWrapper.PassCodeer(jsoncodes, token, "", 0, Servertype, Instance);
                string FacilityCode = string.Empty;
                for (int i = 0; i < parentList.ObjectParam.saleOrderItems.Count; i++)
                {
                    if (parentList.ObjectParam.saleOrderItems[i].shippingPackageCode == Records.Shipment.code)
                    {
                        FacilityCode = parentList.ObjectParam.saleOrderItems[i].facilityCode;
                    }
                }


                RootResponse rootResponse = new RootResponse();
                string primaryid = ObjBusinessLayer.insertWaybillMain(Records, Servertype);
                ObjBusinessLayer.insertWaybillshipment(Records, primaryid, FacilityCode, Servertype);
                List<Item> items = new List<Item>();
                List<CustomField> customfields = new List<CustomField>();
                for (int i = 0; i < Records.Shipment.items.Count; i++)
                {
                    Item item = new Item();
                    item.name = Records.Shipment.items[i].name;
                    item.description = Records.Shipment.items[i].description;
                    item.quantity = Records.Shipment.items[i].quantity;
                    item.skuCode = Records.Shipment.items[i].skuCode;
                    item.itemPrice = Records.Shipment.items[i].itemPrice;
                    item.imageURL = Records.Shipment.items[i].imageURL;
                    item.hsnCode = Records.Shipment.items[i].hsnCode;
                    item.tags = Records.Shipment.items[i].tags;
                    items.Add(item);
                }
                for (int i = 0; i < Records.Shipment.customField.Count; i++)
                {
                    CustomField customFieldValue = new CustomField();
                    customFieldValue.name = Records.Shipment.customField[i].name;
                    customFieldValue.value = Records.Shipment.customField[i].value;
                    customfields.Add(customFieldValue);
                }
                ObjBusinessLayer.insertWaybilldeliveryaddress(Records.deliveryAddressDetails, primaryid, Servertype);
                ObjBusinessLayer.insertWaybillpickupadres(Records.pickupAddressDetails, primaryid, Servertype);
                ObjBusinessLayer.insertWaybillReturnaddress(Records.returnAddressDetails, primaryid, Servertype);
                ObjBusinessLayer.InsertCustomfieldWaybill(customfields, primaryid, Records.Shipment.code, Servertype);
                ObjBusinessLayer.InsertitemWaybill(items, primaryid, Records.Shipment.code, Servertype);

                var sendwaybilldata = ObjBusinessLayer.GetWaybillAllRecrdstosend(Instance, Servertype);
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, WayBill Data Get From Database:- {JsonConvert.SerializeObject(sendwaybilldata)}");
                if (sendwaybilldata.Count > 0)
                {
                    var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendwaybilldata, Servertype, Instance);
                    var postres = _MethodWrapper.WaybillGenerationPostData(sendwaybilldata, 0, triggerid, Servertype);
                    if (postres.IsSuccess)
                    {
                        errorResponse.status = "FAILED";
                        errorResponse.reason = "AWB not generated";
                        errorResponse.message = "AWB generation is in queue, please check after a few mins";
                        CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, WayBill response {JsonConvert.SerializeObject(errorResponse)}");
                    }
                    else
                    {
                        errorResponse.status = "FAILED";
                        errorResponse.reason = postres.ObjectParam;
                        errorResponse.message = "Resource requires authentication. Please check your authorization token.";
                        CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(errorResponse)}");
                    }
                    //_logger.LogInformation($"Reason:-  {postres.ObjectParam},{DateTime.Now.ToLongTimeString()}");
                    //return Accepted(postres.Result.ObjectParam);


                    //return new JsonResult(errorResponse);
                }
                else
                {
                    errorResponse.status = "FAILED";
                    errorResponse.reason = "No Data For Post";
                    errorResponse.message = "Data Not Matching for Post to Pando";
                    CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Waybill Error: {JsonConvert.SerializeObject(errorResponse)}");
                }
            }
            catch (Exception ex)
            {
                errorResponse.status = "FAILED";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Resource requires authentication. Please check your authorization token.";
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Waybill Error: {JsonConvert.SerializeObject(errorResponse)}");
                throw;
            }

        }
        public void CreateLog(string message)
        {
            Log.Information(message);
        }
    }
}
