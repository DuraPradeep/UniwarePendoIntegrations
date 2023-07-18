using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                        Sendingdata.action_by = pds.Tables[0].Rows[i]["eway_bill_date"].ToString();
                        Sendingdata.action_type = pds.Tables[0].Rows[i]["action_type"].ToString();
                        Sendingdata.clear = pds.Tables[0].Rows[i]["clear"].ToString();
                        Sendingdata.weight = pds.Tables[0].Rows[i]["weight"].ToString();
                        Sendingdata.volume = pds.Tables[0].Rows[i]["volume"].ToString();
                        Sendingdata.ship_to = pds.Tables[0].Rows[i]["ship_to"].ToString();
                        Sendingdata.sold_to = pds.Tables[0].Rows[i]["sold_to"].ToString();
                        Sendingdata.invoice_number = pds.Tables[0].Rows[i]["invoice_number"].ToString();
                        Sendingdata.invoice_amount = pds.Tables[0].Rows[i]["invoice_amount"].ToString();
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
                    returncode.Code= pds.Tables[0].Rows[i]["code"].ToString();
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
                    returncode.reference_number= pds.Tables[0].Rows[i]["reference_number"].ToString();
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
                    returncode.invoice_date= pds.Tables[0].Rows[i]["invoice_date"].ToString();
                    returncode.pickup_reference_number= pds.Tables[0].Rows[i]["pickup_reference_number"].ToString();

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

    }
}
