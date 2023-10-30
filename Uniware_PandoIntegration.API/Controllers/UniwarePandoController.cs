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

namespace Uniware_PandoIntegration.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UniwarePandoController : Controller
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
        [HttpGet]
        public IActionResult GetToken()
        {
            //BearerToken _Token = new BearerToken();
            //PandoUniwariToken resu = _Token.GetTokens().Result;
            string Servertype = iconfiguration["ServerType:type"];

            var resu = _Token.GetTokens(Servertype).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            if (resu.ObjectParam != null)
            {
                string token = deres.access_token.ToString();
                //HttpContext.Session.SetString("Token", resu.access_token.ToString());
                HttpContext.Session.SetString("Token", token);
                //return new PandoUniwariToken();
                return Accepted(resu.ObjectParam);
            }
            //HttpContext.Session.SetString("Token", resu.access_token.ToString());
            //string token = HttpContext.Session.GetString("Token");
            //string token = deres.access_token.ToString();
            ////HttpContext.Session.SetString("Token", resu.access_token.ToString());
            //HttpContext.Session.SetString("token", token);    
            else
            {
                return BadRequest("Something Went Wrong");
            }
        }
        [HttpPost]
        public IActionResult SaleOrderAPI(string fromdate = "1695753048000", string todate = "1695835821000", string datatype = "CREATED", string status = "Processing")
        {
            //string fromdate = "1693074600000", string todate = "1693182600000", string datatype = "UPDATED", string status = "Processing"
            //string datatype= iconfiguration.GetSection("")
            //string datatype = iconfiguration["SaleOrderType:datatype"];
            string Servertype = iconfiguration["ServerType:type"];
            //string fromdate = "1692901800000"; string todate = "1693765800000";
            // string datatype = datatype;
            //string status = "Processing";
            //_logger.LogInformation("Token Api called.");
            //datatype="CREATED"

            //var resu = _Token.GetTokens().Result;
            //var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            string token = HttpContext.Session.GetString("Token");
            //string token = deres.access_token.ToString();
            if (token != null)
            {
                _logger.LogInformation("saleOrder/Get Api called.");
                var json = JsonConvert.SerializeObject(new { fromDate = fromdate, toDate = todate, dateType = datatype, status = status });
                //var list = getCode(json, token, 0);
                var list = _MethodWrapper.getCode(json, token, 0, Servertype);
                //var resCode = ObjBusinessLayer.InsertCode(elmt);
                if (list.Count > 0)
                {
                    var resCode = ObjBusinessLayer.InsertCode(list);
                    var ds = ObjBusinessLayer.GetCode();

                    parentList parentList = new parentList();
                    _logger.LogInformation("Saleorder/search Api called.");
                    List<Address> address = new List<Address>();
                    List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
                    List<ShippingPackage> shipingdet = new List<ShippingPackage>();
                    List<Items> qtyitems = new List<Items>();
                    List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
                    for (int i = 0; i < ds.Count; i++)
                    {
                        var jsoncodes = JsonConvert.SerializeObject(ds[i]);
                        string code = ds[i].code;
                        //parentList = PassCode(jsoncodes, token, code, 0);

                        parentList = _MethodWrapper.PassCodeer(jsoncodes, token, code, 0, Servertype);
                        if (parentList.saleOrderItems.Count > 0 || parentList.address.Count > 0 || parentList.Shipment.Count > 0 || parentList.qtyitems.Count > 0 || parentList.elements.Count > 0)
                        {
                            saleOrderItems.AddRange(parentList.saleOrderItems);
                            address.AddRange(parentList.address);
                            shipingdet.AddRange(parentList.Shipment);
                            qtyitems.AddRange(parentList.qtyitems);
                            elements.AddRange(parentList.elements);
                        }
                    }
                    var sires = ObjBusinessLayer.insertsalesorderitem(saleOrderItems);
                    var resshipng = ObjBusinessLayer.InsertBill(shipingdet);
                    var resitems = ObjBusinessLayer.insertItems(qtyitems);
                    var resads = ObjBusinessLayer.InsertAddrsss(address);
                    var resdto = ObjBusinessLayer.insertSalesDTO(elements);


                    var sku = ObjBusinessLayer.GetSKucodesBL();
                    _logger.LogInformation("ItemType/Get Api called.");

                    List<SKucode> skus = new List<SKucode>();
                    List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                    for (int i = 0; i < sku.Count; i++)
                    {
                        skucode sKucodes = new skucode();
                        sKucodes.skuCode = sku[i].SkuCode;
                        var code = sku[i].code;
                        var skucode = sku[i].SkuCode;
                        var jskucode = JsonConvert.SerializeObject(sKucodes);

                        //var insertskucode = ReturnSkuCode(jskucode, token, skucode,0);
                        var insertskucode = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype);
                        if (insertskucode.Code != null)
                        {
                            itemTdto.Add(insertskucode);
                        }
                    }
                    var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto);
                    var allsenddata = ObjBusinessLayer.GetAllRecrdstosend();
                    var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);

                    var sendcode = ObjBusinessLayer.GetAllSendData();
                    if (sendcode.Count > 0)
                    {
                        var resutt = _MethodWrapper.Action(sendcode, triggerid, 0, Servertype);
                        return Accepted(resutt.ObjectParam);
                    }
                    else
                    {
                        return BadRequest("Please retrigger");
                    }
                }
                else
                {
                    return BadRequest("Please Retrigger");
                }
            }
            else return BadRequest("Please Pass Valid Token");
        }

        [HttpGet]
        public string Retrigger()
        {
            //PandoUniwariToken res = _Token.GetTokens().Result;
            //HttpContext.Session.SetString("Token", res.access_token.ToString());
            _logger.LogInformation("Sale Order Retriggered ");
            string Servertype = iconfiguration["ServerType:type"];

            //PandoUniwariToken resu = _Token.GetTokens().Result;
            //HttpContext.Session.SetString("Token", resu.access_token.ToString());
            var resu = _Token.GetTokens(Servertype).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            if (resu.ObjectParam != null)
            {
                string token = deres.access_token.ToString();
                //string token = HttpContext.Session.GetString("Token");
                var ds = ObjBusinessLayer.GetCodeforRetrigger();

                List<ErrorDetails> errorDetails = new List<ErrorDetails>();
                List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
                salesorderRoot details = new salesorderRoot();
                List<Address> address = new List<Address>();
                List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
                List<ShippingPackage> shipingdet = new List<ShippingPackage>();
                List<Items> qtyitems = new List<Items>();
                List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
                parentList parentList = new parentList();
                _logger.LogInformation("Retrigger:-Saleorder/search Api called.");
                for (int i = 0; i < ds.Count; i++)
                {
                    var jsoncodes = JsonConvert.SerializeObject(ds[i]);
                    string codes = ds[i].code;
                    //parentList = _MethodWrapper.RetriggerCode(jsoncodes, token, codes, 0);
                    parentList = _MethodWrapper.PassCodeer(jsoncodes, token, codes, 0, Servertype);
                    if (parentList.saleOrderItems.Count > 0 || parentList.address.Count > 0 || parentList.Shipment.Count > 0 || parentList.qtyitems.Count > 0 || parentList.elements.Count > 0)
                    {
                        //    return;
                        //}
                        //else
                        //{
                        saleOrderItems.AddRange(parentList.saleOrderItems);
                        shipingdet.AddRange(parentList.Shipment);
                        qtyitems.AddRange(parentList.qtyitems);
                        address.AddRange(parentList.address);
                        elements.AddRange(parentList.elements);
                    }
                }

                var sires = ObjBusinessLayer.insertsalesorderitem(saleOrderItems);
                var resshipng = ObjBusinessLayer.InsertBill(shipingdet);
                var resitems = ObjBusinessLayer.insertItems(qtyitems);
                var resads = ObjBusinessLayer.InsertAddrsss(address);
                var resdto = ObjBusinessLayer.insertSalesDTO(elements);

                var sku = ObjBusinessLayer.GetSKucodesForRetrigger();
                _logger.LogInformation("Retrigger:-ItemType/Get Api called.");
                List<SKucode> skus = new List<SKucode>();
                List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();

                for (int i = 0; i < sku.Count; i++)
                {
                    skucode sKucodes = new skucode();
                    sKucodes.skuCode = sku[i].SkuCode;
                    var code = sku[i].code;
                    var skucode = sku[i].SkuCode;
                    var jskucode = JsonConvert.SerializeObject(sKucodes);
                    //var resku = _MethodWrapper.RetriggerSkuCode(jskucode, token, code, skucode, 0);
                    var resku = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype);
                    if (resku.Code != null)
                    {
                        itemTdto.Add(resku);
                    }
                    //else
                    //{
                    //return;
                    //}
                }
                var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto);
                var allsenddata = ObjBusinessLayer.GetAllRecrdstosend();
                if (allsenddata.Count > 0)
                {
                    //var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);
                    //var postretrigger = _MethodWrapper.RetriggerPostDataDelivery(allsenddata, triggerid, 0);
                    //var postretrigger = _MethodWrapper.Action(allsenddata, triggerid, 0);                
                    return "Data Triggered Successfully";
                }
                else
                {
                    return "Get Some Issue Please Retrigger";
                }

            }
            else
            {
                return "Please Pass valid Token";
            }

        }
        [HttpPost]
        public IActionResult RetriggerPushData()
        {
            var allsenddata = ObjBusinessLayer.GetAllRecrdstosend();
            var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);
            string Servertype = iconfiguration["ServerType:type"];

            //var postretrigger = _MethodWrapper.RetriggerPostDataDelivery(allsenddata, triggerid, 0);
            var postretrigger = _MethodWrapper.Action(allsenddata, triggerid, 0, Servertype);
            return Accepted(postretrigger.ObjectParam);

        }

        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> GetErrorCodes()
        {
            var returndata = ObjBusinessLayer.GetErrorCodes();

            return returndata;
        }
       
        [HttpPost]
        public IActionResult authToken(TokenEntity tokenEntity)
        {
            //GenerateToken generateToken=new GenerateToken(null) ;
            var token = _jWTManager.GenerateJWTTokens(tokenEntity, out tokenEntity);
            var result = "";
            _logger.LogInformation($" log Object {JsonConvert.SerializeObject(token)}");
            try
            {
                if (token == null)
                {
                    _logger.LogInformation($" Error Object {JsonConvert.SerializeObject(token)}");
                    return Unauthorized(new { status = "INVALID_CREDENTIALS", token = "Invalid credentials" });
                }
                result = JsonConvert.SerializeObject(new { status = "SUCCESS", token = token });
                _logger.LogInformation($" Debug Object {JsonConvert.SerializeObject(token)}");
            }
            catch (Exception ex) { _logger.LogInformation($" Error Object {JsonConvert.SerializeObject(ex)}"); }
            return Ok(result);
        }
        //[CustomAuthorizationFilter]
        [Authorize]
        [HttpPost]       //[BasicAuthenticationFilter]       
        public IActionResult waybill(OmsToPandoRoot Records)
        {
            _logger.LogInformation($"Waybill Get Data From Pando {JsonConvert.SerializeObject(Records)} ,{DateTime.Now.ToLongTimeString()}");
            string Servertype = iconfiguration["ServerType:type"];

            try
            {

                RootResponse rootResponse = new RootResponse();
                string primaryid = ObjBusinessLayer.insertWaybillMain(Records);
                ObjBusinessLayer.insertWaybillshipment(Records, primaryid);
                List<Item> items = new List<Item>();
                List<CustomFieldValue> customfields = new List<CustomFieldValue>();
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
                //for (int i = 0; i < Records.Shipment.customFieldValues.Count; i++)
                //{
                //    CustomFieldValue customFieldValue = new CustomFieldValue();
                //    customFieldValue.name = Records.Shipment.customFieldValues[i].name;
                //    customFieldValue.value = Records.Shipment.customFieldValues[i].value;
                //    customfields.Add(customFieldValue);
                //}
                ObjBusinessLayer.insertWaybilldeliveryaddress(Records.deliveryAddressDetails, primaryid);
                ObjBusinessLayer.insertWaybillpickupadres(Records.pickupAddressDetails, primaryid);
                ObjBusinessLayer.insertWaybillReturnaddress(Records.returnAddressDetails, primaryid);
                //ObjBusinessLayer.InsertCustomfieldWaybill(customfields, primaryid, Records.Shipment.code);
                ObjBusinessLayer.InsertitemWaybill(items, primaryid, Records.Shipment.code);

                //Data Pushed to Pamdo
                var sendwaybilldata = ObjBusinessLayer.GetWaybillAllRecrdstosend();
                if (sendwaybilldata.Count > 0)
                {
                    var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendwaybilldata);
                    var postres = _MethodWrapper.WaybillGenerationPostData(sendwaybilldata, 0, triggerid, Servertype);
                   
                    _logger.LogInformation($"Reason:-  {postres.Result.ObjectParam},{DateTime.Now.ToLongTimeString()}");
                    //return Accepted(postres.Result.ObjectParam);
                }
                ErrorResponse errorResponse = new ErrorResponse();

                errorResponse.status = "FAILED";
                errorResponse.reason = "AWB not generated";
                errorResponse.message = "AWB generation is in queue, please check after a few mins";
                _logger.LogInformation($" WayBill response {JsonConvert.SerializeObject(errorResponse)}");
                return new JsonResult(errorResponse);
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.status = "FAILED";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Resource requires authentication. Please check your authorization token.";
                _logger.LogInformation($" Error: {JsonConvert.SerializeObject(errorResponse)}");
                return new JsonResult(errorResponse);
                throw;
            }
        }
        //[HttpPost]
        //public IActionResult PostWaybillGeneration()
        //{
        //    string Servertype = iconfiguration["ServerType:type"];

        //    _logger.LogInformation($"Waybill Post ,{DateTime.Now.ToLongTimeString()}");
        //    var sendwaybilldata = ObjBusinessLayer.GetWaybillAllRecrdstosend();
        //    if (sendwaybilldata.Count > 0)
        //    {
        //        var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendwaybilldata);
        //        var postres = _MethodWrapper.WaybillGenerationPostData(sendwaybilldata, 0, triggerid, Servertype);
        //        //return postres;
        //        _logger.LogInformation($"Reason:-  {postres.Result.ObjectParam},{DateTime.Now.ToLongTimeString()}");
        //        return Accepted(postres.Result.ObjectParam);
        //    }
        //    else
        //        return BadRequest("There is no record for Post");

        //}

        [HttpPost]
        public IActionResult ReturnOrderAPI(string returnType = "CIR", string statusCode = "COMPLETE", string createdTo = "2023-07-11T14:20:40", string createdFrom = "2023-07-05T14:20:40")
        {
            //RequestReturnOrder requestReturnOrder = new RequestReturnOrder();
            //requestReturnOrder.returnType = returnType;
            //requestReturnOrder.statusCode = statusCode;
            //requestReturnOrder.createdTo = createdTo;
            //requestReturnOrder.createdFrom = createdFrom;
            //var todaydate = DateTime.Today;
            //var unixepochdateda=todaydate.ToUniversalTime().Ticks;
            string Servertype = iconfiguration["ServerType:type"];

            //            string[] Facilities = {
            //"Hosur_Avigna"
            //,"AVIGNA_DFX",
            //"Gurgaon_New",
            //"CHENNAI",
            //"COCHIN",
            //"KOLKATA",
            //"Hydrabad_Item",
            //"BHIWANDIITEM"
            //                };
            var Facilities = ObjBusinessLayer.GetFacilityList();


            //var json = JsonConvert.SerializeObject(requestReturnOrder);
            //var resu = _Token.GetTokens().Result;
            //var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            string token = HttpContext.Session.GetString("Token");
            if (token != null)
            {
                //string token = deres.access_token.ToString();
                //PandoUniwariToken resu = _Token.GetTokens().Result;
                //HttpContext.Session.SetString("Token", resu.access_token.ToString());
                var json = JsonConvert.SerializeObject(new { returnType, statusCode, createdTo, createdFrom });
                //string token = HttpContext.Session.GetString("Token");
                foreach (var FacilityCode in Facilities)
                {

                    var resuordercode = _MethodWrapper.GetReturnorderCode(json, token, 0, Servertype, FacilityCode.facilityCode);
                    if (resuordercode.Count > 0)
                    {
                        ObjBusinessLayer.insertReturnOrdercoder(resuordercode,FacilityCode.facilityCode);
                        var codes = ObjBusinessLayer.GetReturnOrderCodes();
                        List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
                        List<ReturnSaleOrderItem> returnSaleOrderItems = new List<ReturnSaleOrderItem>();
                        List<ReturnAddressDetailsList> returnAddressDetailsLists = new List<ReturnAddressDetailsList>();
                        for (int j = 0; j < codes.ObjectParam.Count; j++)
                        {
                            ReturnOrderGet returnOrderGet = new ReturnOrderGet();
                            returnOrderGet.reversePickupCode = codes.ObjectParam[j].code;
                            var jdetail = JsonConvert.SerializeObject(returnOrderGet);
                            var Code = codes.ObjectParam[j].code;
                            var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0, Servertype, FacilityCode.facilityCode);
                            if (list.returnAddressDetailsList.Count > 0 || list.returnSaleOrderItems.Count > 0)
                            {
                                returnAddressDetailsLists.AddRange(list.returnAddressDetailsList);
                                returnSaleOrderItems.AddRange(list.returnSaleOrderItems);
                            }
                            //else
                            //    return BadRequest("Please Retrigger");

                        }
                        ObjBusinessLayer.insertReturnSaleOrderitem(returnSaleOrderItems);
                        ObjBusinessLayer.insertReturnaddress(returnAddressDetailsLists);
                        var skucodes = ObjBusinessLayer.GetReturnOrderSkuCodes();

                        List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                        List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();

                        for (int i = 0; i < skucodes.ObjectParam.Count; i++)
                        {
                            skucode sKucodes = new skucode();
                            sKucodes.skuCode = skucodes.ObjectParam[i].skuCode;
                            var code = skucodes.ObjectParam[i].Code;
                            var skucode = skucodes.ObjectParam[i].skuCode;
                            var jskucode = JsonConvert.SerializeObject(sKucodes);

                            var skudetails = _MethodWrapper.getReturnOrderSkuCode(jskucode, token, code, skucode, 0, Servertype);
                            if (skudetails.Code != null)
                            {
                                itemTdto.Add(skudetails);

                                //return status;
                            }
                            //else
                            //    return BadRequest("Please Retrigger");
                        }
                        ObjBusinessLayer.insertReturOrderItemtypes(itemTdto);
                        var sendata = ObjBusinessLayer.GetReturnOrderSendData();
                        if (sendata.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata);
                            var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0, Servertype);
                            return Accepted(status.Result.ObjectParam);
                        }
                        else
                            return BadRequest("Please Retrigger");
                    }
                    else
                    {
                        return BadRequest("Please Retrigger");
                    }
                }
            }
            return BadRequest("Please Pass Valid token");
            //HttpContext.Session.SetString("Token", resu.access_token.ToString());
            //string token = HttpContext.Session.GetString("Token");


        }
        [HttpGet]
        public string ReturnOrderAPIRetrigger()
        {
            Log.Information("Retriggered Return Order API");
            string Servertype = iconfiguration["ServerType:type"];
            var resu = _Token.GetTokens(Servertype).Result;
            string[] Facilities = {
"Hosur_Avigna",
"AVIGNA_DFX",
"Gurgaon_New",
"CHENNAI",
"COCHIN",
"KOLKATA",
"Hydrabad_Item",
"BHIWANDIITEM"
                };
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            if (deres != null)
            {
                foreach (var FacilityCode in Facilities)
                {
                    string token = deres.access_token.ToString();
                    var codes = ObjBusinessLayer.GetReturnOrderCodesForRetrigger();
                    List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
                    List<ReturnSaleOrderItem> returnSaleOrderItems = new List<ReturnSaleOrderItem>();
                    List<ReturnAddressDetailsList> returnAddressDetailsLists = new List<ReturnAddressDetailsList>();
                    for (int j = 0; j < codes.ObjectParam.Count; j++)
                    {
                        ReturnOrderGet returnOrderGet = new ReturnOrderGet();
                        returnOrderGet.reversePickupCode = codes.ObjectParam[j].code;
                        var jdetail = JsonConvert.SerializeObject(returnOrderGet);
                        var Code = codes.ObjectParam[j].code;
                        var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0, Servertype, FacilityCode);
                        if (list.returnAddressDetailsList.Count > 0 || list.returnSaleOrderItems.Count > 0)
                        {
                            returnAddressDetailsLists.AddRange(list.returnAddressDetailsList);
                            returnSaleOrderItems.AddRange(list.returnSaleOrderItems);
                        }
                        //else
                        //    return BadRequest("Please Retrigger");

                    }
                    ObjBusinessLayer.insertReturnSaleOrderitem(returnSaleOrderItems);
                    ObjBusinessLayer.insertReturnaddress(returnAddressDetailsLists);
                    var skucodes = ObjBusinessLayer.GetReturnOrderSkuCodes();

                    List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                    List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();

                    for (int i = 0; i < skucodes.ObjectParam.Count; i++)
                    {
                        skucode sKucodes = new skucode();
                        sKucodes.skuCode = skucodes.ObjectParam[i].skuCode;
                        var code = skucodes.ObjectParam[i].Code;
                        var skucode = skucodes.ObjectParam[i].skuCode;

                        var jskucode = JsonConvert.SerializeObject(sKucodes);

                        var skudetails = _MethodWrapper.getReturnOrderSkuCode(jskucode, token, code, skucode, 0, Servertype);
                        if (skudetails.Code != null)
                        {
                            itemTdto.Add(skudetails);

                            //return status;
                        }
                        //else
                        //    return BadRequest("Please Retrigger");
                    }
                    ObjBusinessLayer.insertReturOrderItemtypes(itemTdto);
                    var sendata = ObjBusinessLayer.GetReturnOrderSendData();
                    if (sendata.ObjectParam.Count > 0)
                    {
                        var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata);
                        var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0, Servertype);
                        //return Accepted(status.Result.ObjectParam);
                        return "Failed Data Triggered Successfully";
                    }
                    else return "Get Some Issue Please Retrigger";
                }
            }

            return "Please Pass valid Token";


            //HttpContext.Session.SetString("Token", resu.access_token.ToString());
            //string token = HttpContext.Session.GetString("Token");


        }
        [HttpGet]
        public IActionResult ReturnorderFinalData()
        {
            string Servertype = iconfiguration["ServerType:type"];

            var sendata = ObjBusinessLayer.GetReturnOrderSendData();
            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata);
            var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0, Servertype);
            return Accepted(status.Result.ObjectParam);

        }

        //[HttpGet]
        //public IActionResult GetTokenForSTO()
        //{
        //    //BearerToken _Token = new BearerToken();
        //    //PandoUniwariToken resu = _Token.GetTokensSTO().Result;
        //    //HttpContext.Session.SetString("STOToken", resu.access_token.ToString());
        //    //return new PandoUniwariToken();
        //    var resu = _Token.GetTokensSTO().Result;
        //    var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
        //    if (resu.ObjectParam != null)
        //    {
        //        string token = deres.access_token.ToString();
        //        HttpContext.Session.SetString("STOToken", token);
        //        return Accepted(resu.ObjectParam);
        //    }
        //    else
        //    {
        //        return BadRequest("Something Went Wrong");
        //    }
        //}
        [HttpPost]
        public IActionResult STOWaybill(string fromDate = "2023-10-06T06:00:00", string toDate = "2023-10-06T06:30:00", string type = "STOCK_TRANSFER", string statusCode = "Return_awaited")
        {
            string token = HttpContext.Session.GetString("Token");
            string Servertype = iconfiguration["ServerType:type"];
            //            string[] Facilities = {
            //"Hosur_Avigna",
            //"AVIGNA_DFX",
            //"Gurgaon_New",
            //"CHENNAI",
            //"COCHIN",
            //"KOLKATA",
            //"Hydrabad_Item",
            //"BHIWANDIITEM"
            //                };
            var Facilities = ObjBusinessLayer.GetFacilityList();

            if (token != null)
            {
                foreach (var FacilityCode in Facilities)
                {
                    var jsonre = JsonConvert.SerializeObject(new { fromDate, toDate, type, statusCode });
                    Log.Information("STO WaybillGetPass Code" + jsonre + ": " + token);
                    List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                    List<Element> elements = new List<Element>();
                    var res = _MethodWrapper.GatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode);
                    if (res.Count > 0)
                    {
                        ObjBusinessLayer.insertGatePassCode(res, FacilityCode.facilityCode);
                        var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCode();
                        for (int i = 0; i < GatePassCode.Count; i++)
                        {
                            string code = GatePassCode[i].code;
                            List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                            var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                            var elemnetsList = _MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode);
                            if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                            {
                                gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                                elements.AddRange(elemnetsList.elements);
                            }

                        }
                        ObjBusinessLayer.insertGatePassElements(elements);
                        ObjBusinessLayer.insertItemTypeDTO(gatePassItemDTOs);
                        var Skucodes = ObjBusinessLayer.GetWaybillSKUCode();
                        List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                        for (int k = 0; k < Skucodes.Count; k++)
                        {
                            string itemsku = Skucodes[k].itemTypeSKU;
                            var skucode = JsonConvert.SerializeObject(new { skuCode = Skucodes[k].itemTypeSKU });
                            var code = Skucodes[k].code;
                            var Itemtypes = _MethodWrapper.GetSTOWaybillSkuDetails(skucode, token, code, itemsku, 0, Servertype);
                            if (Itemtypes.Code != null)
                            {
                                itemTypeDTO.Add(Itemtypes);
                            }
                        }
                        ObjBusinessLayer.insertWaybillItemType(itemTypeDTO);
                        var Records = ObjBusinessLayer.GetAllWaybillSTOPost();
                        if (Records.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records);
                            var status = _MethodWrapper.WaybillSTOPostData(Records, triggerid, 0, Servertype);
                            return Accepted(status.Result.ObjectParam);
                        }
                        else return BadRequest("Please Retrigger");
                    }
                    else
                        return BadRequest("Please Retrigger");
                }
            }
            return BadRequest("Please Pass valid token");

        }

        [HttpGet]
        public string STOWaybillRetrigger()
        {
            Log.Information("Retrigger STOwaybill api");
            string Servertype = iconfiguration["ServerType:type"];

            //BearerToken _Token = new BearerToken();
            var resu = _Token.GetTokens(Servertype).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            //HttpContext.Session.SetString("STOToken", deres.access_token.ToString());
            //string token = HttpContext.Session.GetString("STOToken");
            if (deres.token_type != null)
            {
                string[] Facilities = {
"Hosur_Avigna",
"AVIGNA_DFX",
"Gurgaon_New",
"CHENNAI",
"COCHIN",
"KOLKATA",
"Hydrabad_Item",
"BHIWANDIITEM"
                };
                foreach (var FacilityCode in Facilities)
                {
                    string token = deres.access_token.ToString();
                    List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                    List<Element> elements = new List<Element>();

                    var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCodeForretrigger();
                    for (int i = 0; i < GatePassCode.Count; i++)
                    {
                        string code = GatePassCode[i].code;
                        List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                        var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                        var elemnetsList = _MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode);
                        if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                        {
                            gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                            elements.AddRange(elemnetsList.elements);
                        }
                        //else
                        //{
                        //    return BadRequest("Please Retrigger");
                        //}
                    }
                    ObjBusinessLayer.insertGatePassElements(elements);
                    ObjBusinessLayer.insertItemTypeDTO(gatePassItemDTOs);
                    var Skucodes = ObjBusinessLayer.GetWaybillSKUCode();
                    List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                    for (int k = 0; k < Skucodes.Count; k++)
                    {
                        string itemsku = Skucodes[k].itemTypeSKU;
                        var skucode = JsonConvert.SerializeObject(new { skuCode = Skucodes[k].itemTypeSKU });
                        var code = Skucodes[k].code;
                        var Itemtypes = _MethodWrapper.GetSTOWaybillSkuDetails(skucode, token, code, itemsku, 0, Servertype);
                        if (Itemtypes.Code != null)
                        {
                            itemTypeDTO.Add(Itemtypes);
                        }
                        //else
                        //    return BadRequest("Please Retrigger");
                    }
                    ObjBusinessLayer.insertWaybillItemType(itemTypeDTO);
                    var Records = ObjBusinessLayer.GetAllWaybillSTOPost();
                    if (Records.Count > 0)
                    {
                        //var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records);
                        //var status = _MethodWrapper.WaybillSTOPostData(Records, triggerid, 0);
                        //return Accepted(status.Result.ObjectParam);
                        return "Triggered Successfully";
                    }
                    else return "Get Some Issue Please Retrigger";
                }
            }
            return "Something went Wrong";

        }

        [HttpGet]
        public IActionResult STOwaybillFinalData()
        {
            string Servertype = iconfiguration["ServerType:type"];

            var Records = ObjBusinessLayer.GetAllWaybillSTOPost();
            var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records);
            var status = _MethodWrapper.WaybillSTOPostData(Records, triggerid, 0, Servertype);
            return Accepted(status.Result.ObjectParam);
        }

        [HttpPost]
        public IActionResult STOAPI(string fromDate = "2023-10-06T00:00:00", string toDate = "2023-10-06T11:40:00", string type = "STOCK_TRANSFER", string statusCode = "created")
        {
            string token = HttpContext.Session.GetString("Token");
            string Servertype = iconfiguration["ServerType:type"];

            if (!string.IsNullOrEmpty(token))
            {
                //                string[] Facilities = {
                //"Hosur_Avigna",
                //"AVIGNA_DFX",
                //"Gurgaon_New",
                //"CHENNAI",
                //"COCHIN",
                //"KOLKATA",
                //"Hydrabad_Item",
                //"BHIWANDIITEM"
                //                };
                var Facilities = ObjBusinessLayer.GetFacilityList();
                var jsonre = JsonConvert.SerializeObject(new { fromDate, toDate, type, statusCode });

                List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                List<Element> elements = new List<Element>();
                Log.Information("STO API Called : " + jsonre + " " + token);
                foreach (var FacilityCode in Facilities)
                {
                    var res = _MethodWrapper.STOAPIGatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode);
                    if (res.Count > 0)
                    {

                        ObjBusinessLayer.insertSTOAPIGatePassCode(res,FacilityCode.facilityCode);
                        var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCode();
                        for (int i = 0; i < gatePass.Count; i++)
                        {
                            string code = gatePass[i].code;
                            List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                            var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                            var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode);
                            if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                            {
                                gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                                elements.AddRange(elemnetsList.elements);
                            }
                        }
                        ObjBusinessLayer.insertSTOAPiGatePassElements(elements);
                        ObjBusinessLayer.insertSTOAPiItemTypeDTO(gatePassItemDTOs);
                        var skuitemtype = ObjBusinessLayer.GetSTOSKUCode();
                        List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                        for (int k = 0; k < skuitemtype.Count; k++)
                        {
                            var skucode = JsonConvert.SerializeObject(new { skuCode = skuitemtype[k].itemTypeSKU });
                            var code = skuitemtype[k].code;
                            var skutype = skuitemtype[k].itemTypeSKU;
                            var Itemtypes = _MethodWrapper.GetSTOAPISkuDetails(skucode, token, code, skutype, 0, Servertype);
                            if (Itemtypes.Code != null)
                            {
                                itemTypeDTO.Add(Itemtypes);
                            }
                            //else
                            //    return BadRequest("Please Retrigger");
                        }
                        ObjBusinessLayer.insertSTOAPItemType(itemTypeDTO);
                        var allrecords = ObjBusinessLayer.GetSTOAPISendData();
                        if (allrecords.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords);
                            var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0, Servertype);
                            return Accepted(status.Result.ObjectParam);
                        }
                        else { return BadRequest("Please Retrigger"); }
                    }
                }
                return BadRequest("Please Retrigger");

            }
            else
            {
                return BadRequest("Please Pass Valid token");
            }

        }

        [HttpGet]
        public string STOAPIRetrigger()
        {
            Log.Information("STOAPI Retriggered");
            string Servertype = iconfiguration["ServerType:type"];

            //string token = HttpContext.Session.GetString("STOToken");
            //PandoUniwariToken resu = _Token.GetTokensSTO().Result;
            //HttpContext.Session.SetString("STOToken", resu.access_token.ToString());
            //string token = HttpContext.Session.GetString("STOToken");
            string[] Facilities = {
"Hosur_Avigna",
"AVIGNA_DFX",
"Gurgaon_New",
"CHENNAI",
"COCHIN",
"KOLKATA",
"Hydrabad_Item",
"BHIWANDIITEM"
                };
            var resu = _Token.GetTokens(Servertype).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            if (deres.token_type != null)
            {
                foreach (var FacilityCode in Facilities)
                {
                    string token = deres.access_token.ToString();
                    List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                    List<Element> elements = new List<Element>();
                    var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCodeRetrigger();
                    for (int i = 0; i < gatePass.Count; i++)
                    {
                        string code = gatePass[i].code;
                        List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                        var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                        var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode);
                        if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                        {
                            gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                            elements.AddRange(elemnetsList.elements);
                        }

                    }
                    ObjBusinessLayer.insertSTOAPiGatePassElements(elements);
                    ObjBusinessLayer.insertSTOAPiItemTypeDTO(gatePassItemDTOs);
                    var skuitemtype = ObjBusinessLayer.GetSTOSKUCode();
                    List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                    for (int k = 0; k < skuitemtype.Count; k++)
                    {
                        var skucode = JsonConvert.SerializeObject(new { skuCode = skuitemtype[k].itemTypeSKU });
                        var code = skuitemtype[k].code;
                        var skutype = skuitemtype[k].itemTypeSKU;
                        var Itemtypes = _MethodWrapper.GetSTOAPISkuDetails(skucode, token, code, skutype, 0, Servertype);
                        if (Itemtypes.Code != null)
                        {
                            itemTypeDTO.Add(Itemtypes);
                        }
                        //else
                        //    return BadRequest("Please Retrigger");
                    }
                    ObjBusinessLayer.insertSTOAPItemType(itemTypeDTO);
                    var allrecords = ObjBusinessLayer.GetSTOAPISendData();
                    if (allrecords.ObjectParam.Count > 0)
                    {
                        //var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords);
                        //var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0);
                        //return Accepted(status.Result.ObjectParam);
                        return "Retriggered Successfully";
                    }
                    //else return BadRequest("Please Retrigger");
                    else return "Get Some Issue Please Retrigger";
                }
            }
            return "Something went Wrong";

        }
        [HttpGet]
        public IActionResult STOAPIFinaldata()
        {
            string Servertype = iconfiguration["ServerType:type"];

            var allrecords = ObjBusinessLayer.GetSTOAPISendData();
            var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords);
            var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0, Servertype);
            return Accepted(status.Result.ObjectParam);
        }

        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> STOApiErrorDetails()
        {
            var returndata = ObjBusinessLayer.BLSTOAPI();
            return returndata;
        }
        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> STOWaybillErrorDetails()
        {
            var returndata = ObjBusinessLayer.BLSTOWaybil();
            return returndata;
        }
        [HttpGet]
        public ServiceResponse<List<EndpointErrorDetails>> waybillErrorDetails()
        {
            var returndata = ObjBusinessLayer.BLWaybilStatus();
            return returndata;
        }
        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> ReturnOrderDetails()
        {
            var returndata = ObjBusinessLayer.BLReturnOrderStatus();
            return returndata;
        }
        [HttpPost]
        [Authorize]
        public IActionResult UpdateShippingPackage(List<UpdateShippingpackage> shippingPackages)
        {

            List<UpdateShippingpackagedb> updatelist = new List<UpdateShippingpackagedb>();
            List<ShippingBoxdb> shipbox = new List<ShippingBoxdb>();
            List<addCustomFieldValue> customFields = new List<addCustomFieldValue>();
            _logger.LogInformation($" UpdateShippingPackage Request {JsonConvert.SerializeObject(shippingPackages)}");
            try
            {
                string Servertype = iconfiguration["ServerType:type"];

                for (int i = 0; i < shippingPackages.Count; i++)
                {
                    UpdateShippingpackagedb updateShippingpackage = new UpdateShippingpackagedb();
                    var randomid = ObjBusinessLayer.GenerateNumeric();
                    updateShippingpackage.id = randomid;
                    updateShippingpackage.shippingPackageCode = shippingPackages[i].shippingPackageCode.ToString();
                    updateShippingpackage.shippingProviderCode = shippingPackages[i].shippingProviderCode.ToString();
                    updateShippingpackage.trackingNumber = shippingPackages[i].trackingNumber.ToString();
                    updateShippingpackage.actualWeight = shippingPackages[i].actualWeight;
                    updateShippingpackage.noOfBoxes = shippingPackages[i].noOfBoxes;
                    updatelist.Add(updateShippingpackage);
                    //Shipping Box
                    ShippingBoxdb shippingBox = new ShippingBoxdb();
                    shippingBox.Id = randomid;
                    shippingBox.length = shippingPackages[i].shippingBox.length;
                    shippingBox.height = shippingPackages[i].shippingBox.height;
                    shippingBox.width = shippingPackages[i].shippingBox.width;
                    //for (int l = 0; l < shippingPackages[i].shippingBox.Count; l++)
                    //{
                    //    ShippingBox shippingBox = new ShippingBox();
                    //    shippingBox.Id = randomid;
                    //    shippingBox.length = shippingPackages[i].shippingBox[l].length;
                    //    shippingBox.height = shippingPackages[i].shippingBox[l].height;
                    //    shippingBox.width = shippingPackages[i].shippingBox[l].width;
                    shipbox.Add(shippingBox);
                    //}
                    for (int k = 0; k < shippingPackages[i].customFieldValues.Count; k++)
                    {
                        addCustomFieldValue customFieldValue = new addCustomFieldValue();
                        customFieldValue.Id = randomid;
                        customFieldValue.name = shippingPackages[i].customFieldValues[k].name;
                        customFieldValue.value = shippingPackages[i].customFieldValues[k].value;
                        customFields.Add(customFieldValue);
                    }
                }
                ObjBusinessLayer.InsertUpdateShippingpackage(updatelist);
                ObjBusinessLayer.InsertUpdateShippingpackageBox(shipbox);
                ObjBusinessLayer.InsertCustomFields(customFields);
                //Data Pushing to Pando
                var resu = _Token.GetTokens(Servertype).Result;
                var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                string token = accesstoken.access_token;
                if (token != null)
                {
                    var lists = ObjBusinessLayer.UpdateShipingPck();
                    //var facilitycode = "";
                    if (lists.Count > 0)
                    {
                        for (int i = 0; i < lists.Count; i++)
                        {
                            UpdateShippingpackage updateShippingpackage = new UpdateShippingpackage();
                            updateShippingpackage.shippingBox = new ShippingBox();
                            updateShippingpackage.customFieldValues = new List<CustomFieldValue>();
                            updateShippingpackage.shippingPackageCode = lists[i].shippingPackageCode;
                            updateShippingpackage.shippingProviderCode = lists[i].shippingProviderCode;
                            updateShippingpackage.trackingNumber = lists[i].trackingNumber;
                            //updateShippingpackage.shippingPackageTypeCode = lists[i].shippingPackageTypeCode;
                            updateShippingpackage.actualWeight = lists[i].actualWeight;
                            updateShippingpackage.noOfBoxes = lists[i].noOfBoxes;
                            updateShippingpackage.shippingBox.length = lists[i].shippingBox.length;
                            updateShippingpackage.shippingBox.width = lists[i].shippingBox.width;
                            updateShippingpackage.shippingBox.height = lists[i].shippingBox.height;
                            var facilitycode = lists[i].FacilityCode;
                            //for (int j = 0; j < lists[i].shippingBox.Count; j++)
                            //{
                            //    ShippingBox shippingBox = new ShippingBox();
                            //    shippingBox.length = lists[i].shippingBox[j].length;
                            //    shippingBox.width = lists[i].shippingBox[j].width;
                            //    shippingBox.height = lists[i].shippingBox[j].height;
                            //    updateShippingpackage.shippingBox.Add(shippingBox);
                            //}
                            for (int k = 0; k < lists[i].customFieldValues.Count; k++)
                            {
                                CustomFieldValue customFieldValue = new CustomFieldValue();
                                customFieldValue.name = lists[i].customFieldValues[k].name;
                                customFieldValue.value = lists[i].customFieldValues[k].value;
                                updateShippingpackage.customFieldValues.Add(customFieldValue);
                                //var res = JsonConvert.SerializeObject(new { customFieldValues = new { name = customFieldValue.name, value = customFieldValue.value } });
                                //var dres=JsonConvert.DeserializeObject<CustomFieldValue>(res);
                            }
                            var triggerid = ObjBusinessLayer.UpdateShippingDataPost(updateShippingpackage, facilitycode);
                            //var response = _Token.PostUpdateShippingpckg(updateShippingpackage);
                            var response = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, triggerid, token, facilitycode, Servertype);
                            //return Accepted(response.Result);
                        }
                        //return Accepted("All Records Pushed Successfully");

                    }
                    //else return BadRequest("There is no record for Post");

                }



                //return Accepted(Accepted(shippingPackages));
                SuccessResponse successResponse = new SuccessResponse();
                successResponse.status = "Success";
                successResponse.waybill = "";
                successResponse.shippingLabel = "";
                //successResponse.courierName = Records.courierName;
                _logger.LogInformation($" UpdateShippingPackage response {JsonConvert.SerializeObject(successResponse)}");
                return new JsonResult(successResponse);

            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.status = "Error";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Please Retrigger";
                _logger.LogInformation($" Error: {JsonConvert.SerializeObject(errorResponse)}");
                return new JsonResult(errorResponse);

            }

        }
        //[HttpPost]
        //public IActionResult PostUpdateShipingData()
        //{
        //    string token = HttpContext.Session.GetString("Token");
        //    string Servertype = iconfiguration["ServerType:type"];

        //    if (token != null)
        //    {
        //        var lists = ObjBusinessLayer.UpdateShipingPck();
        //        //var facilitycode = "";
        //        if (lists.Count > 0)
        //        {
        //            for (int i = 0; i < lists.Count; i++)
        //            {
        //                UpdateShippingpackage updateShippingpackage = new UpdateShippingpackage();
        //                updateShippingpackage.shippingBox = new ShippingBox();
        //                updateShippingpackage.customFieldValues = new List<CustomFieldValue>();
        //                updateShippingpackage.shippingPackageCode = lists[i].shippingPackageCode;
        //                updateShippingpackage.shippingProviderCode = lists[i].shippingProviderCode;
        //                updateShippingpackage.trackingNumber = lists[i].trackingNumber;
        //                //updateShippingpackage.shippingPackageTypeCode = lists[i].shippingPackageTypeCode;
        //                updateShippingpackage.actualWeight = lists[i].actualWeight;
        //                updateShippingpackage.noOfBoxes = lists[i].noOfBoxes;
        //                updateShippingpackage.shippingBox.length = lists[i].shippingBox.length;
        //                updateShippingpackage.shippingBox.width = lists[i].shippingBox.width;
        //                updateShippingpackage.shippingBox.height = lists[i].shippingBox.height;
        //                var facilitycode = lists[i].FacilityCode;
        //                //for (int j = 0; j < lists[i].shippingBox.Count; j++)
        //                //{
        //                //    ShippingBox shippingBox = new ShippingBox();
        //                //    shippingBox.length = lists[i].shippingBox[j].length;
        //                //    shippingBox.width = lists[i].shippingBox[j].width;
        //                //    shippingBox.height = lists[i].shippingBox[j].height;
        //                //    updateShippingpackage.shippingBox.Add(shippingBox);
        //                //}
        //                for (int k = 0; k < lists[i].customFieldValues.Count; k++)
        //                {
        //                    CustomFieldValue customFieldValue = new CustomFieldValue();
        //                    customFieldValue.name = lists[i].customFieldValues[k].name;
        //                    customFieldValue.value = lists[i].customFieldValues[k].value;
        //                    updateShippingpackage.customFieldValues.Add(customFieldValue);
        //                    //var res = JsonConvert.SerializeObject(new { customFieldValues = new { name = customFieldValue.name, value = customFieldValue.value } });
        //                    //var dres=JsonConvert.DeserializeObject<CustomFieldValue>(res);
        //                }
        //                var triggerid = ObjBusinessLayer.UpdateShippingDataPost(updateShippingpackage, facilitycode);
        //                //var response = _Token.PostUpdateShippingpckg(updateShippingpackage);
        //                var response = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, triggerid, token, facilitycode, Servertype);

        //                //return Accepted(response.Result);
        //            }
        //            return Accepted("All Records Pushed Successfully");

        //        }
        //        else return BadRequest("There is no record for Post");

        //    }
        //    else
        //        return BadRequest("Please Pass Valid Token");
        //}

        [HttpGet]
        public ServiceResponse<List<EndpointErrorDetails>> UpdateShippingErrorDetails()
        {
            var returndata = ObjBusinessLayer.BLUpdateShippingStatus();
            return returndata;
        }
        [HttpGet]
        public IActionResult RetriggerUpdateShipping()
        {
            _logger.LogInformation($" UpdateShippingPackage Retrigger");
            string Servertype = iconfiguration["ServerType:type"];

            var Token = _Token.GetTokens(Servertype).Result;
            var _Tokens = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(Token.ObjectParam);
            if (_Tokens.access_token != null)
            {
                var lists = ObjBusinessLayer.UpdateShipingPckRetrigger();
                if (lists.Count > 0)
                {
                    for (int i = 0; i < lists.Count; i++)
                    {
                        UpdateShippingpackage updateShippingpackage = new UpdateShippingpackage();
                        updateShippingpackage.shippingBox = new ShippingBox();
                        updateShippingpackage.customFieldValues = new List<CustomFieldValue>();
                        updateShippingpackage.shippingPackageCode = lists[i].shippingPackageCode;
                        updateShippingpackage.shippingProviderCode = lists[i].shippingProviderCode;
                        updateShippingpackage.trackingNumber = lists[i].trackingNumber;
                        //updateShippingpackage.shippingPackageTypeCode = lists[i].shippingPackageTypeCode;
                        updateShippingpackage.actualWeight = lists[i].actualWeight;
                        updateShippingpackage.noOfBoxes = lists[i].noOfBoxes;
                        updateShippingpackage.shippingBox.length = lists[i].shippingBox.length;
                        updateShippingpackage.shippingBox.width = lists[i].shippingBox.width;
                        updateShippingpackage.shippingBox.height = lists[i].shippingBox.height;
                        var facilitycode = lists[i].FacilityCode;
                        //for (int j = 0; j < lists[i].shippingBox.Count; j++)
                        //{
                        //    ShippingBox shippingBox = new ShippingBox();
                        //    shippingBox.length = lists[i].shippingBox[j].length;
                        //    shippingBox.width = lists[i].shippingBox[j].width;
                        //    shippingBox.height = lists[i].shippingBox[j].height;
                        //    updateShippingpackage.shippingBox.Add(shippingBox);
                        //}
                        for (int k = 0; k < lists[i].customFieldValues.Count; k++)
                        {
                            CustomFieldValue customFieldValue = new CustomFieldValue();
                            customFieldValue.name = lists[i].customFieldValues[k].name;
                            customFieldValue.value = lists[i].customFieldValues[k].value;
                            updateShippingpackage.customFieldValues.Add(customFieldValue);
                            //var res = JsonConvert.SerializeObject(new { customFieldValues = new { name = customFieldValue.name, value = customFieldValue.value } });
                            //var dres=JsonConvert.DeserializeObject<CustomFieldValue>(res);
                        }
                        var triggerid = ObjBusinessLayer.UpdateShippingDataPost(updateShippingpackage, facilitycode);
                        //var response = _Token.PostUpdateShippingpckg(updateShippingpackage);
                        var response = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, triggerid, _Tokens.access_token, facilitycode, Servertype);

                        return Accepted(response.Result);
                    }
                    return Accepted("All Records Pushed Successfully");

                }
                else return BadRequest("There is no record for Post");
            }
            else
            {
                return BadRequest("Please pass Valid Token");
            }

        }

        [HttpPost]
        [Authorize]
        public IActionResult AllocateShipping(List<Allocateshipping> allocateshippings)
        {

            _logger.LogInformation($"Request Allocate Shipping {JsonConvert.SerializeObject(allocateshippings)}");
            try
            {
                string Servertype = iconfiguration["ServerType:type"];

                ObjBusinessLayer.InsertAllocate_Shipping(allocateshippings);
                //Post Data To Pando
                var Token = _Token.GetTokens(Servertype).Result;
                var _Tokens = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(Token.ObjectParam);
                if (_Tokens.access_token != null)
                {
                    var results = ObjBusinessLayer.PostGAllocateShippingData();
                    if (results.Count > 0)
                    {
                        for (int i = 0; i < results.Count; i++)
                        {
                            Allocateshipping allocateshipping = new Allocateshipping();
                            allocateshipping.shippingPackageCode = results[i].shippingPackageCode;
                            allocateshipping.shippingLabelMandatory = results[i].shippingLabelMandatory;
                            allocateshipping.shippingProviderCode = results[i].shippingProviderCode;
                            allocateshipping.shippingCourier = results[i].shippingCourier;
                            allocateshipping.trackingNumber = results[i].trackingNumber;
                            //allocateshipping.generateUniwareShippingLabel = results[i].generateUniwareShippingLabel;
                            var facility = results[i].FacilityCode;
                            var Triggerid = ObjBusinessLayer.AllocateShippingDataPost(allocateshipping);
                            var response = _MethodWrapper.AllocatingShippingPostData(allocateshipping, 0, Triggerid, _Tokens.access_token, facility, Servertype);
                            //return Ok(response.Result.ObjectParam);
                        }
                        //return Accepted("All Records Pushed Successfully");
                    }
                    //else { return BadRequest("There is no record for Post"); }
                }



                SuccessResponse successResponse = new SuccessResponse();
                successResponse.status = "Success";
                successResponse.waybill = "";
                successResponse.shippingLabel = "";
                //successResponse.courierName = Records.courierName;
                _logger.LogInformation($" Allocate Shipping response {JsonConvert.SerializeObject(successResponse)}");
                return new JsonResult(successResponse);
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.status = "Error";
                errorResponse.reason = ex.Message;
                errorResponse.message = "Please Retrigger";
                _logger.LogInformation($" Error: {JsonConvert.SerializeObject(errorResponse)}");
                return new JsonResult(errorResponse);
                throw ex;
            }
            // return Accepted();

        }
        //[HttpPost]
        //public IActionResult PostAllocateShipping()
        //{
        //    string Servertype = iconfiguration["ServerType:type"];

        //    _logger.LogInformation($"Post Data Allocate Shipping");
        //    //string token = HttpContext.Session.GetString("STOToken");
        //    var Token = _Token.GetTokens(Servertype).Result;
        //    var _Tokens = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(Token.ObjectParam);
        //    if (_Tokens.access_token != null)
        //    {
        //        var results = ObjBusinessLayer.PostGAllocateShippingData();
        //        if (results.Count > 0)
        //        {
        //            for (int i = 0; i < results.Count; i++)
        //            {
        //                Allocateshipping allocateshipping = new Allocateshipping();
        //                allocateshipping.shippingPackageCode = results[i].shippingPackageCode;
        //                allocateshipping.shippingLabelMandatory = results[i].shippingLabelMandatory;
        //                allocateshipping.shippingProviderCode = results[i].shippingProviderCode;
        //                allocateshipping.shippingCourier = results[i].shippingCourier;
        //                allocateshipping.trackingNumber = results[i].trackingNumber;
        //                //allocateshipping.generateUniwareShippingLabel = results[i].generateUniwareShippingLabel;
        //                var facility = results[i].FacilityCode;
        //                var Triggerid = ObjBusinessLayer.AllocateShippingDataPost(allocateshipping);
        //                var response = _MethodWrapper.AllocatingShippingPostData(allocateshipping, 0, Triggerid, _Tokens.access_token, facility, Servertype);
        //                return Ok(response.Result.ObjectParam);
        //            }
        //            return Accepted("All Records Pushed Successfully");
        //        }
        //        else { return BadRequest("There is no record for Post"); }
        //    }
        //    else
        //    {
        //        return BadRequest("Please Pass valid Token");
        //    }

        //}
        [HttpGet]
        public ServiceResponse<List<EndpointErrorDetails>> AloateShippingErrorDetails()
        {
            var returndata = ObjBusinessLayer.BLAlocateShippingStatus();
            return returndata;
        }
        [HttpGet]
        public IActionResult RetriggerAllocateShipping()
        {
            _logger.LogInformation($"Retrigger Allocate Shipping");
            string Servertype = iconfiguration["ServerType:type"];

            //string token = HttpContext.Session.GetString("STOToken");
            var Token = _Token.GetTokens(Servertype).Result;
            var _Tokens = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(Token.ObjectParam);
            if (_Tokens.access_token != null)
            {
                //var results = ObjBusinessLayer.PostGAllocateShippingData();
                var results = ObjBusinessLayer.PostGAllocateShippingDataForRetrigger();
                if (results.Count > 0)
                {
                    for (int i = 0; i < results.Count; i++)
                    {
                        Allocateshipping allocateshipping = new Allocateshipping();
                        allocateshipping.shippingPackageCode = results[i].shippingPackageCode;
                        allocateshipping.shippingLabelMandatory = results[i].shippingLabelMandatory;
                        allocateshipping.shippingProviderCode = results[i].shippingProviderCode;
                        allocateshipping.shippingCourier = results[i].shippingCourier;
                        allocateshipping.trackingNumber = results[i].trackingNumber;
                        //allocateshipping.generateUniwareShippingLabel = results[i].generateUniwareShippingLabel;
                        var facilitycode = results[i].FacilityCode;

                        var Triggerid = ObjBusinessLayer.AllocateShippingDataPost(allocateshipping);
                        var response = _MethodWrapper.AllocatingShippingPostData(allocateshipping, 0, Triggerid, _Tokens.access_token, facilitycode, Servertype);
                        return Ok(response.Result.ObjectParam);
                    }
                    return Accepted("All Records Pushed Successfully");
                }
                else { return BadRequest("There is no record for Post"); }
            }
            else
            {
                return BadRequest("Please Pass valid Token");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult cancel(calcelwaybill waybill)
        {
            try
            {
                _logger.LogInformation($"Cancel Waybill: {JsonConvert.SerializeObject(waybill.waybill)}");
                ObjBusinessLayer.WaybillCancel(waybill.waybill);
                var canceldata = ObjBusinessLayer.GetWaybillCancelData();

                //var sendwaybilldata = ObjBusinessLayer.GetWaybillAllRecrdstosend();
                if (canceldata.Count > 0)
                {
                    //var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendwaybilldata);
                    var postres = _MethodWrapper.WaybillCancelPostData(canceldata, 0);
                    //return postres;
                    _logger.LogInformation($"Reason:-  {postres.Result.ObjectParam},{DateTime.Now.ToLongTimeString()}");
                    //return Accepted(postres.Result.ObjectParam);
                }

                CancelResponse response = new CancelResponse();
                response.status = "SUCCESS";
                response.waybill = waybill.waybill;
                response.errorMessage = "Order has been canceled";
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                CancelResponse response = new CancelResponse();
                response.status = "FAILED";
                response.waybill = waybill.waybill;
                response.errorMessage = ex.Message;
                return new JsonResult(response);
                throw;
            }

        }
        [HttpPost]
        public ActionResult UploadExcel(List<UploadExcels> empList)
        {
            _logger.LogInformation("Excel Upload Process.");
            string Servertype = iconfiguration["ServerType:type"];

            string ExecResult = string.Empty;
            var SO = empList.Where(r => r.Type == "SO").ToList();
            var RO = empList.Where(r => r.Type == "RO").ToList();
            var STO = empList.Where(r => r.Type == "STO").ToList();
            var resu = _Token.GetTokens(Servertype).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            //string token = HttpContext.Session.GetString("Token");
            string token = deres.access_token.ToString();

            if (SO != null)
            {
                var list = SO.Select(x => new Element() { code = x.Code }).ToList();
                if (list.Count > 0)
                {
                    var resCode = ObjBusinessLayer.InsertCode(list);
                    var ds = ObjBusinessLayer.GetCode();

                    parentList parentList = new parentList();
                    _logger.LogInformation("Saleorder/search Api called.");
                    List<Address> address = new List<Address>();
                    List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
                    List<ShippingPackage> shipingdet = new List<ShippingPackage>();
                    List<Items> qtyitems = new List<Items>();
                    List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
                    for (int i = 0; i < ds.Count; i++)
                    {
                        var jsoncodes = JsonConvert.SerializeObject(ds[i]);
                        string code = ds[i].code;
                        //parentList = PassCode(jsoncodes, token, code, 0);

                        parentList = _MethodWrapper.PassCodeer(jsoncodes, token, code, 0, Servertype);
                        if (parentList.saleOrderItems.Count > 0 || parentList.address.Count > 0 || parentList.Shipment.Count > 0 || parentList.qtyitems.Count > 0 || parentList.elements.Count > 0)
                        {
                            saleOrderItems.AddRange(parentList.saleOrderItems);
                            address.AddRange(parentList.address);
                            shipingdet.AddRange(parentList.Shipment);
                            qtyitems.AddRange(parentList.qtyitems);
                            elements.AddRange(parentList.elements);
                        }
                    }
                    var sires = ObjBusinessLayer.insertsalesorderitem(saleOrderItems);
                    var resshipng = ObjBusinessLayer.InsertBill(shipingdet);
                    var resitems = ObjBusinessLayer.insertItems(qtyitems);
                    var resads = ObjBusinessLayer.InsertAddrsss(address);
                    var resdto = ObjBusinessLayer.insertSalesDTO(elements);


                    var sku = ObjBusinessLayer.GetSKucodesBL();
                    _logger.LogInformation("ItemType/Get Api called.");

                    List<SKucode> skus = new List<SKucode>();
                    List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                    for (int i = 0; i < sku.Count; i++)
                    {
                        skucode sKucodes = new skucode();
                        sKucodes.skuCode = sku[i].SkuCode;
                        var code = sku[i].code;
                        var skucode = sku[i].SkuCode;
                        var jskucode = JsonConvert.SerializeObject(sKucodes);

                        //var insertskucode = ReturnSkuCode(jskucode, token, skucode,0);
                        var insertskucode = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype);
                        if (insertskucode.Code != null)
                        {
                            itemTdto.Add(insertskucode);
                        }
                    }
                    var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto);
                    var allsenddata = ObjBusinessLayer.GetAllRecrdstosend();
                    var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);

                    var sendcode = ObjBusinessLayer.GetAllSendData();
                    if (sendcode.Count > 0)
                    {
                        var resutt = _MethodWrapper.Action(sendcode, triggerid, 0, Servertype);
                        //return Accepted(resutt.ObjectParam);
                        if (resutt.Errcode > 200 || resutt.Errcode < 299)
                        {
                            ExecResult += "SO Data Pushed";
                        }
                    }
                    else
                    {
                        //return BadRequest("Please retrigger");
                    }
                }
                //else
                //{
                //    //return BadRequest("Please Retrigger");
                //    //ExecResult += ", SO Please Retrigger.";

                //}
                //ExecResult += " SO Pushed Successfully ";

            }
            if (RO != null)
            {
                //                string[] Facilities = {
                //"Hosur_Avigna",
                //"AVIGNA_DFX",
                //"Gurgaon_New",
                //"CHENNAI",
                //"COCHIN",
                //"KOLKATA",
                //"Hydrabad_Item",
                //"BHIWANDIITEM"
                //                };
                var Facilities = ObjBusinessLayer.GetFacilityList();

                var targetList = RO.Select(x => new ReturnorderCode() { code = x.Code }).ToList();

                if (targetList.Count > 0)
                {
                    foreach (var FacilityCode in Facilities)
                    {
                        ObjBusinessLayer.insertReturnOrdercoder(targetList, FacilityCode.facilityCode);
                        var codes = ObjBusinessLayer.GetReturnOrderCodes();
                        List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
                        List<ReturnSaleOrderItem> returnSaleOrderItems = new List<ReturnSaleOrderItem>();
                        List<ReturnAddressDetailsList> returnAddressDetailsLists = new List<ReturnAddressDetailsList>();
                        for (int j = 0; j < codes.ObjectParam.Count; j++)
                        {
                            ReturnOrderGet returnOrderGet = new ReturnOrderGet();
                            returnOrderGet.reversePickupCode = codes.ObjectParam[j].code;

                            var jdetail = JsonConvert.SerializeObject(new { reversePickupCode = codes.ObjectParam[j].code });
                            var Code = codes.ObjectParam[j].code;
                            var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0, Servertype, FacilityCode.facilityCode);
                            if (list.returnAddressDetailsList.Count > 0 || list.returnSaleOrderItems.Count > 0)
                            {
                                returnAddressDetailsLists.AddRange(list.returnAddressDetailsList);
                                returnSaleOrderItems.AddRange(list.returnSaleOrderItems);
                            }
                            //return BadRequest("Please Retrigger");
                            //ExecResult += ", RO Please Retrigger";

                        }
                        ObjBusinessLayer.insertReturnSaleOrderitem(returnSaleOrderItems);
                        ObjBusinessLayer.insertReturnaddress(returnAddressDetailsLists);
                        var skucodes = ObjBusinessLayer.GetReturnOrderSkuCodes();

                        List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                        List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();

                        for (int i = 0; i < skucodes.ObjectParam.Count; i++)
                        {
                            skucode sKucodes = new skucode();
                            sKucodes.skuCode = skucodes.ObjectParam[i].skuCode;
                            var code = skucodes.ObjectParam[i].Code;
                            var skucode = skucodes.ObjectParam[i].skuCode;
                            var jskucode = JsonConvert.SerializeObject(sKucodes);

                            var skudetails = _MethodWrapper.getReturnOrderSkuCode(jskucode, token, code, skucode, 0, Servertype);
                            if (skudetails.Code != null)
                            {
                                itemTdto.Add(skudetails);
                            }

                        }
                        ObjBusinessLayer.insertReturOrderItemtypes(itemTdto);
                        var sendata = ObjBusinessLayer.GetReturnOrderSendData();
                        if (sendata.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata);
                            var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0, Servertype);
                            //return Accepted(status.Result.ObjectParam);
                            if (status.Result.Errcode > 200 || status.Result.Errcode < 299)
                            {
                                ExecResult += ", RO Data Pushed";
                            }
                        }

                    }
                }
                //else
                //{
                // return BadRequest("Please Retrigger");
                // ExecResult += ", RO Please Retrigger";

                //}
                //ExecResult += ", RO Pushed Successfully";


            }
            if (STO != null)
            {
                //                string[] Facilities = {
                //"Hosur_Avigna",
                //"AVIGNA_DFX",
                //"Gurgaon_New",
                //"CHENNAI",
                //"COCHIN",
                //"KOLKATA",
                //"Hydrabad_Item",
                //"BHIWANDIITEM"
                //                };
                var Facilities = ObjBusinessLayer.GetFacilityList();

                var targetList = STO.Select(x => new Element() { code = x.Code }).ToList();
                if (targetList.Count > 0)
                {
                    foreach (var FacilityCode in Facilities)
                    {
                        List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                        List<Element> elements = new List<Element>();
                        ObjBusinessLayer.insertSTOAPIGatePassCode(targetList,FacilityCode.facilityCode);
                        var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCode();
                        for (int i = 0; i < gatePass.Count; i++)
                        {
                            string code = gatePass[i].code;
                            List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                            var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                            var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode);
                            if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                            {
                                gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                                elements.AddRange(elemnetsList.elements);
                            }
                            //else
                            //{
                            //    return BadRequest("Please Retrigger");
                            //}


                        }
                        ObjBusinessLayer.insertSTOAPiGatePassElements(elements);
                        ObjBusinessLayer.insertSTOAPiItemTypeDTO(gatePassItemDTOs);
                        var skuitemtype = ObjBusinessLayer.GetSTOSKUCode();
                        List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                        for (int k = 0; k < skuitemtype.Count; k++)
                        {
                            var skucode = JsonConvert.SerializeObject(new { skuCode = skuitemtype[k].itemTypeSKU });
                            var code = skuitemtype[k].code;
                            var skutype = skuitemtype[k].itemTypeSKU;
                            var Itemtypes = _MethodWrapper.GetSTOAPISkuDetails(skucode, token, code, skutype, 0, Servertype);
                            if (Itemtypes.Code != null)
                            {
                                itemTypeDTO.Add(Itemtypes);
                            }
                            //else
                            //    return BadRequest("Please Retrigger");
                        }
                        ObjBusinessLayer.insertSTOAPItemType(itemTypeDTO);
                        var allrecords = ObjBusinessLayer.GetSTOAPISendData();
                        if (allrecords.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords);
                            var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0, Servertype);
                            //return Accepted(status.Result.ObjectParam);
                            if (status.Result.Errcode > 200 || status.Result.Errcode < 299)
                            {
                                ExecResult += ", STO Pushed Successfully.";

                            }
                        }
                        //else { return BadRequest("Please Retrigger"); }

                    }
                }
                //else
                //{
                //    //return BadRequest("Please Retrigger");
                //    ExecResult += ", STO Please retrigger.";


                //}
                //ExecResult += ", STO Pushed Successfully.";

            }
            return new JsonResult(ExecResult);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ReversePickup(List<ReversePickupDb> reversePickup)
        {
            try
            {
                string Servertype = iconfiguration["ServerType:type"];
                List<ReversePickupDb> reverseitems = new List<ReversePickupDb>();
                List<PickUpAddressDb> pickaddressitems = new List<PickUpAddressDb>();
                List<DimensionDb> dimitems = new List<DimensionDb>();
                List<CustomFieldDb> customfields = new List<CustomFieldDb>();
                for (int i = 0; i < reversePickup.Count; i++)
                {
                    var randonid = ObjBusinessLayer.GenerateNumeric();
                    ReversePickupDb reverse = new ReversePickupDb();
                    reverse.CId = randonid;
                    reverse.reversePickupCode = reversePickup[i].reversePickupCode;
                    reverse.pickupInstruction = reversePickup[i].pickupInstruction;
                    reverse.trackingLink = reversePickup[i].trackingLink;
                    reverse.shippingCourier = reversePickup[i].shippingCourier;
                    reverse.trackingNumber = reversePickup[i].trackingNumber;
                    reverse.shippingProviderCode = reversePickup[i].shippingProviderCode;
                    reverseitems.Add(reverse);
                    PickUpAddressDb pickUpAddress = new PickUpAddressDb();
                    pickUpAddress.CId = randonid;
                    pickUpAddress.id = reversePickup[i].pickUpAddress.id;
                    pickUpAddress.name = reversePickup[i].pickUpAddress.name;
                    pickUpAddress.addressLine1 = reversePickup[i].pickUpAddress.addressLine1;
                    pickUpAddress.addressLine2 = reversePickup[i].pickUpAddress.addressLine2;
                    pickUpAddress.city = reversePickup[i].pickUpAddress.city;
                    pickUpAddress.state = reversePickup[i].pickUpAddress.state;
                    pickUpAddress.phone = reversePickup[i].pickUpAddress.phone;
                    pickUpAddress.pincode = reversePickup[i].pickUpAddress.pincode;
                    pickaddressitems.Add(pickUpAddress);
                    DimensionDb dimension = new DimensionDb();
                    dimension.CId = randonid;
                    dimension.boxLength = reversePickup[i].dimension.boxLength;
                    dimension.boxWidth = reversePickup[i].dimension.boxWidth;
                    dimension.boxHeight = reversePickup[i].dimension.boxHeight;
                    dimension.boxWeight = reversePickup[i].dimension.boxWeight;
                    dimitems.Add(dimension);
                    for (int j = 0; j < reversePickup[i].customFields.Count; j++)
                    {
                        CustomFieldDb customField = new CustomFieldDb();
                        customField.CId = randonid;
                        customField.name = reversePickup[i].customFields[j].name;
                        customField.value = reversePickup[i].customFields[j].value;
                        customfields.Add(customField);
                    }
                }
                var revermain = ObjBusinessLayer.BLReversePickupMain(reverseitems);
                var reveraddress = ObjBusinessLayer.BLReversePickUpAddress(pickaddressitems);
                var reverdimension = ObjBusinessLayer.BLReverseDimension(dimitems);
                var revercustom = ObjBusinessLayer.BLReverseCustomField(customfields);
                var resu = _Token.GetTokens(Servertype).Result;
                var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                string token = accesstoken.access_token;
                if (token != null)
                {
                    var lists = ObjBusinessLayer.GetReverseAllData();
                    if (lists.Count > 0)
                    {
                        //                        string[] Facilities = {
                        //"Hosur_Avigna",
                        //"AVIGNA_DFX",
                        //"Gurgaon_New",
                        //"CHENNAI",
                        //"COCHIN",
                        //"KOLKATA",
                        //"Hydrabad_Item",
                        //"BHIWANDIITEM"
                        //                };
                        var Facilities = ObjBusinessLayer.GetFacilityList();
                        for (int i = 0; i < lists.Count; i++)
                        {
                            foreach (var FacilityCode in Facilities)
                            {
                                ReversePickup updateShippingpackage = new ReversePickup();
                                updateShippingpackage.pickUpAddress = new PickUpAddress();
                                updateShippingpackage.dimension = new Dimension();
                                updateShippingpackage.customFields = new List<CustomField>();
                                updateShippingpackage.reversePickupCode = lists[i].reversePickupCode;
                                updateShippingpackage.pickupInstruction = lists[i].pickupInstruction;
                                updateShippingpackage.trackingLink = lists[i].trackingLink;
                                updateShippingpackage.shippingCourier = lists[i].shippingCourier;
                                updateShippingpackage.trackingNumber = lists[i].trackingNumber;
                                updateShippingpackage.shippingProviderCode = lists[i].shippingProviderCode;

                                updateShippingpackage.pickUpAddress.id = lists[i].pickUpAddress.id;
                                updateShippingpackage.pickUpAddress.name = lists[i].pickUpAddress.name;
                                updateShippingpackage.pickUpAddress.addressLine1 = lists[i].pickUpAddress.addressLine1;
                                updateShippingpackage.pickUpAddress.addressLine2 = lists[i].pickUpAddress.addressLine2;
                                updateShippingpackage.pickUpAddress.city = lists[i].pickUpAddress.city;
                                updateShippingpackage.pickUpAddress.state = lists[i].pickUpAddress.state;
                                updateShippingpackage.pickUpAddress.phone = lists[i].pickUpAddress.phone;
                                updateShippingpackage.pickUpAddress.pincode = lists[i].pickUpAddress.pincode;

                                updateShippingpackage.dimension.boxLength = lists[i].dimension.boxLength;
                                updateShippingpackage.dimension.boxWidth = lists[i].dimension.boxWidth;
                                updateShippingpackage.dimension.boxHeight = lists[i].dimension.boxHeight;
                                updateShippingpackage.dimension.boxWeight = lists[i].dimension.boxWeight;
                                for (int j = 0; j < lists[i].customFields.Count; j++)
                                {
                                    CustomField customField = new CustomField();
                                    customField.name = lists[i].customFields[j].name;
                                    customField.value = lists[i].customFields[j].value;
                                    updateShippingpackage.customFields.Add(customField);
                                }
                                var triggerid = ObjBusinessLayer.ReversePickUpData(updateShippingpackage);

                                var response = _MethodWrapper.ReversePickUpdetails(updateShippingpackage, 0, triggerid, token, FacilityCode.facilityCode, Servertype);
                            }
                        }
                    }
                }

                ReversePickupResponse reversePickupResponse = new ReversePickupResponse();
                reversePickupResponse.successful = true;
                reversePickupResponse.message = "";
                reversePickupResponse.errors = "";
                reversePickupResponse.warnings = "";
                return new JsonResult(reversePickupResponse);
            }
            catch (Exception ex)
            {
                ReversePickupResponse reversePickupResponse = new ReversePickupResponse();
                reversePickupResponse.successful = false;
                reversePickupResponse.message = ex.Message;
                reversePickupResponse.errors = "";
                reversePickupResponse.warnings = "";
                return new JsonResult(reversePickupResponse);


            }

        }
        [HttpGet]
        public ServiceResponse<List<EndpointErrorDetails>> ReversePickupErrorDetails()
        {
            var returndata = ObjBusinessLayer.BLGetReversePickUpErrorStatus();
            return returndata;
        }
        [HttpGet]
        public IActionResult RetriggerreversePickup()
        {
            string Servertype = iconfiguration["ServerType:type"];
            var resu = _Token.GetTokens(Servertype).Result;
            string reversePickupResponse = string.Empty;
            var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            string token = accesstoken.access_token;
            if (token != null)
            {
                var lists = ObjBusinessLayer.GetReverseAllData();
                if (lists.Count > 0)
                {
//                    string[] Facilities = {
//"Hosur_Avigna",
//"AVIGNA_DFX",
//"Gurgaon_New",
//"CHENNAI",
//"COCHIN",
//"KOLKATA",
//"Hydrabad_Item",
//"BHIWANDIITEM"
//                };
                    var Facilities = ObjBusinessLayer.GetFacilityList();
                    for (int i = 0; i < lists.Count; i++)
                    {
                        foreach (var FacilityCode in Facilities)
                        {
                            ReversePickup updateShippingpackage = new ReversePickup();
                            updateShippingpackage.pickUpAddress = new PickUpAddress();
                            updateShippingpackage.dimension = new Dimension();
                            updateShippingpackage.customFields = new List<CustomField>();
                            updateShippingpackage.reversePickupCode = lists[i].reversePickupCode;
                            updateShippingpackage.pickupInstruction = lists[i].pickupInstruction;
                            updateShippingpackage.trackingLink = lists[i].trackingLink;
                            updateShippingpackage.shippingCourier = lists[i].shippingCourier;
                            updateShippingpackage.trackingNumber = lists[i].trackingNumber;
                            updateShippingpackage.shippingProviderCode = lists[i].shippingProviderCode;

                            updateShippingpackage.pickUpAddress.id = lists[i].pickUpAddress.id;
                            updateShippingpackage.pickUpAddress.name = lists[i].pickUpAddress.name;
                            updateShippingpackage.pickUpAddress.addressLine1 = lists[i].pickUpAddress.addressLine1;
                            updateShippingpackage.pickUpAddress.addressLine2 = lists[i].pickUpAddress.addressLine2;
                            updateShippingpackage.pickUpAddress.city = lists[i].pickUpAddress.city;
                            updateShippingpackage.pickUpAddress.state = lists[i].pickUpAddress.state;
                            updateShippingpackage.pickUpAddress.phone = lists[i].pickUpAddress.phone;
                            updateShippingpackage.pickUpAddress.pincode = lists[i].pickUpAddress.pincode;

                            updateShippingpackage.dimension.boxLength = lists[i].dimension.boxLength;
                            updateShippingpackage.dimension.boxWidth = lists[i].dimension.boxWidth;
                            updateShippingpackage.dimension.boxHeight = lists[i].dimension.boxHeight;
                            updateShippingpackage.dimension.boxWeight = lists[i].dimension.boxWeight;
                            for (int j = 0; j < lists[i].customFields.Count; j++)
                            {
                                CustomField customField = new CustomField();
                                customField.name = lists[i].customFields[j].name;
                                customField.value = lists[i].customFields[j].value;
                                updateShippingpackage.customFields.Add(customField);
                            }
                            var triggerid = ObjBusinessLayer.ReversePickUpData(updateShippingpackage);

                            var response = _MethodWrapper.ReversePickUpdetails(updateShippingpackage, 0, triggerid, token, FacilityCode.ToString(), Servertype);
                            reversePickupResponse += response.Result.ObjectParam;

                        }
                    }
                    reversePickupResponse += "Please pass valid Token";
                }
            }
            return new JsonResult(reversePickupResponse);
        }
    }
}
