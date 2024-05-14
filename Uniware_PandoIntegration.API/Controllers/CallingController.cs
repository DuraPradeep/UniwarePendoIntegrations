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
    public class CallingController : ControllerBase
    {
        private readonly ILogger<CallingController> _logger;
        private readonly IUniwarePando _jWTManager;
        private readonly string Configuration;
        private readonly IConfiguration iconfiguration;

        public CallingController(ILogger<CallingController> logger, IUniwarePando uniwarePando, IConfiguration configuration)
        {
            _logger = logger;
            _jWTManager = uniwarePando;
            iconfiguration = configuration;
        }
        BearerToken _Token = new BearerToken();
        private UniwareBL ObjBusinessLayer = new();
        MethodWrapper _MethodWrapper = new MethodWrapper();
        DelegateCalling obj = new DelegateCalling();

        [HttpGet]
        public IActionResult GetToken()
        {
            string Servertype = iconfiguration["ServerType:type"];
            string Instance = "SH";
            var resu = _Token.GetTokens(Servertype, Instance).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            if (resu.ObjectParam != null)
            {
                string token = deres.access_token.ToString();
                HttpContext.Session.SetString("Token", token);
                return Accepted(resu.ObjectParam);
            }
            else
            {
                return BadRequest("Something Went Wrong");
            }
        }
        [HttpGet]
        public IActionResult GetTokenDFX()
        {
            //BearerToken _Token = new BearerToken();
            //PandoUniwariToken resu = _Token.GetTokens().Result;
            string Servertype = iconfiguration["ServerType:type"];
            string Instance = "DFX";

            var resu = _Token.GetTokens(Servertype, Instance).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            if (resu.ObjectParam != null)
            {
                string token = deres.access_token.ToString();
                //HttpContext.Session.SetString("Token", resu.access_token.ToString());
                HttpContext.Session.SetString("DFXToken", token);
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
            string Servertype = iconfiguration["ServerType:type"];
            string Instance = "SH"; string token = HttpContext.Session.GetString("Token");
            //string token = deres.access_token.ToString();
            if (token != null)
            {
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, saleOrder/Get Api called.");
                var json = JsonConvert.SerializeObject(new { fromDate = fromdate, toDate = todate, dateType = datatype, status = status });
                //var list = getCode(json, token, 0);
                var list = _MethodWrapper.getCode(json, token, 0, Servertype, Instance);
                //var resCode = ObjBusinessLayer.InsertCode(elmt);
                if (list.Count > 0)
                {
                    var resCode = ObjBusinessLayer.InsertCode(list, Servertype);
                    var ds = ObjBusinessLayer.GetCode(Instance, Servertype);

                    ServiceResponse<parentList> parentList = new ServiceResponse<parentList>();
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Saleorder/search Api called.");
                    List<Address> address = new List<Address>();
                    List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
                    List<ShippingPackage> shipingdet = new List<ShippingPackage>();
                    List<Items> qtyitems = new List<Items>();
                    List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
                    for (int i = 0; i < ds.Count; i++)
                    {
                        //var jsoncodes = JsonConvert.SerializeObject(ds[i]);
                        var jsoncodes = JsonConvert.SerializeObject(new { code = ds[i].code });

                        string code = ds[i].code;
                        //parentList = PassCode(jsoncodes, token, code, 0);

                        parentList = _MethodWrapper.PassCodeer(jsoncodes, token, code, 0, Servertype, Instance);
                        if (parentList.IsSuccess)
                        {
                            if (parentList.ObjectParam.saleOrderItems.Count > 0 || parentList.ObjectParam.address.Count > 0 || parentList.ObjectParam.Shipment.Count > 0 || parentList.ObjectParam.qtyitems.Count > 0 || parentList.ObjectParam.elements.Count > 0)
                            {
                                saleOrderItems.AddRange(parentList.ObjectParam.saleOrderItems);
                                address.AddRange(parentList.ObjectParam.address);
                                shipingdet.AddRange(parentList.ObjectParam.Shipment);
                                qtyitems.AddRange(parentList.ObjectParam.qtyitems);
                                elements.AddRange(parentList.ObjectParam.elements);
                            }
                        }
                        else
                        {
                            //return BadRequest("INVALID_SALE_ORDER_CODE");
                            return BadRequest(parentList.Errdesc);
                        }

                    }
                    var sires = ObjBusinessLayer.insertsalesorderitem(saleOrderItems, Servertype);
                    var resshipng = ObjBusinessLayer.InsertBill(shipingdet, Servertype);
                    var resitems = ObjBusinessLayer.insertItems(qtyitems, Servertype);
                    var resads = ObjBusinessLayer.InsertAddrsss(address, Servertype);
                    var resdto = ObjBusinessLayer.insertSalesDTO(elements, Servertype);


                    var sku = ObjBusinessLayer.GetSKucodesBL(Instance, Servertype);
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, ItemType/Get Api called.");

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
                        var insertskucode = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                        if (insertskucode.Code != null)
                        {
                            itemTdto.Add(insertskucode);
                        }
                    }
                    var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto, Servertype);
                    var allsenddata = ObjBusinessLayer.GetAllRecrdstosend(Instance, Servertype);
                    var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata, Servertype, Instance);

                    //var sendcode = ObjBusinessLayer.GetAllSendData();
                    if (allsenddata.Count > 0)
                    {
                        var resutt = _MethodWrapper.Action(allsenddata, triggerid, 0, Servertype);
                        //return Accepted(resutt.ObjectParam);
                        if (resutt.IsSuccess)
                        {
                            return Accepted(resutt.ObjectParam);
                        }
                        else
                        {
                            return BadRequest(resutt.ObjectParam);

                        }
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

        [HttpPost]
        public IActionResult SaleOrderAPIDFX(string fromdate = "1695753048000", string todate = "1695835821000", string datatype = "CREATED", string status = "Processing")
        {
            string Servertype = iconfiguration["ServerType:type"];
            string Instance = "DFX";
            string token = HttpContext.Session.GetString("DFXToken");
            //string token = deres.access_token.ToString();
            if (token != null)
            {
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, DFX saleOrder/Get Api called.");
                var json = JsonConvert.SerializeObject(new { fromDate = fromdate, toDate = todate, dateType = datatype, status = status });
                //var list = getCode(json, token, 0);
                var list = _MethodWrapper.getCode(json, token, 0, Servertype, Instance);
                //var resCode = ObjBusinessLayer.InsertCode(elmt);
                if (list.Count > 0)
                {
                    var resCode = ObjBusinessLayer.InsertCode(list, Servertype);
                    var ds = ObjBusinessLayer.GetCode(Instance, Servertype);

                    ServiceResponse<parentList> parentList = new ServiceResponse<parentList>();
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, DFX Saleorder/search Api called.");
                    List<Address> address = new List<Address>();
                    List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
                    List<ShippingPackage> shipingdet = new List<ShippingPackage>();
                    List<Items> qtyitems = new List<Items>();
                    List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
                    for (int i = 0; i < ds.Count; i++)
                    {
                        //var jsoncodes = JsonConvert.SerializeObject(ds[i]);
                        var jsoncodes = JsonConvert.SerializeObject(new { code = ds[i].code });

                        string code = ds[i].code;
                        //parentList = PassCode(jsoncodes, token, code, 0);

                        parentList = _MethodWrapper.PassCodeer(jsoncodes, token, code, 0, Servertype, Instance);
                        if (parentList.IsSuccess)
                        {
                            if (parentList.ObjectParam.saleOrderItems.Count > 0 || parentList.ObjectParam.address.Count > 0 || parentList.ObjectParam.Shipment.Count > 0 || parentList.ObjectParam.qtyitems.Count > 0 || parentList.ObjectParam.elements.Count > 0)
                            {
                                saleOrderItems.AddRange(parentList.ObjectParam.saleOrderItems);
                                address.AddRange(parentList.ObjectParam.address);
                                shipingdet.AddRange(parentList.ObjectParam.Shipment);
                                qtyitems.AddRange(parentList.ObjectParam.qtyitems);
                                elements.AddRange(parentList.ObjectParam.elements);
                            }
                        }
                        else
                        {
                            //return BadRequest("INVALID_SALE_ORDER_CODE");
                            return BadRequest(parentList.Errdesc);
                        }

                    }
                    var sires = ObjBusinessLayer.insertsalesorderitem(saleOrderItems, Servertype);
                    var resshipng = ObjBusinessLayer.InsertBill(shipingdet, Servertype);
                    var resitems = ObjBusinessLayer.insertItems(qtyitems, Servertype);
                    var resads = ObjBusinessLayer.InsertAddrsss(address, Servertype);
                    var resdto = ObjBusinessLayer.insertSalesDTO(elements, Servertype);


                    var sku = ObjBusinessLayer.GetSKucodesBL(Instance, Servertype);
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, ItemType/Get Api called.");

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
                        var insertskucode = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                        if (insertskucode.Code != null)
                        {
                            itemTdto.Add(insertskucode);
                        }
                    }
                    var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto, Servertype);
                    var allsenddata = ObjBusinessLayer.GetAllRecrdstosend(Instance, Servertype);
                    var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata, Servertype, Instance);

                    var sendcode = ObjBusinessLayer.GetAllSendData(Servertype);
                    if (sendcode.Count > 0)
                    {
                        var resutt = _MethodWrapper.Action(sendcode, triggerid, 0, Servertype);
                        if (resutt.IsSuccess)
                        {
                            return Accepted(resutt.ObjectParam);
                        }
                        else
                        {
                            return BadRequest(resutt.ObjectParam);

                        }

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

        [HttpPost]
        public ActionResult Retrigger(UserProfile Enviornment)
        {
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Sale Order Retriggered ");
            string Servertype = Enviornment.Environment;
            var ds = ObjBusinessLayer.GetCodeforRetrigger(Servertype);
            string ExecResult = string.Empty;
            string Instance = ds[0].Instance;
            //string Instance = "SH";
            var resu = _Token.GetTokens(Servertype, Instance).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            if (resu.ObjectParam != null)
            {
                string token = deres.access_token.ToString();
                //string token = HttpContext.Session.GetString("Token");

                List<ErrorDetails> errorDetails = new List<ErrorDetails>();
                List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
                salesorderRoot details = new salesorderRoot();
                List<Address> address = new List<Address>();
                List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
                List<ShippingPackage> shipingdet = new List<ShippingPackage>();
                List<Items> qtyitems = new List<Items>();
                List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
                ServiceResponse<parentList> parentList = new ServiceResponse<parentList>();
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Retrigger:-Saleorder/search Api called.");
                for (int i = 0; i < ds.Count; i++)
                {
                    var jsoncodes = JsonConvert.SerializeObject(new { code = ds[i] });
                    string codes = ds[i].code;
                    //parentList = _MethodWrapper.RetriggerCode(jsoncodes, token, codes, 0);
                    parentList = _MethodWrapper.PassCodeer(jsoncodes, token, codes, 0, Servertype, Instance);
                    if (parentList.IsSuccess)
                    {
                        if (parentList.ObjectParam.saleOrderItems.Count > 0 || parentList.ObjectParam.address.Count > 0 || parentList.ObjectParam.Shipment.Count > 0 || parentList.ObjectParam.qtyitems.Count > 0 || parentList.ObjectParam.elements.Count > 0)
                        {
                            saleOrderItems.AddRange(parentList.ObjectParam.saleOrderItems);
                            shipingdet.AddRange(parentList.ObjectParam.Shipment);
                            qtyitems.AddRange(parentList.ObjectParam.qtyitems);
                            address.AddRange(parentList.ObjectParam.address);
                            elements.AddRange(parentList.ObjectParam.elements);
                        }
                    }
                    else
                    {
                        //return "INVALID_SALE_ORDER_CODE";
                        ExecResult += parentList.Errdesc;
                    }
                }

                var sires = ObjBusinessLayer.insertsalesorderitem(saleOrderItems, Servertype);
                var resshipng = ObjBusinessLayer.InsertBill(shipingdet, Servertype);
                var resitems = ObjBusinessLayer.insertItems(qtyitems, Servertype);
                var resads = ObjBusinessLayer.InsertAddrsss(address, Servertype);
                var resdto = ObjBusinessLayer.insertSalesDTO(elements, Servertype);

                var sku = ObjBusinessLayer.GetSKucodesForRetrigger(Servertype);
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Retrigger:-ItemType/Get Api called.");
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
                    var resku = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                    if (resku.Code != null)
                    {
                        itemTdto.Add(resku);
                    }
                    //else
                    //{
                    //return;
                    //}
                }
                var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto, Servertype);
                var allsenddata = ObjBusinessLayer.GetAllRecrdstosend(Instance, Servertype);
                if (allsenddata.Count > 0)
                {
                    var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata, Servertype, Instance);
                    var resutt = _MethodWrapper.Action(allsenddata, triggerid, 0, Servertype);
                    if (resutt.IsSuccess)
                    {
                        ExecResult += "Data Triggered Successfully";
                    }
                    else
                    {
                        ExecResult += "Get Some Issue Please Retrigger";
                    }
                }
                else
                {
                    ExecResult += "Get Some Issue Please Retrigger";
                }

            }
            else
            {
                ExecResult += "Please Pass valid Token";
            }
            return new JsonResult(ExecResult);
        }
        [HttpPost]
        public ActionResult RetriggerPushData(UserProfile Enviornment)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment.Environment;
            string ExecResult = string.Empty;

            var Instances = ObjBusinessLayer.GetInstanceFromTriggerdata(Servertype);
            for (int i = 0; i < Instances.Count; i++)
            {
                string Instance = Instances[i].Instance;
                var Failedtriggerid = Instances[i].TriggerId;
                //var allsenddata = ObjBusinessLayer.GetFailedSendRecords(Instance,Servertype);
                var allsenddata = ObjBusinessLayer.GetFailedSendRecords(Instance, Servertype);
                var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata, Servertype, Instance);

                //var allsenddata = ObjBusinessLayer.GetAllRecrdstosend(Instance);
                //var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata);
                //string Servertype = iconfiguration["ServerType:type"];

                //var postretrigger = _MethodWrapper.RetriggerPostDataDelivery(allsenddata, triggerid, 0);
                var postretrigger = _MethodWrapper.Action(allsenddata, triggerid, 0, Servertype);
                if (postretrigger.IsSuccess)
                {
                    ExecResult += "Data Triggered Successfully";
                    ObjBusinessLayer.UpdateStatusinTriggerTable(Failedtriggerid, Servertype);
                }
                else
                {
                    ExecResult += "Get Some Issue Please Retrigger";
                }
            }
            return new JsonResult(ExecResult);


        }

        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> GetSaleOrderErrorCodes(string Enviornment)
        {
            string Servertype = Enviornment;
            //string Servertype = iconfiguration["ServerType:type"];

            var returndata = ObjBusinessLayer.GetErrorCodes(Servertype);

            return returndata;
        }

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

        [HttpPost]
        public IActionResult ReturnOrderAPI(string returnType = "CIR", string statusCode = "COMPLETE", string createdTo = "2023-07-11T14:20:40", string createdFrom = "2023-07-05T14:20:40")
        {
            string Servertype = iconfiguration["ServerType:type"];

            //            
            var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);
            string Instance = "SH";

            string token = HttpContext.Session.GetString("Token");
            if (token != null)
            {
                var json = JsonConvert.SerializeObject(new { returnType, statusCode, createdTo, createdFrom });
                //string token = HttpContext.Session.GetString("Token");
                foreach (var FacilityCode in Facilities)
                {

                    var resuordercode = _MethodWrapper.GetReturnorderCode(json, token, 0, Servertype, FacilityCode.facilityCode, Instance);
                    if (resuordercode.Count > 0)
                    {
                        ObjBusinessLayer.insertReturnOrdercoder(resuordercode, FacilityCode.facilityCode, Servertype);
                        var codes = ObjBusinessLayer.GetReturnOrderCodes(Instance, Servertype);
                        List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
                        List<ReturnSaleOrderItem> returnSaleOrderItems = new List<ReturnSaleOrderItem>();
                        List<ReturnAddressDetailsList> returnAddressDetailsLists = new List<ReturnAddressDetailsList>();
                        for (int j = 0; j < codes.ObjectParam.Count; j++)
                        {
                            //ReturnOrderGet returnOrderGet = new ReturnOrderGet();
                            //returnOrderGet.reversePickupCode = codes.ObjectParam[j].code;
                            //var jdetail = JsonConvert.SerializeObject(returnOrderGet);
                            var jdetail = JsonConvert.SerializeObject(new { reversePickupCode = codes.ObjectParam[j].code });
                            var Code = codes.ObjectParam[j].code;
                            var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (list.returnAddressDetailsList.Count > 0 || list.returnSaleOrderItems.Count > 0)
                            {
                                returnAddressDetailsLists.AddRange(list.returnAddressDetailsList);
                                returnSaleOrderItems.AddRange(list.returnSaleOrderItems);
                            }
                            //else
                            //    return BadRequest("Please Retrigger");

                        }
                        ObjBusinessLayer.insertReturnSaleOrderitem(returnSaleOrderItems, Servertype);
                        ObjBusinessLayer.insertReturnaddress(returnAddressDetailsLists, Servertype);
                        var skucodes = ObjBusinessLayer.GetReturnOrderSkuCodes(Servertype);

                        List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                        List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();

                        for (int i = 0; i < skucodes.ObjectParam.Count; i++)
                        {
                            skucode sKucodes = new skucode();
                            sKucodes.skuCode = skucodes.ObjectParam[i].skuCode;
                            var code = skucodes.ObjectParam[i].Code;
                            var skucode = skucodes.ObjectParam[i].skuCode;
                            var jskucode = JsonConvert.SerializeObject(sKucodes);

                            var skudetails = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                            if (skudetails.Code != null)
                            {
                                itemTdto.Add(skudetails);
                            }
                        }
                        ObjBusinessLayer.insertReturOrderItemtypes(itemTdto, Servertype);
                        var sendata = ObjBusinessLayer.GetReturnOrderSendData(Instance, Servertype);
                        if (sendata.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata, Servertype);
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
        [HttpPost]
        public IActionResult ReturnOrderAPIDFX(string returnType = "CIR", string statusCode = "COMPLETE", string createdTo = "2023-07-11T14:20:40", string createdFrom = "2023-07-05T14:20:40")
        {
            string Servertype = iconfiguration["ServerType:type"];

            var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);

            string Instance = "DFX";

            string token = HttpContext.Session.GetString("DFXToken");
            if (token != null)
            {
                var json = JsonConvert.SerializeObject(new { returnType, statusCode, createdTo, createdFrom });
                foreach (var FacilityCode in Facilities)
                {

                    var resuordercode = _MethodWrapper.GetReturnorderCode(json, token, 0, Servertype, FacilityCode.facilityCode, Instance);
                    if (resuordercode.Count > 0)
                    {
                        ObjBusinessLayer.insertReturnOrdercoder(resuordercode, FacilityCode.facilityCode, Servertype);
                        var codes = ObjBusinessLayer.GetReturnOrderCodes(Instance, Servertype);
                        List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
                        List<ReturnSaleOrderItem> returnSaleOrderItems = new List<ReturnSaleOrderItem>();
                        List<ReturnAddressDetailsList> returnAddressDetailsLists = new List<ReturnAddressDetailsList>();
                        for (int j = 0; j < codes.ObjectParam.Count; j++)
                        {
                            var jdetail = JsonConvert.SerializeObject(new { reversePickupCode = codes.ObjectParam[j].code });

                            var Code = codes.ObjectParam[j].code;
                            var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (list.returnAddressDetailsList.Count > 0 || list.returnSaleOrderItems.Count > 0)
                            {
                                returnAddressDetailsLists.AddRange(list.returnAddressDetailsList);
                                returnSaleOrderItems.AddRange(list.returnSaleOrderItems);
                            }

                        }
                        ObjBusinessLayer.insertReturnSaleOrderitem(returnSaleOrderItems, Servertype);
                        ObjBusinessLayer.insertReturnaddress(returnAddressDetailsLists, Servertype);
                        var skucodes = ObjBusinessLayer.GetReturnOrderSkuCodes(Servertype);

                        List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                        List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();

                        for (int i = 0; i < skucodes.ObjectParam.Count; i++)
                        {
                            skucode sKucodes = new skucode();
                            sKucodes.skuCode = skucodes.ObjectParam[i].skuCode;
                            var code = skucodes.ObjectParam[i].Code;
                            var skucode = skucodes.ObjectParam[i].skuCode;
                            var jskucode = JsonConvert.SerializeObject(sKucodes);

                            var skudetails = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                            if (skudetails.Code != null)
                            {
                                itemTdto.Add(skudetails);
                            }
                        }
                        ObjBusinessLayer.insertReturOrderItemtypes(itemTdto, Servertype);
                        var sendata = ObjBusinessLayer.GetReturnOrderSendData(Instance, Servertype);
                        if (sendata.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata, Servertype);
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


        }
        [HttpGet]
        public string ReturnOrderAPIRetrigger(UserProfile Enviornment)
        {
            Log.Information($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Retriggered Return Order API");
            string Servertype = Enviornment.Environment;
            //string Servertype = iconfiguration["ServerType:type"];
            string Instance = "SH";
            var resu = _Token.GetTokens(Servertype, Instance).Result;

            var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);

            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            if (deres != null)
            {
                foreach (var FacilityCode in Facilities)
                {
                    string token = deres.access_token.ToString();
                    var codes = ObjBusinessLayer.GetReturnOrderCodesForRetrigger(Servertype);
                    List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
                    List<ReturnSaleOrderItem> returnSaleOrderItems = new List<ReturnSaleOrderItem>();
                    List<ReturnAddressDetailsList> returnAddressDetailsLists = new List<ReturnAddressDetailsList>();
                    for (int j = 0; j < codes.ObjectParam.Count; j++)
                    {
                        ReturnOrderGet returnOrderGet = new ReturnOrderGet();
                        returnOrderGet.reversePickupCode = codes.ObjectParam[j].code;
                        var jdetail = JsonConvert.SerializeObject(returnOrderGet);
                        var Code = codes.ObjectParam[j].code;
                        var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0, Servertype, FacilityCode.facilityCode, Instance);
                        if (list.returnAddressDetailsList.Count > 0 || list.returnSaleOrderItems.Count > 0)
                        {
                            returnAddressDetailsLists.AddRange(list.returnAddressDetailsList);
                            returnSaleOrderItems.AddRange(list.returnSaleOrderItems);
                        }
                        //else
                        //    return BadRequest("Please Retrigger");

                    }
                    ObjBusinessLayer.insertReturnSaleOrderitem(returnSaleOrderItems, Servertype);
                    ObjBusinessLayer.insertReturnaddress(returnAddressDetailsLists, Servertype);
                    var skucodes = ObjBusinessLayer.GetReturnOrderSkuCodes(Servertype);

                    List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                    List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();

                    for (int i = 0; i < skucodes.ObjectParam.Count; i++)
                    {
                        skucode sKucodes = new skucode();
                        sKucodes.skuCode = skucodes.ObjectParam[i].skuCode;
                        var code = skucodes.ObjectParam[i].Code;
                        var skucode = skucodes.ObjectParam[i].skuCode;

                        var jskucode = JsonConvert.SerializeObject(sKucodes);

                        var skudetails = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                        if (skudetails.Code != null)
                        {
                            itemTdto.Add(skudetails);

                            //return status;
                        }
                        //else
                        //    return BadRequest("Please Retrigger");
                    }
                    ObjBusinessLayer.insertReturOrderItemtypes(itemTdto, Servertype);
                    var sendata = ObjBusinessLayer.GetReturnOrderSendData(Instance, Servertype);
                    if (sendata.ObjectParam.Count > 0)
                    {
                        var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata, Servertype);
                        var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0, Servertype);
                        //return Accepted(status.Result.ObjectParam);
                        return "Failed Data Triggered Successfully";
                    }
                    else return "Get Some Issue Please Retrigger";
                }
            }

            return "Please Pass valid Token";


        }
        [HttpGet]
        public IActionResult ReturnorderFinalData(UserProfile Enviornment)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment.Environment;
            string Instance = "SH";
            var sendata = ObjBusinessLayer.GetReturnOrderSendData(Instance, Servertype);
            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata, Servertype);
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
            string Instance = "SH";

            string Servertype = iconfiguration["ServerType:type"];
            var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);

            if (token != null)
            {
                foreach (var FacilityCode in Facilities)
                {
                    var jsonre = JsonConvert.SerializeObject(new { fromDate, toDate, type, statusCode });
                    Log.Information($"DateTime:-  {DateTime.Now.ToLongTimeString()}, STO WaybillGetPass Code" + jsonre + ": " + token);
                    List<GatePassItemDTODb> gatePassItemDTOs = new List<GatePassItemDTODb>();
                    List<Elementdb> elements = new List<Elementdb>();
                    List<CustomFieldValuedb> customFieldDbs = new List<CustomFieldValuedb>();
                    var res = _MethodWrapper.GatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode, Instance);
                    if (res.Count > 0)
                    {
                        ObjBusinessLayer.insertGatePassCode(res, FacilityCode.facilityCode, Instance, Servertype);
                        var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCode(Servertype);
                        for (int i = 0; i < GatePassCode.Count; i++)
                        {
                            string code = GatePassCode[i].code;
                            List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                            var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                            var elemnetsList = _MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                            {
                                gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                                elements.AddRange(elemnetsList.elements);
                                if (elemnetsList.customFieldDbs.Count > 0)
                                {
                                    customFieldDbs.AddRange(elemnetsList.customFieldDbs);
                                }
                            }
                        }
                        ObjBusinessLayer.insertGatePassElements(elements, Servertype);
                        ObjBusinessLayer.insertItemTypeDTO(gatePassItemDTOs, Servertype);
                        ObjBusinessLayer.STOWaybillCustField(customFieldDbs, Servertype);
                        var Skucodes = ObjBusinessLayer.GetWaybillSKUCode(Servertype);
                        List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                        for (int k = 0; k < Skucodes.Count; k++)
                        {
                            string itemsku = Skucodes[k].itemTypeSKU;
                            var skucode = JsonConvert.SerializeObject(new { skuCode = Skucodes[k].itemTypeSKU });
                            var code = Skucodes[k].code;
                            var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, itemsku, 0, Servertype, Instance);
                            if (Itemtypes.Code != null)
                            {
                                itemTypeDTO.Add(Itemtypes);
                            }
                        }
                        ObjBusinessLayer.insertWaybillItemType(itemTypeDTO, Servertype);
                        var Records = ObjBusinessLayer.GetAllWaybillSTOPost(Servertype);
                        if (Records.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records, Servertype);
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

        [HttpPost]
        public IActionResult STOWaybillDFX(string fromDate = "2023-10-06T06:00:00", string toDate = "2023-10-06T06:30:00", string type = "STOCK_TRANSFER", string statusCode = "Return_awaited")
        {
            string token = HttpContext.Session.GetString("Token");
            string Instance = "DFX";

            string Servertype = iconfiguration["ServerType:type"];
            var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);

            if (token != null)
            {
                foreach (var FacilityCode in Facilities)
                {
                    var jsonre = JsonConvert.SerializeObject(new { fromDate, toDate, type, statusCode });
                    Log.Information($" DateTime:-  {DateTime.Now.ToLongTimeString()}, STO WaybillGetPass Code" + jsonre + ": " + token);
                    List<GatePassItemDTODb> gatePassItemDTOs = new List<GatePassItemDTODb>();
                    List<Elementdb> elements = new List<Elementdb>();
                    List<CustomFieldValuedb> customFieldDbs = new List<CustomFieldValuedb>();
                    var res = _MethodWrapper.GatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode, Instance);
                    if (res.Count > 0)
                    {
                        ObjBusinessLayer.insertGatePassCode(res, FacilityCode.facilityCode, Instance, Servertype);
                        var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCode(Servertype);
                        for (int i = 0; i < GatePassCode.Count; i++)
                        {
                            string code = GatePassCode[i].code;
                            List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                            var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                            var elemnetsList = _MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                            {
                                gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                                elements.AddRange(elemnetsList.elements);
                                if (elemnetsList.customFieldDbs.Count > 0)
                                {
                                    customFieldDbs.AddRange(elemnetsList.customFieldDbs);
                                }
                            }
                        }
                        ObjBusinessLayer.insertGatePassElements(elements, Servertype);
                        ObjBusinessLayer.insertItemTypeDTO(gatePassItemDTOs, Servertype);
                        ObjBusinessLayer.STOWaybillCustField(customFieldDbs, Servertype);
                        var Skucodes = ObjBusinessLayer.GetWaybillSKUCode(Servertype);
                        List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                        for (int k = 0; k < Skucodes.Count; k++)
                        {
                            string itemsku = Skucodes[k].itemTypeSKU;
                            var skucode = JsonConvert.SerializeObject(new { skuCode = Skucodes[k].itemTypeSKU });
                            var code = Skucodes[k].code;
                            var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, itemsku, 0, Servertype, Instance);
                            if (Itemtypes.Code != null)
                            {
                                itemTypeDTO.Add(Itemtypes);
                            }
                        }
                        ObjBusinessLayer.insertWaybillItemType(itemTypeDTO, Servertype);
                        var Records = ObjBusinessLayer.GetAllWaybillSTOPost(Servertype);
                        if (Records.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records, Servertype);
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
        public string STOWaybillRetrigger(UserProfile Enviornment)
        {
            Log.Information($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Retrigger STOwaybill api");
            string Servertype = Enviornment.Environment;
            //string Servertype = iconfiguration["ServerType:type"];

            //BearerToken _Token = new BearerToken();
            string Instance = "SH";
            var resu = _Token.GetTokens(Servertype, Instance).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            //HttpContext.Session.SetString("STOToken", deres.access_token.ToString());
            //string token = HttpContext.Session.GetString("STOToken");
            if (deres.token_type != null)
            {

                var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);

                foreach (var FacilityCode in Facilities)
                {
                    string token = deres.access_token.ToString();
                    List<GatePassItemDTODb> gatePassItemDTOs = new List<GatePassItemDTODb>();
                    List<Elementdb> elements = new List<Elementdb>();

                    var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCodeForretrigger(Servertype);
                    for (int i = 0; i < GatePassCode.Count; i++)
                    {
                        string code = GatePassCode[i].code;
                        List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                        var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                        var elemnetsList = _MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
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
                    ObjBusinessLayer.insertGatePassElements(elements, Servertype);
                    ObjBusinessLayer.insertItemTypeDTO(gatePassItemDTOs, Servertype);
                    var Skucodes = ObjBusinessLayer.GetWaybillSKUCode(Servertype);
                    List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                    for (int k = 0; k < Skucodes.Count; k++)
                    {
                        string itemsku = Skucodes[k].itemTypeSKU;
                        var skucode = JsonConvert.SerializeObject(new { skuCode = Skucodes[k].itemTypeSKU });
                        var code = Skucodes[k].code;
                        var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, itemsku, 0, Servertype, Instance);
                        if (Itemtypes.Code != null)
                        {
                            itemTypeDTO.Add(Itemtypes);
                        }
                        //else
                        //    return BadRequest("Please Retrigger");
                    }
                    ObjBusinessLayer.insertWaybillItemType(itemTypeDTO, Servertype);
                    var Records = ObjBusinessLayer.GetAllWaybillSTOPost(Servertype);
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
        public IActionResult STOwaybillFinalData(UserProfile Enviornment)
        {
            string Servertype = Enviornment.Environment;
            //string Servertype = iconfiguration["ServerType:type"];

            var Records = ObjBusinessLayer.GetAllWaybillSTOPost(Servertype);
            var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records, Servertype);
            var status = _MethodWrapper.WaybillSTOPostData(Records, triggerid, 0, Servertype);
            return Accepted(status.Result.ObjectParam);
        }

        [HttpPost]
        public IActionResult STOAPI(string fromDate = "2023-10-06T00:00:00", string toDate = "2023-10-06T11:40:00", string type = "STOCK_TRANSFER", string statusCode = "created")
        {
            string Instance = "SH";

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
                var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);
                var jsonre = JsonConvert.SerializeObject(new { fromDate, toDate, type, statusCode });

                List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                List<Element> elements = new List<Element>();
                Log.Information($"DateTime:-  {DateTime.Now.ToLongTimeString()}, STO API Called : " + jsonre + " " + token);
                foreach (var FacilityCode in Facilities)
                {
                    //var res = _MethodWrapper.STOAPIGatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode, Instance);
                    var res = _MethodWrapper.GatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode, Instance);
                    if (res.Count > 0)
                    {

                        ObjBusinessLayer.insertSTOAPIGatePassCode(res, FacilityCode.facilityCode, Servertype);
                        var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCode(Servertype);
                        for (int i = 0; i < gatePass.Count; i++)
                        {
                            string code = gatePass[i].code;
                            List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                            var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                            var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                            {
                                gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                                elements.AddRange(elemnetsList.elements);
                            }
                        }
                        ObjBusinessLayer.insertSTOAPiGatePassElements(elements, Servertype);
                        ObjBusinessLayer.insertSTOAPiItemTypeDTO(gatePassItemDTOs, Servertype);
                        var skuitemtype = ObjBusinessLayer.GetSTOSKUCode(Servertype);
                        List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                        for (int k = 0; k < skuitemtype.Count; k++)
                        {
                            var skucode = JsonConvert.SerializeObject(new { skuCode = skuitemtype[k].itemTypeSKU });
                            var code = skuitemtype[k].code;
                            var skutype = skuitemtype[k].itemTypeSKU;
                            var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, skutype, 0, Servertype, Instance);
                            if (Itemtypes.Code != null)
                            {
                                itemTypeDTO.Add(Itemtypes);
                            }
                            //else
                            //    return BadRequest("Please Retrigger");
                        }
                        ObjBusinessLayer.insertSTOAPItemType(itemTypeDTO, Servertype);
                        var allrecords = ObjBusinessLayer.GetSTOAPISendData(Instance, Servertype);
                        if (allrecords.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords, Servertype);
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

        [HttpPost]
        public IActionResult STOAPIDFX(string fromDate = "2023-10-06T00:00:00", string toDate = "2023-10-06T11:40:00", string type = "STOCK_TRANSFER", string statusCode = "created")
        {
            string Instance = "DFX";

            string token = HttpContext.Session.GetString("Token");
            string Servertype = iconfiguration["ServerType:type"];

            if (!string.IsNullOrEmpty(token))
            {
                var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);
                var jsonre = JsonConvert.SerializeObject(new { fromDate, toDate, type, statusCode });

                List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                List<Element> elements = new List<Element>();
                Log.Information($"DateTime:-  {DateTime.Now.ToLongTimeString()}, STO API Called : " + jsonre + " " + token);
                foreach (var FacilityCode in Facilities)
                {
                    //var res = _MethodWrapper.STOAPIGatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode, Instance);
                    var res = _MethodWrapper.GatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode, Instance);
                    if (res.Count > 0)
                    {

                        ObjBusinessLayer.insertSTOAPIGatePassCode(res, FacilityCode.facilityCode, Servertype);
                        var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCode(Servertype);
                        for (int i = 0; i < gatePass.Count; i++)
                        {
                            string code = gatePass[i].code;
                            List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                            var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                            var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                            {
                                gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                                elements.AddRange(elemnetsList.elements);
                            }
                        }
                        ObjBusinessLayer.insertSTOAPiGatePassElements(elements, Servertype);
                        ObjBusinessLayer.insertSTOAPiItemTypeDTO(gatePassItemDTOs, Servertype);
                        var skuitemtype = ObjBusinessLayer.GetSTOSKUCode(Servertype);
                        List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                        for (int k = 0; k < skuitemtype.Count; k++)
                        {
                            var skucode = JsonConvert.SerializeObject(new { skuCode = skuitemtype[k].itemTypeSKU });
                            var code = skuitemtype[k].code;
                            var skutype = skuitemtype[k].itemTypeSKU;
                            var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, skutype, 0, Servertype, Instance);
                            if (Itemtypes.Code != null)
                            {
                                itemTypeDTO.Add(Itemtypes);
                            }
                            //else
                            //    return BadRequest("Please Retrigger");
                        }
                        ObjBusinessLayer.insertSTOAPItemType(itemTypeDTO, Servertype);
                        var allrecords = ObjBusinessLayer.GetSTOAPISendData(Instance, Servertype);
                        if (allrecords.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords, Servertype);
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
        public string STOAPIRetrigger(UserProfile Enviornment)
        {
            Log.Information($"DateTime:-  {DateTime.Now.ToLongTimeString()}, STOAPI Retriggered");
            string Servertype = Enviornment.Environment;
            //string Servertype = iconfiguration["ServerType:type"];
            var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);

            //var resu = _Token.GetTokens(Servertype).Result;
            string Instance = "SH";
            var resu = _Token.GetTokens(Servertype, Instance).Result;
            var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            if (deres.token_type != null)
            {
                foreach (var FacilityCode in Facilities)
                {
                    string token = deres.access_token.ToString();
                    List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                    List<Element> elements = new List<Element>();
                    var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCodeRetrigger(Servertype);
                    for (int i = 0; i < gatePass.Count; i++)
                    {
                        string code = gatePass[i].code;
                        List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                        var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                        var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
                        if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                        {
                            gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                            elements.AddRange(elemnetsList.elements);
                        }

                    }
                    ObjBusinessLayer.insertSTOAPiGatePassElements(elements, Servertype);
                    ObjBusinessLayer.insertSTOAPiItemTypeDTO(gatePassItemDTOs, Servertype);
                    var skuitemtype = ObjBusinessLayer.GetSTOSKUCode(Servertype);
                    List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                    for (int k = 0; k < skuitemtype.Count; k++)
                    {
                        var skucode = JsonConvert.SerializeObject(new { skuCode = skuitemtype[k].itemTypeSKU });
                        var code = skuitemtype[k].code;
                        var skutype = skuitemtype[k].itemTypeSKU;
                        var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, skutype, 0, Servertype, Instance);
                        if (Itemtypes.Code != null)
                        {
                            itemTypeDTO.Add(Itemtypes);
                        }
                        //else
                        //    return BadRequest("Please Retrigger");
                    }
                    ObjBusinessLayer.insertSTOAPItemType(itemTypeDTO, Servertype);
                    var allrecords = ObjBusinessLayer.GetSTOAPISendData(Instance, Servertype);
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
        public IActionResult STOAPIFinaldata(UserProfile Enviornment)
        {
            string Servertype = Enviornment.Environment;
            //string Servertype = iconfiguration["ServerType:type"];
            string Instance = "SH";
            var allrecords = ObjBusinessLayer.GetSTOAPISendData(Instance, Servertype);
            var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords, Servertype);
            var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0, Servertype);
            return Accepted(status.Result.ObjectParam);
        }

        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> STOApiErrorDetails(string Enviornment)
        {
            string Servertype = Enviornment;
            //string Servertype = iconfiguration["ServerType:type"];
            var returndata = ObjBusinessLayer.BLSTOAPI(Servertype);
            return returndata;
        }
        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> STOWaybillErrorDetails(string Enviornment)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment;

            var returndata = ObjBusinessLayer.BLSTOWaybil(Servertype);
            return returndata;
        }
        [HttpGet]
        public ServiceResponse<List<EndpointErrorDetails>> waybillErrorDetails(string Enviornment)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment;

            var returndata = ObjBusinessLayer.BLWaybilStatus(Servertype);
            return returndata;
        }
        [HttpPost]
        public ActionResult Retriggerwaybill(UserProfile Enviornment)
        {
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, WayBill response Retrigger FinalData");

            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment.Environment;
            string ExecResult = string.Empty;

            var Instance = ObjBusinessLayer.GetWaybillInstanceName(Servertype);
            //var Instances = ObjBusinessLayer.GetInstanceFromTriggerdata(Servertype);
            for (int i = 0; i < Instance.Count; i++)
            {
                var failedtriggerid = Instance[i].TriggerId;
                var sendfaileddata = ObjBusinessLayer.GetWaybillAllFailedRecrdsto(Instance[i].Instance, Servertype);
                ErrorResponse errorResponse = new ErrorResponse();


                if (sendfaileddata.Count > 0)
                {
                    var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendfaileddata, Servertype, Instance[i].Instance);
                    var postres = _MethodWrapper.WaybillGenerationPostData(sendfaileddata, 0, triggerid, Servertype);
                    if (postres.IsSuccess)
                    {
                        ExecResult += "Successfully Sended Failed Record";
                        ObjBusinessLayer.UpdateStatusinWaybillTriggerTable(failedtriggerid, Servertype);

                        _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, WayBill Retrigger Response {JsonConvert.SerializeObject(postres.ObjectParam)}");
                    }
                    else
                    {
                        errorResponse.status = "FAILED";
                        errorResponse.reason = postres.ObjectParam;
                        errorResponse.message = "Resource requires authentication. Please check your authorization token.";
                        _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: {JsonConvert.SerializeObject(errorResponse)}");
                        ExecResult += postres.ObjectParam;
                    }
                }
                else
                {
                    ExecResult += "There is Some Issue in Retriger";
                }
            }
            return new JsonResult(ExecResult);
        }
        [HttpGet]
        public ServiceResponse<List<CodesErrorDetails>> ReturnOrderDetails(string Enviornment)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment;

            var returndata = ObjBusinessLayer.BLReturnOrderStatus(Servertype);
            return returndata;
        }
        //[ServiceFilter(typeof(ActionFilterExample))]
       
        [HttpPost]
        public IActionResult PostUpdateShipingData()
        {
            //string token = HttpContext.Session.GetString("Token");
            string Servertype = iconfiguration["ServerType:type"];

            //if (token != null)
            //{
            var lists = ObjBusinessLayer.UpdateShipingPck(Servertype);
            var triggerid = ObjBusinessLayer.UpdateShippingDataPost(lists, Servertype);

            //var facilitycode = "";
            if (lists.Count > 0)
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    string Instance = string.Empty;

                    UpdateShippingpackage updateShippingpackage = new UpdateShippingpackage();
                    //updateShippingpackage.shippingBox = new ShippingBox();
                    updateShippingpackage.customFieldValues = new List<CustomFieldValue>();
                    updateShippingpackage.shippingPackageCode = lists[i].shippingPackageCode;
                    //updateShippingpackage.shippingProviderCode = lists[i].shippingProviderCode;
                    //updateShippingpackage.trackingNumber = lists[i].trackingNumber;
                    //updateShippingpackage.shippingPackageTypeCode = lists[i].shippingPackageTypeCode;
                    //updateShippingpackage.actualWeight = lists[i].actualWeight;
                    //updateShippingpackage.noOfBoxes = lists[i].noOfBoxes;
                    //updateShippingpackage.shippingBox.length = lists[i].shippingBox.length;
                    //updateShippingpackage.shippingBox.width = lists[i].shippingBox.width;
                    //updateShippingpackage.shippingBox.height = lists[i].shippingBox.height;
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
                        if (lists[i].customFieldValues[k].name == "INDENTID_SH")
                        {
                            Instance = "SH";
                        }
                        else if (lists[i].customFieldValues[k].name == "INDENTID_DFX")
                        {
                            Instance = "DFX";
                        }

                    }
                    //var triggerid = ObjBusinessLayer.UpdateShippingDataPost(updateShippingpackage, facilitycode, Servertype);
                    //var response = _Token.PostUpdateShippingpckg(updateShippingpackage);
                    var resu = _Token.GetTokens(Servertype, Instance).Result;
                    var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                    string token = accesstoken.access_token;
                    if (token != null)
                    {
                        var response = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, updateShippingpackage.shippingPackageCode, token, facilitycode, Servertype, Instance);

                        //return Accepted(response.Result);
                    }

                }
                return Accepted("All Records Pushed Successfully");

            }
            else return BadRequest("There is no record for Post");

        }
        [HttpGet]
        public ServiceResponse<List<EndpointErrorDetails>> UpdateShippingErrorDetails(string Enviornment)
        {
            string Servertype = Enviornment;
            //string Servertype = iconfiguration["ServerType:type"];

            var returndata = ObjBusinessLayer.BLUpdateShippingStatus(Servertype);
            return returndata;
        }
        [HttpPost]
        public IActionResult RetriggerUpdateShipping(UserProfile Enviornment)
        {
            _logger.LogInformation($"DateTime:- {DateTime.Now.ToLongTimeString()}, UpdateShippingPackage Retrigger");
            string Servertype = Enviornment.Environment;
            //string Servertype = iconfiguration["ServerType:type"];
            string ExecResult = string.Empty;
            //var Token = _Token.GetTokens(Servertype).Result;
            SuccessResponse successResponse = new SuccessResponse();

            var lists = ObjBusinessLayer.UpdateShipingPckRetrigger(Servertype);
            var triggerid = ObjBusinessLayer.UpdateShippingDataPost(lists, Servertype);

            if (lists.Count > 0)
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    string Instance = string.Empty;
                    UpdateShippingpackage updateShippingpackage = new UpdateShippingpackage();
                    updateShippingpackage.customFieldValues = new List<CustomFieldValue>();
                    updateShippingpackage.shippingPackageCode = lists[i].shippingPackageCode;
                    var Shippingpackcode = lists[i].shippingPackageCode;
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
                    //var triggerid = ObjBusinessLayer.UpdateShippingDataPost(updateShippingpackage, facilitycode, Servertype);
                    var resu = _Token.GetTokens(Servertype, Instance).Result;
                    var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                    string token = accesstoken.access_token;
                    if (token != null)
                    {
                        //var response = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, triggerid, token, facilitycode, Servertype, Instance);
                        var response = _MethodWrapper.UpdateShippingPackagePostData(updateShippingpackage, 0, updateShippingpackage.shippingPackageCode, token, facilitycode, Servertype, Instance);

                        if (response.IsSuccess)
                        {
                            ExecResult += "Data Triggered Successfully";
                            ObjBusinessLayer.UpdateStatusinUpdateshippingTriggerTable(Shippingpackcode, Servertype);

                            _logger.LogInformation($"DateTime:- {DateTime.Now.ToLongTimeString()}, Retrigger UpdateShippingPackage response {JsonConvert.SerializeObject(ExecResult)}");
                        }
                        else
                        {
                            ExecResult += response.ObjectParam;

                            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()} , Error: {JsonConvert.SerializeObject(ExecResult)}");
                            //return new JsonResult(ExecResult);
                        }
                    }
                }
                //return Accepted("All Records Pushed Successfully");

            }
            //else return BadRequest("There is no record for Post");
            return new JsonResult(ExecResult);


        }
        //[ServiceFilter(typeof(ActionFilterExample))]
       

        //[HttpPost]
        //public IActionResult PostAllocateShipping()
        //{
        //    string Servertype = iconfiguration["ServerType:type"];
        //    SuccessResponse successResponse = new SuccessResponse();
        //    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Post Data Allocate Shipping");
        //    //string token = HttpContext.Session.GetString("STOToken");
        //    //var Token = _Token.GetTokens(Servertype).Result;
        //    string Instance = string.Empty;
        //    //var Token = _Token.GetTokens(Servertype, Instance).Result;
        //    //var _Tokens = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(Token.ObjectParam);

        //    var results = ObjBusinessLayer.PostGAllocateShippingData(Servertype);
        //    if (results.Count > 0)
        //    {
        //        for (int i = 0; i < results.Count; i++)
        //        {
        //            Allocateshipping allocateshipping = new Allocateshipping();
        //            allocateshipping.shippingPackageCode = results[i].shippingPackageCode;
        //            allocateshipping.shippingLabelMandatory = results[i].shippingLabelMandatory;
        //            allocateshipping.shippingProviderCode = results[i].shippingProviderCode;
        //            allocateshipping.shippingCourier = results[i].shippingCourier;
        //            allocateshipping.trackingNumber = results[i].trackingNumber;
        //            allocateshipping.trackingLink = results[i].trackingLink;
        //            //allocateshipping.generateUniwareShippingLabel = results[i].generateUniwareShippingLabel;
        //            var reference = results[i].generateUniwareShippingLabel;
        //            if (reference == "INDENTID_DFX")
        //            {
        //                Instance = "DFX";
        //            }
        //            else
        //            {
        //                Instance = "SH";
        //            }
        //            var facility = results[i].FacilityCode;
        //            var Token = _Token.GetTokens(Servertype, Instance).Result;
        //            var _Tokens = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(Token.ObjectParam);
        //            var Triggerid = ObjBusinessLayer.AllocateShippingDataPost(allocateshipping, Servertype);
        //            var response = _MethodWrapper.AllocatingShippingPostData(allocateshipping, 0, allocateshipping.shippingPackageCode, _Tokens.access_token, facility, Servertype, Instance);
        //            //var response = _MethodWrapper.AllocatingShippingPostData(allocateshipping, 0, Triggerid, _Tokens.access_token, facility, Servertype, Instance);
        //            if (response.IsSuccess)
        //            {
        //                successResponse.status = "Success";
        //                successResponse.waybill = "";
        //                successResponse.shippingLabel = "";
        //                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping response {JsonConvert.SerializeObject(successResponse)}");
        //                //return new JsonResult(successResponse);
        //            }
        //            else
        //            {
        //                successResponse.status = "False";
        //                successResponse.waybill = response.ObjectParam;
        //                successResponse.shippingLabel = "";
        //                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping response Error {JsonConvert.SerializeObject(successResponse)}");
        //            }
        //            return Ok(successResponse);
        //        }
        //        return Accepted("All Records Pushed Successfully");
        //    }
        //    else { return BadRequest("There is no record for Post"); }


        //}
        [HttpGet]
        public ServiceResponse<List<EndpointErrorDetails>> AloateShippingErrorDetails(string Enviornment)
        {
            string Servertype = Enviornment;
            //string Servertype = iconfiguration["ServerType:type"];

            var returndata = ObjBusinessLayer.BLAlocateShippingStatus(Servertype);
            return returndata;
        }
        [HttpGet]
        public IActionResult RetriggerAllocateShipping(UserProfile Enviornment)
        {
            _logger.LogInformation($"Retrigger Allocate Shipping");
            string Servertype = Enviornment.Environment;
            string ExecResult = string.Empty;

            //string Servertype = iconfiguration["ServerType:type"];
            SuccessResponse successResponse = new SuccessResponse();
            //string token = HttpContext.Session.GetString("STOToken");
            //var Token = _Token.GetTokens(Servertype).Result;
            string Instance = "SH";
            var Token = _Token.GetTokens(Servertype, Instance).Result;
            var _Tokens = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(Token.ObjectParam);
            if (_Tokens.access_token != null)
            {
                //var results = ObjBusinessLayer.PostGAllocateShippingData();
                var results = ObjBusinessLayer.PostGAllocateShippingDataForRetrigger(Servertype);
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

                        var Triggerid = ObjBusinessLayer.AllocateShippingDataPost(allocateshipping, Servertype);
                        var response = _MethodWrapper.AllocatingShippingPostData(allocateshipping, 0, allocateshipping.shippingPackageCode, _Tokens.access_token, facilitycode, Servertype, Instance);
                        //var response = _MethodWrapper.AllocatingShippingPostData(allocateshipping, 0, Triggerid, _Tokens.access_token, facilitycode, Servertype, Instance);
                        if (response.IsSuccess)
                        {
                            ExecResult += "Failed Record Triggered Successfully";
                            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping response {JsonConvert.SerializeObject(response.ObjectParam)}");
                            //return new JsonResult(successResponse);
                        }
                        else
                        {
                            ExecResult += response.ObjectParam;

                            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping response Error {JsonConvert.SerializeObject(response.ObjectParam)}");
                        }
                        //return Ok(response.Result.ObjectParam);
                    }
                    //return Accepted("All Records Pushed Successfully");
                }
                else { ExecResult += "There Is no record For Retrigger"; }
            }
            else
            {
                ExecResult += "Please Pass valid Token";
            }
            return new JsonResult(ExecResult);
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
        [HttpPost]
        public ActionResult UploadExcel(MainClass empLists)
        {
            _logger.LogInformation($" DateTime:-  {DateTime.Now.ToLongTimeString()}, Excel Upload Process. Data:-{JsonConvert.SerializeObject(empLists.UploadExcels)}");
            string Servertype = empLists.Enviornment.ToString();
            string Userid = empLists.Userid;
            ObjBusinessLayer.InsertTransaction(Userid, "Dispatch Data Upload", Servertype);
            //string Servertype = iconfiguration["ServerType:type"];
            List<UploadExcels> empList = empLists.UploadExcels;
            string ExecResult = string.Empty;
            var DSO = empList.Where(r => r.Type == "SO").ToList().Where(q => q.Instance == "DFX");
            var SHSO = empList.Where(r => r.Type == "SO").ToList().Where(q => q.Instance == "SH");

            var DRO = empList.Where(r => r.Type == "RO").ToList().ToList().Where(q => q.Instance == "DFX");
            var SHRO = empList.Where(r => r.Type == "RO").ToList().ToList().Where(q => q.Instance == "SH");

            var DSTO = empList.Where(r => r.Type == "STO").ToList().ToList().ToList().Where(q => q.Instance == "DFX");
            var SHSTO = empList.Where(r => r.Type == "STO").ToList().ToList().ToList().Where(q => q.Instance == "SH");

            var dfx = empList.Where(r => r.Type == "DFX").ToList();
            var Sh = empList.Where(r => r.Type == "SH").ToList();
            var Slist = SHSO.Select(x => new Element() { code = x.Code, source = x.Instance }).ToList();
            var Dlist = DSO.Select(x => new Element() { code = x.Code, source = x.Instance }).ToList();

            var DROlist = DRO.Select(x => new Element() { code = x.Code, source = x.Instance }).ToList();
            var SHROlist = SHRO.Select(x => new Element() { code = x.Code, source = x.Instance }).ToList();

            var SHSTOList = SHRO.Select(x => new Element() { code = x.Code, source = x.Instance }).ToList();

            #region Sale Order process
            if (Slist.Count > 0)
            {
                string Instance = "SH";
                var resu = _Token.GetTokens(Servertype, Instance).Result;
                var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                //string token = HttpContext.Session.GetString("Token");
                string token = deres.access_token.ToString();
                if (token != null)
                {
                    var resCode = ObjBusinessLayer.InsertCode(Slist, Servertype);
                    var ds = ObjBusinessLayer.GetCode(Instance, Servertype);

                    ServiceResponse<parentList> parentList = new ServiceResponse<parentList>();
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Saleorder/search Api called.");
                    List<Address> address = new List<Address>();
                    List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
                    List<ShippingPackage> shipingdet = new List<ShippingPackage>();
                    List<Items> qtyitems = new List<Items>();
                    List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
                    for (int i = 0; i < ds.Count; i++)
                    {
                        //var jsoncodes = JsonConvert.SerializeObject(ds[i]);
                        var jsoncodes = JsonConvert.SerializeObject(new { code = ds[i].code });

                        string code = ds[i].code;
                        //parentList = PassCode(jsoncodes, token, code, 0);
                        parentList = _MethodWrapper.PassCodeer(jsoncodes, token, code, 0, Servertype, Instance);
                        if (parentList.IsSuccess)
                        {
                            if (parentList.ObjectParam.saleOrderItems.Count > 0 || parentList.ObjectParam.address.Count > 0 || parentList.ObjectParam.Shipment.Count > 0 || parentList.ObjectParam.qtyitems.Count > 0 || parentList.ObjectParam.elements.Count > 0)
                            {
                                saleOrderItems.AddRange(parentList.ObjectParam.saleOrderItems);
                                address.AddRange(parentList.ObjectParam.address);
                                shipingdet.AddRange(parentList.ObjectParam.Shipment);
                                qtyitems.AddRange(parentList.ObjectParam.qtyitems);
                                elements.AddRange(parentList.ObjectParam.elements);
                            }
                        }
                        else
                        {
                            //ExecResult += "INVALID_SALE_ORDER_CODE !";
                            ExecResult += parentList.Errdesc;
                            return new JsonResult(ExecResult);
                        }
                    }
                    var sires = ObjBusinessLayer.insertsalesorderitem(saleOrderItems, Servertype);
                    var resshipng = ObjBusinessLayer.InsertBill(shipingdet, Servertype);
                    var resitems = ObjBusinessLayer.insertItems(qtyitems, Servertype);
                    var resads = ObjBusinessLayer.InsertAddrsss(address, Servertype);
                    var resdto = ObjBusinessLayer.insertSalesDTO(elements, Servertype);

                    var sku = ObjBusinessLayer.GetSKucodesBL(Instance, Servertype);
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, ItemType/Get Api called.");

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
                        var insertskucode = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                        if (insertskucode.Code != null)
                        {
                            itemTdto.Add(insertskucode);
                        }
                    }
                    var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto, Servertype);
                    var allsenddata = ObjBusinessLayer.GetAllRecrdstosend(Instance, Servertype);
                    var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata, Servertype, Instance);

                    //var sendcode = ObjBusinessLayer.GetAllSendData();
                    if (allsenddata.Count > 0)
                    {
                        var resutt = _MethodWrapper.Action(allsenddata, triggerid, 0, Servertype);
                        //var resutt = _MethodWrapper.Action(sendcode, triggerid, 0, Servertype);
                        //return Accepted(resutt.ObjectParam);
                        if (resutt.Errcode > 200 || resutt.Errcode < 299)
                        {
                            ExecResult += "SH SO Data Pushed";
                        }
                        else
                        {
                            ExecResult += resutt.Errdesc;
                        }

                    }
                    else
                    {
                        ExecResult += "There is Issue In SaleOrderCode !";
                    }
                }
                else
                {
                    ExecResult += "Token Not Generated !";
                }
            }
            if (Dlist.Count > 0)
            {
                string Instance = "DFX";
                var resu = _Token.GetTokens(Servertype, Instance).Result;
                var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                //string token = HttpContext.Session.GetString("Token");
                string token = deres.access_token.ToString();
                if (token != null)
                {
                    var resCode = ObjBusinessLayer.InsertCode(Dlist, Servertype);
                    var ds = ObjBusinessLayer.GetCode(Instance, Servertype);

                    ServiceResponse<parentList> parentList = new ServiceResponse<parentList>();
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, DFX Saleorder/search Api called.");
                    List<Address> address = new List<Address>();
                    List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
                    List<ShippingPackage> shipingdet = new List<ShippingPackage>();
                    List<Items> qtyitems = new List<Items>();
                    List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
                    for (int i = 0; i < ds.Count; i++)
                    {
                        var jsoncodes = JsonConvert.SerializeObject(new { code = ds[i].code });
                        string code = ds[i].code;
                        //parentList = PassCode(jsoncodes, token, code, 0);

                        parentList = _MethodWrapper.PassCodeer(jsoncodes, token, code, 0, Servertype, Instance);
                        if (parentList.IsSuccess)
                        {
                            if (parentList.ObjectParam.saleOrderItems.Count > 0 || parentList.ObjectParam.address.Count > 0 || parentList.ObjectParam.Shipment.Count > 0 || parentList.ObjectParam.qtyitems.Count > 0 || parentList.ObjectParam.elements.Count > 0)
                            {
                                saleOrderItems.AddRange(parentList.ObjectParam.saleOrderItems);
                                address.AddRange(parentList.ObjectParam.address);
                                shipingdet.AddRange(parentList.ObjectParam.Shipment);
                                qtyitems.AddRange(parentList.ObjectParam.qtyitems);
                                elements.AddRange(parentList.ObjectParam.elements);
                            }
                        }
                        else
                        {
                            ExecResult += parentList.Errdesc;
                            return new JsonResult(ExecResult);
                        }

                    }
                    var sires = ObjBusinessLayer.insertsalesorderitem(saleOrderItems, Servertype);
                    var resshipng = ObjBusinessLayer.InsertBill(shipingdet, Servertype);
                    var resitems = ObjBusinessLayer.insertItems(qtyitems, Servertype);
                    var resads = ObjBusinessLayer.InsertAddrsss(address, Servertype);
                    var resdto = ObjBusinessLayer.insertSalesDTO(elements, Servertype);


                    var sku = ObjBusinessLayer.GetSKucodesBL(Instance, Servertype);
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, ItemType/Get Api called.");

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
                        var insertskucode = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                        if (insertskucode.Code != null)
                        {
                            itemTdto.Add(insertskucode);
                        }
                    }
                    var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto, Servertype);
                    var allsenddata = ObjBusinessLayer.GetAllRecrdstosend(Instance, Servertype);
                    var triggerid = ObjBusinessLayer.InsertAllsendingData(allsenddata, Servertype, Instance);

                    //var sendcode = ObjBusinessLayer.GetAllSendData();
                    if (allsenddata.Count > 0)
                    {
                        var resutt = _MethodWrapper.Action(allsenddata, triggerid, 0, Servertype);
                        //return Accepted(resutt.ObjectParam);
                        if (resutt.Errcode > 200 || resutt.Errcode < 299)
                        {
                            ExecResult += "DFX SO Data Pushed";
                        }
                        else
                        {
                            ExecResult += resutt.Errdesc;
                        }
                    }
                    else
                    {
                        ExecResult += "There is Issue In SaleOrderCode !";
                    }
                }
                else
                {
                    ExecResult += "Token Not Generated !";
                }
            }
            #endregion

            #region Return Order Process
            if (DROlist.Count > 0)
            {
                string Instance = "SH";
                var resu = _Token.GetTokens(Servertype, Instance).Result;
                var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                //string token = HttpContext.Session.GetString("Token");
                string token = deres.access_token.ToString();
                if (token != null)
                {
                    var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);

                    //var targetList = DROlist.Select(x => new ReturnorderCode() { code = x.Code }).ToList();


                    var List2 = DROlist.Select(p => new ReturnorderCode
                    {
                        code = p.code,
                        Source = p.source
                    }).ToList();



                    foreach (var FacilityCode in Facilities)
                    {
                        ObjBusinessLayer.insertReturnOrdercoder(List2, FacilityCode.facilityCode, Servertype);
                        var codes = ObjBusinessLayer.GetReturnOrderCodes(Instance, Servertype);
                        List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
                        List<ReturnSaleOrderItem> returnSaleOrderItems = new List<ReturnSaleOrderItem>();
                        List<ReturnAddressDetailsList> returnAddressDetailsLists = new List<ReturnAddressDetailsList>();
                        for (int j = 0; j < codes.ObjectParam.Count; j++)
                        {
                            ReturnOrderGet returnOrderGet = new ReturnOrderGet();
                            returnOrderGet.reversePickupCode = codes.ObjectParam[j].code;

                            var jdetail = JsonConvert.SerializeObject(new { reversePickupCode = codes.ObjectParam[j].code });
                            var Code = codes.ObjectParam[j].code;
                            var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (list.returnAddressDetailsList.Count > 0 || list.returnSaleOrderItems.Count > 0)
                            {
                                returnAddressDetailsLists.AddRange(list.returnAddressDetailsList);
                                returnSaleOrderItems.AddRange(list.returnSaleOrderItems);
                            }
                            //return BadRequest("Please Retrigger");
                            //ExecResult += ", RO Please Retrigger";

                        }
                        ObjBusinessLayer.insertReturnSaleOrderitem(returnSaleOrderItems, Servertype);
                        ObjBusinessLayer.insertReturnaddress(returnAddressDetailsLists, Servertype);
                        var skucodes = ObjBusinessLayer.GetReturnOrderSkuCodes(Servertype);

                        List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                        List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();

                        for (int i = 0; i < skucodes.ObjectParam.Count; i++)
                        {
                            skucode sKucodes = new skucode();
                            sKucodes.skuCode = skucodes.ObjectParam[i].skuCode;
                            var code = skucodes.ObjectParam[i].Code;
                            var skucode = skucodes.ObjectParam[i].skuCode;
                            var jskucode = JsonConvert.SerializeObject(sKucodes);

                            var skudetails = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                            if (skudetails.Code != null)
                            {
                                itemTdto.Add(skudetails);
                            }

                        }
                        ObjBusinessLayer.insertReturOrderItemtypes(itemTdto, Servertype);
                        var sendata = ObjBusinessLayer.GetReturnOrderSendData(Instance, Servertype);
                        if (sendata.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata, Servertype);
                            var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0, Servertype);
                            //return Accepted(status.Result.ObjectParam);
                            if (status.Result.Errcode > 200 || status.Result.Errcode < 299)
                            {
                                ExecResult += ",SH RO Data Pushed";
                            }
                            else
                            {
                                ExecResult += status.Result.Errdesc;
                            }
                        }
                        else
                        {
                            ExecResult += "There is Issue In Code !";
                        }
                    }
                }
                else
                {
                    ExecResult += "Token Not Generated !";
                }



            }
            if (SHROlist.Count > 0)
            {
                string Instance = "DFX";
                var resu = _Token.GetTokens(Servertype, Instance).Result;
                var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                //string token = HttpContext.Session.GetString("Token");
                string token = deres.access_token.ToString();
                if (token != null)
                {
                    var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);

                    //var targetList = DROlist.Select(x => new ReturnorderCode() { code = x.Code }).ToList();


                    var List2 = SHROlist.Select(p => new ReturnorderCode
                    {
                        code = p.code,
                        Source = p.source
                    }).ToList();



                    foreach (var FacilityCode in Facilities)
                    {
                        ObjBusinessLayer.insertReturnOrdercoder(List2, FacilityCode.facilityCode, Servertype);
                        var codes = ObjBusinessLayer.GetReturnOrderCodes(Instance, Servertype);
                        List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
                        List<ReturnSaleOrderItem> returnSaleOrderItems = new List<ReturnSaleOrderItem>();
                        List<ReturnAddressDetailsList> returnAddressDetailsLists = new List<ReturnAddressDetailsList>();
                        for (int j = 0; j < codes.ObjectParam.Count; j++)
                        {
                            ReturnOrderGet returnOrderGet = new ReturnOrderGet();
                            returnOrderGet.reversePickupCode = codes.ObjectParam[j].code;

                            var jdetail = JsonConvert.SerializeObject(new { reversePickupCode = codes.ObjectParam[j].code });
                            var Code = codes.ObjectParam[j].code;
                            var list = _MethodWrapper.GetReurnOrderget(jdetail, token, Code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (list.returnAddressDetailsList.Count > 0 || list.returnSaleOrderItems.Count > 0)
                            {
                                returnAddressDetailsLists.AddRange(list.returnAddressDetailsList);
                                returnSaleOrderItems.AddRange(list.returnSaleOrderItems);
                            }
                            //return BadRequest("Please Retrigger");
                            //ExecResult += ", RO Please Retrigger";

                        }
                        ObjBusinessLayer.insertReturnSaleOrderitem(returnSaleOrderItems, Servertype);
                        ObjBusinessLayer.insertReturnaddress(returnAddressDetailsLists, Servertype);
                        var skucodes = ObjBusinessLayer.GetReturnOrderSkuCodes(Servertype);

                        List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
                        List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();

                        for (int i = 0; i < skucodes.ObjectParam.Count; i++)
                        {
                            skucode sKucodes = new skucode();
                            sKucodes.skuCode = skucodes.ObjectParam[i].skuCode;
                            var code = skucodes.ObjectParam[i].Code;
                            var skucode = skucodes.ObjectParam[i].skuCode;
                            var jskucode = JsonConvert.SerializeObject(sKucodes);

                            var skudetails = _MethodWrapper.ReturnSkuCode(jskucode, token, code, skucode, 0, Servertype, Instance);
                            if (skudetails.Code != null)
                            {
                                itemTdto.Add(skudetails);
                            }

                        }
                        ObjBusinessLayer.insertReturOrderItemtypes(itemTdto, Servertype);
                        var sendata = ObjBusinessLayer.GetReturnOrderSendData(Instance, Servertype);
                        if (sendata.ObjectParam.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertAllsendingDataReturnorder(sendata, Servertype);
                            var status = _MethodWrapper.PostDataReturnOrder(sendata, triggerid, 0, Servertype);
                            //return Accepted(status.Result.ObjectParam);
                            if (status.Result.Errcode > 200 || status.Result.Errcode < 299)
                            {
                                ExecResult += ",DFX RO Data Pushed";
                            }
                        }
                        else
                        {
                            ExecResult += "There is Issue In Code !";
                        }
                    }
                }
                else
                {
                    ExecResult += "Token Not Generated !";
                }
            }
            #endregion

            #region STO Order Process
            if (SHSTOList.Count > 0)
            {
                string Instance = "SH";
                var resu = _Token.GetTokens(Servertype, Instance).Result;
                var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                //string token = HttpContext.Session.GetString("Token");
                string token = deres.access_token.ToString();
                if (token != null)
                {
                    var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);
                    var List2 = DROlist.Select(p => new ReturnorderCode
                    {
                        code = p.code,
                        Source = p.source
                    }).ToList();

                    //var targetList = STO.Select(x => new Element() { code = x.Code }).ToList();
                    if (SHSTOList.Count > 0)
                    {
                        foreach (var FacilityCode in Facilities)
                        {
                            List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                            List<Element> elements = new List<Element>();
                            ObjBusinessLayer.insertSTOAPIGatePassCode(SHSTOList, FacilityCode.facilityCode, Servertype);
                            var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCode(Servertype);
                            for (int i = 0; i < gatePass.Count; i++)
                            {
                                string code = gatePass[i].code;
                                List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                                var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                                var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
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
                            ObjBusinessLayer.insertSTOAPiGatePassElements(elements, Servertype);
                            ObjBusinessLayer.insertSTOAPiItemTypeDTO(gatePassItemDTOs, Servertype);
                            var skuitemtype = ObjBusinessLayer.GetSTOSKUCode(Servertype);
                            List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                            for (int k = 0; k < skuitemtype.Count; k++)
                            {
                                var skucode = JsonConvert.SerializeObject(new { skuCode = skuitemtype[k].itemTypeSKU });
                                var code = skuitemtype[k].code;
                                var skutype = skuitemtype[k].itemTypeSKU;
                                var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, skutype, 0, Servertype, Instance);
                                if (Itemtypes.Code != null)
                                {
                                    itemTypeDTO.Add(Itemtypes);
                                }
                                //else
                                //    return BadRequest("Please Retrigger");
                            }
                            ObjBusinessLayer.insertSTOAPItemType(itemTypeDTO, Servertype);
                            var allrecords = ObjBusinessLayer.GetSTOAPISendData(Instance, Servertype);
                            if (allrecords.ObjectParam.Count > 0)
                            {
                                var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords, Servertype);
                                var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0, Servertype);
                                //return Accepted(status.Result.ObjectParam);
                                if (status.Result.Errcode > 200 || status.Result.Errcode < 299 || status.Result.IsSuccess)
                                {
                                    ExecResult += ",SH STO Pushed Successfully.";
                                }
                            }
                            else
                            {
                                ExecResult += "There is Issue In Code !";
                            }

                        }
                    }
                }
                else
                {
                    ExecResult += "Token Not Generated !";
                }

            }
            if (SHSTOList.Count > 0)
            {
                string Instance = "DFX";
                var resu = _Token.GetTokens(Servertype, Instance).Result;
                var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                //string token = HttpContext.Session.GetString("Token");
                string token = deres.access_token.ToString();
                if (token != null)
                {
                    var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);
                    var List2 = DROlist.Select(p => new ReturnorderCode
                    {
                        code = p.code,
                        Source = p.source
                    }).ToList();

                    //var targetList = STO.Select(x => new Element() { code = x.Code }).ToList();
                    if (SHSTOList.Count > 0)
                    {
                        foreach (var FacilityCode in Facilities)
                        {
                            List<GatePassItemDTO> gatePassItemDTOs = new List<GatePassItemDTO>();
                            List<Element> elements = new List<Element>();
                            ObjBusinessLayer.insertSTOAPIGatePassCode(SHSTOList, FacilityCode.facilityCode, Servertype);
                            var gatePass = ObjBusinessLayer.GetSTOAPIgatePassCode(Servertype);
                            for (int i = 0; i < gatePass.Count; i++)
                            {
                                string code = gatePass[i].code;
                                List<string> gatePassCodes = new List<string> { gatePass[i].code.ToString() };
                                var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                                var elemnetsList = _MethodWrapper.GetSTOAPIGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
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
                            ObjBusinessLayer.insertSTOAPiGatePassElements(elements, Servertype);
                            ObjBusinessLayer.insertSTOAPiItemTypeDTO(gatePassItemDTOs, Servertype);
                            var skuitemtype = ObjBusinessLayer.GetSTOSKUCode(Servertype);
                            List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                            for (int k = 0; k < skuitemtype.Count; k++)
                            {
                                var skucode = JsonConvert.SerializeObject(new { skuCode = skuitemtype[k].itemTypeSKU });
                                var code = skuitemtype[k].code;
                                var skutype = skuitemtype[k].itemTypeSKU;
                                var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, skutype, 0, Servertype, Instance);
                                if (Itemtypes.Code != null)
                                {
                                    itemTypeDTO.Add(Itemtypes);
                                }
                                //else
                                //    return BadRequest("Please Retrigger");
                            }
                            ObjBusinessLayer.insertSTOAPItemType(itemTypeDTO, Servertype);
                            var allrecords = ObjBusinessLayer.GetSTOAPISendData(Instance, Servertype);
                            if (allrecords.ObjectParam.Count > 0)
                            {
                                var triggerid = ObjBusinessLayer.InsertAllsendingDataSTOAPI(allrecords, Servertype);
                                var status = _MethodWrapper.STOAPiPostData(allrecords, triggerid, 0, Servertype);
                                //return Accepted(status.Result.ObjectParam);
                                if (status.Result.Errcode > 200 || status.Result.Errcode < 299 || status.Result.IsSuccess)
                                {
                                    ExecResult += ",DFX STO Pushed Successfully.";
                                }
                            }
                            else
                            {
                                ExecResult += "There is Issue In orderCode!";
                            }
                        }
                    }
                }
                else
                {
                    ExecResult += "Token Not Generated !";
                }
            }

            #endregion
            return new JsonResult(ExecResult);
        }
        [ServiceFilter(typeof(ActionFilterExample))]
        [Authorize]
        [HttpPost]
        public IActionResult ReversePickup(List<ReversePickupDb> reversePickup)
        {
            try
            {
                _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Request Reverse Pickup {JsonConvert.SerializeObject(reversePickup)}");
                Thread.Sleep(5000);


                //string Servertype = iconfiguration["ServerType:type"];
                //string myTempFile = Path.Combine(Path.GetTempPath(), "SaveFile.txt");
                //string Username = System.IO.File.ReadAllText(myTempFile).Remove(System.IO.File.ReadAllText(myTempFile).Length - 2);
                string Username = string.Empty;
                //string Username = System.IO.File.ReadAllText(myTempFile).Remove(System.IO.File.ReadAllText(myTempFile).Length - 2);
                using (StreamReader sr = new StreamReader(Path.Combine(Path.GetTempPath(), "SaveFile.txt")))
                {
                    //var name = sr.ReadLine();
                    Username = sr.ReadLine();
                    sr.Close();
                }
                string Servertype = ObjBusinessLayer.GetEnviroment(Username);
                string Instance = "SH";
                var resu = _Token.GetTokens(Servertype, Instance).Result;
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
                var revermain = ObjBusinessLayer.BLReversePickupMain(reverseitems, Servertype);
                var reveraddress = ObjBusinessLayer.BLReversePickUpAddress(pickaddressitems, Servertype);
                var reverdimension = ObjBusinessLayer.BLReverseDimension(dimitems, Servertype);
                var revercustom = ObjBusinessLayer.BLReverseCustomField(customfields, Servertype);
                //var resu = _Token.GetTokens(Servertype).Result;
                var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                string token = accesstoken.access_token;
                if (token != null)
                {
                    var lists = ObjBusinessLayer.GetReverseAllData(Servertype);
                    if (lists.Count > 0)
                    {
                        for (int i = 0; i < lists.Count; i++)
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
                            var triggerid = ObjBusinessLayer.ReversePickUpData(updateShippingpackage, lists[i].FaciityCode, Servertype);

                            var response = _MethodWrapper.ReversePickUpdetails(updateShippingpackage, 0, triggerid, token, lists[i].FaciityCode, Servertype, Instance);
                            //}
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
        public ServiceResponse<List<EndpointErrorDetails>> ReversePickupErrorDetails(string Enviornment)
        {
            string Servertype = Enviornment;
            //string Servertype = iconfiguration["ServerType:type"];

            var returndata = ObjBusinessLayer.BLGetReversePickUpErrorStatus(Servertype);
            return returndata;
        }
        [HttpGet]
        public IActionResult RetriggerreversePickup(UserProfile Enviornment)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment.Environment;
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Retrigger Reverse Pickup");

            //var resu = _Token.GetTokens(Servertype).Result;
            string Instance = "SH";
            var resu = _Token.GetTokens(Servertype, Instance).Result;
            string reversePickupResponse = string.Empty;
            var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            string token = accesstoken.access_token;
            if (token != null)
            {
                var lists = ObjBusinessLayer.GetReverseAllData(Servertype);
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
                    //var Facilities = ObjBusinessLayer.GetFacilityList();
                    for (int i = 0; i < lists.Count; i++)
                    {
                        //foreach (var FacilityCode in Facilities)
                        //{
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
                        var triggerid = ObjBusinessLayer.ReversePickUpData(updateShippingpackage, lists[i].FaciityCode, Servertype);

                        var response = _MethodWrapper.ReversePickUpdetails(updateShippingpackage, 0, triggerid, token, lists[i].FaciityCode, Servertype, Instance);
                        reversePickupResponse += response.Result.ObjectParam;

                        //}
                    }
                    reversePickupResponse += "Please pass valid Token";
                }
            }
            return new JsonResult(reversePickupResponse);
        }

        [HttpGet]
        public IEnumerable<FacilityMaintain> GetFacilityMaster_Details(string Enviornment)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment;

            List<FacilityMaintain> ResultList = ObjBusinessLayer.GetFacilityData(Servertype);
            return ResultList;
        }

        [HttpPost]
        public ActionResult FacilityMasterUploads(FacilityList FacilityList)
        {
            List<FacilityMaintain> lists = new List<FacilityMaintain>();
            lists = FacilityList.Listoffacility;
            string Servertype = FacilityList.Enviornment;

            ObjBusinessLayer.InsertTransaction(FacilityList.UserId, "Facility Master Upload", Servertype);

            //string Servertype = iconfiguration["ServerType:type"];

            string ExecResult = string.Empty;
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Facility Master Updated. {JsonConvert.SerializeObject(lists)}");

            ExecResult = ObjBusinessLayer.UploadFacilityMaster(lists, Servertype);
            return new JsonResult(ExecResult.Trim());
        }
        //[ServiceFilter(typeof(ActionFilterExample))]
        



        [HttpPost]
        public ActionResult TruckDetailsUpdate(TruckdetailsMap TruckDetails)
        {
            string Servertype = TruckDetails.Enviornment;
            ObjBusinessLayer.InsertTransaction(TruckDetails.Userid, "Truck Details Master Upload", Servertype);
            //string Servertype = iconfiguration["ServerType:type"];
            List<TruckDetails> trucklist = new List<TruckDetails>();
            trucklist = TruckDetails.TruckDetails;
            string ExecResult = string.Empty;
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Truck Details Master Updated. {JsonConvert.SerializeObject(trucklist)}");
            ExecResult = ObjBusinessLayer.UpdateTruckDetailsMaster(trucklist, Servertype);
            return new JsonResult(ExecResult.Trim());
        }
        [HttpGet]
        public IEnumerable<TruckDetails> GetTruckMaster_Details(string Enviornment)
        {
            string Servertype = Enviornment;
            //string Servertype = iconfiguration["ServerType:type"];

            List<TruckDetails> ResultList = ObjBusinessLayer.GetTruckDetails(Servertype);
            return ResultList;
        }

        [HttpPost]
        public ActionResult STOUpload(MainClass uploadExcels)
        {
            //string token = HttpContext.Session.GetString("Token");
            string ExecResult = string.Empty;
            string Servertype = uploadExcels.Enviornment;
            ObjBusinessLayer.InsertTransaction(uploadExcels.Userid, "STO Waybill Upload", Servertype);
            List<UploadExcels> Excels = new List<UploadExcels>();
            Excels = uploadExcels.UploadExcels;
            //string Servertype = iconfiguration["ServerType:type"];
            var DSTO = Excels.Where(r => r.Type == "STO").ToList().Where(q => q.Instance == "DFX");
            var SHSTO = Excels.Where(r => r.Type == "STO").ToList().Where(q => q.Instance == "SH");

            var Slist = SHSTO.Select(x => new Element() { code = x.Code, source = x.Instance }).ToList();
            var Dlist = DSTO.Select(x => new Element() { code = x.Code, source = x.Instance }).ToList();
            if (Slist.Count > 0)
            {
                var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);
                List<Element> res = new List<Element>();
                foreach (var Codes in Slist)
                {
                    Element element = new Element();
                    element.code = Codes.code;
                    res.Add(element);
                }
                //var resu = _Token.GetTokens(Servertype).Result;
                string Instance = "SH";
                var resu = _Token.GetTokens(Servertype, Instance).Result;
                var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                //string token = HttpContext.Session.GetString("Token");
                string token = deres.access_token.ToString();
                foreach (var FacilityCode in Facilities)
                {
                    var jsonre = JsonConvert.SerializeObject(new { res });
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, STO WaybillGetPass Code Upload" + jsonre + ": " + token);
                    List<GatePassItemDTODb> gatePassItemDTOs = new List<GatePassItemDTODb>();
                    List<Elementdb> elements = new List<Elementdb>();
                    List<CustomFieldValuedb> customFieldDbs = new List<CustomFieldValuedb>();
                    //var res = _MethodWrapper.GatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode);
                    if (res.Count > 0)
                    {
                        ObjBusinessLayer.insertGatePassCode(res, FacilityCode.facilityCode, Instance, Servertype);
                        var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCode(Servertype);
                        for (int i = 0; i < GatePassCode.Count; i++)
                        {
                            string code = GatePassCode[i].code;
                            List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                            var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                            var elemnetsList = _MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                            {
                                gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                                elements.AddRange(elemnetsList.elements);
                                if (elemnetsList.customFieldDbs.Count > 0)
                                {
                                    customFieldDbs.AddRange(elemnetsList.customFieldDbs);
                                }
                            }
                        }
                        ObjBusinessLayer.insertGatePassElements(elements, Servertype);
                        ObjBusinessLayer.insertItemTypeDTO(gatePassItemDTOs, Servertype);
                        ObjBusinessLayer.STOWaybillCustField(customFieldDbs, Servertype);
                        var Skucodes = ObjBusinessLayer.GetWaybillSKUCode(Servertype);
                        List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                        for (int k = 0; k < Skucodes.Count; k++)
                        {
                            string itemsku = Skucodes[k].itemTypeSKU;
                            var skucode = JsonConvert.SerializeObject(new { skuCode = Skucodes[k].itemTypeSKU });
                            var code = Skucodes[k].code;
                            var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, itemsku, 0, Servertype, Instance);
                            if (Itemtypes.Code != null)
                            {
                                itemTypeDTO.Add(Itemtypes);
                            }
                        }
                        ObjBusinessLayer.insertWaybillItemType(itemTypeDTO, Servertype);
                        var Records = ObjBusinessLayer.GetAllWaybillSTOPost(Servertype);
                        if (Records.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records, Servertype);
                            var status = _MethodWrapper.WaybillSTOPostData(Records, triggerid, 0, Servertype);
                            //return Accepted(status.Result.ObjectParam);
                            if (status.Result.Errcode > 200 || status.Result.Errcode < 299)
                            {
                                ExecResult += "SH STO Data Pushed";
                            }
                        }
                        //else return BadRequest("Please Retrigger");
                    }
                }

                //else
                //    return BadRequest("Please Retrigger");
            }
            if (Dlist.Count > 0)
            {
                var Facilities = ObjBusinessLayer.GetFacilityList(Servertype);
                List<Element> res = new List<Element>();
                foreach (var Codes in Dlist)
                {
                    Element element = new Element();
                    element.code = Codes.code;
                    res.Add(element);
                }
                //var resu = _Token.GetTokens(Servertype).Result;
                string Instance = "DFX";
                var resu = _Token.GetTokens(Servertype, Instance).Result;
                var deres = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
                //string token = HttpContext.Session.GetString("Token");
                string token = deres.access_token.ToString();
                foreach (var FacilityCode in Facilities)
                {
                    var jsonre = JsonConvert.SerializeObject(new { res });
                    _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, STO WaybillGetPass Code Upload" + jsonre + ": " + token);
                    List<GatePassItemDTODb> gatePassItemDTOs = new List<GatePassItemDTODb>();
                    List<Elementdb> elements = new List<Elementdb>();
                    List<CustomFieldValuedb> customFieldDbs = new List<CustomFieldValuedb>();
                    //var res = _MethodWrapper.GatePass(jsonre, token, 0, Servertype, FacilityCode.facilityCode);
                    if (res.Count > 0)
                    {
                        ObjBusinessLayer.insertGatePassCode(res, FacilityCode.facilityCode, Instance, Servertype);
                        var GatePassCode = ObjBusinessLayer.GetWaybillgatePassCode(Servertype);
                        for (int i = 0; i < GatePassCode.Count; i++)
                        {
                            string code = GatePassCode[i].code;
                            List<string> gatePassCodes = new List<string> { GatePassCode[i].code.ToString() };
                            var jsogatePassCodesnre = JsonConvert.SerializeObject(new { gatePassCodes = gatePassCodes });
                            var elemnetsList = _MethodWrapper.GetGatePassElements(jsogatePassCodesnre, token, code, 0, Servertype, FacilityCode.facilityCode, Instance);
                            if (elemnetsList.gatePassItemDTOs.Count > 0 || elemnetsList.elements.Count > 0)
                            {
                                gatePassItemDTOs.AddRange(elemnetsList.gatePassItemDTOs);
                                elements.AddRange(elemnetsList.elements);
                                if (elemnetsList.customFieldDbs.Count > 0)
                                {
                                    customFieldDbs.AddRange(elemnetsList.customFieldDbs);
                                }
                            }
                        }
                        ObjBusinessLayer.insertGatePassElements(elements, Servertype);
                        ObjBusinessLayer.insertItemTypeDTO(gatePassItemDTOs, Servertype);
                        ObjBusinessLayer.STOWaybillCustField(customFieldDbs, Servertype);
                        var Skucodes = ObjBusinessLayer.GetWaybillSKUCode(Servertype);
                        List<ItemTypeDTO> itemTypeDTO = new List<ItemTypeDTO>();
                        for (int k = 0; k < Skucodes.Count; k++)
                        {
                            string itemsku = Skucodes[k].itemTypeSKU;
                            var skucode = JsonConvert.SerializeObject(new { skuCode = Skucodes[k].itemTypeSKU });
                            var code = Skucodes[k].code;
                            var Itemtypes = _MethodWrapper.ReturnSkuCode(skucode, token, code, itemsku, 0, Servertype, Instance);
                            if (Itemtypes.Code != null)
                            {
                                itemTypeDTO.Add(Itemtypes);
                            }
                        }
                        ObjBusinessLayer.insertWaybillItemType(itemTypeDTO, Servertype);
                        var Records = ObjBusinessLayer.GetAllWaybillSTOPost(Servertype);
                        if (Records.Count > 0)
                        {
                            var triggerid = ObjBusinessLayer.InsertWaybillSTOsendingData(Records, Servertype);
                            var status = _MethodWrapper.WaybillSTOPostData(Records, triggerid, 0, Servertype);
                            //return Accepted(status.Result.ObjectParam);
                            if (status.Result.Errcode > 200 || status.Result.Errcode < 299)
                            {
                                ExecResult += "DFX STO Data Pushed";
                            }
                        }
                        //else return BadRequest("Please Retrigger");
                    }
                }
            }
            return new JsonResult(ExecResult);
        }

        [HttpPost]
        public ActionResult RegionMasterUpdate(RegionMasterMap ReagonList)
        {
            List<RegionMaster> regionlist = new List<RegionMaster>();
            string ExecResult = string.Empty;
            regionlist = ReagonList.RegionMasters;
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = ReagonList.Enviornment;
            ObjBusinessLayer.InsertTransaction(ReagonList.Userid, "Reagion Master Upload", Servertype);

            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Reagon Details Master Updated. {JsonConvert.SerializeObject(regionlist)}");
            ExecResult = ObjBusinessLayer.UpdateRegionMaster(regionlist, Servertype);
            return new JsonResult(ExecResult.Trim());
        }

        [HttpGet]
        public IEnumerable<RegionMaster> GetReagonMaster_Details(string Enviornment)
        {
            string Servertype = Enviornment;
            //string Servertype = iconfiguration["ServerType:type"];

            List<RegionMaster> ResultList = ObjBusinessLayer.GetRegionDetails(Servertype);
            return ResultList;
        }

        [HttpGet]
        public IEnumerable<TrackingMaster> GetTrackingMasterDetails(string Enviornment)
        {
            string Servertype = Enviornment;
            //string Servertype = iconfiguration["ServerType:type"];

            List<TrackingMaster> ResultList = ObjBusinessLayer.GetTrackingStatusDetails(Servertype);
            return ResultList;
        }

        [HttpPost]
        public ActionResult TrackingStatusMasterUpload(TrackingMasterMapping TruckDetails)
        {
            string ExecResult = string.Empty;
            List<TrackingMaster> MasterList = new List<TrackingMaster>();
            MasterList = TruckDetails.TrackingMasters;
            string Servertype = TruckDetails.Enviornment;
            ObjBusinessLayer.InsertTransaction(TruckDetails.Userid, "Tracking Status Master Upload", Servertype);
            //string Servertype = iconfiguration["ServerType:type"];

            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Master Updated. {JsonConvert.SerializeObject(MasterList)}");
            ExecResult = ObjBusinessLayer.UploadtTackingMasterDetails(MasterList, Servertype);
            return new JsonResult(ExecResult.Trim());
        }

        [HttpGet]
        public IEnumerable<TrackingMaster> GetCourierNameDetails(string Enviornment)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment;

            List<TrackingMaster> ResultList = ObjBusinessLayer.GetCourierNameDetails(Servertype);
            return ResultList;
        }
        [HttpPost]
        public ActionResult CourierListUpload(TrackingMasterMapping TruckDetails)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = TruckDetails.Enviornment;
            ObjBusinessLayer.InsertTransaction(TruckDetails.Userid, "Courier List Upload", Servertype);
            List<TrackingMaster> trackingMaster = new List<TrackingMaster>();
            trackingMaster = TruckDetails.TrackingMasters;

            string ExecResult = string.Empty;
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, CourierName Update. {JsonConvert.SerializeObject(TruckDetails)}");
            ExecResult = ObjBusinessLayer.UploadtCourireDetails(trackingMaster, Servertype);
            return new JsonResult(ExecResult.Trim());
        }

        [HttpGet]
        public IEnumerable<TrackingLinkMapping> GetTrackingLinkList(string Enviornment)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = Enviornment;

            List<TrackingLinkMapping> ResultList = ObjBusinessLayer.GetTrackingMappingList(Servertype);
            return ResultList;
        }
        [HttpPost]
        public ActionResult BulkUploadtrackingMapping(TrackingLinkMappingMap trackingLinkMappings)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = trackingLinkMappings.Enviornment;
            ObjBusinessLayer.InsertTransaction(trackingLinkMappings.Userid, "Tracking Link Mapping Upload", Servertype);
            List<TrackingLinkMapping> trackingLinkMapping = new List<TrackingLinkMapping>();
            trackingLinkMapping = trackingLinkMappings.TrackingLinkMappings;

            string ExecResult = string.Empty;
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Mapping Update. {JsonConvert.SerializeObject(trackingLinkMappings)}");
            ExecResult = ObjBusinessLayer.UploadTrackingMapping(trackingLinkMapping, Servertype);
            return new JsonResult(ExecResult.Trim());
        }
        //[HttpGet]
        //public IEnumerable<UserProfile> GetRoleMaster(string Environment)
        //{
        //    ObjBusinessLayer = new UniwareBL();
        //    _logger.LogInformation("Get Role Master at {DT}", DateTime.Now.ToLongTimeString());
        //    return ObjBusinessLayer.GetRoleMaster(Environment);
        //}

        [HttpGet]
        public IEnumerable<ShippingStatus> GetShippingStatus(string Enviornment)
        {
            string Servertype = Enviornment;
            //string Servertype = iconfiguration["ServerType:type"];

            List<ShippingStatus> ResultList = ObjBusinessLayer.GetShippingStatus(Servertype);
            return ResultList;
        }

        [HttpPost]
        public ActionResult UpdateShippingStatus(ShippingStatusList shippingStatusList)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = shippingStatusList.Enviornment;
            ObjBusinessLayer.InsertTransaction(shippingStatusList.UserId, "Shipping Status Master", Servertype);
            List<ShippingStatus> trackingLinkMapping = new List<ShippingStatus>();
            trackingLinkMapping = shippingStatusList.ShippingStatus;

            string ExecResult = string.Empty;
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Shipping Status Master Update. {JsonConvert.SerializeObject(shippingStatusList)}");
            ExecResult = ObjBusinessLayer.UpdateShippingStatusMaster(trackingLinkMapping, Servertype);
            return new JsonResult(ExecResult.Trim());
        }
        [HttpPost]
        public ActionResult ResetPassword(UserProfile userProfile)
        {
            //string Servertype = iconfiguration["ServerType:type"];
            string Servertype = userProfile.Environment;
            string ExecResult = string.Empty;
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, ResetPassword. {JsonConvert.SerializeObject(userProfile)}");
            ExecResult = ObjBusinessLayer.ResetPassword(userProfile, Servertype);
            return new JsonResult(ExecResult.Trim());
        }

        [HttpGet]
        public ActionResult GetSpecialCharacters(string Enviornment)
        {
            string Servertype = Enviornment;
            string CHaracters = string.Empty;
            CHaracters = ObjBusinessLayer.GetSpecialCharacter(Servertype);
            return new JsonResult(CHaracters);
        }
        [HttpPost]
        public ActionResult UpdateSpecialCharacters(SpecialCharacterEntity specialCharacterEntity)
        {
            ObjBusinessLayer.InsertTransaction(specialCharacterEntity.userid, "Special Character Master", specialCharacterEntity.Enviornment);

            string ExecResult = string.Empty;
            _logger.LogInformation($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Special Character Master Update. {JsonConvert.SerializeObject(specialCharacterEntity)}");
            ExecResult = ObjBusinessLayer.UpdateSpecialCharacterMaster(specialCharacterEntity.Contents, specialCharacterEntity.Enviornment);
            return new JsonResult(ExecResult.Trim());
        }

        [HttpGet]
        public ServiceResponse<DashboardsLists> GetDashboardDetails(string Enviornment)
        {
            string Servertype = Enviornment;

            var returndata = ObjBusinessLayer.GetDashboardDetails(Servertype);
            return returndata;
        }
        [HttpGet]
        public List<TDashboardDetails> GetDashboardDetailsByName(string Enviornment, string Name)
        {
            string Servertype = Enviornment;

            var returndata = ObjBusinessLayer.GetTrackingDetailsByName(Servertype, Name);
            return returndata;
        }

        [HttpGet]
        public List<TDashboardDetails> GetTrackingLink(string Enviornment, string SearchBy, string trackingNo)
        {
            string Servertype = Enviornment;

            var returndata = ObjBusinessLayer.GetTrackingLink(Servertype, SearchBy, trackingNo);
            return returndata;
        }
    }
}
