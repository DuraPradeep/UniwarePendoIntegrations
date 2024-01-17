using Newtonsoft.Json;
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
        public static ServiceResponse<List<ReturnorderCode>> GetReturnOrderCode(DataSet pds)
        {
            ServiceResponse<List<ReturnorderCode>> skucodes = new ServiceResponse<List<ReturnorderCode>>();
            List<ReturnorderCode> userProfile = new List<ReturnorderCode>();
            try
            {
                for (int i = 0; i < pds.Tables[0].Rows.Count; i++)
                {
                    ReturnorderCode returncode = new ReturnorderCode();
                    returncode.code = pds.Tables[0].Rows[i]["code"].ToString();
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
                    CustomField customField=new CustomField();

                    reversePickup.reversePickupCode = pds.Tables[0].Rows[i]["reversepickupcode"].ToString();
                    reversePickup.pickupInstruction = pds.Tables[0].Rows[i]["pickupInstruction"].ToString();
                    reversePickup.trackingLink = pds.Tables[0].Rows[i]["trackingLink"].ToString();
                    reversePickup.shippingCourier = pds.Tables[0].Rows[i]["shippingCourier"].ToString();
                    reversePickup.trackingNumber = pds.Tables[0].Rows[i]["trackingNumber"].ToString();
                    reversePickup.shippingProviderCode = pds.Tables[0].Rows[i]["shippingProviderCode"].ToString();
                    reversePickup.FaciityCode = pds.Tables[0].Rows[i]["Facility"].ToString();

                    pickUpAddress.id= pds.Tables[0].Rows[i]["id"].ToString();
                    pickUpAddress.name= pds.Tables[0].Rows[i]["name"].ToString();
                    pickUpAddress.addressLine1= pds.Tables[0].Rows[i]["addressLine1"].ToString();
                    pickUpAddress.addressLine2= pds.Tables[0].Rows[i]["addressLine2"].ToString();
                    pickUpAddress.city= pds.Tables[0].Rows[i]["city"].ToString();
                    pickUpAddress.state=pds.Tables[0].Rows[i]["state"].ToString();
                    pickUpAddress.phone= pds.Tables[0].Rows[i]["phone"].ToString();
                    pickUpAddress.pincode= pds.Tables[0].Rows[i]["pincode"].ToString();
                    reversePickup.pickUpAddress = pickUpAddress;

                    dimension.boxLength = pds.Tables[0].Rows[i]["boxLength"].ToString();
                    dimension.boxWidth = pds.Tables[0].Rows[i]["boxWidth"].ToString();
                    dimension.boxHeight = pds.Tables[0].Rows[i]["boxHeight"].ToString();
                    dimension.boxWeight = pds.Tables[0].Rows[i]["boxWeight"].ToString();
                    reversePickup.dimension = dimension;

                    customField.name= pds.Tables[0].Rows[i]["name"].ToString();
                    customField.value= pds.Tables[0].Rows[i]["value"].ToString();
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
                    returncode.Instance =  pds.Tables[0].Rows[i]["Instance"].ToString();
                   
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
    }
}
