using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.BusinessLayer;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.API
{
    public class MethodWrapper
    {
        private UniwareBL ObjBusinessLayer = new();
        BearerToken _Token = new BearerToken();
        public List<Element> getCode(string json, string token, int checkcount)
        {
            int Lcheckcount = checkcount;
            var result = _Token.GetCode(json, token);
            List<Element> elmt = new List<Element>();

            List<ErrorDetails> errorDetails = new List<ErrorDetails>();
            List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
            if (result.Result.Errcode < 200 || result.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    getCode(json, token, Lcheckcount);
                }
                else
                {
                    ObjBusinessLayer.UpdateSaleOrderFirst(result.Result.ObjectParam);
                }
            }
            else
            {
                var code = JsonConvert.DeserializeObject<Root>(result.Result.ObjectParam);
                for (int i = 0; i < code.elements.Count; i++)
                {
                    Element elmts = new Element();
                    elmts.code = code.elements[i].code;
                    elmt.Add(elmts);
                    //dtinstcode.Rows.Add(code.elements[i].code);
                }
            }
            return elmt;
        }
        public parentList PassCode(string jsoncodes, string token, string code, int checkcount)
        {
            int Lcheckcount = checkcount;
            parentList parentList = new parentList();
            parentList.elements = new List<SaleOrderDTO>();
            parentList.Shipment = new List<ShippingPackage>();
            parentList.saleOrderItems = new List<SaleOrderItem>();
            parentList.qtyitems = new List<Items>();
            parentList.address = new List<Address>();
            List<ErrorDetails> errorDetails = new List<ErrorDetails>();
            salesorderRoot details = new salesorderRoot();
            List<Address> address = new List<Address>();
            List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
            List<ShippingPackage> shipingdet = new List<ShippingPackage>();
            List<Items> qtyitems = new List<Items>();
            List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
            var results = _Token.GetCodeDetails(jsoncodes, token);
            if (results.Result.Errcode < 200 || results.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    string codes = code;
                    ErrorDetails ed = new ErrorDetails();
                    ed.Code = codes;
                    ed.Reason = results.Result.ObjectParam;
                    ed.Status = true;
                    errorDetails.Add(ed);
                    // var errorcode = ObjBusinessLayer.UpdateSalesOrderError(errorDetails, 1);
                    PassCode(jsoncodes, token, code, Lcheckcount);

                }
                else
                {
                    parentList = null;
                }
            }
            else
            {
                JObject stuff = JObject.Parse(results.Result.ObjectParam);
                var abc = stuff.SelectTokens("saleOrderDTO");
                if (results.Result.ObjectParam.Contains("saleOrderDTO"))
                {
                    var hello = JToken.FromObject(stuff);
                    var jso = JsonConvert.SerializeObject(hello);
                    salesorderRoot items = JsonConvert.DeserializeObject<salesorderRoot>(jso);
                    details = items;
                    List<ShippingPackage> shippingPackages = new List<ShippingPackage>();
                    shippingPackages = details.saleOrderDTO.shippingPackages;

                    foreach (JProperty item in abc.Children())
                    {
                        if (item.Path == "saleOrderDTO.shippingPackages")
                        {
                            foreach (var item2 in item.Children())
                            {
                                var mi = 0;
                                foreach (var item3 in item2.Children()["items"])
                                {
                                    ShippingPackage shippingPackage = new ShippingPackage();
                                    for (mi = 0; mi < shippingPackages.Count(); mi++)
                                    {
                                        shippingPackage = shippingPackages[mi];
                                        var adc = item3.Values();
                                        var helloq = JToken.FromObject(adc);
                                        var jsonq = JsonConvert.SerializeObject(helloq);

                                        List<Items> itemsqq = JsonConvert.DeserializeObject<List<Items>>(jsonq);
                                        shippingPackage.items = itemsqq.FirstOrDefault();
                                        break;
                                    }
                                    mi++;
                                }
                            }

                        }
                    }
                    details.saleOrderDTO.shippingPackages = shippingPackages;
                }
                //List SalesDTO Details
                SaleOrderDTO em = new SaleOrderDTO();
                em.code = details.saleOrderDTO.code;
                em.displayOrderCode = details.saleOrderDTO.displayOrderCode;
                //elements.Add(em);
                parentList.elements.Add(em);

                //List addressdetails

                for (int ads = 0; ads < details.saleOrderDTO.addresses.Count; ads++)
                {
                    Address adrs = new Address();
                    adrs.Code = details.saleOrderDTO.code;
                    adrs.name = details.saleOrderDTO.addresses[ads].name;
                    adrs.addressLine1 = details.saleOrderDTO.addresses[ads].addressLine1;
                    adrs.addressLine2 = details.saleOrderDTO.addresses[ads].addressLine2;
                    adrs.city = details.saleOrderDTO.addresses[ads].city;
                    adrs.state = details.saleOrderDTO.addresses[ads].state;
                    adrs.pincode = details.saleOrderDTO.addresses[ads].pincode;
                    adrs.phone = details.saleOrderDTO.addresses[ads].phone;
                    adrs.email = details.saleOrderDTO.addresses[ads].email;
                    //address.Add(adrs);
                    parentList.address.Add(adrs);

                }
                //Console.WriteLine(i);
                // }


                //List shippingDetails
                for (int sd = 0; sd < details.saleOrderDTO.shippingPackages.Count; sd++)
                {
                    ShippingPackage shipdetails = new ShippingPackage();
                    shipdetails.code = details.saleOrderDTO.code;
                    shipdetails.invoiceCode = details.saleOrderDTO.shippingPackages[sd].invoiceCode;
                    shipdetails.invoiceDate = details.saleOrderDTO.shippingPackages[sd].invoiceDate;
                    //shipingdet.Add(shipdetails);
                    parentList.Shipment.Add(shipdetails);
                    Items qty = new Items();
                    qty.Code = details.saleOrderDTO.code;
                    qty.quantity = details.saleOrderDTO.shippingPackages[sd].items.quantity;
                    //qtyitems.Add(qty);
                    parentList.qtyitems.Add(qty);
                }

                //List salesorderitem
                for (int l = 0; l < details.saleOrderDTO.saleOrderItems.Count; l++)
                {
                    SaleOrderItem sitem = new SaleOrderItem();
                    sitem.code = details.saleOrderDTO.code;
                    sitem.shippingPackageCode = details.saleOrderDTO.saleOrderItems[l].shippingPackageCode;
                    sitem.id = details.saleOrderDTO.saleOrderItems[l].id;
                    sitem.itemSku = details.saleOrderDTO.saleOrderItems[l].itemSku;
                    sitem.prepaidAmount = details.saleOrderDTO.saleOrderItems[l].prepaidAmount;
                    sitem.taxPercentage = details.saleOrderDTO.saleOrderItems[l].taxPercentage;
                    sitem.totalPrice = details.saleOrderDTO.saleOrderItems[l].totalPrice;
                    sitem.facilityCode = details.saleOrderDTO.saleOrderItems[l].facilityCode;
                    //saleOrderItems.Add(sitem);
                    parentList.saleOrderItems.Add(sitem);
                }

            }
            var errorcode = ObjBusinessLayer.UpdateSalesOrderError(errorDetails, 1);
            return parentList;
        }
        public ItemTypeDTO ReturnSkuCode(string jskucode, string token, string code, string skucode, int checkcount)
        {
            int Lcheckcount = checkcount;
            ItemTypeDTO itemsSku = new ItemTypeDTO();
            List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
            var resul = _Token.GetSkuDetails(jskucode, token);
            //List<ItemTypeDTO> itemTdto = new List<ItemTypeDTO>();
            if (resul.Result.Errcode < 200 || resul.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ErrorDetails ed = new ErrorDetails();
                    ed.Status = true;
                    ed.SkuCode = skucode;
                    ed.Reason = resul.Result.ObjectParam;
                    errorskuDetails.Add(ed);
                    ReturnSkuCode(jskucode, token, code, skucode, Lcheckcount);
                }
                else
                {
                    errorskuDetails = null;
                }
            }
            else
            {
                var resl = JsonConvert.DeserializeObject<SkuRoot>(resul.Result.ObjectParam);
                itemsSku.Code = code;
                itemsSku.categoryCode = resl.itemTypeDTO.itemDetailFieldsText;//categoryCode;
                itemsSku.width = resl.itemTypeDTO.width;
                itemsSku.height = resl.itemTypeDTO.height;
                itemsSku.length = resl.itemTypeDTO.length;
                itemsSku.weight = resl.itemTypeDTO.weight;
                // itemTdto.Add(itemsSku);
            }
            //var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto);
            var errorskucode = ObjBusinessLayer.UpdateSkucodeError(errorskuDetails, 0);
            return itemsSku;
        }
        public ServiceResponse<string> Action(List<Data> sendcode, string triggerid, int checkcount)
        {
            int Lcheckcount = checkcount;
            ServiceResponse<string> resfinal = null;

            resfinal = _Token.PostDataToDeliverypackList(sendcode).Result;
            if (resfinal.Errcode < 200 || resfinal.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdatePostDatadetails(true, resfinal.ObjectParam, triggerid);
                    Action(sendcode, triggerid, Lcheckcount);
                }
                else
                {
                    resfinal = null;
                }

            }

            return resfinal;
        }

        public parentList RetriggerCode(string jsoncodes, string token, string code, int checkcount)
        {
            int Lcheckcount = checkcount;
            parentList parentList = new parentList();
            parentList.elements = new List<SaleOrderDTO>();
            parentList.Shipment = new List<ShippingPackage>();
            parentList.saleOrderItems = new List<SaleOrderItem>();
            parentList.qtyitems = new List<Items>();
            parentList.address = new List<Address>();
            List<ErrorDetails> errorDetails = new List<ErrorDetails>();
            salesorderRoot details = new salesorderRoot();
            List<Address> address = new List<Address>();
            List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
            List<ShippingPackage> shipingdet = new List<ShippingPackage>();
            List<Items> qtyitems = new List<Items>();
            List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
            var results = _Token.GetCodeDetails(jsoncodes, token);
            if (results.Result.Errcode < 200 || results.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    string codes = code;
                    ErrorDetails ed = new ErrorDetails();
                    ed.Code = codes;
                    ed.Reason = results.Result.ObjectParam;
                    ed.Status = true;
                    errorDetails.Add(ed);
                    RetriggerCode(jsoncodes, token, code, Lcheckcount);
                }
                else
                {
                    parentList = null;
                }
            }
            else
            {
                JObject stuff = JObject.Parse(results.Result.ObjectParam);
                var abc = stuff.SelectTokens("saleOrderDTO");
                if (results.Result.ObjectParam.Contains("saleOrderDTO"))
                {
                    var hello = JToken.FromObject(stuff);
                    var jso = JsonConvert.SerializeObject(hello);
                    salesorderRoot items = JsonConvert.DeserializeObject<salesorderRoot>(jso);
                    details = items;
                    List<ShippingPackage> shippingPackages = new List<ShippingPackage>();
                    shippingPackages = details.saleOrderDTO.shippingPackages;

                    foreach (JProperty item in abc.Children())
                    {
                        if (item.Path == "saleOrderDTO.shippingPackages")
                        {
                            foreach (var item2 in item.Children())
                            {
                                var mi = 0;
                                foreach (var item3 in item2.Children()["items"])
                                {
                                    ShippingPackage shippingPackage = new ShippingPackage();
                                    for (mi = 0; mi < shippingPackages.Count(); mi++)
                                    {
                                        shippingPackage = shippingPackages[mi];
                                        var adc = item3.Values();
                                        var helloq = JToken.FromObject(adc);
                                        var jsonq = JsonConvert.SerializeObject(helloq);

                                        List<Items> itemsqq = JsonConvert.DeserializeObject<List<Items>>(jsonq);
                                        shippingPackage.items = itemsqq.FirstOrDefault();
                                        break;
                                    }
                                    mi++;
                                }
                            }

                        }
                    }
                    details.saleOrderDTO.shippingPackages = shippingPackages;
                }
                //List SalesDTO Details
                SaleOrderDTO em = new SaleOrderDTO();
                em.code = details.saleOrderDTO.code;
                em.displayOrderCode = details.saleOrderDTO.displayOrderCode;
                //elements.Add(em);
                parentList.elements.Add(em);

                //List addressdetails

                for (int ads = 0; ads < details.saleOrderDTO.addresses.Count; ads++)
                {
                    Address adrs = new Address();
                    adrs.Code = details.saleOrderDTO.code;
                    adrs.name = details.saleOrderDTO.addresses[ads].name;
                    adrs.addressLine1 = details.saleOrderDTO.addresses[ads].addressLine1;
                    adrs.addressLine2 = details.saleOrderDTO.addresses[ads].addressLine2;
                    adrs.city = details.saleOrderDTO.addresses[ads].city;
                    adrs.state = details.saleOrderDTO.addresses[ads].state;
                    adrs.pincode = details.saleOrderDTO.addresses[ads].pincode;
                    adrs.phone = details.saleOrderDTO.addresses[ads].phone;
                    adrs.email = details.saleOrderDTO.addresses[ads].email;
                    //address.Add(adrs);
                    parentList.address.Add(adrs);

                }
                //Console.WriteLine(i);
                // }


                //List shippingDetails
                for (int sd = 0; sd < details.saleOrderDTO.shippingPackages.Count; sd++)
                {
                    ShippingPackage shipdetails = new ShippingPackage();
                    shipdetails.code = details.saleOrderDTO.code;
                    shipdetails.invoiceCode = details.saleOrderDTO.shippingPackages[sd].invoiceCode;
                    shipdetails.invoiceDate = details.saleOrderDTO.shippingPackages[sd].invoiceDate;
                    shipingdet.Add(shipdetails);
                    Items qty = new Items();
                    qty.Code = details.saleOrderDTO.code;
                    qty.quantity = details.saleOrderDTO.shippingPackages[sd].items.quantity;
                    //qtyitems.Add(qty);
                    parentList.qtyitems.Add(qty);
                }

                //List salesorderitem
                for (int l = 0; l < details.saleOrderDTO.saleOrderItems.Count; l++)
                {
                    SaleOrderItem sitem = new SaleOrderItem();
                    sitem.code = details.saleOrderDTO.code;
                    sitem.id = details.saleOrderDTO.saleOrderItems[l].id;
                    sitem.itemSku = details.saleOrderDTO.saleOrderItems[l].itemSku;
                    sitem.prepaidAmount = details.saleOrderDTO.saleOrderItems[l].prepaidAmount;
                    sitem.taxPercentage = details.saleOrderDTO.saleOrderItems[l].taxPercentage;
                    sitem.totalPrice = details.saleOrderDTO.saleOrderItems[l].totalPrice;
                    sitem.facilityCode = details.saleOrderDTO.saleOrderItems[l].facilityCode;
                    //saleOrderItems.Add(sitem);
                    parentList.saleOrderItems.Add(sitem);
                }
            }
            var errorcode = ObjBusinessLayer.UpdateSalesOrderError(errorDetails, 1);
            return parentList;
        }

        public ItemTypeDTO RetriggerSkuCode(string jskucode, string token, string code, string skucode, int checkcount)
        {
            int Lcheckcount = checkcount;
            ItemTypeDTO itemsSku = new ItemTypeDTO();

            List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
            var resul = _Token.GetSkuDetails(jskucode, token);
            if (resul.Result.Errcode < 200 || resul.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ErrorDetails ed = new ErrorDetails();
                    ed.Status = true;
                    ed.SkuCode = skucode;
                    ed.Reason = resul.Result.ObjectParam;
                    errorskuDetails.Add(ed);
                    RetriggerSkuCode(jskucode, token, code, skucode, Lcheckcount);
                }

            }
            else
            {
                var resl = JsonConvert.DeserializeObject<SkuRoot>(resul.Result.ObjectParam);

                //ItemTypeDTO itemsSku = new ItemTypeDTO();
                itemsSku.Code = code;
                itemsSku.categoryCode = resl.itemTypeDTO.itemDetailFieldsText;//resl.itemTypeDTO.categoryCode;
                itemsSku.width = resl.itemTypeDTO.width;
                itemsSku.height = resl.itemTypeDTO.height;
                itemsSku.length = resl.itemTypeDTO.length;
                itemsSku.weight = resl.itemTypeDTO.weight;
                //itemTdto.Add(itemsSku);
            }
            var errorskucode = ObjBusinessLayer.UpdateSkucodeError(errorskuDetails, 0);
            return itemsSku;
        }
        public ServiceResponse<string> RetriggerPostDataDelivery(List<Data> allsenddata, string triggerid, int checkcount)
        {
            int Lcheckcount = checkcount;
            ServiceResponse<string> resfinal = null;
            resfinal = _Token.PostDataToDeliverypackList(allsenddata).Result;
            if (resfinal.Errcode < 200 || resfinal.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdatePostDatadetails(true, resfinal.ObjectParam, triggerid);
                    RetriggerPostDataDelivery(allsenddata, triggerid, Lcheckcount);
                }
            }
            return resfinal;
        }
        public Task<ServiceResponse<string>> WaybillGenerationPostData(List<WaybillSend> AllData, int checkcount)
        {
            int Lcheckcount = checkcount;
            var ResStatus = _Token.PostDataTomaterialinvoice(AllData);
            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdateWaybillErrordetails(true, ResStatus.Result.ObjectParam);
                    WaybillGenerationPostData(AllData, Lcheckcount);
                }
                {
                    return ResStatus = null;

                }
            }
            return ResStatus;
        }

        public List<ReturnorderCode> GetReturnorderCode(string json, string token, int checkcount)
        {
            int Lcheckcount = checkcount;
            List<ReturnorderCode> returnorderCode = new List<ReturnorderCode>();
            var results = _Token.ReturnOrderGetCode(json, token);
            if (results.Result.Errcode < 200 || results.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    GetReturnorderCode(json, token, Lcheckcount);
                }
            }
            else
            {
                var Dresult = JsonConvert.DeserializeObject<RootReturnOrder>(results.Result.ObjectParam);
                for (int i = 0; i < Dresult.returnOrders.Count; i++)
                {
                    ReturnorderCode elmts = new ReturnorderCode();
                    elmts.code = Dresult.returnOrders[i].code;
                    returnorderCode.Add(elmts);
                }
            }
            return returnorderCode;
        }
        public RootReturnorderAPI GetReurnOrderget(string jdetail, string token, string Code, int checkcount)
        {
            int Lcheckcount = checkcount;
            var list = _Token.ReturnOrderGet(jdetail, token);
            RootReturnorderAPI rootReturnorderAPI = new RootReturnorderAPI();
            rootReturnorderAPI.returnAddressDetailsList = new List<ReturnAddressDetailsList>();
            rootReturnorderAPI.returnSaleOrderItems = new List<ReturnSaleOrderItem>();
            List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();

            if (list.Result.Errcode < 200 || list.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ErrorDetails errorDetails = new ErrorDetails();
                    errorDetails.Status = true;
                    errorDetails.Code = Code;
                    errorDetails.Reason = list.Result.ObjectParam;
                    errorCodeDetails.Add(errorDetails);
                    GetReurnOrderget(jdetail, token, Code, Lcheckcount);
                }
                else
                    rootReturnorderAPI = null;

            }
            else
            {
                var Dlist = JsonConvert.DeserializeObject<RootReturnorderAPI>(list.Result.ObjectParam);
                for (int k = 0; k < Dlist.returnSaleOrderItems.Count; k++)
                {
                    ReturnSaleOrderItem returnSaleOrderItem = new ReturnSaleOrderItem();
                    returnSaleOrderItem.Code = Code;
                    returnSaleOrderItem.reversePickupCode = Dlist.returnSaleOrderItems[k].reversePickupCode;
                    returnSaleOrderItem.skuCode = Dlist.returnSaleOrderItems[k].skuCode;
                    returnSaleOrderItem.quantity = Dlist.returnSaleOrderItems.Count.ToString();
                    //returnSaleOrderItems.Add(returnSaleOrderItem);
                    rootReturnorderAPI.returnSaleOrderItems.Add(returnSaleOrderItem);
                }
                for (int l = 0; l < Dlist.returnAddressDetailsList.Count; l++)
                {
                    ReturnAddressDetailsList returnAddressDetailsList = new ReturnAddressDetailsList();
                    returnAddressDetailsList.Code = Code;
                    returnAddressDetailsList.name = Dlist.returnAddressDetailsList[l].name;
                    returnAddressDetailsList.state = Dlist.returnAddressDetailsList[l].state;
                    returnAddressDetailsList.addressLine1 = Dlist.returnAddressDetailsList[l].addressLine1;
                    returnAddressDetailsList.addressLine2 = Dlist.returnAddressDetailsList[l].addressLine2;
                    returnAddressDetailsList.pincode = Dlist.returnAddressDetailsList[l].pincode;
                    returnAddressDetailsList.phone = Dlist.returnAddressDetailsList[l].phone;
                    returnAddressDetailsList.email = Dlist.returnAddressDetailsList[l].email;
                    returnAddressDetailsList.city = Dlist.returnAddressDetailsList[l].city;
                    //returnAddressDetailsLists.Add(returnAddressDetailsList);
                    rootReturnorderAPI.returnAddressDetailsList.Add(returnAddressDetailsList);
                }
            }
            var status = ObjBusinessLayer.UpdateReturnOrderErrordetails(errorCodeDetails);

            return rootReturnorderAPI;
        }
        public ItemTypeDTO getReturnOrderSkuCode(string jskucode,string token,string Code,string Skucode,int checkcount)
        {
            int Lcheckcount = checkcount;
           ItemTypeDTO itemsSku = new ItemTypeDTO();
            List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
            var resul = _Token.GetSkuDetails(jskucode, token);
            if (resul.Result.Errcode < 200 || resul.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ErrorDetails errorDetails = new ErrorDetails();
                    errorDetails.Status = true;
                    errorDetails.SkuCode = Skucode;
                    errorDetails.Reason = resul.Result.ObjectParam;
                    errorskuDetails.Add(errorDetails);
                    getReturnOrderSkuCode(jskucode, token, Code, Skucode, Lcheckcount);
                }
                else
                {
                    itemsSku = null;
                }
            }
            else
            {
                var resl = JsonConvert.DeserializeObject<SkuRoot>(resul.Result.ObjectParam);
               // ItemTypeDTO itemsSku = new ItemTypeDTO();
                itemsSku.Code = Code;
                itemsSku.itemDetailFieldsText = resl.itemTypeDTO.itemDetailFieldsText;//categoryCode;
                itemsSku.width = resl.itemTypeDTO.width;
                //itemsSku.height = resl.itemTypeDTO.height;
                itemsSku.length = resl.itemTypeDTO.length;
                itemsSku.weight = resl.itemTypeDTO.weight;
                itemsSku.maxRetailPrice = resl.itemTypeDTO.maxRetailPrice;
                //itemTdto.Add(itemsSku);
            }
            var skuerror = ObjBusinessLayer.UpdateReturnOrderSKUErrordetails(errorskuDetails);

            return itemsSku;
        }

        public Task<ServiceResponse<string>> PostDataReturnOrder(ServiceResponse<List<ReturnOrderSendData>> AllData, int checkcount)
        {
            int Lcheckcount = checkcount;
           var ResStatus= _Token.PostDataReturnOrderAPI(AllData.ObjectParam);
            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdateWaybillErrordetails(true, ResStatus.Result.ObjectParam);
                    PostDataReturnOrder(AllData, Lcheckcount);
                }
                {
                    return ResStatus = null;

                }
            }
            return ResStatus;
        }
    }
}
