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
        public void GetCode(string fromdate = "1681368375000", string todate = "1681454775000", string datatype = "CREATED")
        {
            _logger.LogInformation("Token Api called.");
            PandoUniwariToken resu = _Token.GetTokens().Result;
            HttpContext.Session.SetString("Token", resu.access_token.ToString());

            //HttpContext.Session.SetString("Token", resu.expires_in.ToString());
            //var jwthandler = new JwtSecurityTokenHandler();
            //var tokendetails = HttpContext.Session.GetString("Token");
            //var readtoken=jwthandler.ReadToken(tokendetails);


            ReqSalesOrderSearch reqSalesOrderSearch = new Entities.ReqSalesOrderSearch();
            reqSalesOrderSearch.fromDate = fromdate;//= "1681368375000";
            reqSalesOrderSearch.toDate = todate;//=1681454775000;
            reqSalesOrderSearch.dateType = datatype;//=CREATED;
            var json = JsonConvert.SerializeObject(reqSalesOrderSearch);
            string token = HttpContext.Session.GetString("Token");
            _logger.LogInformation("saleOrder/Get Api called.");

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

                    parentList = _MethodWrapper.PassCode(jsoncodes, token, code, 0);
                    if (parentList == null)
                    {
                        return;
                    }
                    else
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
                    var skucode = sku[i].code;
                    var jskucode = JsonConvert.SerializeObject(sKucodes);

                    //var insertskucode = ReturnSkuCode(jskucode, token, skucode,0);
                    var insertskucode = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0);
                    if (insertskucode != null)
                    {
                        itemTdto.Add(insertskucode);
                    }
                    else
                        return;
                    Console.WriteLine(i);
                }
                var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto);
                var allsenddata = ObjBusinessLayer.GetAllRecrdstosend();
                var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);

                var sendcode = ObjBusinessLayer.GetAllSendData();
                var resutt = _MethodWrapper.Action(sendcode, triggerid, 0);
            }
            else
            {
                return;
            }




        }
        //[HttpGet]        
        //public ServiceResponse<string> Action(List<Data> sendcode, string triggerid,int checkcount)
        //{
        //    int Lcheckcount = checkcount;            
        //    ServiceResponse<string> resfinal = null;

        //    resfinal = _Token.PostDataToDeliverypackList(sendcode).Result;
        //    if (resfinal.Errcode < 200 || resfinal.Errcode > 299)
        //    {
        //        Thread.Sleep(3000);
        //        if (Lcheckcount != 3)
        //        {
        //            Lcheckcount += 1;
        //            ObjBusinessLayer.UpdatePostDatadetails(true, resfinal.ObjectParam, triggerid);

        //            Action(sendcode, triggerid,Lcheckcount);
        //        }
        //        else
        //        {
        //            resfinal = null;
        //        }

        //    }

        //    return resfinal;
        //}
        //[HttpGet]
        //public ItemTypeDTO ReturnSkuCode(string jskucode,string token,string skucode, int checkcount)
        //{
        //    int Lcheckcount = checkcount;
        //    ItemTypeDTO itemsSku = new ItemTypeDTO();
        //    List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
        //    var resul = _Token.GetSkuDetails(jskucode, token);
        //    //List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
        //    if (resul.Result.Errcode < 200 || resul.Result.Errcode > 299)
        //    {
        //        if (Lcheckcount != 3)
        //        {
        //            Thread.Sleep(3000);
        //            Lcheckcount += 1;
        //            ErrorDetails ed = new ErrorDetails();
        //            ed.Status = true;
        //            ed.SkuCode = skucode;
        //            ed.Reason = resul.Result.ObjectParam;
        //            errorskuDetails.Add(ed);
        //            ReturnSkuCode(jskucode, token, skucode, Lcheckcount);
        //        }
        //        else
        //        {
        //            errorskuDetails = null;
        //        }
        //    }
        //    else
        //    {
        //        var resl = JsonConvert.DeserializeObject<SkuRoot>(resul.Result.ObjectParam);               
        //        itemsSku.Code = skucode;
        //        itemsSku.categoryCode = resl.itemTypeDTO.itemDetailFieldsText;//categoryCode;
        //        itemsSku.width = resl.itemTypeDTO.width;
        //        itemsSku.height = resl.itemTypeDTO.height;
        //        itemsSku.length = resl.itemTypeDTO.length;
        //        itemsSku.weight = resl.itemTypeDTO.weight;
        //       // itemTdto.Add(itemsSku);
        //    }
        //    //var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto);
        //    var errorskucode = ObjBusinessLayer.UpdateSkucodeError(errorskuDetails, 0);
        //   return itemsSku;
        //}
        //[HttpGet]
        //public parentList PassCode( string jsoncodes,string token,string code, int checkcount )
        //{
        //    int Lcheckcount = checkcount;
        //    parentList parentList = new parentList();
        //    parentList.elements = new List<SaleOrderDTO>();
        //    parentList.Shipment = new List<ShippingPackage>();
        //    parentList.saleOrderItems = new List<SaleOrderItem>();
        //    parentList.qtyitems = new List<Items>();
        //    parentList.address = new List<Address>();
        //    List<ErrorDetails> errorDetails = new List<ErrorDetails>();
        //    salesorderRoot details = new salesorderRoot();
        //    List<Address> address = new List<Address>();
        //    List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
        //    List<ShippingPackage> shipingdet = new List<ShippingPackage>();
        //    List<Items> qtyitems = new List<Items>();
        //    List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
        //    var results = _Token.GetCodeDetails(jsoncodes, token);
        //    if (results.Result.Errcode < 200 || results.Result.Errcode > 299)
        //    {
        //        if (Lcheckcount != 3)
        //        {
        //            Thread.Sleep(3000);
        //            Lcheckcount += 1;
        //            string codes = code;
        //            ErrorDetails ed = new ErrorDetails();
        //            ed.Code = codes;
        //            ed.Reason = results.Result.ObjectParam;
        //            ed.Status = true;                  
        //            errorDetails.Add(ed);
        //            // var errorcode = ObjBusinessLayer.UpdateSalesOrderError(errorDetails, 1);
        //            PassCode(jsoncodes, token, code, Lcheckcount);

        //        }
        //        else
        //        {
        //            parentList = null;
        //        }
        //    }
        //    else
        //    {
        //        JObject stuff = JObject.Parse(results.Result.ObjectParam);
        //        var abc = stuff.SelectTokens("saleOrderDTO");
        //        if (results.Result.ObjectParam.Contains("saleOrderDTO"))
        //        {
        //            var hello = JToken.FromObject(stuff);
        //            var jso = JsonConvert.SerializeObject(hello);
        //            salesorderRoot items = JsonConvert.DeserializeObject<salesorderRoot>(jso);
        //            details = items;
        //            List<ShippingPackage> shippingPackages = new List<ShippingPackage>();
        //            shippingPackages = details.saleOrderDTO.shippingPackages;

        //            foreach (JProperty item in abc.Children())
        //            {
        //                if (item.Path == "saleOrderDTO.shippingPackages")
        //                {
        //                    foreach (var item2 in item.Children())
        //                    {
        //                        var mi = 0;
        //                        foreach (var item3 in item2.Children()["items"])
        //                        {
        //                            ShippingPackage shippingPackage = new ShippingPackage();
        //                            for (mi = 0; mi < shippingPackages.Count(); mi++)
        //                            {
        //                                shippingPackage = shippingPackages[mi];
        //                                var adc = item3.Values();
        //                                var helloq = JToken.FromObject(adc);
        //                                var jsonq = JsonConvert.SerializeObject(helloq);

        //                                List<Items> itemsqq = JsonConvert.DeserializeObject<List<Items>>(jsonq);
        //                                shippingPackage.items = itemsqq.FirstOrDefault();
        //                                break;
        //                            }
        //                            mi++;
        //                        }
        //                    }

        //                }
        //            }
        //            details.saleOrderDTO.shippingPackages = shippingPackages;
        //        }
        //        //List SalesDTO Details
        //        SaleOrderDTO em = new SaleOrderDTO();
        //        em.code = details.saleOrderDTO.code;
        //        em.displayOrderCode = details.saleOrderDTO.displayOrderCode;
        //        //elements.Add(em);
        //        parentList.elements.Add(em);

        //        //List addressdetails

        //        for (int ads = 0; ads < details.saleOrderDTO.addresses.Count; ads++)
        //        {
        //            Address adrs = new Address();
        //            adrs.Code = details.saleOrderDTO.code;
        //            adrs.name = details.saleOrderDTO.addresses[ads].name;
        //            adrs.addressLine1 = details.saleOrderDTO.addresses[ads].addressLine1;
        //            adrs.addressLine2 = details.saleOrderDTO.addresses[ads].addressLine2;
        //            adrs.city = details.saleOrderDTO.addresses[ads].city;
        //            adrs.state = details.saleOrderDTO.addresses[ads].state;
        //            adrs.pincode = details.saleOrderDTO.addresses[ads].pincode;
        //            adrs.phone = details.saleOrderDTO.addresses[ads].phone;
        //            adrs.email = details.saleOrderDTO.addresses[ads].email;
        //            //address.Add(adrs);
        //            parentList.address.Add(adrs);

        //        }
        //        //Console.WriteLine(i);
        //        // }


        //        //List shippingDetails
        //        for (int sd = 0; sd < details.saleOrderDTO.shippingPackages.Count; sd++)
        //        {
        //            ShippingPackage shipdetails = new ShippingPackage();
        //            shipdetails.code = details.saleOrderDTO.code;
        //            shipdetails.invoiceCode = details.saleOrderDTO.shippingPackages[sd].invoiceCode;
        //            shipdetails.invoiceDate = details.saleOrderDTO.shippingPackages[sd].invoiceDate;
        //            //shipingdet.Add(shipdetails);
        //            parentList.Shipment.Add(shipdetails);
        //            Items qty = new Items();
        //            qty.Code = details.saleOrderDTO.code;
        //            qty.quantity = details.saleOrderDTO.shippingPackages[sd].items.quantity;
        //            //qtyitems.Add(qty);
        //            parentList.qtyitems.Add(qty);
        //        }

        //        //List salesorderitem
        //        for (int l = 0; l < details.saleOrderDTO.saleOrderItems.Count; l++)
        //        {
        //            SaleOrderItem sitem = new SaleOrderItem();
        //            sitem.code = details.saleOrderDTO.code;
        //            sitem.shippingPackageCode = details.saleOrderDTO.saleOrderItems[l].shippingPackageCode;
        //            sitem.id = details.saleOrderDTO.saleOrderItems[l].id;
        //            sitem.itemSku = details.saleOrderDTO.saleOrderItems[l].itemSku;
        //            sitem.prepaidAmount = details.saleOrderDTO.saleOrderItems[l].prepaidAmount;
        //            sitem.taxPercentage = details.saleOrderDTO.saleOrderItems[l].taxPercentage;
        //            sitem.totalPrice = details.saleOrderDTO.saleOrderItems[l].totalPrice;
        //            sitem.facilityCode = details.saleOrderDTO.saleOrderItems[l].facilityCode;
        //            //saleOrderItems.Add(sitem);
        //            parentList.saleOrderItems.Add(sitem);
        //        }

        //    }
        //    var errorcode = ObjBusinessLayer.UpdateSalesOrderError(errorDetails, 1);
        //    return parentList;
        //}

        //[HttpGet]
        //public List<Element> getCode(string json,string token, int checkcount )
        //{
        //    int Lcheckcount = checkcount;
        //    var result = _Token.GetCode(json, token);
        //    List<Element> elmt = new List<Element>();

        //    List<ErrorDetails> errorDetails = new List<ErrorDetails>();
        //    List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
        //    if (result.Result.Errcode < 200 || result.Result.Errcode > 299)
        //    {
        //        if (Lcheckcount != 3)
        //        {
        //            Thread.Sleep(3000);
        //            Lcheckcount += 1;
        //            getCode(json, token, Lcheckcount);
        //        }
        //        else
        //        {
        //            ObjBusinessLayer.UpdateSaleOrderFirst(result.Result.ObjectParam);
        //        }
        //    }
        //    else
        //    {
        //        var code = JsonConvert.DeserializeObject<Root>(result.Result.ObjectParam);
        //        for (int i = 0; i < code.elements.Count; i++)
        //        {
        //            Element elmts = new Element();
        //            elmts.code = code.elements[i].code;
        //            elmt.Add(elmts);
        //            //dtinstcode.Rows.Add(code.elements[i].code);
        //        }
        //    }
        //    return elmt;
        //}

        [HttpGet]
        public void Retrigger()
        {
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
            _logger.LogInformation("Retrigger:-Saleorder/search Api called.");
            for (int i = 0; i < ds.Count; i++)
            {
                var jsoncodes = JsonConvert.SerializeObject(ds[i]);
                string codes = ds[i].code;
                var res = _MethodWrapper.RetriggerCode(jsoncodes, token, codes, 0);
                if (res == null)
                {
                    return;
                }
                else
                {
                    saleOrderItems.AddRange(res.saleOrderItems);
                    shipingdet.AddRange(res.Shipment);
                    qtyitems.AddRange(res.qtyitems);
                    address.AddRange(res.address);
                    elements.AddRange(res.elements);
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
                var resku = _MethodWrapper.RetriggerSkuCode(jskucode, token, code, skucode, 0);
                if (resku != null)
                {
                    itemTdto.Add(resku);
                }
                else
                {
                    return;
                }
            }
            var allsenddata = ObjBusinessLayer.GetAllRecrdstosend();
            var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);
            var postretrigger = _MethodWrapper.RetriggerPostDataDelivery(allsenddata, triggerid, 0);

        }
        [HttpPost]
        public void RetriggerPushData()
        {
            var allsenddata = ObjBusinessLayer.GetAllRecrdstosend();
            var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);
            //var resfinal = _Token.PostDataToDeliverypackList(allsenddata);
            //if (resfinal.Result.Errcode < 200 || resfinal.Result.Errcode > 299)
            //{
            //    ObjBusinessLayer.UpdatePostDatadetails(true, resfinal.Result.ObjectParam, triggerid);
            //}
            var postretrigger = _MethodWrapper.RetriggerPostDataDelivery(allsenddata, triggerid, 0);


        }

        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> GetErrorCodes()
        {
            var returndata = ObjBusinessLayer.GetErrorCodes();

            return returndata;
        }
        [HttpGet]
        public ServiceResponse<List<PostErrorDetails>> SendRecordStatus()
        {
            var status = ObjBusinessLayer.PostDataStatus();
            return status;
        }


        //Step-2
        [HttpGet]
        public IActionResult GetJWTToken()
        {
            //GenerateToken generateToken=new GenerateToken(null) ;
            var token = _jWTManager.GenerateJWTTokens();
            _logger.LogInformation($" log Object {JsonConvert.SerializeObject(token)}");
            try
            {
                if (token == null)
                {
                    _logger.LogInformation($" Error Object {JsonConvert.SerializeObject(token)}");
                    return Unauthorized();
                }
                _logger.LogInformation($" Debug Object {JsonConvert.SerializeObject(token)}");
            }
            catch (Exception ex) { _logger.LogInformation($" Error Object {JsonConvert.SerializeObject(ex)}"); }
            return Ok(token);
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
        public Task<ServiceResponse<string>> PostWaybillGeneration()
        {
            var sendwaybilldata = ObjBusinessLayer.GetWaybillAllRecrdstosend();

            var postres = _MethodWrapper.WaybillGenerationPostData(sendwaybilldata, 0);
            return postres;
        }


        [HttpPost]
        public void ReturnOrderAPI(string returnType = "CIR", string statusCode = "COMPLETE", string createdTo = "2023-07-11T14:20:40", string createdFrom = "2023-07-05T14:20:40")
        {
            RequestReturnOrder requestReturnOrder = new RequestReturnOrder();
            requestReturnOrder.returnType = returnType;
            requestReturnOrder.statusCode = statusCode;
            requestReturnOrder.createdTo = createdTo;
            requestReturnOrder.createdFrom = createdFrom;

            var json = JsonConvert.SerializeObject(requestReturnOrder);
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
                    if (list != null)
                    {
                        returnAddressDetailsLists.AddRange(list.returnAddressDetailsList);
                        returnSaleOrderItems.AddRange(list.returnSaleOrderItems);
                    }
                    else
                        return;

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
                    if (skudetails != null)
                    {
                        itemTdto.Add(skudetails);

                        //return status;
                    }
                    else
                        return;
                }
                ObjBusinessLayer.insertReturOrderItemtypes(itemTdto);
                var sendata = ObjBusinessLayer.GetReturnOrderSendData();
                var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata);
                var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0);
            }
            else
            {
                return;
            }

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
        public void STOWaybillGatePass(string fromDate= "2022-06-30T00:00:00", string toDate= "2022-07-02T00:00:00", string type= "STOCK_TRANSFER", string statusCode = "Return_awaited")
        {
            var jsonre = JsonConvert.SerializeObject(new { fromDate, toDate , type, statusCode });
            string token = HttpContext.Session.GetString("STOToken");
            List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
            List<Element> elements = new List<Element>();
            var res=_MethodWrapper.GatePass(jsonre, token,0);
            if (res.Count >= 0)
            {
                ObjBusinessLayer.insertGatePassCode(res);
                var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCode();
                for (int i = 0;i<GatePassCode.Count;i++)
                {
                    List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                    var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes= gatePassCodes });
                    var elemnetsList=_MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, 0);
                    if(elemnetsList != null)
                    {
                        gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                        elements.AddRange(elemnetsList.elements);
                    }
                    else
                    {
                        return;
                    }
                }
                ObjBusinessLayer.insertGatePassElements(elements);
                ObjBusinessLayer.insertItemTypeDTO(gatePassItemDTOs);
                var Skucodes = ObjBusinessLayer.GetWaybillSKUCode();
                List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                for (int k = 0; k<Skucodes.Count;k++)
                {
                    var skucode = JsonConvert.SerializeObject(new { skuCode= Skucodes[k].itemTypeSKU});
                    var code= Skucodes[k].code;
                    var Itemtypes=_MethodWrapper.GetSTOWaybillSkuDetails(skucode, token, code,0);
                    if (Itemtypes != null)
                    {
                        itemTypeDTO.Add(Itemtypes);
                    }
                    else
                        return;
                }
                ObjBusinessLayer.insertWaybillItemType(itemTypeDTO);
            }
            else
                return;
        }



    }



}
