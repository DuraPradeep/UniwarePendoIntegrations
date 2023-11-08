using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.BusinessLayer;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.API
{

    public class MethodWrapper : IDisposable
    {
        private UniwareBL ObjBusinessLayer = new();
        BearerToken _Token = new BearerToken();
        private bool disposedValue;
        Emailtrigger Emailtrigger = new Emailtrigger();
        public List<Element> getCode(string json, string token, int checkcount, string servertype)
        {
            int Lcheckcount = checkcount;
            var result = _Token.GetCode(json, token, servertype);
            List<Element> elmt = new List<Element>();

            List<ErrorDetails> errorDetails = new List<ErrorDetails>();
            List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
            if (result.Result.Errcode < 200 || result.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    getCode(json, token, Lcheckcount, servertype);
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
        public parentList PassCodeer(string jsoncodes, string token, string code, int checkcounter, string Servertype)
        {
            int LLcheckcount = checkcounter;
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
            var results = _Token.GetCodeDetails(jsoncodes, token, Servertype);
            if (results.Result.Errcode < 200 || results.Result.Errcode > 299)
            {
                if (checkcounter < 3)
                {
                    Thread.Sleep(3000);
                    LLcheckcount += 1;
                    string codes = code;
                    ErrorDetails ed = new ErrorDetails();
                    ed.Code = codes;
                    ed.Reason = results.Result.ObjectParam;
                    ed.Status = true;
                    errorDetails.Add(ed);
                    // var errorcode = ObjBusinessLayer.UpdateSalesOrderError(errorDetails, 1);
                    var errorcode = ObjBusinessLayer.UpdateSalesOrderError(errorDetails, 1);
                    PassCodeer(jsoncodes, token, code, LLcheckcount, Servertype);

                }
                else
                {
                    LLcheckcount = 0;
                    Emailtrigger.SendEmailToAdmin("Sale Order");
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
                                        shippingPackage.items = new Items();
                                        shippingPackage.items = itemsqq.FirstOrDefault();

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
                em.code = code;
                //em.code = details.saleOrderDTO.code;
                em.displayOrderCode = details.saleOrderDTO.displayOrderCode;
                //elements.Add(em);
                parentList.elements.Add(em);

                //List addressdetails

                for (int ads = 0; ads < details.saleOrderDTO.addresses.Count; ads++)
                {
                    Address adrs = new Address();
                    adrs.Code = code;
                    //adrs.Code = details.saleOrderDTO.code;
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
                    shipdetails.code = code;
                    //shipdetails.code = details.saleOrderDTO.code;
                    shipdetails.invoiceCode = details.saleOrderDTO.shippingPackages[sd].invoiceCode;
                    shipdetails.invoiceDate = details.saleOrderDTO.shippingPackages[sd].invoiceDate;
                    shipdetails.status = details.saleOrderDTO.shippingPackages[sd].status;
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
                    sitem.code = code;
                    //sitem.code = details.saleOrderDTO.code;
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
            LLcheckcount = 0;
            return parentList;
        }
        public ItemTypeDTO ReturnSkuCode(string jskucode, string token, string code, string skucode, int checkcount, string Servertype)
        {
            int Lcheckcount = checkcount;
            ItemTypeDTO itemsSku = new ItemTypeDTO();
            List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
            Log.Information(" Return order Api itemType_Get -" + jskucode + ": " + token);

            var resul = _Token.GetSkuDetails(jskucode, token, Servertype);
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
                    var errorskucode = ObjBusinessLayer.UpdateSkucodeError(errorskuDetails, 0);
                    var abc = ReturnSkuCode(jskucode, token, code, skucode, Lcheckcount, Servertype);
                }
                else
                {
                    itemsSku = null;
                    Emailtrigger.SendEmailToAdmin("Sale Order");
                }
            }
            else
            {
                var resl = JsonConvert.DeserializeObject<SkuRoot>(resul.Result.ObjectParam);
                itemsSku.Code = code;
                itemsSku.skuType = skucode;
                itemsSku.categoryCode = resl.itemTypeDTO.itemDetailFieldsText;//categoryCode;
                itemsSku.width = resl.itemTypeDTO.width;
                itemsSku.height = resl.itemTypeDTO.height;
                itemsSku.length = resl.itemTypeDTO.length;
                itemsSku.weight = resl.itemTypeDTO.weight;
                // itemTdto.Add(itemsSku);
            }
            //var resitemtype = ObjBusinessLayer.InsertitemSku(itemTdto);
            return itemsSku;
        }
        public ServiceResponse<string> Action(List<Data> sendcode, string triggerid, int checkcount, string ServerType)
        {
            int Lcheckcount = checkcount;
            ServiceResponse<string> resfinal = null;
            var jsonre = JsonConvert.SerializeObject(new { data = sendcode });
            //resfinal = _Token.PostDataToDeliverypackList(sendcode).Result;
            Log.Information($"Post Data to Pando: {jsonre}");
            resfinal = _Token.PostDataToDeliverypackList(jsonre, ServerType).Result;

            if (resfinal.Errcode < 200 || resfinal.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdatePostDatadetails(true, resfinal.ObjectParam, triggerid);

                    Action(sendcode, triggerid, Lcheckcount, ServerType);
                }
                else
                {
                    resfinal = null;
                }

            }

            return resfinal;
        }

        //public parentList RetriggerCode(string jsoncodes, string token, string code, int checkcount)
        //{
        //	int Lcheckcount = checkcount;
        //	parentList parentList = new parentList();
        //	parentList.elements = new List<SaleOrderDTO>();
        //	parentList.Shipment = new List<ShippingPackage>();
        //	parentList.saleOrderItems = new List<SaleOrderItem>();
        //	parentList.qtyitems = new List<Items>();
        //	parentList.address = new List<Address>();
        //	List<ErrorDetails> errorDetails = new List<ErrorDetails>();
        //	salesorderRoot details = new salesorderRoot();
        //	List<Address> address = new List<Address>();
        //	List<SaleOrderItem> saleOrderItems = new List<SaleOrderItem>();
        //	List<ShippingPackage> shipingdet = new List<ShippingPackage>();
        //	List<Items> qtyitems = new List<Items>();
        //	List<SaleOrderDTO> elements = new List<SaleOrderDTO>();
        //	var results = _Token.GetCodeDetails(jsoncodes, token);
        //	if (results.Result.Errcode < 200 || results.Result.Errcode > 299)
        //	{
        //		if (Lcheckcount != 3)
        //		{
        //			Thread.Sleep(3000);
        //			Lcheckcount += 1;
        //			string codes = code;
        //			ErrorDetails ed = new ErrorDetails();
        //			ed.Code = codes;
        //			ed.Reason = results.Result.ObjectParam;
        //			ed.Status = true;
        //			errorDetails.Add(ed);
        //			RetriggerCode(jsoncodes, token, code, Lcheckcount);
        //		}
        //		else
        //		{
        //			var errorcode = ObjBusinessLayer.UpdateSalesOrderError(errorDetails, 1);
        //			parentList = null;
        //		}
        //	}
        //	else
        //	{
        //		JObject stuff = JObject.Parse(results.Result.ObjectParam);
        //		var abc = stuff.SelectTokens("saleOrderDTO");
        //		if (results.Result.ObjectParam.Contains("saleOrderDTO"))
        //		{
        //			var hello = JToken.FromObject(stuff);
        //			var jso = JsonConvert.SerializeObject(hello);
        //			salesorderRoot items = JsonConvert.DeserializeObject<salesorderRoot>(jso);
        //			details = items;
        //			List<ShippingPackage> shippingPackages = new List<ShippingPackage>();
        //			shippingPackages = details.saleOrderDTO.shippingPackages;

        //			foreach (JProperty item in abc.Children())
        //			{
        //				if (item.Path == "saleOrderDTO.shippingPackages")
        //				{
        //					foreach (var item2 in item.Children())
        //					{
        //						var mi = 0;
        //						foreach (var item3 in item2.Children()["items"])
        //						{
        //							ShippingPackage shippingPackage = new ShippingPackage();
        //							for (mi = 0; mi < shippingPackages.Count(); mi++)
        //							{
        //								shippingPackage = shippingPackages[mi];
        //								var adc = item3.Values();
        //								var helloq = JToken.FromObject(adc);
        //								var jsonq = JsonConvert.SerializeObject(helloq);

        //								List<Items> itemsqq = JsonConvert.DeserializeObject<List<Items>>(jsonq);
        //								shippingPackage.items = itemsqq.FirstOrDefault();
        //								break;
        //							}
        //							mi++;
        //						}
        //					}

        //				}
        //			}
        //			details.saleOrderDTO.shippingPackages = shippingPackages;
        //		}
        //		//List SalesDTO Details
        //		SaleOrderDTO em = new SaleOrderDTO();
        //		em.code = details.saleOrderDTO.code;
        //		em.displayOrderCode = details.saleOrderDTO.displayOrderCode;
        //		//elements.Add(em);
        //		parentList.elements.Add(em);

        //		//List addressdetails

        //		for (int ads = 0; ads < details.saleOrderDTO.addresses.Count; ads++)
        //		{
        //			Address adrs = new Address();
        //			adrs.Code = details.saleOrderDTO.code;
        //			adrs.name = details.saleOrderDTO.addresses[ads].name;
        //			adrs.addressLine1 = details.saleOrderDTO.addresses[ads].addressLine1;
        //			adrs.addressLine2 = details.saleOrderDTO.addresses[ads].addressLine2;
        //			adrs.city = details.saleOrderDTO.addresses[ads].city;
        //			adrs.state = details.saleOrderDTO.addresses[ads].state;
        //			adrs.pincode = details.saleOrderDTO.addresses[ads].pincode;
        //			adrs.phone = details.saleOrderDTO.addresses[ads].phone;
        //			adrs.email = details.saleOrderDTO.addresses[ads].email;
        //			//address.Add(adrs);
        //			parentList.address.Add(adrs);

        //		}
        //		//Console.WriteLine(i);
        //		// }


        //		//List shippingDetails
        //		for (int sd = 0; sd < details.saleOrderDTO.shippingPackages.Count; sd++)
        //		{
        //			ShippingPackage shipdetails = new ShippingPackage();
        //			shipdetails.code = details.saleOrderDTO.code;
        //			shipdetails.invoiceCode = details.saleOrderDTO.shippingPackages[sd].invoiceCode;
        //			shipdetails.invoiceDate = details.saleOrderDTO.shippingPackages[sd].invoiceDate;
        //			parentList.Shipment.Add(shipdetails);
        //			Items qty = new Items();


        //			qty.Code = details.saleOrderDTO.code;
        //			qty.quantity = details.saleOrderDTO.shippingPackages[sd].items.quantity;
        //			//qtyitems.Add(qty);
        //			parentList.qtyitems.Add(qty);
        //		}

        //		//List salesorderitem
        //		for (int l = 0; l < details.saleOrderDTO.saleOrderItems.Count; l++)
        //		{
        //			SaleOrderItem sitem = new SaleOrderItem();
        //			sitem.code = details.saleOrderDTO.code;
        //			sitem.id = details.saleOrderDTO.saleOrderItems[l].id;
        //			sitem.itemSku = details.saleOrderDTO.saleOrderItems[l].itemSku;
        //			sitem.prepaidAmount = details.saleOrderDTO.saleOrderItems[l].prepaidAmount;
        //			sitem.taxPercentage = details.saleOrderDTO.saleOrderItems[l].taxPercentage;
        //			sitem.totalPrice = details.saleOrderDTO.saleOrderItems[l].totalPrice;
        //			sitem.facilityCode = details.saleOrderDTO.saleOrderItems[l].facilityCode;
        //			//saleOrderItems.Add(sitem);
        //			parentList.saleOrderItems.Add(sitem);
        //		}
        //	}
        //	return parentList;
        //}

        //public ItemTypeDTO RetriggerSkuCode(string jskucode, string token, string code, string skucode, int checkcount)
        //{
        //	int Lcheckcount = checkcount;
        //	ItemTypeDTO itemsSku = new ItemTypeDTO();

        //	List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
        //	var resul = _Token.GetSkuDetails(jskucode, token);
        //	if (resul.Result.Errcode < 200 || resul.Result.Errcode > 299)
        //	{
        //		if (Lcheckcount != 3)
        //		{
        //			Thread.Sleep(3000);
        //			Lcheckcount += 1;
        //			ErrorDetails ed = new ErrorDetails();
        //			ed.Status = true;
        //			ed.SkuCode = skucode;
        //			ed.Reason = resul.Result.ObjectParam;
        //			errorskuDetails.Add(ed);
        //                  var errorskucode = ObjBusinessLayer.UpdateSkucodeError(errorskuDetails, 0);
        //                  RetriggerSkuCode(jskucode, token, code, skucode, Lcheckcount);
        //		}
        //		else
        //		{
        //			return itemsSku = null;
        //		}

        //	}
        //	else
        //	{
        //		var resl = JsonConvert.DeserializeObject<SkuRoot>(resul.Result.ObjectParam);

        //		//ItemTypeDTO itemsSku = new ItemTypeDTO();
        //		itemsSku.Code = code;
        //		itemsSku.categoryCode = resl.itemTypeDTO.itemDetailFieldsText;//resl.itemTypeDTO.categoryCode;
        //		itemsSku.width = resl.itemTypeDTO.width;
        //		itemsSku.height = resl.itemTypeDTO.height;
        //		itemsSku.length = resl.itemTypeDTO.length;
        //		itemsSku.weight = resl.itemTypeDTO.weight;
        //		//itemTdto.Add(itemsSku);
        //	}
        //	return itemsSku;
        //}
        //public ServiceResponse<string> RetriggerPostDataDelivery(List<Data> allsenddata, string triggerid, int checkcount)
        //{
        //	int Lcheckcount = checkcount;
        //	//ServiceResponse<string> resfinal = null;
        //	var resfinal = _Token.PostDataToDeliverypackList(allsenddata).Result;
        //	if (resfinal.Errcode < 200 || resfinal.Errcode > 299)
        //	{
        //		if (Lcheckcount != 3)
        //		{
        //			Thread.Sleep(3000);
        //			Lcheckcount += 1;
        //			ObjBusinessLayer.UpdatePostDatadetails(true, resfinal.ObjectParam, triggerid);
        //			RetriggerPostDataDelivery(allsenddata, triggerid, Lcheckcount);
        //		}
        //		else
        //		{
        //			return resfinal = null;
        //		}
        //	}
        //	return resfinal;
        //}
        public Task<ServiceResponse<string>> WaybillGenerationPostData(List<WaybillSend> AllData, int checkcount, string triggerid, string ServerType)
        {
            int Lcheckcount = checkcount;
            var jsonre = JsonConvert.SerializeObject(new { data = AllData });
            Log.Information($"Waybill Post Data : {jsonre}");

            var ResStatus = _Token.PostDataTomaterialinvoice(jsonre, ServerType);
            //var ResStatus = _Token.PostDataTomaterialinvoice(AllData);
            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdateWaybillErrordetails(true, ResStatus.Result.ObjectParam, triggerid);
                    WaybillGenerationPostData(AllData, Lcheckcount, triggerid, ServerType);
                }
                {
                    Emailtrigger.SendEmailToAdmin("Waybill Generation");
                    return ResStatus = null;

                }
            }
            return ResStatus;
        }
        public List<ReturnorderCode> GetReturnorderCode(string json, string token, int checkcount, string ServerType, string FacilityCode)
        {
            int Lcheckcount = checkcount;
            List<ReturnorderCode> returnorderCode = new List<ReturnorderCode>();
            var results = _Token.ReturnOrderGetCode(json, token, ServerType, FacilityCode.Trim());
            if (results.Result.Errcode < 200 || results.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    GetReturnorderCode(json, token, Lcheckcount, ServerType, FacilityCode);
                }
                else
                {
                    ObjBusinessLayer.BLReturnOrderError(results.Result.ObjectParam);

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
        public RootReturnorderAPI GetReurnOrderget(string jdetail, string token, string Code, int checkcount, string ServerType, string FacilityCode)
        {
            int Lcheckcount = checkcount;
            var list = _Token.ReturnOrderGet(jdetail, token, ServerType, FacilityCode);
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
                    var status = ObjBusinessLayer.UpdateReturnOrderErrordetails(errorCodeDetails, 1);
                    GetReurnOrderget(jdetail, token, Code, Lcheckcount, ServerType, FacilityCode);
                }
                else
                {
                    Emailtrigger.SendEmailToAdmin("Return Order");
                    rootReturnorderAPI = null;
                }
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
                    returnSaleOrderItem.saleOrderCode = Dlist.returnSaleOrderItems[k].saleOrderCode;
                    returnSaleOrderItem.quantity = Dlist.returnSaleOrderItems.Count.ToString();

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

            return rootReturnorderAPI;
        }
        public ItemTypeDTO getReturnOrderSkuCode(string jskucode, string token, string Code, string Skucode, int checkcount, string Servertype)
        {
            int Lcheckcount = checkcount;
            ItemTypeDTO itemsSku = new ItemTypeDTO();
            List<ErrorDetails> errorskuDetails = new List<ErrorDetails>();
            Log.Information(" Return Order Api itemType_Get -" + jskucode + ": " + token);

            var resul = _Token.GetSkuDetails(jskucode, token, Servertype);
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
                    var skuerror = ObjBusinessLayer.UpdateReturnOrderSKUErrordetails(errorskuDetails, 0);
                    getReturnOrderSkuCode(jskucode, token, Code, Skucode, Lcheckcount, Servertype);
                }
                else
                {
                    Emailtrigger.SendEmailToAdmin("Return Order");
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
                itemsSku.height = resl.itemTypeDTO.height;
                itemsSku.length = resl.itemTypeDTO.length;
                itemsSku.weight = resl.itemTypeDTO.weight;
                itemsSku.maxRetailPrice = resl.itemTypeDTO.maxRetailPrice;
                //itemTdto.Add(itemsSku);
            }

            return itemsSku;
        }

        public Task<ServiceResponse<string>> PostDataReturnOrder(ServiceResponse<List<ReturnOrderSendData>> AllData, string Trigerid, int checkcount, string ServerType)
        {
            int Lcheckcount = checkcount;
            var jsonre = JsonConvert.SerializeObject(new { data = AllData.ObjectParam });
            //var ResStatus= _Token.PostDataReturnOrderAPI(jsonre);
            Log.Information($"Return Order Data Post: {jsonre}");
            var ResStatus = _Token.PostDataToDeliverypackList(jsonre, ServerType);

            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdateReturnOrderPostDataError(true, ResStatus.Result.ObjectParam, Trigerid);
                    PostDataReturnOrder(AllData, Trigerid, Lcheckcount, ServerType);
                }
                {
                    return ResStatus = null;

                }
            }
            return ResStatus;
        }

        public List<Element> GatePass(string jdetail, string token, int checkcount, string ServerType, string FacilityCode)
        {
            int Lcheckcount = checkcount;
            var list = _Token.FetchingGetPassCode(jdetail, token, ServerType, FacilityCode);
            Element rootReturnorderAPI = new Element();
            List<Element> listcode = new List<Element>();
            Log.Information("STO WayBill response: " + list.Result.ObjectParam);
            List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();

            if (list.Result.Errcode < 200 || list.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    GatePass(jdetail, token, Lcheckcount, ServerType, FacilityCode);
                }
                else
                {
                    ObjBusinessLayer.STOWaybillErrorCodes(list.Result.ObjectParam);
                    rootReturnorderAPI = null;
                }
            }
            else
            {
                var Dlist = JsonConvert.DeserializeObject<STOGatePass>(list.Result.ObjectParam);
                for (int i = 0; i < Dlist.elements.Count; i++)
                {
                    Element code = new Element();
                    code.code = Dlist.elements[i].code;
                    listcode.Add(code);
                }
            }

            //return rootReturnorderAPI;
            return listcode;
        }
        public STOlists GetGatePassElements(string jdetail, string token, string code, int checkcount, string ServerType, string FacilityCode)
        {
            int Lcheckcount = checkcount;
            Log.Information("STO WayBill Elements: request " + jdetail);
            var list = _Token.FetchingGetPassElements(jdetail, token, ServerType, FacilityCode);
            Log.Information("STO WayBill Elements response " + list.Result.ObjectParam);

            List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
            STOlists STOlists = new STOlists();
            STOlists.gatePassItemDTOs = new List<GatePassItemDTO>();
            STOlists.elements = new List<Element>();

            if (list.Result.Errcode < 200 || list.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ErrorDetails errorDetails = new ErrorDetails();
                    errorDetails.Status = true;
                    errorDetails.Code = code;
                    errorDetails.Reason = list.Result.ObjectParam;
                    errorCodeDetails.Add(errorDetails);
                    ObjBusinessLayer.UpdateWaybillGatepassError(errorCodeDetails, 1);
                    GetGatePassElements(jdetail, token, code, Lcheckcount, ServerType, FacilityCode);
                }
                else
                {
                    Emailtrigger.SendEmailToAdmin("STO Waybill");
                    STOlists = null;
                }
            }
            else
            {
                var Dlist = JsonConvert.DeserializeObject<STOwaybillGetCode>(list.Result.ObjectParam);
                for (int i = 0; i < Dlist.elements.Count; i++)
                {
                    Element codes = new Element();
                    codes.code = Dlist.elements[i].code;
                    codes.reference = Dlist.elements[i].reference;
                    codes.toPartyName = Dlist.elements[i].toPartyName;
                    codes.invoiceCode = Dlist.elements[i].invoiceCode;
                    STOlists.elements.Add(codes);
                    for (int j = 0; j < Dlist.elements[i].gatePassItemDTOs.Count; j++)
                    {
                        GatePassItemDTO gatePassItemDTO = new GatePassItemDTO();
                        gatePassItemDTO.code = Dlist.elements[i].code;
                        gatePassItemDTO.quantity = Dlist.elements[i].gatePassItemDTOs[j].quantity;
                        gatePassItemDTO.itemTypeSKU = Dlist.elements[i].gatePassItemDTOs[j].itemTypeSKU;
                        gatePassItemDTO.unitPrice = Dlist.elements[i].gatePassItemDTOs[j].unitPrice;
                        STOlists.gatePassItemDTOs.Add(gatePassItemDTO);
                    }
                }
            }
            //return rootReturnorderAPI;
            return STOlists;
        }
        public ItemTypeDTO GetSTOWaybillSkuDetails(string jdetail, string token, string code, string itemsku, int checkcount, string ServerType)
        {
            int Lcheckcount = checkcount;
            //var list = _Token.GetSTOSkuDetails(jdetail, token);
            Log.Information(" STO Waybill itemType_Get -" + itemsku + ": " + token);

            var list = _Token.GetSkuDetails(jdetail, token, ServerType);

            List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
            ItemTypeDTO itemType = new ItemTypeDTO();
            if (list.Result.Errcode < 200 || list.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ErrorDetails errorDetails = new ErrorDetails();
                    errorDetails.Status = true;
                    errorDetails.Code = itemsku;
                    errorDetails.Reason = list.Result.ObjectParam;
                    errorCodeDetails.Add(errorDetails);
                    ObjBusinessLayer.UpdateWaybillGatepassError(errorCodeDetails, 0);
                    GetSTOWaybillSkuDetails(jdetail, token, code, itemsku, Lcheckcount, ServerType);
                }
                else
                {
                    Emailtrigger.SendEmailToAdmin("STO Waybill");
                    itemType = null;
                }
            }
            else
            {
                var itemtypes = JsonConvert.DeserializeObject<WaybillSTOItemtypeDTO>(list.Result.ObjectParam);
                itemType.Code = code;
                itemType.length = itemtypes.itemTypeDTO.length;
                itemType.width = itemtypes.itemTypeDTO.width;
                itemType.height = itemtypes.itemTypeDTO.height;
                itemType.weight = itemtypes.itemTypeDTO.weight;
                itemType.itemDetailFieldsText = itemtypes.itemTypeDTO.itemDetailFieldsText;
            }
            //return rootReturnorderAPI;
            return itemType;
        }
        public Task<ServiceResponse<string>> WaybillSTOPostData(List<PostDataSTOWaybill> AllData, string triggerid, int checkcount, string ServerType)
        {
            int Lcheckcount = checkcount;
            var jsonre = JsonConvert.SerializeObject(new { data = AllData });
            //var ResStatus = _Token.WaybillSTOPostDataDeliverypackList(jsonre);
            Log.Information($"STO Waybill Post Data : {jsonre}");
            var ResStatus = _Token.PostDataTomaterialinvoice(jsonre, ServerType);

            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdateSTOWaybillPosterreoe(true, ResStatus.Result.ObjectParam, triggerid);
                    WaybillSTOPostData(AllData, triggerid, Lcheckcount, ServerType);
                }
                {
                    return ResStatus = null;

                }
            }
            return ResStatus;
        }
        public List<Element> STOAPIGatePass(string jdetail, string token, int checkcount, string ServerType, string FacilityCode)
        {
            int Lcheckcount = checkcount;
            ServiceResponse<string> response = new ServiceResponse<string>();
            var list = _Token.FetchingGetPassCode(jdetail, token, ServerType, FacilityCode);
            Element rootReturnorderAPI = new Element();
            List<Element> listcode = new List<Element>();
            Log.Information("STO APi response: " + list.Result.ObjectParam);
            List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();

            if (list.Result.Errcode < 200 || list.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    STOAPIGatePass(jdetail, token, Lcheckcount, ServerType, FacilityCode);
                }
                else
                {
                    ObjBusinessLayer.STOAPIErrorCodes(list.Result.ObjectParam);
                    rootReturnorderAPI = null;
                }
            }
            else
            {
                var Dlist = JsonConvert.DeserializeObject<STOGatePass>(list.Result.ObjectParam);
                for (int i = 0; i < Dlist.elements.Count; i++)
                {
                    Element code = new Element();
                    code.code = Dlist.elements[i].code;
                    listcode.Add(code);
                }
            }

            //return rootReturnorderAPI;
            return listcode;
        }
        public STOlists GetSTOAPIGatePassElements(string jdetail, string token, string code, int checkcount, string ServerType, string Facilitycode)
        {
            int Lcheckcount = checkcount;
            var list = _Token.FetchingGetPassElements(jdetail, token, ServerType, Facilitycode);
            Log.Information("STOAPI Response: " + jdetail);
            List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
            STOlists STOlists = new STOlists();
            STOlists.gatePassItemDTOs = new List<GatePassItemDTO>();
            STOlists.elements = new List<Element>();
            ServiceResponse<string> response = new ServiceResponse<string>();

            if (list.Result.Errcode < 200 || list.Result.Errcode > 299 || list.Status.ToString() == "Calceled")
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ErrorDetails errorDetails = new ErrorDetails();
                    errorDetails.Status = true;
                    errorDetails.Code = code;
                    errorDetails.Reason = list.Result.ObjectParam;
                    errorCodeDetails.Add(errorDetails);
                    ObjBusinessLayer.UpdateSTOAPIError(errorCodeDetails, 1);
                    GetSTOAPIGatePassElements(jdetail, token, code, Lcheckcount, ServerType, Facilitycode);
                }
                else
                {
                    //var status = ObjBusinessLayer.UpdateReturnOrderErrordetails(errorCodeDetails);
                    Emailtrigger.SendEmailToAdmin("STO API");
                    STOlists = null;
                }
            }
            else
            {
                var Dlist = JsonConvert.DeserializeObject<STOwaybillGetCode>(list.Result.ObjectParam);
                for (int i = 0; i < Dlist.elements.Count; i++)
                {
                    Element codes = new Element();
                    codes.code = Dlist.elements[i].code;
                    codes.reference = Dlist.elements[i].reference;
                    codes.toPartyName = Dlist.elements[i].toPartyName;
                    codes.invoiceCode = Dlist.elements[i].invoiceCode;
                    STOlists.elements.Add(codes);
                    for (int j = 0; j < Dlist.elements[i].gatePassItemDTOs.Count; j++)
                    {
                        GatePassItemDTO gatePassItemDTO = new GatePassItemDTO();
                        gatePassItemDTO.code = Dlist.elements[i].code;
                        gatePassItemDTO.quantity = Dlist.elements[i].gatePassItemDTOs[j].quantity;
                        gatePassItemDTO.itemTypeSKU = Dlist.elements[i].gatePassItemDTOs[j].itemTypeSKU;
                        gatePassItemDTO.unitPrice = Dlist.elements[i].gatePassItemDTOs[j].unitPrice;
                        STOlists.gatePassItemDTOs.Add(gatePassItemDTO);
                    }
                }
            }
            //return rootReturnorderAPI;
            return STOlists;
        }
        public ItemTypeDTO GetSTOAPISkuDetails(string jdetail, string token, string code, string skutype, int checkcount, string ServerType)
        {
            int Lcheckcount = checkcount;
            //var list = _Token.GetSTOSkuDetails(jdetail, token);
            var list = _Token.GetSkuDetails(jdetail, token, ServerType);

            List<ErrorDetails> errorCodeDetails = new List<ErrorDetails>();
            Log.Information("STOAPI Sku Response: " + list.Result.ObjectParam);
            ItemTypeDTO itemType = new ItemTypeDTO();
            if (list.Result.Errcode < 200 || list.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ErrorDetails errorDetails = new ErrorDetails();
                    errorDetails.Status = true;
                    errorDetails.Code = skutype;
                    errorDetails.Reason = list.Result.ObjectParam;
                    errorCodeDetails.Add(errorDetails);
                    ObjBusinessLayer.UpdateSTOAPIError(errorCodeDetails, 0);
                    GetSTOAPISkuDetails(jdetail, token, code, skutype, Lcheckcount, ServerType);
                }
                else
                {
                    Emailtrigger.SendEmailToAdmin("STO API");
                    itemType = null;
                }
            }
            else
            {
                var itemtypes = JsonConvert.DeserializeObject<WaybillSTOItemtypeDTO>(list.Result.ObjectParam);
                itemType.Code = code;
                itemType.length = itemtypes.itemTypeDTO.length;
                itemType.width = itemtypes.itemTypeDTO.width;
                itemType.height = itemtypes.itemTypeDTO.height;
                itemType.weight = itemtypes.itemTypeDTO.weight;
                itemType.itemDetailFieldsText = itemtypes.itemTypeDTO.itemDetailFieldsText;
            }
            //return rootReturnorderAPI;
            return itemType;
        }
        public Task<ServiceResponse<string>> STOAPiPostData(ServiceResponse<List<ReturnOrderSendData>> AllData, string triggerid, int checkcount, string ServerType)
        {
            int Lcheckcount = checkcount;
            var jsonre = JsonConvert.SerializeObject(new { data = AllData.ObjectParam });
            //var ResStatus = _Token.STOPApiostDataDeliverypackList(jsonre);
            Log.Information($"STO API Post Data : {jsonre}");
            var ResStatus = _Token.PostDataToDeliverypackList(jsonre, ServerType);

            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdateSTOAPIPosterreoe(true, ResStatus.Result.ObjectParam, triggerid);
                    STOAPiPostData(AllData, triggerid, Lcheckcount, ServerType);
                }
                {
                    Emailtrigger.SendEmailToAdmin("STO API");
                    return ResStatus; ;

                }
            }
            return ResStatus;
        }

        public Task<ServiceResponse<string>> UpdateShippingPackagePostData(UpdateShippingpackage AllData, int checkcount, string triggerid, string Token, string FacilityCode, string Servertype)
        {
            int Lcheckcount = checkcount;
            var ResStatus = _Token.PostUpdateShippingpckg(AllData, Token, FacilityCode, Servertype);
            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.UpdateShippingErrordetails(true, ResStatus.Result.ObjectParam, triggerid);
                    UpdateShippingPackagePostData(AllData, Lcheckcount, triggerid, Token, FacilityCode, Servertype);
                }
                {
                    Emailtrigger.SendEmailToAdmin("Update Shipping Package");
                    return ResStatus = null;

                }
            }
            else
            {
                ObjBusinessLayer.UpdateShippingErrordetails(AllData.shippingPackageCode);
                return ResStatus;

            }

        }
        public Task<ServiceResponse<string>> AllocatingShippingPostData(Allocateshipping AllData, int checkcount, string triggerid, string Token, string FacilityCode, string ServerType)
        {
            int Lcheckcount = checkcount;
            var ResStatus = _Token.PostAllocateShipping(AllData, Token, FacilityCode, ServerType);
            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.AllocateErrorDetails(true, ResStatus.Result.ObjectParam, triggerid);
                    AllocatingShippingPostData(AllData, Lcheckcount, triggerid, Token, FacilityCode, ServerType);
                }
                {
                    Emailtrigger.SendEmailToAdmin("Allocate Shipping");
                    return ResStatus = null;

                }
            }
            return ResStatus;
        }

        public Task<ServiceResponse<string>> WaybillCancelPostData(List<CancelData> AllData, int checkcount)
        {
            int Lcheckcount = checkcount;
            var jsonre = JsonConvert.SerializeObject(new { data = AllData });
            var ResStatus = _Token.DeleteDataTomaterialinvoice(jsonre);
            Log.Information($" Way Bill Cancel Data:-  {jsonre}");

            //var ResStatus = _Token.PostDataTomaterialinvoice(AllData);
            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    //ObjBusinessLayer.UpdateWaybillErrordetails(true, ResStatus.Result.ObjectParam, triggerid);
                    WaybillCancelPostData(AllData, Lcheckcount);
                }
                {
                    //Emailtrigger.SendEmailToAdmin("Waybill Generation");
                    return ResStatus = null;

                }
            }
            return ResStatus;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }


                disposedValue = true;
            }
        }
        public Task<ServiceResponse<string>> ReversePickUpdetails(ReversePickup AllData, int checkcount, string triggerid, string Token, string FacilityCode, string Servertype)
        {
            int Lcheckcount = checkcount;
            var jsonre = JsonConvert.SerializeObject(AllData);
            Log.Information($"Reverse PickUp Data:-  {jsonre}");

            var ResStatus = _Token.ReversePickUp(jsonre, Token, FacilityCode, Servertype);
            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    ObjBusinessLayer.ReversePickUpErrorDetails(true, ResStatus.Result.ObjectParam, triggerid);
                    ReversePickUpdetails(AllData, Lcheckcount, triggerid, Token, FacilityCode, Servertype);
                }
                {
                    Emailtrigger.SendEmailToAdmin("Reverse PickUp");
                    return ResStatus = null;

                }
            }
            else
            {
                ObjBusinessLayer.BLUpdateErrorDetailsReversePickup(AllData.reversePickupCode);
                return ResStatus;

            }

        }
        public Task<ServiceResponse<string>> TrackingStatus(TrackingStatus AllData, int checkcount, string Token, string FacilityCode, string Servertype)
        {
            int Lcheckcount = checkcount;
            var jsonre = JsonConvert.SerializeObject(AllData);
            Log.Information($"Tracking Status data:-  {jsonre}");

            var ResStatus = _Token.TrackingStatus(jsonre, Token, FacilityCode, Servertype);
            if (ResStatus.Result.Errcode < 200 || ResStatus.Result.Errcode > 299)
            {
                if (Lcheckcount != 3)
                {
                    Thread.Sleep(3000);
                    Lcheckcount += 1;
                    TrackingStatus(AllData, Lcheckcount, Token, FacilityCode, Servertype);
                }
                {
                    //Emailtrigger.SendEmailToAdmin("Reverse PickUp");
                    return ResStatus = null;
                }
            }
            else
            {
                return ResStatus;
            }

        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
