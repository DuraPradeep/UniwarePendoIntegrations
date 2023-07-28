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

namespace Uniware_PandoIntegration.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UniwarePandoController : Controller
    {
        private readonly ILogger<UniwarePandoController> _logger;
        private readonly IUniwarePando _jWTManager;

        public UniwarePandoController(ILogger<UniwarePandoController> logger, IUniwarePando uniwarePando)
        {
            _logger = logger;
            _jWTManager = uniwarePando;
        }
        BearerToken _Token = new BearerToken();
        private UniwareBL ObjBusinessLayer = new();
        MethodWrapper _MethodWrapper = new MethodWrapper();
        [HttpGet]
        public PandoUniwariToken GetToken()
        {
            BearerToken _Token = new BearerToken();
            PandoUniwariToken resu = _Token.GetTokens().Result;
            HttpContext.Session.SetString("Token", resu.access_token.ToString());
            return new PandoUniwariToken();
        }
        [HttpPost]
        public IActionResult SaleOrderAPI(string fromdate = "1681368375000", string todate = "1681454775000", string datatype = "CREATED")
        {
            _logger.LogInformation("Token Api called.");
            PandoUniwariToken resu = _Token.GetTokens().Result;
            HttpContext.Session.SetString("Token", resu.access_token.ToString());

            //HttpContext.Session.SetString("Token", resu.expires_in.ToString());
            //var jwthandler = new JwtSecurityTokenHandler();
            //var tokendetails = HttpContext.Session.GetString("Token");
            //var readtoken=jwthandler.ReadToken(tokendetails);


            //ReqSalesOrderSearch reqSalesOrderSearch = new Entities.ReqSalesOrderSearch();
            //reqSalesOrderSearch.fromDate = fromdate;//= "1681368375000";
            //reqSalesOrderSearch.toDate = todate;//=1681454775000;
            //reqSalesOrderSearch.dateType = datatype;//=CREATED;
            //var json = JsonConvert.SerializeObject(reqSalesOrderSearch);
            string token = HttpContext.Session.GetString("Token");
            _logger.LogInformation("saleOrder/Get Api called.");
            var json = JsonConvert.SerializeObject(new { fromDate = fromdate, toDate = todate, dateType = datatype });
            //var list = getCode(json, token, 0);
            var list = _MethodWrapper.getCode(json, token, 0);
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

                    parentList = _MethodWrapper.PassCodeer(jsoncodes, token, code);
                    if (parentList.saleOrderItems.Count > 0 || parentList.address.Count > 0 || parentList.Shipment.Count > 0 || parentList.qtyitems.Count > 0 || parentList.elements.Count > 0)
                    {
                        //    return;
                        //}
                        //else
                        //{
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
                    var insertskucode = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0);
                    if (insertskucode.Code != null)
                    {
                        itemTdto.Add(insertskucode);
                    }
                    //else
                    //return;
                    //Console.WriteLine(i);
                }
                var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto);
                var allsenddata = ObjBusinessLayer.GetAllRecrdstosend();
                var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);

                var sendcode = ObjBusinessLayer.GetAllSendData();
                if (sendcode.Count > 0)
                {
                    var resutt = _MethodWrapper.Action(sendcode, triggerid, 0);
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


        [HttpGet]
        public IActionResult Retrigger()
        {
            //PandoUniwariToken res = _Token.GetTokens().Result;
            //HttpContext.Session.SetString("Token", res.access_token.ToString());
            _logger.LogInformation("Retrigger:- Token Api called.");
            PandoUniwariToken resu = _Token.GetTokens().Result;
            HttpContext.Session.SetString("Token", resu.access_token.ToString());
            string token = HttpContext.Session.GetString("Token");
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
                parentList = _MethodWrapper.PassCodeer(jsoncodes, token, codes, 0);
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

            //var errorcode = ObjBusinessLayer.UpdateSalesOrderError(errorDetails, 1);
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
                var resku = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0);                
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
                var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);
                //var postretrigger = _MethodWrapper.RetriggerPostDataDelivery(allsenddata, triggerid, 0);
                var postretrigger = _MethodWrapper.Action(allsenddata, triggerid, 0);
                return Accepted(postretrigger.ObjectParam);
            }
            else
            {
                return BadRequest("Please Retrigger");
            }

        }
        [HttpPost]
        public IActionResult RetriggerPushData()
        {
            var allsenddata = ObjBusinessLayer.GetAllRecrdstosend();
            var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);

            //var postretrigger = _MethodWrapper.RetriggerPostDataDelivery(allsenddata, triggerid, 0);
            var postretrigger = _MethodWrapper.Action(allsenddata, triggerid, 0);
            return Accepted(postretrigger.ObjectParam);

        }

        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> GetErrorCodes()
        {
            var returndata = ObjBusinessLayer.GetErrorCodes();

            return returndata;
        }
        //[HttpGet]
        //public ServiceResponse<List<PostErrorDetails>> SendRecordStatus()
        //{
        //    var status = ObjBusinessLayer.PostDataStatus();
        //    return status;
        //}


        //Step-2
        [HttpGet]
        public IActionResult GetJWTToken(TokenEntity tokenEntity)
        {
            //GenerateToken generateToken=new GenerateToken(null) ;
            var token = _jWTManager.GenerateJWTTokens(tokenEntity);
            var result = "";
            _logger.LogInformation($" log Object {JsonConvert.SerializeObject(token)}");
            try
            {
                if (token == null)
                {
                    _logger.LogInformation($" Error Object {JsonConvert.SerializeObject(token)}");
                    return Unauthorized();
                }
                result = JsonConvert.SerializeObject(new { status = "Success", token = token });
                _logger.LogInformation($" Debug Object {JsonConvert.SerializeObject(token)}");
            }
            catch (Exception ex) { _logger.LogInformation($" Error Object {JsonConvert.SerializeObject(ex)}"); }
            return Ok(result);
        }
        [Authorize]
        [HttpPost]       //[BasicAuthenticationFilter]       
        public IActionResult Waybill(OmsToPandoRoot Records)
        {
            _logger.LogInformation($"Waybill GetData From Pando {JsonConvert.SerializeObject(Records)} ,{DateTime.Now.ToLongTimeString()}");
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
                for (int i = 0; i < Records.Shipment.customFieldValues.Count; i++)
                {
                    CustomFieldValue customFieldValue = new CustomFieldValue();
                    customFieldValue.name = Records.Shipment.customFieldValues[i].name;
                    customFieldValue.value = Records.Shipment.customFieldValues[i].value;
                    customfields.Add(customFieldValue);
                }
                ObjBusinessLayer.insertWaybilldeliveryaddress(Records.deliveryAddressDetails, primaryid);
                ObjBusinessLayer.insertWaybillpickupadres(Records.pickupAddressDetails, primaryid);
                ObjBusinessLayer.insertWaybillReturnaddress(Records.returnAddressDetails, primaryid);
                ObjBusinessLayer.InsertCustomfieldWaybill(customfields, primaryid, Records.Shipment.code);
                ObjBusinessLayer.InsertitemWaybill(items, primaryid, Records.Shipment.code);


                SuccessResponse successResponse = new SuccessResponse();
                successResponse.status = "Success";
                successResponse.waybill = "";
                successResponse.shippingLabel = "";
                successResponse.courierName = Records.courierName;
                _logger.LogInformation($" WayBill response {JsonConvert.SerializeObject(successResponse)}");
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
                throw;
            }
        }
        [HttpPost]
        public IActionResult PostWaybillGeneration()
        {
            var sendwaybilldata = ObjBusinessLayer.GetWaybillAllRecrdstosend();
            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendwaybilldata);
            var postres = _MethodWrapper.WaybillGenerationPostData(sendwaybilldata, 0, triggerid);
            //return postres;
            return Accepted(postres.Result.ObjectParam);
        }

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

            //var json = JsonConvert.SerializeObject(requestReturnOrder);
            PandoUniwariToken resu = _Token.GetTokens().Result;
            HttpContext.Session.SetString("Token", resu.access_token.ToString());
            var json = JsonConvert.SerializeObject(new { returnType, statusCode, createdTo, createdFrom });
            string token = HttpContext.Session.GetString("Token");

            var resuordercode = _MethodWrapper.GetReturnorderCode(json, token, 0);
            if (resuordercode.Count > 0)
            {
                ObjBusinessLayer.insertReturnOrdercoder(resuordercode);
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
                    var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0);
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

                    var skudetails = _MethodWrapper.getReturnOrderSkuCode(jskucode, token, code, skucode, 0);
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
                    var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0);
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
        [HttpGet]
        public IActionResult ReturnOrderAPIRetrigger()
        {
            Log.Information("Retriggered Return Order API");
            string token = HttpContext.Session.GetString("Token");
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
                var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0);
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

                var skudetails = _MethodWrapper.getReturnOrderSkuCode(jskucode, token, code, skucode, 0);
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
                var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0);
                return Accepted(status.Result.ObjectParam);
            }
            else return BadRequest("Please Retrigger");

        }
        [HttpGet]
        public IActionResult ReturnorderFinalData()
        {
            var sendata = ObjBusinessLayer.GetReturnOrderSendData();
            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata);
            var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0);
            return Accepted(status.Result.ObjectParam);

        }

        [HttpGet]
        public PandoUniwariToken GetTokenForSTO()
        {
            BearerToken _Token = new BearerToken();
            PandoUniwariToken resu = _Token.GetTokensSTO().Result;
            HttpContext.Session.SetString("STOToken", resu.access_token.ToString());
            return new PandoUniwariToken();
        }
        [HttpPost]
        public IActionResult STOWaybill(string fromDate = "2022-06-30T00:00:00", string toDate = "2022-07-02T00:00:00", string type = "STOCK_TRANSFER", string statusCode = "Return_awaited")
        {
            var jsonre = JsonConvert.SerializeObject(new { fromDate, toDate, type, statusCode });
            string token = HttpContext.Session.GetString("STOToken");
            List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
            List<Element> elements = new List<Element>();
            var res = _MethodWrapper.GatePass(jsonre, token, 0);
            if (res.Count > 0)
            {
                ObjBusinessLayer.insertGatePassCode(res);
                var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCode();
                for (int i = 0; i < GatePassCode.Count; i++)
                {
                    string code = GatePassCode[i].code;
                    List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                    var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                    var elemnetsList = _MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, code, 0);
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
                    var Itemtypes = _MethodWrapper.GetSTOWaybillSkuDetails(skucode, token, code, itemsku, 0);
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
                    var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records);
                    var status = _MethodWrapper.WaybillSTOPostData(Records, triggerid, 0);
                    return Accepted(status.Result.ObjectParam);
                }
                else return BadRequest("Please Retrigger");
            }
            else
                return BadRequest("Please Retrigger");
        }

        [HttpGet]
        public IActionResult STOWaybillRetrigger()
        {
            Log.Information("Retrigger STOwaybill api");
            string token = HttpContext.Session.GetString("STOToken");
            List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
            List<Element> elements = new List<Element>();

            var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCodeForretrigger();
            for (int i = 0; i < GatePassCode.Count; i++)
            {
                string code = GatePassCode[i].code;
                List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                var elemnetsList = _MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, code, 0);
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
                var Itemtypes = _MethodWrapper.GetSTOWaybillSkuDetails(skucode, token, code, itemsku, 0);
                if (Itemtypes.Code != null)
                {
                    itemTypeDTO.Add(Itemtypes);
                }
                //else
                //    return BadRequest("Please Retrigger");
            }
            ObjBusinessLayer.insertWaybillItemType(itemTypeDTO);
            var Records = ObjBusinessLayer.GetAllWaybillSTOPost();
            var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records);
            var status = _MethodWrapper.WaybillSTOPostData(Records, triggerid, 0);
            return Accepted(status.Result.ObjectParam);

        }

        [HttpGet]
        public IActionResult STOwaybillFinalData()
        {
            var Records = ObjBusinessLayer.GetAllWaybillSTOPost();
            var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records);
            var status = _MethodWrapper.WaybillSTOPostData(Records, triggerid, 0);
            return Accepted(status.Result.ObjectParam);
        }

        [HttpPost]
        public IActionResult STOAPI(string fromDate = "2022-06-30T00:00:00", string toDate = "2022-07-02T00:00:00", string type = "STOCK_TRANSFER", string statusCode = "created")
        {
            var jsonre = JsonConvert.SerializeObject(new { fromDate, toDate, type, statusCode });
            string token = HttpContext.Session.GetString("STOToken");
            List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
            List<Element> elements = new List<Element>();
            Log.Information("STO API Called : " + jsonre + " " + token);
            var res = _MethodWrapper.STOAPIGatePass(jsonre, token, 0);
            if (res.Count > 0)
            {
                ObjBusinessLayer.insertSTOAPIGatePassCode(res);
                var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCode();
                for (int i = 0; i < gatePass.Count; i++)
                {
                    string code = gatePass[i].code;
                    List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                    var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                    var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0);
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
                    var Itemtypes = _MethodWrapper.GetSTOAPISkuDetails(skucode, token, code, skutype, 0);
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
                    var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0);
                    return Accepted(status.Result.ObjectParam);
                }
                else { return BadRequest("Please Retrigger"); }

            }
            return BadRequest("Please Retrigger");

        }

        [HttpGet]
        public IActionResult STOAPIRetrigger()
        {
            Log.Information("STOAPI Retriggered");
            string token = HttpContext.Session.GetString("STOToken");
            List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
            List<Element> elements = new List<Element>();
            var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCodeRetrigger();
            for (int i = 0; i < gatePass.Count; i++)
            {
                string code = gatePass[i].code;
                List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0);
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
                var Itemtypes = _MethodWrapper.GetSTOAPISkuDetails(skucode, token, code, skutype, 0);
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
                var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0);
                return Accepted(status.Result.ObjectParam);
            }
            else return BadRequest("Please Retrigger");

        }
        [HttpGet]
        public IActionResult STOAPIFinaldata()
        {
            var allrecords = ObjBusinessLayer.GetSTOAPISendData();
            var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords);
            var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0);
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
        public ServiceResponse<List<CodesErrorDetails>> waybillErrorDetails()
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

    }



}
