﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.DataAccessLayer
{
    public class Mapper
    {
        public static List<Salesorder> GetCodes(DataSet pds)
        {
            List<Salesorder> customerProfile = new List<Salesorder>();
            ServiceResponse<Salesorder> serviceResponse = new ServiceResponse<Salesorder>();
            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                    {
                        Salesorder sales = new Salesorder();
                        sales.code = pds.Tables[0].Rows[i][0].ToString();
                        customerProfile.Add(sales);
                    }
                    // serviceResponse.Errcode = 200;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return customerProfile;
        }
        public static List<Salesorder> GetCodesForRetrigger(DataSet pds)
        {
            List<Salesorder> customerProfile = new List<Salesorder>();
            ServiceResponse<Salesorder> serviceResponse = new ServiceResponse<Salesorder>();
            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                    {
                        Salesorder sales = new Salesorder();
                        sales.code = pds.Tables[0].Rows[i]["CODE"].ToString();
                        sales.Instance = pds.Tables[0].Rows[i]["Instance"].ToString();
                        customerProfile.Add(sales);
                    }
                    // serviceResponse.Errcode = 200;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return customerProfile;
        }
        public static List<SKucode> Getskucodes(DataSet pds)
        {
            List<SKucode> skucodes = new List<SKucode>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    SKucode sKucode = new SKucode();
                    sKucode.code = pds.Tables[0].Rows[i]["Code"].ToString();
                    sKucode.SkuCode = pds.Tables[0].Rows[i]["itemsku"].ToString();
                    skucodes.Add(sKucode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }
        public static List<Data> GetSendingAllrecords(DataSet pds)
        {
            List<Data> Finaldata = new List<Data>();

            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                    {
                        Data Sendingdata = new Data();
                        Sendingdata.name = pds.Tables[0].Rows[i]["name"].ToString();
                        Sendingdata.reference_number = pds.Tables[0].Rows[i]["Reference_Number"].ToString();
                        Sendingdata.address = pds.Tables[0].Rows[i]["Address"].ToString();
                        Sendingdata.city = pds.Tables[0].Rows[i]["city"].ToString();
                        Sendingdata.state = pds.Tables[0].Rows[i]["state"].ToString();
                        Sendingdata.pincode = pds.Tables[0].Rows[i]["pincode"].ToString();
                        Sendingdata.region = pds.Tables[0].Rows[i]["region"].ToString();
                        Sendingdata.mobile_number = pds.Tables[0].Rows[i]["Mobile_number"].ToString();
                        Sendingdata.email = pds.Tables[0].Rows[i]["email"].ToString();
                        Sendingdata.category = pds.Tables[0].Rows[i]["Category"].ToString();
                        Sendingdata.delivery_number = pds.Tables[0].Rows[i]["Delivery_Number"].ToString();
                        Sendingdata.mrp_price = pds.Tables[0].Rows[i]["Mrp_Price"].ToString();
                        Sendingdata.material_code = pds.Tables[0].Rows[i]["Material_Code"].ToString();
                        Sendingdata.material_taxable_amount = pds.Tables[0].Rows[i]["material_taxable_amount"].ToString();
                        Sendingdata.quantity = pds.Tables[0].Rows[i]["quentity"].ToString();
                        Sendingdata.weight = pds.Tables[0].Rows[i]["weight"].ToString();
                        Sendingdata.volume = pds.Tables[0].Rows[i]["Volume"].ToString();
                        Sendingdata.ship_to = pds.Tables[0].Rows[i]["ship_to"].ToString();
                        Sendingdata.sold_to = pds.Tables[0].Rows[i]["sold_to"].ToString();
                        Sendingdata.line_item_no = pds.Tables[0].Rows[i]["Line_item_no"].ToString();
                        Sendingdata.pickup_reference_number = pds.Tables[0].Rows[i]["pickup_reference_number"].ToString();
                        Sendingdata.customer_type = pds.Tables[0].Rows[i]["Customer_type"].ToString();
                        Sendingdata.source_system = pds.Tables[0].Rows[i]["source_system"].ToString();
                        Sendingdata.division = pds.Tables[0].Rows[i]["division"].ToString();
                        Sendingdata.quantity_unit = pds.Tables[0].Rows[i]["quantity_unit"].ToString();
                        Sendingdata.volume_unit = pds.Tables[0].Rows[i]["volume_unit"].ToString();
                        Sendingdata.type = pds.Tables[0].Rows[i]["type"].ToString();
                        Sendingdata.weight_unit = pds.Tables[0].Rows[i]["weight_unit"].ToString();
                        Sendingdata.cust_category = pds.Tables[0].Rows[i]["cust_category"].ToString();
                        Sendingdata.cust_ref_id = pds.Tables[0].Rows[i]["cust_refid"].ToString();
                        Sendingdata.expected_delivery_date = pds.Tables[0].Rows[i]["expected_delivery_date"].ToString();
                        Sendingdata.exclude_vehicle_type = new List<string>();
                        for (int j = 0; j < pds.Tables[1].Rows.Count; j++)
                        {
                            Sendingdata.exclude_vehicle_type.Add(pds.Tables[1].Rows[j]["Details"].ToString());
                        }
                        Finaldata.Add(Sendingdata);
                    }

                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return Finaldata;
        }
        public static List<UserInstance> GetInstanceFromTriggerData(DataSet pds)
        {
            List<UserInstance> Finaldata = new List<UserInstance>();

            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                    {
                        UserInstance Sendingdata = new UserInstance();
                        Sendingdata.Instance = pds.Tables[0].Rows[i]["Instance"].ToString();
                        Sendingdata.TriggerId = pds.Tables[0].Rows[i]["TriggerId"].ToString();

                        Finaldata.Add(Sendingdata);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Finaldata;
        }
        public static List<Data> GetSendData(DataSet pds)
        {
            List<Data> Finaldata = new List<Data>();

            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                    {
                        Data Sendingdata = new Data();
                        Sendingdata.name = pds.Tables[0].Rows[i]["Name"].ToString();
                        Sendingdata.reference_number = pds.Tables[0].Rows[i]["Reference_Number"].ToString();
                        Sendingdata.address = pds.Tables[0].Rows[i]["address"].ToString();
                        Sendingdata.city = pds.Tables[0].Rows[i]["City"].ToString();
                        Sendingdata.state = pds.Tables[0].Rows[i]["state"].ToString();
                        Sendingdata.pincode = pds.Tables[0].Rows[i]["Pincode"].ToString();
                        Sendingdata.region = pds.Tables[0].Rows[i]["Region"].ToString();
                        Sendingdata.mobile_number = pds.Tables[0].Rows[i]["Mobile_Number"].ToString();
                        Sendingdata.email = pds.Tables[0].Rows[i]["Email"].ToString();
                        Sendingdata.category = pds.Tables[0].Rows[i]["Category"].ToString();
                        Sendingdata.delivery_number = pds.Tables[0].Rows[i]["Delivery_Number"].ToString();
                        Sendingdata.mrp_price = pds.Tables[0].Rows[i]["Mrp_Price"].ToString();
                        Sendingdata.material_code = pds.Tables[0].Rows[i]["Material_Code"].ToString();
                        Sendingdata.material_taxable_amount = pds.Tables[0].Rows[i]["material_taxable_amount"].ToString();
                        Sendingdata.quantity = pds.Tables[0].Rows[i]["quentity"].ToString();
                        Sendingdata.weight = pds.Tables[0].Rows[i]["Weight"].ToString();
                        Sendingdata.volume = pds.Tables[0].Rows[i]["Volume"].ToString();
                        Sendingdata.ship_to = pds.Tables[0].Rows[i]["Ship_to"].ToString();
                        Sendingdata.sold_to = pds.Tables[0].Rows[i]["Sold_to"].ToString();
                        Sendingdata.line_item_no = pds.Tables[0].Rows[i]["Line_item_no"].ToString();
                        Sendingdata.pickup_reference_number = pds.Tables[0].Rows[i]["pickup_reference_number"].ToString();
                        Sendingdata.customer_type = pds.Tables[0].Rows[i]["Customer_type"].ToString();
                        Sendingdata.source_system = pds.Tables[0].Rows[i]["source_system"].ToString();
                        Sendingdata.division = pds.Tables[0].Rows[i]["division"].ToString();
                        Sendingdata.quantity_unit = pds.Tables[0].Rows[i]["quantity_unit"].ToString();
                        Sendingdata.volume_unit = pds.Tables[0].Rows[i]["volume_unit"].ToString();
                        Sendingdata.type = pds.Tables[0].Rows[i]["type"].ToString();
                        Sendingdata.weight_unit = pds.Tables[0].Rows[i]["weight_unit"].ToString();
                        Sendingdata.cust_category = pds.Tables[0].Rows[i]["cust_category"].ToString();
                        Sendingdata.cust_ref_id = pds.Tables[0].Rows[i]["cust_refid"].ToString();
                        Sendingdata.exclude_vehicle_type = new List<string>();
                        for (int j = 0; j < pds.Tables[1].Rows.Count; j++)
                        {
                            Sendingdata.exclude_vehicle_type.Add(pds.Tables[1].Rows[j]["Details"].ToString());
                        }
                        Finaldata.Add(Sendingdata);
                    }
                    // serviceResponse.Errcode = 200;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return Finaldata;
        }
        public static ServiceResponse<List<CodesErrorDetails>> GetErrorCodeDetailas(DataSet pds)
        {
            ServiceResponse<List<CodesErrorDetails>> skucodes = new ServiceResponse<List<CodesErrorDetails>>();
            List<CodesErrorDetails> userProfile = new List<CodesErrorDetails>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    CodesErrorDetails sKucode = new CodesErrorDetails();

                    sKucode.CODE = pds.Tables[0].Rows[i]["CODE"].ToString();
                    sKucode.itemSku = pds.Tables[0].Rows[i]["itemSku"].ToString();
                    sKucode.Triggerid = pds.Tables[0].Rows[i]["triggerid"].ToString();
                    sKucode.Reason = pds.Tables[0].Rows[i]["Reason"].ToString();
                    //skucodes.Add(sKucode);
                    userProfile.Add(sKucode);//new CodesErrorDetails();// Add(sKucode);
                }
                skucodes.ObjectParam = userProfile;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }
        public static ServiceResponse<ErrorCountEntitys> GetErrorCount(DataSet pds)
        {
            ServiceResponse<ErrorCountEntitys> ServiceError = new ServiceResponse<ErrorCountEntitys>();
            ErrorCountEntitys ErrorDetails = new ErrorCountEntitys();
            ErrorDetails.SaleorderDetails = new List<CodesErrorDetails>();
            ErrorDetails.WaybillError = new List<EndpointErrorDetails>();
            ErrorDetails.STOWaybill = new List<CodesErrorDetails>();
            ErrorDetails.STOAPI = new List<CodesErrorDetails>();
            ErrorDetails.UpdateShippingError = new List<EndpointErrorDetails>();
            ErrorDetails.AllocateShippingError = new List<EndpointErrorDetails>();
            try
            {
                //Saleorder details
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    CodesErrorDetails sKucode = new CodesErrorDetails();

                    sKucode.CODE = pds.Tables[0].Rows[i]["CODE"].ToString();
                    sKucode.itemSku = pds.Tables[0].Rows[i]["itemSku"].ToString();
                    sKucode.Triggerid = pds.Tables[0].Rows[i]["triggerid"].ToString();
                    sKucode.Reason = pds.Tables[0].Rows[i]["Reason"].ToString();
                    ErrorDetails.SaleorderDetails.Add(sKucode);
                }
                //Waybill Details
                for (int i = 0; i < pds.Tables[1].Rows.Count; i++)
                {
                    EndpointErrorDetails sKucode = new EndpointErrorDetails();
                    sKucode.Reason = pds.Tables[1].Rows[i]["reason"].ToString();
                    ErrorDetails.WaybillError.Add(sKucode);
                }
                //STOWaybill
                for (int i = 0; i < pds.Tables[2].Rows.Count; i++)
                {
                    CodesErrorDetails sKucode = new CodesErrorDetails();

                    sKucode.CODE = pds.Tables[2].Rows[i]["CODE"].ToString();
                    sKucode.itemSku = pds.Tables[2].Rows[i]["itemSku"].ToString();
                    sKucode.Triggerid = pds.Tables[2].Rows[i]["triggerid"].ToString();
                    sKucode.Reason = pds.Tables[2].Rows[i]["Reason"].ToString();
                    ErrorDetails.STOWaybill.Add(sKucode);
                }
                //STOAPI
                for (int i = 0; i < pds.Tables[3].Rows.Count; i++)
                {
                    CodesErrorDetails sKucode = new CodesErrorDetails();

                    sKucode.CODE = pds.Tables[3].Rows[i]["CODE"].ToString();
                    sKucode.itemSku = pds.Tables[3].Rows[i]["itemSku"].ToString();
                    sKucode.Triggerid = pds.Tables[3].Rows[i]["triggerid"].ToString();
                    sKucode.Reason = pds.Tables[3].Rows[i]["Reason"].ToString();
                    ErrorDetails.STOAPI.Add(sKucode);
                }
                //Update Shipping
                for (int i = 0; i < pds.Tables[4].Rows.Count; i++)
                {
                    EndpointErrorDetails sKucode = new EndpointErrorDetails();
                    sKucode.Reason = pds.Tables[4].Rows[i]["reason"].ToString();
                    ErrorDetails.UpdateShippingError.Add(sKucode);
                }
                //Allocate Shipping
                for (int i = 0; i < pds.Tables[5].Rows.Count; i++)
                {
                    EndpointErrorDetails sKucode = new EndpointErrorDetails();
                    sKucode.Reason = pds.Tables[5].Rows[i]["reason"].ToString();
                    ErrorDetails.AllocateShippingError.Add(sKucode);
                }

                ServiceError.ObjectParam = ErrorDetails;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ServiceError;
        }
        public static ServiceResponse<List<PostErrorDetails>> PostErrorDetails(DataSet pds)
        {
            ServiceResponse<List<PostErrorDetails>> serviceResponse = new ServiceResponse<List<PostErrorDetails>>();
            List<PostErrorDetails> resul = new List<PostErrorDetails>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    PostErrorDetails tid = new PostErrorDetails();
                    tid.TriggerId = pds.Tables[0].Rows[i][0].ToString();
                    resul.Add(tid);
                }
                serviceResponse.ObjectParam = resul;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return serviceResponse;
        }
        public static List<WaybillSend> GetWayBillSendrecords(DataSet pds)
        {
            List<WaybillSend> Finaldata = new List<WaybillSend>();

            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                    {
                        WaybillSend Sendingdata = new WaybillSend();
                        Sendingdata.indent_no = pds.Tables[0].Rows[i]["indent_no"].ToString();
                        Sendingdata.delivery_number = pds.Tables[0].Rows[i]["delivery_number"].ToString();
                        Sendingdata.mrp_price = pds.Tables[0].Rows[i]["mrp_price"].ToString();
                        Sendingdata.material_code = pds.Tables[0].Rows[i]["material_code"].ToString();
                        Sendingdata.actual_source = pds.Tables[0].Rows[i]["actual_source"].ToString();
                        Sendingdata.source_system = pds.Tables[0].Rows[i]["source_system"].ToString();
                        Sendingdata.gate_ref_id = pds.Tables[0].Rows[i]["gate_ref_id"].ToString();
                        Sendingdata.division = pds.Tables[0].Rows[i]["division"].ToString();
                        Sendingdata.quantity_unit = pds.Tables[0].Rows[i]["quantity_unit"].ToString();
                        Sendingdata.weight_unit = pds.Tables[0].Rows[i]["weight_unit"].ToString();
                        Sendingdata.volume_unit = pds.Tables[0].Rows[i]["volume_unit"].ToString();
                        Sendingdata.type = pds.Tables[0].Rows[i]["type"].ToString();
                        Sendingdata.quantity = pds.Tables[0].Rows[i]["quantity"].ToString();
                        Sendingdata.category = pds.Tables[0].Rows[i]["category"].ToString();
                        Sendingdata.invoice_date = pds.Tables[0].Rows[i]["invoice_date"].ToString();
                        Sendingdata.line_item_no = pds.Tables[0].Rows[i]["line_item_no"].ToString();
                        Sendingdata.eway_bill_number = pds.Tables[0].Rows[i]["eway_bill_number"].ToString();
                        Sendingdata.eway_bill_date = pds.Tables[0].Rows[i]["eway_bill_date"].ToString();
                        Sendingdata.action_by = pds.Tables[0].Rows[i]["action_by"].ToString();
                        Sendingdata.action_type = pds.Tables[0].Rows[i]["action_type"].ToString();
                        Sendingdata.clear = pds.Tables[0].Rows[i]["clear"].ToString();
                        Sendingdata.weight = pds.Tables[0].Rows[i]["weight"].ToString();
                        Sendingdata.volume = pds.Tables[0].Rows[i]["volume"].ToString();
                        Sendingdata.ship_to = pds.Tables[0].Rows[i]["ship_to"].ToString();
                        Sendingdata.sold_to = pds.Tables[0].Rows[i]["sold_to"].ToString();
                        Sendingdata.invoice_number = pds.Tables[0].Rows[i]["invoice_number"].ToString();
                        Sendingdata.invoice_amount = pds.Tables[0].Rows[i]["invoice_amount"].ToString();
                        Sendingdata.cust_ref_id = pds.Tables[0].Rows[i]["cust_refid"].ToString();
                        Finaldata.Add(Sendingdata);
                    }
                    // serviceResponse.Errcode = 200;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return Finaldata;
        }
        public static ServiceResponse<List<UploadReturnOrder>> GetReturnOrderCode(DataSet pds)
        {
            ServiceResponse<List<UploadReturnOrder>> skucodes = new ServiceResponse<List<UploadReturnOrder>>();
            List<UploadReturnOrder> userProfile = new List<UploadReturnOrder>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    UploadReturnOrder returncode = new UploadReturnOrder();
                    returncode.code = pds.Tables[0].Rows[i]["code"].ToString();
                    returncode.facility = pds.Tables[0].Rows[i]["FacilityCode"].ToString();
                    userProfile.Add(returncode);
                }
                skucodes.ObjectParam = userProfile;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }
        public static ServiceResponse<List<ReturnSaleOrderItem>> GetReturnOrderSkuCode(DataSet pds)
        {
            ServiceResponse<List<ReturnSaleOrderItem>> skucodes = new ServiceResponse<List<ReturnSaleOrderItem>>();
            List<ReturnSaleOrderItem> userProfile = new List<ReturnSaleOrderItem>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    ReturnSaleOrderItem returncode = new ReturnSaleOrderItem();
                    returncode.Code = pds.Tables[0].Rows[i]["code"].ToString();
                    returncode.skuCode = pds.Tables[0].Rows[i]["skuCode"].ToString();
                    userProfile.Add(returncode);
                }
                skucodes.ObjectParam = userProfile;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }
        public static ServiceResponse<List<ReturnOrderSendData>> GetReturnSendData(DataSet pds)
        {
            ServiceResponse<List<ReturnOrderSendData>> skucodes = new ServiceResponse<List<ReturnOrderSendData>>();
            List<ReturnOrderSendData> userProfile = new List<ReturnOrderSendData>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    ReturnOrderSendData returncode = new ReturnOrderSendData();
                    returncode.name = pds.Tables[0].Rows[i]["name"].ToString();
                    returncode.reference_number = pds.Tables[0].Rows[i]["reference_number"].ToString();
                    returncode.address = pds.Tables[0].Rows[i]["address"].ToString();
                    returncode.city = pds.Tables[0].Rows[i]["city"].ToString();
                    returncode.state = pds.Tables[0].Rows[i]["state"].ToString();
                    returncode.pincode = pds.Tables[0].Rows[i]["pincode"].ToString();
                    returncode.region = pds.Tables[0].Rows[i]["region"].ToString();
                    returncode.mobile_number = pds.Tables[0].Rows[i]["mobile_number"].ToString();
                    returncode.email = pds.Tables[0].Rows[i]["email"].ToString();
                    returncode.customer_type = pds.Tables[0].Rows[i]["customer_type"].ToString();
                    returncode.category = pds.Tables[0].Rows[i]["category"].ToString();
                    returncode.delivery_number = pds.Tables[0].Rows[i]["delivery_number"].ToString();
                    returncode.mrp_price = pds.Tables[0].Rows[i]["mrp_price"].ToString();
                    returncode.material_code = pds.Tables[0].Rows[i]["material_code"].ToString();
                    returncode.source_system = pds.Tables[0].Rows[i]["source_system"].ToString();
                    returncode.material_taxable_amount = pds.Tables[0].Rows[i]["material_taxable_amount"].ToString();
                    returncode.division = pds.Tables[0].Rows[i]["division"].ToString();
                    returncode.quantity = pds.Tables[0].Rows[i]["quantity"].ToString();
                    returncode.quantity_unit = pds.Tables[0].Rows[i]["quantity_unit"].ToString();
                    returncode.weight = pds.Tables[0].Rows[i]["weight"].ToString();
                    returncode.weight_unit = pds.Tables[0].Rows[i]["weight_unit"].ToString();
                    returncode.volume = pds.Tables[0].Rows[i]["volume"].ToString();
                    returncode.volume_unit = pds.Tables[0].Rows[i]["volume_unit"].ToString();
                    returncode.ship_to = pds.Tables[0].Rows[i]["ship_to"].ToString();
                    returncode.sold_to = pds.Tables[0].Rows[i]["sold_to"].ToString();
                    returncode.type = pds.Tables[0].Rows[i]["type"].ToString();
                    returncode.invoice_number = pds.Tables[0].Rows[i]["invoice_number"].ToString();
                    returncode.line_item_no = pds.Tables[0].Rows[i]["line_item_no"].ToString();
                    returncode.invoice_amount = pds.Tables[0].Rows[i]["invoice_amount"].ToString();
                    returncode.invoice_date = pds.Tables[0].Rows[i]["invoice_date"].ToString();
                    returncode.pickup_reference_number = pds.Tables[0].Rows[i]["pickup_reference_number"].ToString();
                    returncode.cust_ref_id = pds.Tables[0].Rows[i]["cust_refid"].ToString();
                    returncode.expected_delivery_date = pds.Tables[0].Rows[i]["expected_delivery_date"].ToString();
                    returncode.exclude_vehicle_type = new List<string>();
                    for (int j = 0; j < pds.Tables[1].Rows.Count; j++)
                    {
                        returncode.exclude_vehicle_type.Add(pds.Tables[1].Rows[j]["Details"].ToString());
                    }
                    userProfile.Add(returncode);
                }
                skucodes.ObjectParam = userProfile;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }

        public static List<Element> GetGatePassCode(DataSet pds)
        {
            List<Element> skucodes = new List<Element>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    Element sKucode = new Element();
                    sKucode.code = pds.Tables[0].Rows[i]["Code"].ToString();

                    skucodes.Add(sKucode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }
        public static List<GatePassItemDTO> GetSKUCode(DataSet pds)
        {
            List<GatePassItemDTO> skucodes = new List<GatePassItemDTO>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    GatePassItemDTO sKucode = new GatePassItemDTO();
                    sKucode.code = pds.Tables[0].Rows[i]["Code"].ToString();
                    sKucode.itemTypeSKU = pds.Tables[0].Rows[i]["itemtypeSKU"].ToString();

                    skucodes.Add(sKucode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }
        public static List<PostDataSTOWaybill> GetSendingWayBillSTOData(DataSet pds)
        {
            List<PostDataSTOWaybill> Finaldata = new List<PostDataSTOWaybill>();

            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                    {
                        PostDataSTOWaybill Sendingdata = new PostDataSTOWaybill();
                        Sendingdata.indent_no = pds.Tables[0].Rows[i]["indent_no"].ToString();
                        Sendingdata.delivery_number = pds.Tables[0].Rows[i]["delivery_number"].ToString();
                        Sendingdata.mrp_price = pds.Tables[0].Rows[i]["mrp_price"].ToString();
                        Sendingdata.material_code = pds.Tables[0].Rows[i]["material_code"].ToString();
                        Sendingdata.actual_source = pds.Tables[0].Rows[i]["actual_source"].ToString();
                        Sendingdata.source_system = pds.Tables[0].Rows[i]["source_system"].ToString();
                        Sendingdata.gate_ref_id = pds.Tables[0].Rows[i]["gate_ref_id"].ToString();
                        Sendingdata.division = pds.Tables[0].Rows[i]["division"].ToString();
                        Sendingdata.quantity = pds.Tables[0].Rows[i]["quantity"].ToString();
                        Sendingdata.quantity_unit = pds.Tables[0].Rows[i]["quantity_unit"].ToString();
                        Sendingdata.weight = pds.Tables[0].Rows[i]["weight"].ToString();
                        Sendingdata.weight_unit = pds.Tables[0].Rows[i]["weight_unit"].ToString();
                        Sendingdata.volume = pds.Tables[0].Rows[i]["volume"].ToString();
                        Sendingdata.volume_unit = pds.Tables[0].Rows[i]["volume_unit"].ToString();
                        Sendingdata.ship_to = pds.Tables[0].Rows[i]["ship_to"].ToString();
                        Sendingdata.sold_to = pds.Tables[0].Rows[i]["sold_to"].ToString();
                        Sendingdata.type = pds.Tables[0].Rows[i]["type"].ToString();
                        Sendingdata.invoice_number = pds.Tables[0].Rows[i]["invoice_number"].ToString();
                        Sendingdata.invoice_amount = pds.Tables[0].Rows[i]["invoice_amount"].ToString();
                        Sendingdata.category = pds.Tables[0].Rows[i]["category"].ToString();
                        Sendingdata.invoice_date = pds.Tables[0].Rows[i]["invoice_date"].ToString();
                        Sendingdata.line_item_no = pds.Tables[0].Rows[i]["line_item_no"].ToString();
                        Sendingdata.eway_bill_number = pds.Tables[0].Rows[i]["eway_bill_number"].ToString();
                        Sendingdata.eway_bill_date = pds.Tables[0].Rows[i]["eway_bill_date"].ToString();
                        Sendingdata.action_by = pds.Tables[0].Rows[i]["action_by"].ToString();
                        Sendingdata.action_type = pds.Tables[0].Rows[i]["action_type"].ToString();
                        Sendingdata.clear = pds.Tables[0].Rows[i]["clear"].ToString();
                        Sendingdata.cust_ref_id = pds.Tables[0].Rows[i]["cust_ref_id"].ToString();



                        Finaldata.Add(Sendingdata);
                    }
                    // serviceResponse.Errcode = 200;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
            return Finaldata;
        }


        public static ServiceResponse<UserLogin> CheckLoginCredentials(DataSet pds)
        {
            ServiceResponse<UserLogin> serviceResponse = new ServiceResponse<UserLogin>();
            UserLogin userLogin = new UserLogin();
            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    userLogin.UserName = Convert.ToString(pds.Tables[0].Rows[0]["UserName"]);
                    userLogin.Password = pds.Tables[0].Rows[0]["Password"].ToString();
                    userLogin.LoginID = pds.Tables[0].Rows[0]["LoginID"].ToString();
                    userLogin.RoleId = pds.Tables[0].Rows[0]["RoleId"].ToString();
                    userLogin.PhoneNumber = pds.Tables[0].Rows[0]["MobileNumber"].ToString();
                    userLogin.Email = pds.Tables[0].Rows[0]["Email"].ToString();
                    userLogin.Environment = pds.Tables[0].Rows[0]["Environment"].ToString();
                    serviceResponse.ObjectParam = userLogin;
                    serviceResponse.Errcode = 200;
                }
                else
                {
                    serviceResponse.Errcode = Convert.ToInt32(300);
                    serviceResponse.Errdesc = "Data Not Found";
                    serviceResponse.ObjectParam = new UserLogin();
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Errcode = 500;
                serviceResponse.Errdesc = ex.Message;
                serviceResponse.ObjectParam = new UserLogin();
            }
            return serviceResponse;
        }

        public static List<Element> GetSTOAPIGatePassCode(DataSet pds)
        {
            List<Element> skucodes = new List<Element>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    Element sKucode = new Element();
                    sKucode.code = pds.Tables[0].Rows[i]["Code"].ToString();

                    skucodes.Add(sKucode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }
        public static ServiceResponse<List<ReturnOrderSendData>> GetSTOAPIAllData(DataSet pds)
        {
            ServiceResponse<List<ReturnOrderSendData>> skucodes = new ServiceResponse<List<ReturnOrderSendData>>();
            List<ReturnOrderSendData> userProfile = new List<ReturnOrderSendData>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    ReturnOrderSendData returncode = new ReturnOrderSendData();
                    returncode.name = pds.Tables[0].Rows[i]["name"].ToString();
                    returncode.reference_number = pds.Tables[0].Rows[i]["reference_number"].ToString();
                    returncode.address = pds.Tables[0].Rows[i]["address"].ToString();
                    returncode.city = pds.Tables[0].Rows[i]["city"].ToString();
                    returncode.state = pds.Tables[0].Rows[i]["state"].ToString();
                    returncode.pincode = pds.Tables[0].Rows[i]["pincode"].ToString();
                    returncode.region = pds.Tables[0].Rows[i]["region"].ToString();
                    returncode.mobile_number = pds.Tables[0].Rows[i]["mobile_number"].ToString();
                    returncode.email = pds.Tables[0].Rows[i]["email"].ToString();
                    returncode.customer_type = pds.Tables[0].Rows[i]["customer_type"].ToString();
                    returncode.category = pds.Tables[0].Rows[i]["category"].ToString();
                    returncode.delivery_number = pds.Tables[0].Rows[i]["delivery_number"].ToString();
                    returncode.mrp_price = pds.Tables[0].Rows[i]["mrp_price"].ToString();
                    returncode.material_code = pds.Tables[0].Rows[i]["material_code"].ToString();
                    returncode.source_system = pds.Tables[0].Rows[i]["source_system"].ToString();
                    returncode.material_taxable_amount = pds.Tables[0].Rows[i]["material_taxable_amount"].ToString();
                    returncode.division = pds.Tables[0].Rows[i]["division"].ToString();
                    returncode.quantity = pds.Tables[0].Rows[i]["quantity"].ToString();
                    returncode.quantity_unit = pds.Tables[0].Rows[i]["quantity_unit"].ToString();
                    returncode.weight = pds.Tables[0].Rows[i]["weight"].ToString();
                    returncode.weight_unit = pds.Tables[0].Rows[i]["weight_unit"].ToString();
                    returncode.volume = pds.Tables[0].Rows[i]["volume"].ToString();
                    returncode.volume_unit = pds.Tables[0].Rows[i]["volume_unit"].ToString();
                    returncode.ship_to = pds.Tables[0].Rows[i]["ship_to"].ToString();
                    returncode.sold_to = pds.Tables[0].Rows[i]["sold_to"].ToString();
                    returncode.type = pds.Tables[0].Rows[i]["type"].ToString();
                    returncode.invoice_number = pds.Tables[0].Rows[i]["invoice_number"].ToString();
                    returncode.line_item_no = pds.Tables[0].Rows[i]["line_item_no"].ToString();
                    returncode.invoice_amount = pds.Tables[0].Rows[i]["invoice_amount"].ToString();
                    returncode.invoice_date = pds.Tables[0].Rows[i]["invoice_date"].ToString();
                    returncode.pickup_reference_number = pds.Tables[0].Rows[i]["pickup_reference_number"].ToString();
                    returncode.cust_ref_id = pds.Tables[0].Rows[i]["cust_refid"].ToString();
                    returncode.expected_delivery_date = pds.Tables[0].Rows[i]["expected_delivery_date"].ToString();
                    returncode.exclude_vehicle_type = new List<string>();
                    for (int j = 0; j < pds.Tables[1].Rows.Count; j++)
                    {
                        returncode.exclude_vehicle_type.Add(pds.Tables[1].Rows[j]["Details"].ToString());
                    }
                    userProfile.Add(returncode);
                }
                skucodes.ObjectParam = userProfile;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }
        public static ServiceResponse<List<EndpointErrorDetails>> ErrorWaybillPostData(DataSet pds)
        {
            ServiceResponse<List<EndpointErrorDetails>> skucodes = new ServiceResponse<List<EndpointErrorDetails>>();
            List<EndpointErrorDetails> userProfile = new List<EndpointErrorDetails>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    EndpointErrorDetails sKucode = new EndpointErrorDetails();
                    sKucode.Reason = pds.Tables[0].Rows[i]["reason"].ToString();

                    //skucodes.Add(sKucode);
                    userProfile.Add(sKucode);//new CodesErrorDetails();// Add(sKucode);
                }
                skucodes.ObjectParam = userProfile;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return skucodes;
        }

        public static List<UpdateShippingpackagedb> GetUpdateShippingDetails(DataSet pds)
        {
            //List<UpdateShippingpackage> skucodes = new List<UpdateShippingpackage>();

            List<UpdateShippingpackagedb> userProfile = new List<UpdateShippingpackagedb>();

            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    UpdateShippingpackagedb Updateship = new UpdateShippingpackagedb();
                    Updateship.customFieldValues = new List<CustomFieldValue>();
                    ShippingBox shippingBox = new ShippingBox();
                    CustomFieldValue customFieldValue = new CustomFieldValue();

                    Updateship.shippingPackageCode = pds.Tables[0].Rows[i]["shippingPackageCode"].ToString();
                    //Updateship.shippingProviderCode = pds.Tables[0].Rows[i]["shippingProviderCode"].ToString();
                    //Updateship.trackingNumber = pds.Tables[0].Rows[i]["trackingNumber"].ToString();
                    //Updateship.shippingPackageTypeCode = pds.Tables[0].Rows[i]["shippingPackageTypeCode"].ToString();
                    //Updateship.actualWeight = Convert.ToInt32(pds.Tables[0].Rows[i]["actualWeight"]);
                    //Updateship.noOfBoxes = Convert.ToInt32(pds.Tables[0].Rows[i]["noOfBoxes"]);
                    //sKucode.shippingBox.length = Convert.ToInt32(pds.Tables[0].Rows[i]["length"]);
                    //sKucode.shippingBox.length = Convert.ToInt32(pds.Tables[0].Rows[i]["width"]);
                    //sKucode.shippingBox.length = Convert.ToInt32(pds.Tables[0].Rows[i]["height"]);

                    //shippingBox.length = Convert.ToInt32(pds.Tables[0].Rows[i]["length"]);
                    //shippingBox.width = Convert.ToInt32(pds.Tables[0].Rows[i]["width"]);
                    //shippingBox.height = Convert.ToInt32(pds.Tables[0].Rows[i]["height"]);
                    //Updateship.shippingBox = shippingBox;
                    customFieldValue.name = pds.Tables[0].Rows[i]["name"].ToString();
                    customFieldValue.value = pds.Tables[0].Rows[i]["value"].ToString();
                    Updateship.FacilityCode = pds.Tables[0].Rows[i]["facilityCode"].ToString();
                    //userProfile.Add(sKucode);
                    //sKucode.shippingBox. Add(shippingBox);
                    Updateship.customFieldValues.Add(customFieldValue);
                    //ShippingBoxs.Add(shippingBox);
                    //CustomFieldValues.Add(customFieldValue);
                    userProfile.Add(Updateship);
                }
                //skucodes.Add( userProfile);
                // skucodes =
                return userProfile;
                //userProfile.AddRange(sKucode)
            }
            catch (Exception ex)
            {

                //throw ex;
                return userProfile = null;
            }

        }

        public static List<AllocateshippingDb> GetAllocateShipping(DataSet pds)
        {
            List<Allocateshipping> skucodes = new List<Allocateshipping>();

            List<AllocateshippingDb> userProfile = new List<AllocateshippingDb>();

            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    AllocateshippingDb sKucode = new AllocateshippingDb();


                    sKucode.shippingPackageCode = pds.Tables[0].Rows[i]["shippingPackageCode"].ToString();
                    sKucode.shippingLabelMandatory = pds.Tables[0].Rows[i]["shippingLabelMandatory"].ToString();
                    sKucode.shippingProviderCode = pds.Tables[0].Rows[i]["shippingProviderCode"].ToString();
                    sKucode.shippingCourier = pds.Tables[0].Rows[i]["shippingCourier"].ToString();
                    sKucode.trackingNumber = pds.Tables[0].Rows[i]["trackingNumber"].ToString();
                    //sKucode.generateUniwareShippingLabel = pds.Tables[0].Rows[i]["generateUniwareShippingLabel"].ToString();
                    sKucode.FacilityCode = pds.Tables[0].Rows[i]["facilityCode"].ToString();
                    sKucode.Instance = pds.Tables[0].Rows[i]["Instance"].ToString();
                    sKucode.trackingLink = pds.Tables[0].Rows[i]["trackingLink"].ToString();

                    userProfile.Add(sKucode);

                }
                //skucodes.Add( userProfile);
                return userProfile;
                //userProfile.AddRange(sKucode)
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //return skucodes;
        }
        public static List<CancelData> GetWayBillCancelData(DataSet pds)
        {
            List<CancelData> Finaldata = new List<CancelData>();

            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                    {
                        CancelData Sendingdata = new CancelData();
                        Sendingdata.indent_no = pds.Tables[0].Rows[i]["indent_no"].ToString();
                        Sendingdata.material_invoice_number = pds.Tables[0].Rows[i]["material_invoice_number"].ToString();
                        Sendingdata.material_code = pds.Tables[0].Rows[i]["material_code"].ToString();
                        Finaldata.Add(Sendingdata);
                    }
                    // serviceResponse.Errcode = 200;
                }
            }
            catch (Exception ex)
            {

            }
            return Finaldata;
        }
        public static List<ReversePickupDb> GetReverseAllData(DataSet pds)
        {
            List<ReversePickupDb> userProfile = new List<ReversePickupDb>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    ReversePickupDb reversePickup = new ReversePickupDb();
                    reversePickup.customFields = new List<CustomField>();

                    PickUpAddress pickUpAddress = new PickUpAddress();
                    Dimension dimension = new Dimension();
                    CustomField customField = new CustomField();

                    reversePickup.reversePickupCode = pds.Tables[0].Rows[i]["reversepickupcode"].ToString();
                    reversePickup.pickupInstruction = pds.Tables[0].Rows[i]["pickupInstruction"].ToString();
                    reversePickup.trackingLink = pds.Tables[0].Rows[i]["trackingLink"].ToString();
                    reversePickup.shippingCourier = pds.Tables[0].Rows[i]["shippingCourier"].ToString();
                    reversePickup.trackingNumber = pds.Tables[0].Rows[i]["trackingNumber"].ToString();
                    reversePickup.shippingProviderCode = pds.Tables[0].Rows[i]["shippingProviderCode"].ToString();
                    reversePickup.FaciityCode = pds.Tables[0].Rows[i]["Facility"].ToString();

                    pickUpAddress.id = pds.Tables[0].Rows[i]["id"].ToString();
                    pickUpAddress.name = pds.Tables[0].Rows[i]["name"].ToString();
                    pickUpAddress.addressLine1 = pds.Tables[0].Rows[i]["addressLine1"].ToString();
                    pickUpAddress.addressLine2 = pds.Tables[0].Rows[i]["addressLine2"].ToString();
                    pickUpAddress.city = pds.Tables[0].Rows[i]["city"].ToString();
                    pickUpAddress.state = pds.Tables[0].Rows[i]["state"].ToString();
                    pickUpAddress.phone = pds.Tables[0].Rows[i]["phone"].ToString();
                    pickUpAddress.pincode = pds.Tables[0].Rows[i]["pincode"].ToString();
                    reversePickup.pickUpAddress = pickUpAddress;

                    dimension.boxLength = pds.Tables[0].Rows[i]["boxLength"].ToString();
                    dimension.boxWidth = pds.Tables[0].Rows[i]["boxWidth"].ToString();
                    dimension.boxHeight = pds.Tables[0].Rows[i]["boxHeight"].ToString();
                    dimension.boxWeight = pds.Tables[0].Rows[i]["boxWeight"].ToString();
                    reversePickup.dimension = dimension;

                    customField.name = pds.Tables[0].Rows[i]["name"].ToString();
                    customField.value = pds.Tables[0].Rows[i]["value"].ToString();
                    reversePickup.customFields.Add(customField);

                    userProfile.Add(reversePickup);
                }

                return userProfile;

            }
            catch (Exception ex)
            {
                return userProfile = null;
            }

        }
        public static List<FacilityDetails> GetFacilityCode(DataSet pds)
        {
            List<FacilityDetails> facilityList = new List<FacilityDetails>();

            try
            {
                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                    {
                        FacilityDetails facilityDetails = new FacilityDetails();
                        facilityDetails.facilityCode = pds.Tables[0].Rows[i]["facilityCode"].ToString();
                        facilityList.Add(facilityDetails);
                    }
                    // serviceResponse.Errcode = 200;
                }
            }
            catch (Exception ex)
            {

            }
            return facilityList;
        }
        public static List<FacilityMaintain> GetFacilityData(DataSet pds)
        {
            List<FacilityMaintain> FacilityList = new List<FacilityMaintain>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    FacilityMaintain returncode = new FacilityMaintain();
                    returncode.FacilityCode = pds.Tables[0].Rows[i]["FacilityCode"].ToString();
                    returncode.FacilityName = pds.Tables[0].Rows[i]["FacilityName"].ToString();
                    returncode.Address = pds.Tables[0].Rows[i]["Address"].ToString();
                    returncode.City = pds.Tables[0].Rows[i]["City"].ToString();
                    returncode.State = pds.Tables[0].Rows[i]["State"].ToString();
                    returncode.Pincode = pds.Tables[0].Rows[i]["Pincode"].ToString();
                    returncode.Mobile = pds.Tables[0].Rows[i]["Mobile_number"].ToString();
                    returncode.Region = pds.Tables[0].Rows[i]["Region"].ToString();
                    returncode.Email = pds.Tables[0].Rows[i]["email"].ToString();
                    returncode.Instance = pds.Tables[0].Rows[i]["Instance"].ToString();

                    FacilityList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return FacilityList;
        }
        public static List<TrackingStatusDb> GetTrackingDetails(DataSet pds)
        {
            List<TrackingStatusDb> FacilityList = new List<TrackingStatusDb>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TrackingStatusDb returncode = new TrackingStatusDb();
                    returncode.providerCode = pds.Tables[0].Rows[i]["providerCode"].ToString();
                    returncode.trackingStatus = pds.Tables[0].Rows[i]["trackingStatus"].ToString();
                    returncode.trackingNumber = pds.Tables[0].Rows[i]["trackingNumber"].ToString();
                    returncode.statusDate = pds.Tables[0].Rows[i]["statusDate"].ToString();
                    returncode.shipmentTrackingStatusName = pds.Tables[0].Rows[i]["shipmentTrackingStatusName"].ToString();
                    returncode.facilitycode = pds.Tables[0].Rows[i]["facilitycode"].ToString();
                    returncode.Instance = pds.Tables[0].Rows[i]["Instance"].ToString();
                    FacilityList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return FacilityList;
        }
        public static List<TruckDetails> GetTruckDetails(DataSet pds)
        {
            List<TruckDetails> FacilityList = new List<TruckDetails>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TruckDetails returncode = new TruckDetails();
                    returncode.Details = pds.Tables[0].Rows[i]["Details"].ToString();
                    returncode.Instance = pds.Tables[0].Rows[i]["Instance"].ToString();
                    FacilityList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return FacilityList;
        }
        public static List<RegionMaster> MPGetRegionDetails(DataSet pds)
        {
            List<RegionMaster> FacilityList = new List<RegionMaster>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    RegionMaster returncode = new RegionMaster();
                    returncode.State = pds.Tables[0].Rows[i]["State"].ToString();
                    returncode.Region = pds.Tables[0].Rows[i]["Region"].ToString();
                    FacilityList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return FacilityList;
        }
        public static List<TrackingMaster> GetTrackingStatusDetails(DataSet pds)
        {
            List<TrackingMaster> FacilityList = new List<TrackingMaster>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TrackingMaster returncode = new TrackingMaster();
                    returncode.UniwareStatus = pds.Tables[0].Rows[i]["UniwareStatus"].ToString();
                    returncode.PandoStatus = pds.Tables[0].Rows[i]["PandoStatus"].ToString();
                    returncode.CourierName = pds.Tables[0].Rows[i]["CourierName"].ToString();
                    FacilityList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return FacilityList;
        }
        public static List<TrackingMaster> GetCourierNameDetails(DataSet pds)
        {
            List<TrackingMaster> FacilityList = new List<TrackingMaster>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TrackingMaster returncode = new TrackingMaster();
                    returncode.CourierName = pds.Tables[0].Rows[i]["CourierName"].ToString();
                    FacilityList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return FacilityList;
        }

        public static List<TrackingLinkMapping> GetTrackingLink(DataSet pds)
        {
            List<TrackingLinkMapping> MappingList = new List<TrackingLinkMapping>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TrackingLinkMapping returncode = new TrackingLinkMapping();
                    returncode.CourierName = pds.Tables[0].Rows[i]["CourierName"].ToString();
                    returncode.TrackingLink = pds.Tables[0].Rows[i]["TrackingLink"].ToString();
                    MappingList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return MappingList;
        }


        public static ServiceResponse<MenusAccess> GetRoleMenuAccess(DataSet pds)
        {
            ServiceResponse<MenusAccess> serviceResponse = new ServiceResponse<MenusAccess>();
            List<RoleMenuAccess> RoleMenuAccessList = new List<RoleMenuAccess>();
            List<Menus> MenuLists = new List<Menus>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    Menus uobj = new Menus
                    {
                        MenuID = Convert.ToInt32(pds.Tables[0].Rows[i]["MenuID"]),
                        MenuTitle = pds.Tables[0].Rows[i]["MenuTitle"].ToString(),
                        ActionName = pds.Tables[0].Rows[i]["ActionName"].ToString(),
                        ControllerName = pds.Tables[0].Rows[i]["ControllerName"].ToString()
                    };

                    MenuLists.Add(uobj);
                }

                for (int i = 0; i < pds.Tables[1].Rows.Count; i++)
                {
                    RoleMenuAccess uobj = new RoleMenuAccess
                    {
                        MenuAccessId = Convert.ToInt32(pds.Tables[1].Rows[i]["MenuAccessId"]),
                        MenuId = Convert.ToInt32(pds.Tables[1].Rows[i]["MenuId"]),
                        Role = Convert.ToInt16(pds.Tables[1].Rows[i]["Role"]),
                        UserID = Convert.ToInt32(pds.Tables[1].Rows[i]["UserID"])
                    };

                    RoleMenuAccessList.Add(uobj);
                }
                MenusAccess MenusAccess = new MenusAccess();
                MenusAccess.RoleMenuAccessesList = RoleMenuAccessList;
                MenusAccess.MenusList = MenuLists;
                serviceResponse.ObjectParam = MenusAccess;
            }
            catch (Exception ex)
            {
                serviceResponse.Errcode = 500;
                serviceResponse.Errdesc = ex.Message;
                serviceResponse.ObjectParam = new MenusAccess();
            }
            return serviceResponse;
        }
        public static List<UserProfile> GetRoleMaster(DataSet pds)
        {
            ServiceResponse<List<UserProfile>> serviceResponse = new ServiceResponse<List<UserProfile>>();
            //List<RoleMenuAccess> RoleMenuAccessList = new List<RoleMenuAccess>();
            List<UserProfile> MenuLists = new List<UserProfile>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    UserProfile uobj = new UserProfile
                    {
                        Roleid = Convert.ToInt32(pds.Tables[0].Rows[i]["RoleId"]),
                        RoleName = pds.Tables[0].Rows[i]["RoleName"].ToString()
                    };

                    MenuLists.Add(uobj);
                }

                //serviceResponse.ObjectParam = MenuLists;
            }
            catch (Exception ex)
            {
                //serviceResponse.Errcode = 500;
                //serviceResponse.Errdesc = ex.Message;
                //serviceResponse.ObjectParam = new List<UserProfile>();
            }
            return MenuLists;
        }

        public static List<ShippingStatus> GetShippingMaster(DataSet pds)
        {
            List<ShippingStatus> ShippingStatus = new List<ShippingStatus>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    ShippingStatus returncode = new ShippingStatus();
                    returncode.StatusName = pds.Tables[0].Rows[i]["StatusName"].ToString();
                    //returncode.Instanc = pds.Tables[0].Rows[i]["Instance"].ToString();
                    ShippingStatus.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ShippingStatus;
        }


        public static ServiceResponse<DashboardsLists> GetDashBoardDetails(DataSet pds)
        {
            ServiceResponse<DashboardsLists> DashboardDetails = new ServiceResponse<DashboardsLists>();
            DashboardsLists dashboardsLists = new DashboardsLists();
            List<TDashboardDetails> userProfile = new List<TDashboardDetails>();
            List<TrackingDetails> trackingDetails = new List<TrackingDetails>();
            try
            {
                for (int i = 0; i < pds.Tables[1].Rows.Count; i++)
                {
                    TrackingDetails details = new TrackingDetails();
                    details.StatusName = pds.Tables[1].Rows[i]["Name"].ToString();
                    details.Count = pds.Tables[1].Rows[i]["Total"].ToString();
                    trackingDetails.Add(details);
                }
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TDashboardDetails details = new TDashboardDetails();
                    details.TrackingNumber = pds.Tables[0].Rows[i]["TrackingNumber"].ToString();
                    details.DisplayOrder = pds.Tables[0].Rows[i]["DisplayOrderCode"].ToString();
                    details.ShipmentID = pds.Tables[0].Rows[i]["ShipmentId"].ToString();
                    details.LatestStatus = pds.Tables[0].Rows[i]["Status"].ToString();
                    details.CourierName = pds.Tables[0].Rows[i]["CourierName"].ToString();
                    details.trackingLink = pds.Tables[0].Rows[i]["TrackingLink"].ToString();
                    details.CustomerName = pds.Tables[0].Rows[i]["CustomerName"].ToString();
                    details.CustomerPhone = pds.Tables[0].Rows[i]["CustomerPhone"].ToString();
                    details.FacilityCode = pds.Tables[0].Rows[i]["FacilityCode"].ToString();
                    details.CustomerCity = pds.Tables[0].Rows[i]["CustomerCity"].ToString();
                    details.InvoiceDate = pds.Tables[0].Rows[i]["InvoiceDate"].ToString();
                    details.MaterialCode = pds.Tables[0].Rows[i]["MaterialCode"].ToString();
                    details.Quantity = pds.Tables[0].Rows[i]["Quantity"].ToString();
                    details.UOM = pds.Tables[0].Rows[i]["UOM"].ToString();
                    details.IndentID = pds.Tables[0].Rows[i]["IndentID"].ToString();
                    details.Pincode = pds.Tables[0].Rows[i]["Pincode"].ToString();
                    details.state = pds.Tables[0].Rows[i]["state"].ToString();
                    details.Region = pds.Tables[0].Rows[i]["Region"].ToString();
                    details.MileStone = pds.Tables[0].Rows[i]["MileStone"].ToString();
                    //skucodes.Add(sKucode);
                    userProfile.Add(details);//new CodesErrorDetails();// Add(sKucode);
                }
                dashboardsLists.dashboardDetails = userProfile;
                dashboardsLists.trackingDetails = trackingDetails;
                DashboardDetails.ObjectParam = dashboardsLists;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return DashboardDetails;
        }
        public static List<TDashboardDetails> GetDashBoardDetailsByName(DataSet pds)
        {
            List<TDashboardDetails> DashboardDetails = new List<TDashboardDetails>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TDashboardDetails details = new TDashboardDetails();
                    details.TrackingNumber = pds.Tables[0].Rows[i]["TrackingNumber"].ToString();
                    details.DisplayOrder = pds.Tables[0].Rows[i]["DisplayOrderCode"].ToString();
                    details.ShipmentID = pds.Tables[0].Rows[i]["ShipmentId"].ToString();
                    details.LatestStatus = pds.Tables[0].Rows[i]["Status"].ToString();
                    details.CourierName = pds.Tables[0].Rows[i]["CourierName"].ToString();
                    details.trackingLink = pds.Tables[0].Rows[i]["TrackingLink"].ToString();
                    details.CustomerName = pds.Tables[0].Rows[i]["CustomerName"].ToString();
                    details.CustomerPhone = pds.Tables[0].Rows[i]["CustomerPhone"].ToString();
                    details.FacilityCode = pds.Tables[0].Rows[i]["FacilityCode"].ToString();
                    details.CustomerCity = pds.Tables[0].Rows[i]["CustomerCity"].ToString();
                    details.InvoiceDate = pds.Tables[0].Rows[i]["InvoiceDate"].ToString();
                    details.MaterialCode = pds.Tables[0].Rows[i]["MaterialCode"].ToString();
                    details.Quantity = pds.Tables[0].Rows[i]["Quantity"].ToString();
                    details.UOM = pds.Tables[0].Rows[i]["UOM"].ToString();
                    details.IndentID = pds.Tables[0].Rows[i]["IndentID"].ToString();
                    details.Pincode = pds.Tables[0].Rows[i]["Pincode"].ToString();
                    details.state = pds.Tables[0].Rows[i]["state"].ToString();
                    details.Region = pds.Tables[0].Rows[i]["Region"].ToString();
                    details.MileStone = pds.Tables[0].Rows[i]["MileStone"].ToString();


                    //skucodes.Add(sKucode);
                    DashboardDetails.Add(details);//new CodesErrorDetails();// Add(sKucode);
                    //DashboardDetails.Add(details);
                }
                //dashboardsLists. = userProfile;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return DashboardDetails;
        }
        public static List<TDashboardDetails> GetTrackingDetailsByName(DataSet pds)
        {
            List<TDashboardDetails> TrackingDetails = new List<TDashboardDetails>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TDashboardDetails details = new TDashboardDetails();
                    details.TrackingNumber = pds.Tables[0].Rows[i]["TrackingNumber"].ToString();
                    details.DisplayOrder = pds.Tables[0].Rows[i]["DisplayOrderCode"].ToString();
                    details.OrderStatus = pds.Tables[0].Rows[i]["OrderStatus"].ToString();
                    details.CourierName = pds.Tables[0].Rows[i]["CourierName"].ToString();
                    details.trackingLink = pds.Tables[0].Rows[i]["TrackingLink"].ToString();
                    details.CustomerName = pds.Tables[0].Rows[i]["CustomerName"].ToString();
                    details.CustomerPhone = pds.Tables[0].Rows[i]["CustomerPhone"].ToString();
                    details.CustomerCity = pds.Tables[0].Rows[i]["CustomerCity"].ToString();
                    details.Pincode = pds.Tables[0].Rows[i]["Pincode"].ToString();
                    TrackingDetails.Add(details);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TrackingDetails;
        }
        public static bool MapinsertTrackingDetails(List<TrackingStatusDb> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Id");
                dtinstcode.Columns.Add("providerCode");
                dtinstcode.Columns.Add("trackingNumber");
                dtinstcode.Columns.Add("trackingStatus");
                dtinstcode.Columns.Add("statusDate");
                dtinstcode.Columns.Add("shipmentTrackingStatusName");
                dtinstcode.Columns.Add("facilitycode");
                dtinstcode.Columns.Add("Instance");


                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Id"] = elements[i].Id;
                    dr["providerCode"] = elements[i].providerCode;
                    dr["trackingNumber"] = elements[i].trackingNumber;
                    dr["trackingStatus"] = elements[i].trackingStatus;
                    dr["statusDate"] = elements[i].statusDate;
                    dr["shipmentTrackingStatusName"] = elements[i].shipmentTrackingStatusName;
                    dr["facilitycode"] = elements[i].facilitycode;
                    dr["Instance"] = elements[i].Instance;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertTrackingDetails(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public static List<TDashboardDetails> GetHistoryData(DataSet pds)
        {
            List<TDashboardDetails> HistoryData = new List<TDashboardDetails>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TDashboardDetails returncode = new TDashboardDetails();
                    returncode.TrackingNumber = pds.Tables[0].Rows[i]["TrackingNumber"].ToString();
                    returncode.DisplayOrder = pds.Tables[0].Rows[i]["DisplayOrderCode"].ToString();
                    returncode.ShipmentID = pds.Tables[0].Rows[i]["ShipmentId"].ToString();
                    returncode.LatestStatus = pds.Tables[0].Rows[i]["Status"].ToString();
                    returncode.MileStone = pds.Tables[0].Rows[i]["MileStone"].ToString();
                    returncode.CourierName = pds.Tables[0].Rows[i]["CourierName"].ToString();
                    returncode.trackingLink = pds.Tables[0].Rows[i]["TrackingLink"].ToString();
                    returncode.CustomerName = pds.Tables[0].Rows[i]["CustomerName"].ToString();
                    returncode.CustomerPhone = pds.Tables[0].Rows[i]["CustomerPhone"].ToString();
                    returncode.FacilityCode = pds.Tables[0].Rows[i]["FacilityCode"].ToString();
                    returncode.CustomerCity = pds.Tables[0].Rows[i]["CustomerCity"].ToString();
                    returncode.InvoiceDate = pds.Tables[0].Rows[i]["InvoiceDate"].ToString();
                    returncode.MaterialCode = pds.Tables[0].Rows[i]["MaterialCode"].ToString();
                    returncode.Quantity = pds.Tables[0].Rows[i]["Quantity"].ToString();
                    returncode.UOM = pds.Tables[0].Rows[i]["UOM"].ToString();
                    returncode.IndentID = pds.Tables[0].Rows[i]["IndentID"].ToString();
                    returncode.Pincode = pds.Tables[0].Rows[i]["Pincode"].ToString();
                    returncode.state = pds.Tables[0].Rows[i]["state"].ToString();
                    returncode.Region = pds.Tables[0].Rows[i]["Region"].ToString();
                    HistoryData.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return HistoryData;
        }

        public static List<AllocateshippingPando> getallocateshippingpost(DataSet pds)
        {
            //List<UpdateShippingpackage> skucodes = new List<UpdateShippingpackage>();

            List<AllocateshippingPando> userProfile = new List<AllocateshippingPando>();

            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    AllocateshippingPando Updateship = new AllocateshippingPando();

                    Updateship.shippingPackageCode = pds.Tables[0].Rows[i]["shippingPackageCode"].ToString();
                    Updateship.shippingLabelMandatory = pds.Tables[0].Rows[i]["shippingLabelMandatory"].ToString();
                    Updateship.shippingProviderCode = pds.Tables[0].Rows[i]["shippingProviderCode"].ToString();
                    Updateship.shippingCourier = pds.Tables[0].Rows[i]["shippingCourier"].ToString();
                    Updateship.trackingNumber = pds.Tables[0].Rows[i]["trackingNumber"].ToString();
                    Updateship.tracking_link_url = pds.Tables[0].Rows[i]["trackingLink"].ToString();
                    userProfile.Add(Updateship);
                }
                return userProfile;
                //userProfile.AddRange(sKucode)
            }
            catch (Exception ex)
            {

                //throw ex;
                return userProfile = null;
            }


        }
        public static List<TrackingStatusDb> getTrackingstatuspost(DataSet pds)
        {
            //List<UpdateShippingpackage> skucodes = new List<UpdateShippingpackage>();

            List<TrackingStatusDb> userProfile = new List<TrackingStatusDb>();

            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TrackingStatusDb Updateship = new TrackingStatusDb();

                    Updateship.providerCode = pds.Tables[0].Rows[i]["providerCode"].ToString();
                    Updateship.trackingNumber = pds.Tables[0].Rows[i]["trackingNumber"].ToString();
                    Updateship.trackingStatus = pds.Tables[0].Rows[i]["trackingStatus"].ToString();
                    Updateship.statusDate = pds.Tables[0].Rows[i]["statusDate"].ToString();
                    Updateship.shipmentTrackingStatusName = pds.Tables[0].Rows[i]["shipmentTrackingStatusName"].ToString();
                    Updateship.facilitycode = pds.Tables[0].Rows[i]["facilitycode"].ToString();
                    userProfile.Add(Updateship);
                }
                return userProfile;
                //userProfile.AddRange(sKucode)
            }
            catch (Exception ex)
            {

                //throw ex;
                return userProfile = null;
            }


        }
        public static List<CityMasterEntity> GetCityList(DataSet pds)
        {
            List<CityMasterEntity> CityList = new List<CityMasterEntity>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    CityMasterEntity returncode = new CityMasterEntity();
                    returncode.ReferenceName = pds.Tables[0].Rows[i]["ReferenceCityName"].ToString();
                    returncode.ActualName = pds.Tables[0].Rows[i]["ActualName"].ToString();
                    CityList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return CityList;
        }

        public static List<DashboardStatusMasterEntity> GetDashboardStatusList(DataSet pds)
        {
            List<DashboardStatusMasterEntity> CityList = new List<DashboardStatusMasterEntity>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    DashboardStatusMasterEntity returncode = new DashboardStatusMasterEntity();
                    returncode.TrackingStatus = pds.Tables[0].Rows[i]["Tracking_Status"].ToString();
                    returncode.DashboardStatus = pds.Tables[0].Rows[i]["Dashboard_Status"].ToString();
                    CityList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return CityList;
        }
        //public static List<TruckDetails> GetLast30daysStatus(DataSet pds)
        //{
        //    List<TruckDetails> FacilityList = new List<TruckDetails>();
        //    try
        //    {
        //        for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
        //        {
        //            TruckDetails returncode = new TruckDetails();
        //            returncode.Details = pds.Tables[0].Rows[i]["Details"].ToString();
        //            returncode.Instance = pds.Tables[0].Rows[i]["Instance"].ToString();
        //            FacilityList.Add(returncode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return FacilityList;
        //}
        public static List<TrackingStatusDb> GetLast30daysStatus(DataSet pds)
        {
            List<TrackingStatusDb> FacilityList = new List<TrackingStatusDb>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    TrackingStatusDb returncode = new TrackingStatusDb();
                    returncode.providerCode = pds.Tables[0].Rows[i]["providerCode"].ToString();
                    returncode.trackingStatus = pds.Tables[0].Rows[i]["trackingStatus"].ToString();
                    returncode.trackingNumber = pds.Tables[0].Rows[i]["trackingNumber"].ToString();
                    returncode.statusDate = pds.Tables[0].Rows[i]["statusDate"].ToString();
                    returncode.shipmentTrackingStatusName = pds.Tables[0].Rows[i]["shipmentTrackingStatusName"].ToString();
                    FacilityList.Add(returncode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return FacilityList;
        }
        public static List<ReturnAllocateShippingDb> GetReturnAllocateShipping(DataSet pds)
        {

            List<ReturnAllocateShippingDb> returnallocate = new List<ReturnAllocateShippingDb>();

            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    ReturnAllocateShippingDb allocate = new ReturnAllocateShippingDb();


                    allocate.shippingPackageCode = pds.Tables[0].Rows[i]["shippingPackageCode"].ToString();
                    allocate.shippingLabelMandatory = pds.Tables[0].Rows[i]["shippingLabelMandatory"].ToString();
                    allocate.shippingProviderCode = pds.Tables[0].Rows[i]["shippingProviderCode"].ToString();
                    allocate.shippingCourier = pds.Tables[0].Rows[i]["shippingCourier"].ToString();
                    allocate.trackingNumber = pds.Tables[0].Rows[i]["trackingNumber"].ToString();
                    allocate.FacilityCode = pds.Tables[0].Rows[i]["facilityCode"].ToString();
                    allocate.trackingLink = pds.Tables[0].Rows[i]["trackingLink"].ToString();

                    returnallocate.Add(allocate);

                }
                return returnallocate;
            }
            catch (Exception ex)
            {

                return returnallocate = null;
            }
        }

        public static ServiceResponse<List<TrackOrderDto>> GetTrackOrder(DataSet pds)
        {
            ServiceResponse<List<TrackOrderDto>> response = new ServiceResponse<List<TrackOrderDto>>();
            List<TrackOrderDto> orders = new List<TrackOrderDto>();

            try
            {

                if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
                {
                    var groupedOrders = pds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            OrderID = row["OrderID"].ToString(),
                            OrderDate = row["OrderDate"].ToString()
                        });

                    foreach (var group in groupedOrders)
                    {
                        TrackOrderDto orderDto = new TrackOrderDto
                        {
                            OrderID = group.Key.OrderID,
                            OrderDate = group.Key.OrderDate,
                            Items = new List<OrderItemDto>()
                        };

                        foreach (var row in group)
                        {
                            OrderItemDto item = new OrderItemDto
                            {
                                ProductName = row["ProductName"].ToString(),
                                TrackingStatus = row["TrackingStatus"].ToString(),
                                TrackingLink = row["TrackingLink"].ToString()
                            };
                            orderDto.Items.Add(item);
                        }

                        // Set item count per order
                        orderDto.ItemCount = orderDto.Items.Count;

                        orders.Add(orderDto);
                    }

                    // Set response values
                    response.ObjectParam = orders;
                    response.Id = orders.Count; // 👈 Total order count here
                    response.IsSuccess = true;
                    response.Errcode = 200;
                    response.Message = "Success";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No data found";
                    response.Id = 0;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Error: " + ex.Message;
                response.ObjectParam = null;
            }

            return response;
        }



    }
}
