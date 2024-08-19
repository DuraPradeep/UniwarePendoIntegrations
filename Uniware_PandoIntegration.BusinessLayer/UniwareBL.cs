using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.DataAccessLayer;
using Uniware_PandoIntegration.Entities;


namespace Uniware_PandoIntegration.BusinessLayer
{

    public class UniwareBL
    {
        public bool InsertCode(List<Element> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("Instance");

                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].code;
                    dr["Instance"] = elements[i].source;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertCodeInUniware(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<Salesorder> GetCode(string Instance, string Enviornment)
        {
            List<Salesorder> serviceResponse = new List<Salesorder>();
            try
            {
                //CreateLog($" Get Code from DB ");

                serviceResponse = Mapper.GetCodes(SPWrapper.GetCodeDB(Instance, Enviornment));
                //CreateLog($" Get Code from DB Data{serviceResponse} ");
            }
            catch (Exception Ex)
            {
                //CreateLog($"Error: {Ex.Message}");
            }
            return serviceResponse;
        }
        public bool insertSalesDTO(List<SaleOrderDTO> salesordrsearch, string Enviornment)
        {
            bool res;
            try
            {
                //CreateLog($"SalesOrder DTO data :-{salesordrsearch}");

                DataTable dt = new DataTable();
                dt.Columns.Add("Code");
                dt.Columns.Add("displayOrderCode");
                dt.Columns.Add("Instance");
                for (int i = 0; i < salesordrsearch.Count; i++)
                {
                    DataRow myDataRow = dt.NewRow();
                    myDataRow["Code"] = salesordrsearch[i].code;
                    myDataRow["displayOrderCode"] = salesordrsearch[i].displayOrderCode;
                    myDataRow["Instance"] = salesordrsearch[i].source;
                    dt.Rows.Add(myDataRow);
                }
                res = SPWrapper.InsertSaleOrderDTO(dt, Enviornment);
                //CreateLog($"SalesOrder DTO data insert Status :-{res}");

            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }

        public bool InsertAddrsss(List<Address> addresses, string Enviornment)
        {
            bool res;
            try
            {
                //CreateLog($"Address data insert Data :-{addresses}");
                //CreateLog($"Address data insert Data :-{addresses}");

                DataTable dtaddress = new DataTable();
                dtaddress.Columns.Add("Code");
                dtaddress.Columns.Add("name");
                dtaddress.Columns.Add("addressLine1");
                dtaddress.Columns.Add("addressLine2");
                dtaddress.Columns.Add("city");
                dtaddress.Columns.Add("state");
                dtaddress.Columns.Add("pincode");
                dtaddress.Columns.Add("phone");
                dtaddress.Columns.Add("email");
                dtaddress.Columns.Add("AddressId");
                dtaddress.Columns.Add("Instance");

                for (int i = 0; i < addresses.Count; i++)
                {
                    DataRow dr = dtaddress.NewRow();
                    dr["Code"] = addresses[i].Code;
                    dr["name"] = addresses[i].name;
                    dr["addressLine1"] = addresses[i].addressLine1;
                    dr["addressLine2"] = addresses[i].addressLine2;
                    dr["city"] = addresses[i].city;
                    dr["state"] = addresses[i].state;
                    dr["pincode"] = addresses[i].pincode;
                    dr["phone"] = addresses[i].phone;
                    dr["email"] = addresses[i].email;
                    dr["AddressId"] = addresses[i].id;
                    dr["Instance"] = addresses[i].Source;
                    dtaddress.Rows.Add(dr);

                }
                res = SPWrapper.Insertaddress(dtaddress, Enviornment);
                //CreateLog($"Address data insert status :-{res}");

            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

            return res;
        }
        public bool InsertBill(List<ShippingPackage> shippingPackages, string Enviornment)
        {
            bool res = false;
            try
            {
                //CreateLog($"shipping data :-{shippingPackages}");
                DataTable dtshipping = new DataTable();
                dtshipping.Columns.Add("Code");
                dtshipping.Columns.Add("invoiceCode");
                dtshipping.Columns.Add("invoiceDate");
                dtshipping.Columns.Add("Status");
                dtshipping.Columns.Add("Instance");
                for (int k = 0; k < shippingPackages.Count; k++)
                {
                    DataRow drbilling = dtshipping.NewRow();
                    drbilling["Code"] = shippingPackages[k].code;
                    drbilling["invoiceCode"] = shippingPackages[k].invoiceCode;
                    drbilling["invoiceDate"] = shippingPackages[k].invoiceDate;
                    drbilling["Status"] = shippingPackages[k].status;
                    drbilling["Instance"] = shippingPackages[k].Source;
                    dtshipping.Rows.Add(drbilling);
                }
                res = SPWrapper.InsertShippingDetails(dtshipping, Enviornment);
                //CreateLog($"shipping data inserted status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }

        public bool insertItems(List<Items> items, string Enviornment)
        {
            bool res;
            DataTable dtitems = new DataTable();
            dtitems.Columns.Add("Code");
            dtitems.Columns.Add("quentity");
            dtitems.Columns.Add("Instance");
            try
            {
                //CreateLog($"Items inserted DB Data:-{items}");
                for (int k = 0; k < items.Count; k++)
                {
                    DataRow dritems = dtitems.NewRow();
                    dritems["Code"] = items[k].Code;
                    dritems["quentity"] = items[k].quantity;
                    dritems["Instance"] = items[k].Source;
                    dtitems.Rows.Add(dritems);
                }
                res = SPWrapper.InsertItems(dtitems, Enviornment);
                //CreateLog($"Items inserted DB status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }

        public bool insertsalesorderitem(List<SaleOrderItem> sitems, string Enviornment)
        {
            bool res;
            try
            {
                //CreateLog($"Sales Order Item inserted DB Data:-{sitems}");
                DataTable dtslesorder = new DataTable();
                dtslesorder.Columns.Add("Code");
                dtslesorder.Columns.Add("shippingPackageCode");
                dtslesorder.Columns.Add("OrderItem_id");
                dtslesorder.Columns.Add("itemsku");
                dtslesorder.Columns.Add("prepaidAmount");
                dtslesorder.Columns.Add("taxPercentage");
                dtslesorder.Columns.Add("TotalPrice");
                dtslesorder.Columns.Add("facilityCode");
                dtslesorder.Columns.Add("shippingAddressId");
                dtslesorder.Columns.Add("Instance");
                dtslesorder.Columns.Add("ShippingPackageStatus");

                for (int l = 0; l < sitems.Count; l++)
                {
                    DataRow drsalesorder = dtslesorder.NewRow();
                    drsalesorder["Code"] = sitems[l].code;
                    drsalesorder["shippingPackageCode"] = sitems[l].shippingPackageCode;
                    drsalesorder["OrderItem_id"] = sitems[l].id;
                    drsalesorder["itemsku"] = sitems[l].itemSku;
                    drsalesorder["prepaidAmount"] = sitems[l].prepaidAmount;
                    drsalesorder["taxPercentage"] = sitems[l].taxPercentage;
                    drsalesorder["TotalPrice"] = sitems[l].totalPrice;
                    drsalesorder["facilityCode"] = sitems[l].facilityCode;
                    drsalesorder["shippingAddressId"] = sitems[l].shippingAddressId;
                    drsalesorder["Instance"] = sitems[l].Source;
                    drsalesorder["ShippingPackageStatus"] = sitems[l].shippingPackageStatus;
                    dtslesorder.Rows.Add(drsalesorder);
                }
                res = SPWrapper.InsertsalesorderItems(dtslesorder, Enviornment);
                //CreateLog($"Sales Order Item inserted DB res:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }
        public List<SKucode> GetSKucodesBL(string Instance, string Enviornment)
        {
            List<SKucode> codes = new List<SKucode>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                codes = Mapper.Getskucodes(SPWrapper.GetSkuCodeDB(Instance, Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return codes;
        }

        public bool InsertitemSku(List<ItemTypeDTO> itemDTO, string Enviornment)
        {
            bool res;
            try
            {
                //CreateLog($"item sku code insert DB:-{itemDTO}");
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("Code");
                dtsku.Columns.Add("itemSku");
                dtsku.Columns.Add("itemDetailFieldsText");//itemDetailFieldsText  dtsku.Columns.Add("CategoryCode")
                dtsku.Columns.Add("Width");
                dtsku.Columns.Add("height");
                dtsku.Columns.Add("length");
                dtsku.Columns.Add("weight");
                dtsku.Columns.Add("Instance");

                for (int i = 0; i < itemDTO.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["Code"] = itemDTO[i].Code;
                    drsku["itemSku"] = itemDTO[i].skuType;
                    drsku["itemDetailFieldsText"] = itemDTO[i].itemDetailFieldsText;
                    drsku["Width"] = itemDTO[i].width;
                    drsku["height"] = itemDTO[i].height;
                    drsku["length"] = itemDTO[i].length;
                    drsku["weight"] = itemDTO[i].weight;
                    drsku["Instance"] = itemDTO[i].Source;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.IsertItemtypes(dtsku, Enviornment);
                //CreateLog($"item sku insert DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<Data> GetAllRecrdstosend(string Instance, string Enviornment)
        {
            List<Data> AllRes = new List<Data>();
            try
            {
                //CreateLog($" Get Code from DB ");

                AllRes = Mapper.GetSendingAllrecords(SPWrapper.GetAllSendRecords(Instance, Enviornment));
                //CreateLog($" Get Code from DB Data{AllRes} ");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return AllRes;
        }
        //public ServiceResponse<List<PostErrorDetails>> PostDataStatus()
        //{
        //    ServiceResponse<List<PostErrorDetails>> Triggerid = new ServiceResponse<List<PostErrorDetails>>();
        //    try
        //    {
        //        Triggerid = Mapper.PostErrorDetails(SPWrapper.PostStatus());
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return Triggerid;
        //}
        //public static void CreateLog(string message)
        //{
        //    Log.Information(message);
        //}
        public string InsertAllsendingData(List<Data> itemDatun, string Enviornment, string Instance)
        {
            string res;
            try
            {
                string id = "Tri_" + GenerateNumeric();
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("TriggerID");
                dtsku.Columns.Add("Name");
                dtsku.Columns.Add("Reference_Number");
                dtsku.Columns.Add("Address");
                dtsku.Columns.Add("City");
                dtsku.Columns.Add("State");
                dtsku.Columns.Add("Pincode");
                dtsku.Columns.Add("Region");
                dtsku.Columns.Add("Mobile_number");
                dtsku.Columns.Add("Email");
                dtsku.Columns.Add("Catgory");
                dtsku.Columns.Add("Delivefr_number");
                dtsku.Columns.Add("Mrp_Price");
                dtsku.Columns.Add("Material_Code");
                dtsku.Columns.Add("material_taxable_amount");
                dtsku.Columns.Add("quentity");
                dtsku.Columns.Add("Weight");
                dtsku.Columns.Add("Volume");
                dtsku.Columns.Add("Ship_to");
                dtsku.Columns.Add("Sold_to");
                dtsku.Columns.Add("Line_item_no");
                dtsku.Columns.Add("pickup_reference_number");
                dtsku.Columns.Add("Customer_type");
                dtsku.Columns.Add("source_system");
                dtsku.Columns.Add("division");
                dtsku.Columns.Add("quantity_unit");
                dtsku.Columns.Add("volume_unit");
                dtsku.Columns.Add("type");
                dtsku.Columns.Add("weight_unit");
                dtsku.Columns.Add("cust_category");
                dtsku.Columns.Add("cust_refid");
                dtsku.Columns.Add("expected_delivery_date");
                dtsku.Columns.Add("Instance");


                for (int i = 0; i < itemDatun.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["TriggerID"] = id;
                    drsku["Name"] = itemDatun[i].name;
                    drsku["Reference_Number"] = itemDatun[i].reference_number;
                    drsku["Address"] = itemDatun[i].address;
                    drsku["City"] = itemDatun[i].city;
                    drsku["State"] = itemDatun[i].state;
                    drsku["Pincode"] = itemDatun[i].pincode;
                    drsku["Region"] = itemDatun[i].region;
                    drsku["Mobile_number"] = itemDatun[i].mobile_number;
                    drsku["Email"] = itemDatun[i].email;
                    drsku["Catgory"] = itemDatun[i].category;
                    drsku["Delivefr_number"] = itemDatun[i].delivery_number;
                    drsku["Mrp_Price"] = itemDatun[i].mrp_price;
                    drsku["Material_Code"] = itemDatun[i].material_code;
                    drsku["material_taxable_amount"] = itemDatun[i].material_taxable_amount;
                    drsku["quentity"] = itemDatun[i].quantity;
                    drsku["Weight"] = itemDatun[i].weight;
                    drsku["Volume"] = itemDatun[i].volume;
                    drsku["Ship_to"] = itemDatun[i].ship_to;
                    drsku["Sold_to"] = itemDatun[i].sold_to;
                    drsku["Line_item_no"] = itemDatun[i].line_item_no;
                    drsku["pickup_reference_number"] = itemDatun[i].pickup_reference_number;
                    drsku["Customer_type"] = itemDatun[i].customer_type;
                    drsku["source_system"] = itemDatun[i].source_system;
                    drsku["division"] = itemDatun[i].division;
                    drsku["quantity_unit"] = itemDatun[i].quantity_unit;
                    drsku["volume_unit"] = itemDatun[i].volume_unit;
                    drsku["type"] = itemDatun[i].type;
                    drsku["weight_unit"] = itemDatun[i].weight_unit;
                    drsku["cust_category"] = itemDatun[i].cust_category;
                    drsku["cust_refid"] = itemDatun[i].cust_ref_id;
                    drsku["expected_delivery_date"] = itemDatun[i].expected_delivery_date;
                    drsku["Instance"] = Instance;
                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.IsertAllsendingrec(dtsku, Enviornment);
                //CreateLog($"itemsending data DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public bool UpdateSalesOrderError(List<ErrorDetails> ErrorDt, int type, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("Code");
                dtsku.Columns.Add("Reason");
                dtsku.Columns.Add("Status");

                for (int i = 0; i < ErrorDt.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["Code"] = ErrorDt[i].Code;
                    drsku["Reason"] = ErrorDt[i].Reason;
                    drsku["Status"] = ErrorDt[i].Status;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.UpdateSalesorderDetails(dtsku, type, Enviornment);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        //public bool UpdateSkucodeError(List<ErrorDetails> ErrorDt, int type)
        //{
        //    bool res;
        //    try
        //    {
        //        DataTable dtsku = new DataTable();
        //        dtsku.Columns.Add("Code");
        //        dtsku.Columns.Add("Reason");
        //        dtsku.Columns.Add("Status");


        //        for (int i = 0; i < ErrorDt.Count; i++)
        //        {
        //            DataRow drsku = dtsku.NewRow();
        //            drsku["Code"] = ErrorDt[i].SkuCode;
        //            drsku["Reason"] = ErrorDt[i].Reason;
        //            drsku["Status"] = ErrorDt[i].Status;

        //            dtsku.Rows.Add(drsku);
        //        }
        //        res = SPWrapper.UpdateSalesorderDetails(dtsku, type);

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return res;
        //}
        public void UpdatePostDatadetails(bool status, string Reason, string triggerid, string Enviornment)
        {
            try
            {
                SPWrapper.Updatedetailspostdata(status, Reason, triggerid, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Data> GetFailedSendRecords(string Instance, string Enviornment)
        {
            List<Data> AllRes = new List<Data>();
            try
            {
                //CreateLog($" Get Code from DB ");

                AllRes = Mapper.GetSendingAllrecords(SPWrapper.GetFailedSendRecords(Instance, Enviornment));
                //CreateLog($" Get Code from DB Data{AllRes} ");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return AllRes;
        }
        public List<UserInstance> GetInstanceFromTriggerdata(string Enviornment)
        {
            List<UserInstance> AllRes = new List<UserInstance>();
            try
            {
                //CreateLog($" Get Code from DB ");

                AllRes = Mapper.GetInstanceFromTriggerData(SPWrapper.GetInstanceFromTriggerTable(Enviornment));
                //CreateLog($" Get Code from DB Data{AllRes} ");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return AllRes;
        }
        public void UpdateStatusinTriggerTable(string triggerid, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateStatusinTriggerTable(triggerid, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Salesorder> GetCodeforRetrigger(string Enviornment)
        {
            List<Salesorder> serviceResponse = new List<Salesorder>();
            try
            {
                //CreateLog($" Get Code from DB ");

                serviceResponse = Mapper.GetCodesForRetrigger(SPWrapper.GetCoderetrigger(Enviornment));
                //CreateLog($" Get Code from DB Data{serviceResponse} ");
            }
            catch (Exception Ex)
            {
                //CreateLog($"Error: {Ex.Message}");
            }
            return serviceResponse;
        }
        public List<SKucode> GetSKucodesForRetrigger(string Enviornment)
        {
            List<SKucode> codes = new List<SKucode>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                codes = Mapper.Getskucodes(SPWrapper.GetSkuCodeforRetrigger(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return codes;
        }
        public List<Data> GetAllSendData(string Enviornment)
        {
            List<Data> sendData = new List<Data>();
            try
            {
                sendData = Mapper.GetSendData(SPWrapper.GetSendCode(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sendData;
        }
        public ServiceResponse<List<CodesErrorDetails>> GetErrorCodes(string Enviornment)
        {
            ServiceResponse<List<CodesErrorDetails>> codes = new ServiceResponse<List<CodesErrorDetails>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetErrorCodeDetailas(SPWrapper.GetFailedCode(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }

        public string GenerateNumeric()
        {
            int numbers = 5;
            Random objrandom = new Random();
            string strrandom = "";
            for (int i = 0; i < 5; i++)
            {
                int temp = objrandom.Next(0, numbers);
                strrandom += temp;
            }
            var rnd = strrandom;
            //Random generator = new Random();
            //string r = generator.Next(0, 1000000).ToString("D6");
            return rnd;
        }

        public string insertWaybillMain(OmsToPandoRoot omsToPandoRoot, string Enviornment)
        {

            string res;
            try
            {

                res = SPWrapper.WaybillinsertMain(omsToPandoRoot, Enviornment);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public bool insertWaybillshipment(OmsToPandoRoot omsToPandoRoot, string primaryid, string FacilityCode, string Enviornment)
        {

            bool res;
            try
            {

                res = SPWrapper.WaybillShipment(omsToPandoRoot, primaryid, FacilityCode, Enviornment);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public bool InsertitemWaybill(List<Item> itemDTO, string ID, string Code, string Enviornment)
        {
            bool res;
            try
            {
                //CreateLog($"item sku code insert DB:-{itemDTO}");
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("ID");
                dtsku.Columns.Add("Code");
                dtsku.Columns.Add("name");
                dtsku.Columns.Add("description");
                dtsku.Columns.Add("quantity");
                dtsku.Columns.Add("skuCode");
                dtsku.Columns.Add("itemPrice");
                dtsku.Columns.Add("imageURL");
                dtsku.Columns.Add("hsnCode");
                dtsku.Columns.Add("tags");

                for (int i = 0; i < itemDTO.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["ID"] = ID;
                    drsku["Code"] = Code;
                    drsku["name"] = itemDTO[i].name;
                    drsku["description"] = itemDTO[i].description;
                    drsku["quantity"] = itemDTO[i].quantity;
                    drsku["skuCode"] = itemDTO[i].skuCode;
                    drsku["itemPrice"] = itemDTO[i].itemPrice;
                    drsku["imageURL"] = itemDTO[i].imageURL;
                    drsku["hsnCode"] = itemDTO[i].hsnCode;
                    drsku["tags"] = itemDTO[i].tags;
                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.Waybillinsertitems(dtsku, Enviornment);
                //CreateLog($"item sku insert DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public bool InsertCustomfieldWaybill(List<CustomField> itemDTO, string ID, string Code, string Enviornment)
        {
            bool res;
            try
            {
                //CreateLog($"item sku code insert DB:-{itemDTO}");
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("ID");
                dtsku.Columns.Add("Code");
                dtsku.Columns.Add("name");

                dtsku.Columns.Add("value");


                for (int i = 0; i < itemDTO.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["ID"] = ID;
                    drsku["Code"] = Code;
                    drsku["name"] = itemDTO[i].name;
                    drsku["value"] = itemDTO[i].value;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.WaybillinsertCustomfield(dtsku, Enviornment);
                //CreateLog($"item sku insert DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public bool insertWaybillpickupadres(PickupAddressDetails pickupaddress, string primaryid, string Enviornment)
        {

            bool res;
            try
            {

                res = SPWrapper.WaybillPickupAddress(pickupaddress, primaryid, Enviornment);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public bool insertWaybillReturnaddress(ReturnAddressDetails pickupaddress, string primaryid, string Enviornment)
        {

            bool res;
            try
            {

                res = SPWrapper.WaybillreturnAddress(pickupaddress, primaryid, Enviornment);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public bool insertWaybilldeliveryaddress(DeliveryAddressDetails pickupaddress, string primaryid, string Enviornment)
        {
            bool res;
            try
            {
                res = SPWrapper.WaybilldeliveryAddress(pickupaddress, primaryid, Enviornment);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public List<WaybillSend> GetWaybillAllRecrdstosend(string Instance, string Enviornment)
        {
            List<WaybillSend> AllRes = new List<WaybillSend>();
            try
            {
                AllRes = Mapper.GetWayBillSendrecords(SPWrapper.GetWaybillSendData(Instance, Enviornment));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return AllRes;
        }
        public List<WaybillSend> GetWaybillAllFailedRecrdsto(string Instance, string Enviornment)
        {
            List<WaybillSend> AllRes = new List<WaybillSend>();
            try
            {
                AllRes = Mapper.GetWayBillSendrecords(SPWrapper.GetWaybillFailedData(Instance, Enviornment));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return AllRes;
        }
        public string InsertAllsendingDataReturnorder(List<WaybillSend> itemDatun, string Enviornment, string Instance)
        {
            string res;
            try
            {
                string id = "Tri_" + GenerateNumeric();
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("indent_no");
                dtsku.Columns.Add("delivery_number");
                dtsku.Columns.Add("mrp_price");
                dtsku.Columns.Add("material_code");
                dtsku.Columns.Add("actual_source");
                dtsku.Columns.Add("source_system");
                dtsku.Columns.Add("gate_ref_id");
                dtsku.Columns.Add("division");
                dtsku.Columns.Add("quantity");
                dtsku.Columns.Add("quantity_unit");
                dtsku.Columns.Add("weight");
                dtsku.Columns.Add("weight_unit");
                dtsku.Columns.Add("volume");
                dtsku.Columns.Add("volume_unit");
                dtsku.Columns.Add("ship_to");
                dtsku.Columns.Add("sold_to");
                dtsku.Columns.Add("type");
                dtsku.Columns.Add("invoice_number");
                dtsku.Columns.Add("invoice_amount");
                dtsku.Columns.Add("category");
                dtsku.Columns.Add("invoice_date");
                dtsku.Columns.Add("line_item_no");
                dtsku.Columns.Add("eway_bill_number");
                dtsku.Columns.Add("eway_bill_date");
                dtsku.Columns.Add("action_by");
                dtsku.Columns.Add("action_type");
                dtsku.Columns.Add("clear");
                dtsku.Columns.Add("cust_refid");
                dtsku.Columns.Add("Trigger_ID");
                dtsku.Columns.Add("Instance");



                for (int i = 0; i < itemDatun.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["indent_no"] = itemDatun[i].indent_no;
                    drsku["delivery_number"] = itemDatun[i].delivery_number;
                    drsku["mrp_price"] = itemDatun[i].mrp_price;
                    drsku["material_code"] = itemDatun[i].material_code;
                    drsku["actual_source"] = itemDatun[i].actual_source;
                    drsku["source_system"] = itemDatun[i].source_system;
                    drsku["gate_ref_id"] = itemDatun[i].gate_ref_id;
                    drsku["division"] = itemDatun[i].division;
                    drsku["quantity"] = itemDatun[i].quantity;
                    drsku["quantity_unit"] = itemDatun[i].quantity_unit;
                    drsku["weight"] = itemDatun[i].weight;
                    drsku["weight_unit"] = itemDatun[i].weight_unit;
                    drsku["volume"] = itemDatun[i].volume;
                    drsku["volume_unit"] = itemDatun[i].volume_unit;
                    drsku["ship_to"] = itemDatun[i].ship_to;
                    drsku["sold_to"] = itemDatun[i].sold_to;
                    drsku["type"] = itemDatun[i].type;
                    drsku["invoice_number"] = itemDatun[i].invoice_number;
                    drsku["invoice_amount"] = itemDatun[i].invoice_amount;
                    drsku["category"] = itemDatun[i].category;
                    drsku["invoice_date"] = itemDatun[i].invoice_date;
                    drsku["line_item_no"] = itemDatun[i].line_item_no;
                    drsku["eway_bill_number"] = itemDatun[i].eway_bill_number;
                    drsku["eway_bill_date"] = itemDatun[i].eway_bill_date;
                    drsku["action_by"] = itemDatun[i].action_by;
                    drsku["action_type"] = itemDatun[i].action_type;
                    drsku["clear"] = itemDatun[i].clear;
                    drsku["cust_refid"] = itemDatun[i].cust_ref_id;
                    drsku["Trigger_ID"] = id;
                    drsku["Instance"] = Instance;
                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.IsertwaybillPostData(dtsku, Enviornment);
                //CreateLog($"itemsending data DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public bool insertReturnOrdercoder(List<ReturnorderCode> elements, string FacilityCode, string Enviornment,string Instance)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("FacilityCode");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].code;
                    dr["FacilityCode"] = FacilityCode;
                    dr["Instance"] = Instance;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertReturnOrderCode(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public ServiceResponse<List<ReturnorderCode>> GetReturnOrderCodes(string Instacne, string Enviornment)
        {
            ServiceResponse<List<ReturnorderCode>> codes = new ServiceResponse<List<ReturnorderCode>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetReturnOrderCode(SPWrapper.GetReturnOrderCode(Instacne, Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public bool insertReturnSaleOrderitem(List<ReturnSaleOrderItem> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("reversePickupCode");
                dtinstcode.Columns.Add("skuCode");
                dtinstcode.Columns.Add("quantity");
                dtinstcode.Columns.Add("saleOrderCode");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].Code;
                    dr["reversePickupCode"] = elements[i].reversePickupCode;
                    dr["skuCode"] = elements[i].skuCode;
                    dr["quantity"] = elements[i].quantity;
                    dr["saleOrderCode"] = elements[i].saleOrderCode;
                    dr["Instance"] = elements[i].Source;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertReturnSaleOrderitem(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public bool insertReturnaddress(List<ReturnAddressDetailsList> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("name");
                dtinstcode.Columns.Add("addressLine1");
                dtinstcode.Columns.Add("addressLine2");
                dtinstcode.Columns.Add("city");
                dtinstcode.Columns.Add("state");
                dtinstcode.Columns.Add("pincode");
                dtinstcode.Columns.Add("phone");
                dtinstcode.Columns.Add("email");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].Code;
                    dr["name"] = elements[i].name;
                    dr["addressLine1"] = elements[i].addressLine1;
                    dr["addressLine2"] = elements[i].addressLine2;
                    dr["city"] = elements[i].city;
                    dr["state"] = elements[i].state;
                    dr["pincode"] = elements[i].pincode;
                    dr["phone"] = elements[i].phone;
                    dr["email"] = elements[i].email;
                    dr["Instance"] = elements[i].Source;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertReturnaddress(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public ServiceResponse<List<ReturnSaleOrderItem>> GetReturnOrderSkuCodes(string Enviornment)
        {
            ServiceResponse<List<ReturnSaleOrderItem>> codes = new ServiceResponse<List<ReturnSaleOrderItem>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetReturnOrderSkuCode(SPWrapper.GetReturnOrderSkuCode(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public bool insertReturOrderItemtypes(List<ItemTypeDTO> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("Weight");
                dtinstcode.Columns.Add("length");
                dtinstcode.Columns.Add("width");
                dtinstcode.Columns.Add("itemDetailFieldsText");
                dtinstcode.Columns.Add("maxRetailPrice");
                dtinstcode.Columns.Add("height");
                dtinstcode.Columns.Add("Instance");

                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].Code;
                    dr["Weight"] = elements[i].weight;
                    dr["length"] = elements[i].length;
                    dr["width"] = elements[i].width;
                    dr["itemDetailFieldsText"] = elements[i].itemDetailFieldsText;
                    dr["maxRetailPrice"] = elements[i].maxRetailPrice;
                    dr["height"] = elements[i].height;
                    dr["Instance"] = elements[i].Source;

                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertReturnOrderItemtypes(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public ServiceResponse<List<ReturnOrderSendData>> GetReturnOrderSendData(string instance, string Enviornment)
        {
            ServiceResponse<List<ReturnOrderSendData>> codes = new ServiceResponse<List<ReturnOrderSendData>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetReturnSendData(SPWrapper.GetReturnOrderSendData(instance, Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public void UpdateWaybillErrordetails(bool status, string Reason, string triggerid, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateWaybillError(status, Reason, triggerid, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateReturnOrderErrordetails(List<ErrorDetails> ErrorDt, int type, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("Code");
                dtsku.Columns.Add("Reason");
                dtsku.Columns.Add("Status");


                for (int i = 0; i < ErrorDt.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["Code"] = ErrorDt[i].Code;
                    drsku["Reason"] = ErrorDt[i].Reason;
                    drsku["Status"] = ErrorDt[i].Status;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.UpdateReurnOrdercodeError(dtsku, type, Enviornment);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public bool UpdateReturnOrderSKUErrordetails(List<ErrorDetails> ErrorDt, int type, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("Code");
                dtsku.Columns.Add("Reason");
                dtsku.Columns.Add("Status");


                for (int i = 0; i < ErrorDt.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["Code"] = ErrorDt[i].SkuCode;
                    drsku["Reason"] = ErrorDt[i].Reason;
                    drsku["Status"] = ErrorDt[i].Status;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.UpdateReurnOrdercodeError(dtsku, type, Enviornment);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public void UpdateSaleOrderFirst(string Reason, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateSaleOrderSearchError(Reason, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void BLReturnOrderError(string Reason, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateReturnOrderError(Reason, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string InsertAllsendingDataReturnorder(ServiceResponse<List<ReturnOrderSendData>> itemDatun, string Enviornment)
        {
            string res;
            try
            {
                string id = "Tri_" + GenerateNumeric();
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("name");
                dtsku.Columns.Add("reference_number");
                dtsku.Columns.Add("address");
                dtsku.Columns.Add("city");
                dtsku.Columns.Add("state");
                dtsku.Columns.Add("pincode");
                dtsku.Columns.Add("region");
                dtsku.Columns.Add("mobile_number");
                dtsku.Columns.Add("email");
                dtsku.Columns.Add("customer_type");
                dtsku.Columns.Add("category");
                dtsku.Columns.Add("delivery_number");
                dtsku.Columns.Add("mrp_price");
                dtsku.Columns.Add("material_code");
                dtsku.Columns.Add("source_system");
                dtsku.Columns.Add("material_taxable_amount");
                dtsku.Columns.Add("division");
                dtsku.Columns.Add("quantity");
                dtsku.Columns.Add("quantity_unit");
                dtsku.Columns.Add("weight");
                dtsku.Columns.Add("weight_unit");
                dtsku.Columns.Add("volume");
                dtsku.Columns.Add("volume_unit");
                dtsku.Columns.Add("ship_to");
                dtsku.Columns.Add("sold_to");
                dtsku.Columns.Add("type");
                dtsku.Columns.Add("invoice_number");
                dtsku.Columns.Add("invoice_amount");
                dtsku.Columns.Add("invoice_date");
                dtsku.Columns.Add("line_item_no");
                dtsku.Columns.Add("pickup_reference_number");
                dtsku.Columns.Add("TriggerID");
                //dtsku.Columns.Add("Instance");


                for (int i = 0; i < itemDatun.ObjectParam.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["name"] = itemDatun.ObjectParam[i].name;
                    drsku["reference_number"] = itemDatun.ObjectParam[i].reference_number;
                    drsku["address"] = itemDatun.ObjectParam[i].address;
                    drsku["city"] = itemDatun.ObjectParam[i].city;
                    drsku["state"] = itemDatun.ObjectParam[i].state;
                    drsku["pincode"] = itemDatun.ObjectParam[i].pincode;
                    drsku["region"] = itemDatun.ObjectParam[i].region;
                    drsku["mobile_number"] = itemDatun.ObjectParam[i].mobile_number;
                    drsku["email"] = itemDatun.ObjectParam[i].email;
                    drsku["customer_type"] = itemDatun.ObjectParam[i].customer_type;
                    drsku["category"] = itemDatun.ObjectParam[i].category;
                    drsku["delivery_number"] = itemDatun.ObjectParam[i].delivery_number;
                    drsku["mrp_price"] = itemDatun.ObjectParam[i].mrp_price;
                    drsku["material_code"] = itemDatun.ObjectParam[i].material_code;
                    drsku["source_system"] = itemDatun.ObjectParam[i].source_system;
                    drsku["material_taxable_amount"] = itemDatun.ObjectParam[i].material_taxable_amount;
                    drsku["division"] = itemDatun.ObjectParam[i].division;
                    drsku["quantity"] = itemDatun.ObjectParam[i].quantity;
                    drsku["quantity_unit"] = itemDatun.ObjectParam[i].quantity_unit;
                    drsku["weight"] = itemDatun.ObjectParam[i].weight;
                    drsku["weight_unit"] = itemDatun.ObjectParam[i].weight_unit;
                    drsku["volume"] = itemDatun.ObjectParam[i].volume;
                    drsku["volume_unit"] = itemDatun.ObjectParam[i].volume_unit;
                    drsku["ship_to"] = itemDatun.ObjectParam[i].ship_to;
                    drsku["sold_to"] = itemDatun.ObjectParam[i].sold_to;
                    drsku["type"] = itemDatun.ObjectParam[i].type;
                    drsku["invoice_number"] = itemDatun.ObjectParam[i].invoice_number;
                    drsku["invoice_amount"] = itemDatun.ObjectParam[i].invoice_amount;
                    drsku["invoice_date"] = itemDatun.ObjectParam[i].invoice_date;
                    drsku["line_item_no"] = itemDatun.ObjectParam[i].line_item_no;
                    drsku["pickup_reference_number"] = itemDatun.ObjectParam[i].pickup_reference_number;
                    drsku["cust_refid"] = itemDatun.ObjectParam[i].cust_ref_id;
                    drsku["expected_delivery_date"] = itemDatun.ObjectParam[i].expected_delivery_date;
                    drsku["TriggerID"] = id;
                    //drsku["Instance"] = Instance;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.IsertReturnOrderPostData(dtsku, Enviornment);
                //CreateLog($"itemsending data DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public void UpdateReturnOrderPostDataError(bool status, string Reason, string triggerid, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateReturnOrderPostDataError(status, Reason, triggerid, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool insertGatePassCode(List<Element> elements, string FacilityCode, string instance, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("FacilityCode");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].code;
                    dr["FacilityCode"] = FacilityCode;
                    dr["Instance"] = instance;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertGetPassCode(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<Element> GetWaybillgatePassCode(string Enviornment)
        {
            List<Element> codes = new List<Element>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetGatePassCode(SPWrapper.GetWaybillgatepassCode(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public bool insertGatePassElements(List<Elementdb> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("reference");
                dtinstcode.Columns.Add("topartyname");
                dtinstcode.Columns.Add("invoicecode");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].code;
                    dr["reference"] = elements[i].reference;
                    dr["topartyname"] = elements[i].toPartyName;
                    dr["invoicecode"] = elements[i].invoiceCode;
                    dr["Instance"] = elements[i].Source;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertGetPassElements(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public bool insertItemTypeDTO(List<GatePassItemDTODb> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("quantity");
                dtinstcode.Columns.Add("itemtypeSKU");
                dtinstcode.Columns.Add("unitprice");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].code;
                    dr["quantity"] = elements[i].quantity;
                    dr["itemtypeSKU"] = elements[i].itemTypeSKU;
                    dr["unitprice"] = elements[i].unitPrice;
                    dr["Instance"] = elements[i].Source;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertItemTypeDTO(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<GatePassItemDTO> GetWaybillSKUCode(string Enviornment)
        {
            List<GatePassItemDTO> codes = new List<GatePassItemDTO>();

            try
            {

                return codes = Mapper.GetSKUCode(SPWrapper.GetWaybillSKUCde(Enviornment));
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public bool insertWaybillItemType(List<ItemTypeDTO> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("weight");
                dtinstcode.Columns.Add("length");
                dtinstcode.Columns.Add("width");
                dtinstcode.Columns.Add("itemdetailfieldstext");
                dtinstcode.Columns.Add("height");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].Code;
                    dr["weight"] = elements[i].weight;
                    dr["length"] = elements[i].length;
                    dr["width"] = elements[i].width;
                    dr["itemdetailfieldstext"] = elements[i].itemDetailFieldsText;
                    dr["height"] = elements[i].height;
                    dr["Instance"] = elements[i].Source;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertWaybillItemsType(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public static void CreateLog(string message)
        {
            Log.Information(message);
        }
        public ServiceResponse<UserLogin> CheckLoginCredentials(string UserName, string Password/*, string Enviornment*/)
        {
            ServiceResponse<UserLogin> serviceResponse;
            try
            {
                serviceResponse = Mapper.CheckLoginCredentials(SPWrapper.CheckLoginCredentials(UserName, Password/*,Enviornment*/));
                CreateLog($"ServiceResponse Object {JsonConvert.SerializeObject(serviceResponse)}");
            }
            catch (Exception Ex)
            {
                Log.Error($"Excetion at :", Ex);
                serviceResponse = null;
            }
            return serviceResponse;

        }

        public List<PostDataSTOWaybill> GetAllWaybillSTOPost(string Enviornment)
        {
            List<PostDataSTOWaybill> AllRes = new List<PostDataSTOWaybill>();
            try
            {
                //CreateLog($" Get Code from DB ");

                AllRes = Mapper.GetSendingWayBillSTOData(SPWrapper.GetWaybillSTOSendData(Enviornment));
                //CreateLog($" Get Code from DB Data{AllRes} ");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return AllRes;
        }
        public string InsertWaybillSTOsendingData(List<PostDataSTOWaybill> itemDatun, string Enviornment)
        {
            string res;
            try
            {
                string id = "Tri_" + GenerateNumeric();
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("indent_no");
                dtsku.Columns.Add("delivery_number");
                dtsku.Columns.Add("mrp_price");
                dtsku.Columns.Add("material_code");
                dtsku.Columns.Add("actual_source");
                dtsku.Columns.Add("source_system");
                dtsku.Columns.Add("gate_ref_id");
                dtsku.Columns.Add("division");
                dtsku.Columns.Add("quantity");
                dtsku.Columns.Add("quantity_unit");
                dtsku.Columns.Add("weight");
                dtsku.Columns.Add("weight_unit");
                dtsku.Columns.Add("volume");
                dtsku.Columns.Add("volume_unit");
                dtsku.Columns.Add("ship_to");
                dtsku.Columns.Add("sold_to");
                dtsku.Columns.Add("type");
                dtsku.Columns.Add("invoice_number");
                dtsku.Columns.Add("invoice_amount");
                dtsku.Columns.Add("category");
                dtsku.Columns.Add("invoice_date");
                dtsku.Columns.Add("line_item_no");
                dtsku.Columns.Add("eway_bill_number");
                dtsku.Columns.Add("eway_bill_date");
                dtsku.Columns.Add("action_by");
                dtsku.Columns.Add("clear");
                dtsku.Columns.Add("TriggerID");



                for (int i = 0; i < itemDatun.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();

                    drsku["indent_no"] = itemDatun[i].indent_no;
                    drsku["delivery_number"] = itemDatun[i].delivery_number;
                    drsku["mrp_price"] = itemDatun[i].mrp_price;
                    drsku["material_code"] = itemDatun[i].material_code;
                    drsku["actual_source"] = itemDatun[i].actual_source;
                    drsku["source_system"] = itemDatun[i].source_system;
                    drsku["gate_ref_id"] = itemDatun[i].gate_ref_id;
                    drsku["division"] = itemDatun[i].division;
                    drsku["quantity"] = itemDatun[i].quantity;
                    drsku["quantity_unit"] = itemDatun[i].quantity_unit;
                    drsku["weight"] = itemDatun[i].weight;
                    drsku["weight_unit"] = itemDatun[i].weight_unit;
                    drsku["volume"] = itemDatun[i].volume;
                    drsku["volume_unit"] = itemDatun[i].volume_unit;
                    drsku["ship_to"] = itemDatun[i].ship_to;
                    drsku["sold_to"] = itemDatun[i].sold_to;
                    drsku["type"] = itemDatun[i].type;
                    drsku["invoice_number"] = itemDatun[i].invoice_number;
                    drsku["invoice_amount"] = itemDatun[i].invoice_amount;
                    drsku["category"] = itemDatun[i].category;
                    drsku["invoice_date"] = itemDatun[i].invoice_date;
                    drsku["line_item_no"] = itemDatun[i].line_item_no;
                    drsku["eway_bill_number"] = itemDatun[i].eway_bill_number;
                    drsku["eway_bill_date"] = itemDatun[i].eway_bill_date;
                    drsku["action_by"] = itemDatun[i].action_by;
                    drsku["clear"] = itemDatun[i].clear;
                    drsku["TriggerID"] = id;
                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.IsertWaybillPostData(dtsku, Enviornment);
                //CreateLog($"itemsending data DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public bool insertSTOAPIGatePassCode(List<Element> elements, string FacilityCode, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("FacilityCode");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].code;
                    dr["FacilityCode"] = FacilityCode;
                    dr["Instance"] = elements[i].source;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertSTOAPIGetPassCode(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<Element> GetSTOAPIgatePassCode(string Enviornment)
        {
            List<Element> codes = new List<Element>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetSTOAPIGatePassCode(SPWrapper.GetSTOAPIgatepassCode(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public bool insertSTOAPiGatePassElements(List<Element> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("topartyname");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].code;

                    dr["topartyname"] = elements[i].toPartyName;
                    dr["Instance"] = elements[i].source;

                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertSTOAPIGetPassElements(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public bool insertSTOAPiItemTypeDTO(List<GatePassItemDTO> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("quantity");
                dtinstcode.Columns.Add("itemtypeSKU");
                dtinstcode.Columns.Add("unitprice");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].code;
                    dr["quantity"] = elements[i].quantity;
                    dr["itemtypeSKU"] = elements[i].itemTypeSKU;
                    dr["Instance"] = elements[i].Source;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertSTOAPIItemTypeDTO(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public bool STOWaybillCustField(List<CustomFieldValuedb> itemDTO, string Enviornment)
        {
            bool res;
            try
            {
                //CreateLog($"item sku code insert DB:-{itemDTO}");
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("Code");
                dtsku.Columns.Add("name");
                dtsku.Columns.Add("value");
                dtsku.Columns.Add("Instance");
                for (int i = 0; i < itemDTO.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["Code"] = itemDTO[i].Code;
                    drsku["name"] = itemDTO[i].fieldName;
                    drsku["value"] = itemDTO[i].fieldValue;
                    drsku["Instance"] = itemDTO[i].Source;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.STOWaybillCustField(dtsku, Enviornment);
                //CreateLog($"item sku insert DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<GatePassItemDTO> GetSTOSKUCode(string Enviornment)
        {
            List<GatePassItemDTO> codes = new List<GatePassItemDTO>();

            try
            {

                return codes = Mapper.GetSKUCode(SPWrapper.GetSTOAPISKUCde(Enviornment));
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public bool insertSTOAPItemType(List<ItemTypeDTO> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("Code");
                dtinstcode.Columns.Add("weight");
                dtinstcode.Columns.Add("length");
                dtinstcode.Columns.Add("width");
                dtinstcode.Columns.Add("itemdetailfieldstext");
                dtinstcode.Columns.Add("height");
                dtinstcode.Columns.Add("Instance");
                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["Code"] = elements[i].Code;
                    dr["weight"] = elements[i].weight;
                    dr["length"] = elements[i].length;
                    dr["width"] = elements[i].width;
                    dr["itemdetailfieldstext"] = elements[i].itemDetailFieldsText;
                    dr["height"] = elements[i].height;
                    dr["Instance"] = elements[i].Source;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.InsertSTOAPiItemsType(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public ServiceResponse<List<ReturnOrderSendData>> GetSTOAPISendData(string Instance, string Enviornment)
        {
            ServiceResponse<List<ReturnOrderSendData>> codes = new ServiceResponse<List<ReturnOrderSendData>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetSTOAPIAllData(SPWrapper.GetSTOAPiSendData(Instance, Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public string InsertAllsendingDataSTOAPI(ServiceResponse<List<ReturnOrderSendData>> itemDatun, string Enviornment)
        {
            string res;
            try
            {
                string id = "Tri_" + GenerateNumeric();
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("name");
                dtsku.Columns.Add("reference_number");
                dtsku.Columns.Add("address");
                dtsku.Columns.Add("city");
                dtsku.Columns.Add("state");
                dtsku.Columns.Add("pincode");
                dtsku.Columns.Add("region");
                dtsku.Columns.Add("mobile_number");
                dtsku.Columns.Add("email");
                dtsku.Columns.Add("customer_type");
                dtsku.Columns.Add("category");
                dtsku.Columns.Add("delivery_number");
                dtsku.Columns.Add("mrp_price");
                dtsku.Columns.Add("material_code");
                dtsku.Columns.Add("source_system");
                dtsku.Columns.Add("material_taxable_amount");
                dtsku.Columns.Add("division");
                dtsku.Columns.Add("quantity");
                dtsku.Columns.Add("quantity_unit");
                dtsku.Columns.Add("weight");
                dtsku.Columns.Add("weight_unit");
                dtsku.Columns.Add("volume");
                dtsku.Columns.Add("volume_unit");
                dtsku.Columns.Add("ship_to");
                dtsku.Columns.Add("sold_to");
                dtsku.Columns.Add("type");
                dtsku.Columns.Add("invoice_number");
                dtsku.Columns.Add("invoice_amount");
                dtsku.Columns.Add("invoice_date");
                dtsku.Columns.Add("line_item_no");
                dtsku.Columns.Add("pickup_reference_number");
                dtsku.Columns.Add("cust_refid");
                dtsku.Columns.Add("expected_delivery_date");
                dtsku.Columns.Add("TriggerID");


                for (int i = 0; i < itemDatun.ObjectParam.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["name"] = itemDatun.ObjectParam[i].name;
                    drsku["reference_number"] = itemDatun.ObjectParam[i].reference_number;
                    drsku["address"] = itemDatun.ObjectParam[i].address;
                    drsku["city"] = itemDatun.ObjectParam[i].city;
                    drsku["state"] = itemDatun.ObjectParam[i].state;
                    drsku["pincode"] = itemDatun.ObjectParam[i].pincode;
                    drsku["region"] = itemDatun.ObjectParam[i].region;
                    drsku["mobile_number"] = itemDatun.ObjectParam[i].mobile_number;
                    drsku["email"] = itemDatun.ObjectParam[i].email;
                    drsku["customer_type"] = itemDatun.ObjectParam[i].customer_type;
                    drsku["category"] = itemDatun.ObjectParam[i].category;
                    drsku["delivery_number"] = itemDatun.ObjectParam[i].delivery_number;
                    drsku["mrp_price"] = itemDatun.ObjectParam[i].mrp_price;
                    drsku["material_code"] = itemDatun.ObjectParam[i].material_code;
                    drsku["source_system"] = itemDatun.ObjectParam[i].source_system;
                    drsku["material_taxable_amount"] = itemDatun.ObjectParam[i].material_taxable_amount;
                    drsku["division"] = itemDatun.ObjectParam[i].division;
                    drsku["quantity"] = itemDatun.ObjectParam[i].quantity;
                    drsku["quantity_unit"] = itemDatun.ObjectParam[i].quantity_unit;
                    drsku["weight"] = itemDatun.ObjectParam[i].weight;
                    drsku["weight_unit"] = itemDatun.ObjectParam[i].weight_unit;
                    drsku["volume"] = itemDatun.ObjectParam[i].volume;
                    drsku["volume_unit"] = itemDatun.ObjectParam[i].volume_unit;
                    drsku["ship_to"] = itemDatun.ObjectParam[i].ship_to;
                    drsku["sold_to"] = itemDatun.ObjectParam[i].sold_to;
                    drsku["type"] = itemDatun.ObjectParam[i].type;
                    drsku["invoice_number"] = itemDatun.ObjectParam[i].invoice_number;
                    drsku["invoice_amount"] = itemDatun.ObjectParam[i].invoice_amount;
                    drsku["invoice_date"] = itemDatun.ObjectParam[i].invoice_date;
                    drsku["line_item_no"] = itemDatun.ObjectParam[i].line_item_no;
                    drsku["pickup_reference_number"] = itemDatun.ObjectParam[i].pickup_reference_number;
                    drsku["cust_refid"] = itemDatun.ObjectParam[i].cust_ref_id;
                    drsku["expected_delivery_date"] = itemDatun.ObjectParam[i].expected_delivery_date;
                    drsku["TriggerID"] = id;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.IsertSTOAPIAllData(dtsku, Enviornment);
                //CreateLog($"itemsending data DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }


        public void STOWaybillErrorCodes(string Reason, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateSTOWaybillErrorCodesError(Reason, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateWaybillGatepassError(List<ErrorDetails> ErrorDt, int type, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("Code");
                dtsku.Columns.Add("Reason");
                dtsku.Columns.Add("Status");

                for (int i = 0; i < ErrorDt.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["Code"] = ErrorDt[i].Code;
                    drsku["Reason"] = ErrorDt[i].Reason;
                    drsku["Status"] = ErrorDt[i].Status;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.UpdateErrorWaybill(dtsku, type, Enviornment);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public void UpdateSTOWaybillPosterreoe(bool status, string Reason, string triggerid, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateSTOwaybillErrorpostdata(status, Reason, triggerid, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void STOAPIErrorCodes(string Reason, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateSTOAPIError(Reason, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateSTOAPIError(List<ErrorDetails> ErrorDt, int type, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("Code");
                dtsku.Columns.Add("Reason");
                dtsku.Columns.Add("Status");

                for (int i = 0; i < ErrorDt.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["Code"] = ErrorDt[i].Code;
                    drsku["Reason"] = ErrorDt[i].Reason;
                    drsku["Status"] = ErrorDt[i].Status;

                    dtsku.Rows.Add(drsku);
                }
                res = SPWrapper.UpdateSTOAPI(dtsku, type, Enviornment);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;
        }
        public void UpdateSTOAPIPosterreoe(bool status, string Reason, string triggerid, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateSTOAPIErrorpostdata(status, Reason, triggerid, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ServiceResponse<List<CodesErrorDetails>> BLSTOAPI(string Enviornment)
        {
            ServiceResponse<List<CodesErrorDetails>> codes = new ServiceResponse<List<CodesErrorDetails>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetErrorCodeDetailas(SPWrapper.GetSTOAPIFailedCode(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public ServiceResponse<List<CodesErrorDetails>> BLSTOWaybil(string Enviornment)
        {
            ServiceResponse<List<CodesErrorDetails>> codes = new ServiceResponse<List<CodesErrorDetails>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetErrorCodeDetailas(SPWrapper.GetSTOErrorstatusCode(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public ServiceResponse<List<EndpointErrorDetails>> BLWaybilStatus(string Enviornment)
        {
            ServiceResponse<List<EndpointErrorDetails>> codes = new ServiceResponse<List<EndpointErrorDetails>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.ErrorWaybillPostData(SPWrapper.GetWaybillPoststatus(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public List<UserInstance> GetWaybillInstanceName(string Enviornment)
        {
            List<UserInstance> AllRes = new List<UserInstance>();
            try
            {
                AllRes = Mapper.GetInstanceFromTriggerData(SPWrapper.GetWaybillInstanceName(Enviornment));
                //CreateLog($" Get Code from DB Data{AllRes} ");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return AllRes;
        }
        public void UpdateStatusinWaybillTriggerTable(string triggerid, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateStatusinWaybillTriggerTable(triggerid, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateStatusinUpdateshippingTriggerTable(string ShippingPackageCode, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateStatusinUpdateShippingTriggerTable(ShippingPackageCode, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ServiceResponse<List<CodesErrorDetails>> BLReturnOrderStatus(string Enviornment)
        {
            ServiceResponse<List<CodesErrorDetails>> codes = new ServiceResponse<List<CodesErrorDetails>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetErrorCodeDetailas(SPWrapper.ReturnOrderStatus(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public List<Element> GetWaybillgatePassCodeForretrigger(string Enviornment)
        {
            List<Element> codes = new List<Element>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetGatePassCode(SPWrapper.GetWaybillgatepassCodeRetrigger(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public List<Element> GetSTOAPIgatePassCodeRetrigger(string Enviornment)
        {
            List<Element> codes = new List<Element>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetSTOAPIGatePassCode(SPWrapper.GetSTOAPIgatepassCodeRetrigger(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public ServiceResponse<List<ReturnorderCode>> GetReturnOrderCodesForRetrigger(string Enviornment)
        {
            ServiceResponse<List<ReturnorderCode>> codes = new ServiceResponse<List<ReturnorderCode>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetReturnOrderCode(SPWrapper.GetReturnOrderCodeRetrigger(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }

        //public void InsertUpdateShippingpackageBox(List<ShippingBoxdb> itemDatun, string Enviornment)
        //{
        //    string res;
        //    try
        //    {

        //        DataTable dtsku = new DataTable();
        //        dtsku.Columns.Add("Id");
        //        dtsku.Columns.Add("length");
        //        dtsku.Columns.Add("width");
        //        dtsku.Columns.Add("height");



        //        for (int i = 0; i < itemDatun.Count; i++)
        //        {
        //            DataRow drsku = dtsku.NewRow();
        //            drsku["Id"] = itemDatun[i].Id;
        //            drsku["length"] = itemDatun[i].length;
        //            drsku["width"] = itemDatun[i].height;
        //            drsku["height"] = itemDatun[i].width;


        //            dtsku.Rows.Add(drsku);
        //        }
        //        SPWrapper.IsertShippingBox(dtsku, Enviornment);
        //        //CreateLog($"itemsending data DB Status:-{res}");
        //    }
        //    catch (Exception ex)
        //    {
        //        //CreateLog($"Error: {ex.Message}");
        //        throw;
        //    }

        //}
        public void InsertUpdateShippingpackage(List<UpdateShippingpackagedb> itemDatun, string Enviornment)
        {
            string res;
            try
            {
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("Id");
                dtsku.Columns.Add("shippingPackageCode");
                //dtsku.Columns.Add("shippingProviderCode");
                //dtsku.Columns.Add("trackingNumber");
                //dtsku.Columns.Add("shippingPackageTypeCode");
                //dtsku.Columns.Add("actualWeight");
                //dtsku.Columns.Add("noOfBoxes");


                for (int i = 0; i < itemDatun.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["Id"] = itemDatun[i].id;
                    drsku["shippingPackageCode"] = itemDatun[i].shippingPackageCode;
                    //drsku["shippingProviderCode"] = itemDatun[i].shippingProviderCode;
                    //drsku["trackingNumber"] = itemDatun[i].trackingNumber;
                    //drsku["shippingPackageTypeCode"] = itemDatun[i].shippingPackageTypeCode;
                    //drsku["actualWeight"] = itemDatun[i].actualWeight;
                    //drsku["noOfBoxes"] = itemDatun[i].noOfBoxes; ;

                    dtsku.Rows.Add(drsku);
                }
                SPWrapper.IsertshippingUpdate(dtsku, Enviornment);
                //CreateLog($"itemsending data DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }

        }
        public void InsertCustomFields(List<addCustomFieldValue> itemDatun, string Enviornment)
        {
            string res;
            try
            {
                DataTable dtsku = new DataTable();
                dtsku.Columns.Add("Id");
                dtsku.Columns.Add("name");
                dtsku.Columns.Add("value");



                for (int i = 0; i < itemDatun.Count; i++)
                {
                    DataRow drsku = dtsku.NewRow();
                    drsku["Id"] = itemDatun[i].Id;
                    drsku["name"] = itemDatun[i].name;
                    drsku["value"] = itemDatun[i].value;
                    dtsku.Rows.Add(drsku);
                }
                SPWrapper.IsertCustomFields(dtsku, Enviornment);
                //CreateLog($"itemsending data DB Status:-{res}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }

        }
        public List<UpdateShippingpackagedb> UpdateShipingPck(string Enviornment)
        {
            List<UpdateShippingpackagedb> codes = new List<UpdateShippingpackagedb>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetUpdateShippingDetails(SPWrapper.GetUpdateShippingData(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public List<UpdateShippingpackagedb> UpdateShipingPckRetrigger(string Enviornment)
        {
            List<UpdateShippingpackagedb> codes = new List<UpdateShippingpackagedb>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.GetUpdateShippingDetails(SPWrapper.GetUpdateShippingRetrigger(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }

        public async Task<SuccessResponse> InsertAllocate_Shipping(List<AllocateshippingPando> itemDatun, string Enviornment)
        {
            //string res;
            bool res;
            SuccessResponse successResponse = new SuccessResponse();

            try
            {
                var dataTable = ConvertDataTable.ToDataTable(itemDatun);
                res= SPWrapper.IsertAllocate_Shipping(dataTable, Enviornment);
                //CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Success to Pando {JsonConvert.SerializeObject(reversePickupResponse)}");

                if (res)
                {
                    successResponse.status = true;
                    successResponse.waybill = "Data Received from Pando";
                    successResponse.shippingLabel = "";
          
                }
                else
                {
                    successResponse.status = false;
                    successResponse.waybill = "No Data Received";
                    successResponse.shippingLabel = "";
              
                }

                return successResponse;

            }
            catch (Exception ex)
            {
                successResponse.status = false;
                successResponse.waybill = $"No Data Received with Error {ex.Message}";
                successResponse.shippingLabel = "";
                return successResponse;
            }


        }

        public List<AllocateshippingDb> PostGAllocateShippingData(string Enviornment, List<AllocateshippingPando> itemDatun)
        {
            //List<Allocateshipping> codes = new List<Allocateshipping>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                var dataTable = ConvertDataTable.ToDataTable(itemDatun);

                return Mapper.GetAllocateShipping(SPWrapper.GetAllocateShippingData(Enviornment, dataTable));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {

                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public List<AllocateshippingDb> PostGAllocateShippingDataForRetrigger(string Enviornment)
        {
            List<Allocateshipping> codes = new List<Allocateshipping>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return Mapper.GetAllocateShipping(SPWrapper.GetAllocateShippingDataForRetrigger(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {

                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }

        //public string UpdateShippingDataPost(UpdateShippingpackage updateShippingpackage, string FacilityCode, string Enviornment)
        //{
        //    var id = GenerateNumeric();
        //    return SPWrapper.IsertUpdateShippingrecords(updateShippingpackage, id, FacilityCode,Enviornment);



        //}
        public bool UpdateShippingDataPost(List<UpdateShippingpackagedb> updateShippingpackage, string Enviornment)
        {
            var id = GenerateNumeric();
            //DataTable UpdateListData = ConvertDataTable.ToDataTable(updateShippingpackage);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("shippingPackageCode");
            dataTable.Columns.Add("name");
            dataTable.Columns.Add("value");
            dataTable.Columns.Add("FacilityCode");
            for (int i = 0; i < updateShippingpackage.Count; i++)
            {
                DataRow drsku = dataTable.NewRow();

                drsku["shippingPackageCode"] = updateShippingpackage[i].shippingPackageCode;
                for (int j = 0; j < updateShippingpackage[i].customFieldValues.Count; j++)
                {
                    drsku["name"] = updateShippingpackage[i].customFieldValues[j].name;
                    drsku["value"] = updateShippingpackage[i].customFieldValues[j].value;
                }
                drsku["FacilityCode"] = updateShippingpackage[i].FacilityCode;
                dataTable.Rows.Add(drsku);

            }
            return SPWrapper.IsertUpdateShippingrecords(dataTable, Enviornment);
        }

        public void UpdateShippingErrordetails(bool status, string Reason, string triggerid, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateShippingError(status, Reason, triggerid, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ServiceResponse<List<EndpointErrorDetails>> BLUpdateShippingStatus(string Enviornment)
        {
            ServiceResponse<List<EndpointErrorDetails>> codes = new ServiceResponse<List<EndpointErrorDetails>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.ErrorWaybillPostData(SPWrapper.GetUpdateShippingStatus(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public void AllocateErrorDetails(bool status, string Reason, string shippingPackageCode, string Enviornment)
        {
            try
            {
                SPWrapper.AllocateShippingError(status, Reason, shippingPackageCode, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AllocateShippingDataPost(Allocateshipping updateShippingpackage, string Enviornment)
        {
            var id = GenerateNumeric();
            return SPWrapper.IsertAllocateShippingrecords(updateShippingpackage, id, Enviornment);
        }


        public ServiceResponse<List<EndpointErrorDetails>> BLAlocateShippingStatus(string Enviornment)
        {
            ServiceResponse<List<EndpointErrorDetails>> codes = new ServiceResponse<List<EndpointErrorDetails>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.ErrorWaybillPostData(SPWrapper.GetAlocateShippingStatus(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public void UpdateShippingErrordetails(string Shippingpck, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateShippingErrorDetais(Shippingpck, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void WaybillCancel(string Shippingpck, string Enviornment)
        {
            try
            {
                SPWrapper.WaybillCancelId(Shippingpck, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CancelData> GetWaybillCancelData(string Enviornment)
        {
            List<CancelData> AllRes = new List<CancelData>();
            try
            {
                AllRes = Mapper.GetWayBillCancelData(SPWrapper.GetWaybillCancelData(Enviornment));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return AllRes;
        }

        public bool BLReversePickupMain(List<ReversePickupDb> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("CId");
                dtinstcode.Columns.Add("reversePickupCode");
                dtinstcode.Columns.Add("pickupInstruction");
                dtinstcode.Columns.Add("trackingLink");
                dtinstcode.Columns.Add("shippingCourier");
                dtinstcode.Columns.Add("trackingNumber");
                dtinstcode.Columns.Add("shippingProviderCode");

                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["CId"] = elements[i].CId;
                    dr["reversePickupCode"] = elements[i].reversePickupCode;
                    dr["pickupInstruction"] = elements[i].pickupInstruction;
                    dr["trackingLink"] = elements[i].trackingLink;
                    dr["shippingCourier"] = elements[i].shippingCourier;
                    dr["trackingNumber"] = elements[i].trackingNumber;
                    dr["shippingProviderCode"] = elements[i].shippingProviderCode;

                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.ReversePickupMain(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public bool BLReversePickUpAddress(List<PickUpAddressDb> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("CId");
                dtinstcode.Columns.Add("id");
                dtinstcode.Columns.Add("name");
                dtinstcode.Columns.Add("addressLine1");
                dtinstcode.Columns.Add("addressLine2");
                dtinstcode.Columns.Add("city");
                dtinstcode.Columns.Add("state");
                dtinstcode.Columns.Add("phone");
                dtinstcode.Columns.Add("pincode");

                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["CId"] = elements[i].CId;
                    dr["id"] = elements[i].id;
                    dr["name"] = elements[i].name;
                    dr["addressLine1"] = elements[i].addressLine1;
                    dr["addressLine2"] = elements[i].addressLine2;
                    dr["city"] = elements[i].city;
                    dr["state"] = elements[i].state;
                    dr["phone"] = elements[i].phone;
                    dr["pincode"] = elements[i].pincode;

                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.ReversePickUpAddress(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public bool BLReverseDimension(List<DimensionDb> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("CId");
                dtinstcode.Columns.Add("boxLength");
                dtinstcode.Columns.Add("boxWidth");
                dtinstcode.Columns.Add("boxHeight");
                dtinstcode.Columns.Add("boxWeight");


                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["CId"] = elements[i].CId;
                    dr["boxLength"] = elements[i].boxLength;
                    dr["boxWidth"] = elements[i].boxWidth;
                    dr["boxHeight"] = elements[i].boxHeight;
                    dr["boxWeight"] = elements[i].boxWeight;


                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.ReverseDimension(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public bool BLReverseCustomField(List<CustomFieldDb> elements, string Enviornment)
        {
            bool res;
            try
            {
                DataTable dtinstcode = new DataTable();
                dtinstcode.Columns.Add("CId");
                dtinstcode.Columns.Add("name");
                dtinstcode.Columns.Add("value");


                for (int i = 0; i < elements.Count; i++)
                {
                    DataRow dr = dtinstcode.NewRow();
                    dr["CId"] = elements[i].CId;
                    dr["name"] = elements[i].name;
                    dr["value"] = elements[i].value;
                    dtinstcode.Rows.Add(dr);
                }
                res = SPWrapper.ReverseCustomField(dtinstcode, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<ReversePickupDb> GetReverseAllData(string Enviornment)
        {
            List<ReversePickupDb> codes = new List<ReversePickupDb>();
            try
            {
                return codes = Mapper.GetReverseAllData(SPWrapper.GetReverseAllData(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ReversePickUpData(ReversePickup updateShippingpackage, string FacilityCode, string Enviornment)
        {
            var id = GenerateNumeric();
            return SPWrapper.IsertRevrserePickUprecords(updateShippingpackage, id, FacilityCode, Enviornment);
        }
        public void ReversePickUpErrorDetails(bool status, string Reason, string triggerid, string Enviornment)
        {
            try
            {
                SPWrapper.ReversePickUpError(status, Reason, triggerid, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void BLUpdateErrorDetailsReversePickup(string reversepickupcode, string Enviornment)
        {
            try
            {
                SPWrapper.UpdateErrorDetailsReversePickup(reversepickupcode, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ServiceResponse<List<EndpointErrorDetails>> BLGetReversePickUpErrorStatus(string Enviornment)
        {
            ServiceResponse<List<EndpointErrorDetails>> codes = new ServiceResponse<List<EndpointErrorDetails>>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.ErrorWaybillPostData(SPWrapper.GetReversePickUpErrorStatus(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public List<ReversePickupDb> GetRetriggerDataReversePickup(string Enviornment)
        {
            List<ReversePickupDb> codes = new List<ReversePickupDb>();
            try
            {
                return codes = Mapper.GetReverseAllData(SPWrapper.GetReversePickupDataForRetrigger(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FacilityDetails> GetFacilityList(string Enviornment)
        {
            List<FacilityDetails> codes = new List<FacilityDetails>();
            try
            {
                return codes = Mapper.GetFacilityCode(SPWrapper.GetFacilityCode(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FacilityMaintain> GetFacilityData(string Enviornment)
        {
            List<FacilityMaintain> codes = new List<FacilityMaintain>();

            try
            {
                return codes = Mapper.GetFacilityData(SPWrapper.GetFacilityMaintainData(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string UploadFacilityMaster(List<FacilityMaintain> cloned, string Enviornment)
        {
            string res;
            try
            {
                DataTable Facility = new DataTable();

                Facility.Columns.Add("FacilityCode");
                Facility.Columns.Add("FacilityName");
                Facility.Columns.Add("Address");
                Facility.Columns.Add("City");
                Facility.Columns.Add("State");
                Facility.Columns.Add("Pincode");
                Facility.Columns.Add("Region");
                Facility.Columns.Add("Mobile_number");
                Facility.Columns.Add("email");
                Facility.Columns.Add("Instance");
                for (var i = 0; i < cloned.Count; i++)
                {
                    DataRow SOrow = Facility.NewRow();
                    SOrow["FacilityCode"] = cloned[i].FacilityCode;
                    SOrow["FacilityName"] = cloned[i].FacilityName;
                    SOrow["Address"] = cloned[i].Address;
                    SOrow["City"] = cloned[i].City;
                    SOrow["State"] = cloned[i].State;
                    SOrow["Pincode"] = cloned[i].Pincode;
                    SOrow["Region"] = cloned[i].Region;
                    SOrow["Mobile_number"] = cloned[i].Mobile;
                    SOrow["email"] = cloned[i].Email;
                    SOrow["Instance"] = cloned[i].Instance;
                    Facility.Rows.Add(SOrow);
                }
                res = SPWrapper.UpdateFaciitymaster(Facility, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public async Task<TrackingResponse> BLinsertTrackingDetails(List<TrackingStatusDb> elements, string Enviornment)
        {
            bool res;
            TrackingResponse reversePickupResponse = new TrackingResponse();
            try
            {
                res =  Mapper.MapinsertTrackingDetails(elements, Enviornment);
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Success to Pando {JsonConvert.SerializeObject(reversePickupResponse)}");

                if (res)
                {
                    reversePickupResponse.successful = true;
                    reversePickupResponse.message = "Data Received from Pando";
                    reversePickupResponse.errors = "";
                    reversePickupResponse.warnings = "";
                }
                else
                {
                    reversePickupResponse.successful = false;
                    reversePickupResponse.message = "No Data Received";
                    reversePickupResponse.errors = "";
                    reversePickupResponse.warnings = "";
                }

                return reversePickupResponse;
            }
            catch (Exception ex)
            {
                reversePickupResponse.successful = false;
                reversePickupResponse.message = "No Data Received";
                reversePickupResponse.errors = ex.Message;
                reversePickupResponse.warnings = "";
                return reversePickupResponse;
            }

        }
        public void InsertTrackingStatusPostdata(List<TrackingStatusDb> updateShippingpackage, string Enviornment)
        {
            //var id = GenerateNumeric();
            DataTable dtinstcode = new DataTable();
            //dtinstcode.Columns.Add("Id");
            dtinstcode.Columns.Add("providerCode");
            dtinstcode.Columns.Add("trackingNumber");
            dtinstcode.Columns.Add("trackingStatus");
            dtinstcode.Columns.Add("statusDate");
            dtinstcode.Columns.Add("shipmentTrackingStatusName");
            dtinstcode.Columns.Add("facilitycode");


            for (int i = 0; i < updateShippingpackage.Count; i++)
            {
                DataRow dr = dtinstcode.NewRow();
                //dr["Id"] = elements[i].Id;
                dr["providerCode"] = updateShippingpackage[i].providerCode;
                dr["trackingNumber"] = updateShippingpackage[i].trackingNumber;
                dr["trackingStatus"] = updateShippingpackage[i].trackingStatus;
                dr["statusDate"] = updateShippingpackage[i].statusDate;
                dr["shipmentTrackingStatusName"] = updateShippingpackage[i].shipmentTrackingStatusName;
                dr["facilitycode"] = updateShippingpackage[i].facilitycode;
                dtinstcode.Rows.Add(dr);
            }
            SPWrapper.InsertTrackingDetailsPostData(dtinstcode, Enviornment);
        }

        public string GetInstanceName(string TrackingNo, string Enviornment)
        {
            //var id = GenerateNumeric();
            return SPWrapper.GetInstanceName(TrackingNo, Enviornment);
        }
        public List<TrackingStatusDb> GetTrackingDetails(string Enviornment, List<TrackingStatusDb> elements)
        {
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
                return Mapper.GetTrackingDetails(SPWrapper.GetTrackingDetails(Enviornment, dtinstcode));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string UpdateTruckDetailsMaster(List<TruckDetails> cloned, string Enviornment)
        {
            string res;
            try
            {
                DataTable TruckDetails = new DataTable();

                TruckDetails.Columns.Add("Details");
                TruckDetails.Columns.Add("Instance");
                for (var i = 0; i < cloned.Count; i++)
                {
                    DataRow SOrow = TruckDetails.NewRow();
                    SOrow["Details"] = cloned[i].Details;
                    SOrow["Instance"] = cloned[i].Instance;
                    TruckDetails.Rows.Add(SOrow);
                }
                res = SPWrapper.UpdateTruckDetails(TruckDetails, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<TruckDetails> GetTruckDetails(string Enviornment)
        {
            List<TruckDetails> codes = new List<TruckDetails>();

            try
            {
                return codes = Mapper.GetTruckDetails(SPWrapper.GetTruckDetails(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string UpdateRegionMaster(List<RegionMaster> cloned, string Enviornment)
        {
            string res;
            try
            {
                DataTable TruckDetails = new DataTable();

                TruckDetails.Columns.Add("State");
                TruckDetails.Columns.Add("Region");
                for (var i = 0; i < cloned.Count; i++)
                {
                    DataRow SOrow = TruckDetails.NewRow();
                    SOrow["State"] = cloned[i].State;
                    SOrow["Region"] = cloned[i].Region;
                    TruckDetails.Rows.Add(SOrow);
                }
                res = SPWrapper.UpdateRegionMaster(TruckDetails, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<RegionMaster> GetRegionDetails(string Enviornment)
        {
            List<RegionMaster> codes = new List<RegionMaster>();

            try
            {
                return codes = Mapper.MPGetRegionDetails(SPWrapper.GetRegionDetails(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<TrackingMaster> GetTrackingStatusDetails(string Enviornment)
        {
            List<TrackingMaster> codes = new List<TrackingMaster>();
            try
            {
                return codes = Mapper.GetTrackingStatusDetails(SPWrapper.GetTrackingStatusDetails(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string UploadtTackingMasterDetails(List<TrackingMaster> cloned, string Enviornment)
        {
            string res;
            try
            {
                DataTable TruckDetails = new DataTable();

                TruckDetails.Columns.Add("UniwareStatus");
                TruckDetails.Columns.Add("PandoStatus");
                TruckDetails.Columns.Add("CourierName");
                for (var i = 0; i < cloned.Count; i++)
                {
                    DataRow SOrow = TruckDetails.NewRow();
                    SOrow["PandoStatus"] = cloned[i].PandoStatus;
                    SOrow["UniwareStatus"] = cloned[i].UniwareStatus;
                    SOrow["CourierName"] = cloned[i].CourierName;
                    TruckDetails.Rows.Add(SOrow);
                }
                res = SPWrapper.UpdateTrackingStatus(TruckDetails, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<TrackingMaster> GetCourierNameDetails(string Enviornment)
        {
            List<TrackingMaster> codes = new List<TrackingMaster>();
            try
            {
                return codes = Mapper.GetCourierNameDetails(SPWrapper.GetCourierNameDetails(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string UploadtCourireDetails(List<TrackingMaster> cloned, string Enviornment)
        {
            string res;
            try
            {
                DataTable TruckDetails = new DataTable();

                TruckDetails.Columns.Add("CourierName");
                for (var i = 0; i < cloned.Count; i++)
                {
                    DataRow SOrow = TruckDetails.NewRow();

                    SOrow["CourierName"] = cloned[i].CourierName;
                    TruckDetails.Rows.Add(SOrow);
                }
                res = SPWrapper.UpdateCourierList(TruckDetails, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public List<TrackingLinkMapping> GetTrackingMappingList(string Enviornment)
        {
            List<TrackingLinkMapping> codes = new List<TrackingLinkMapping>();
            try
            {
                return codes = Mapper.GetTrackingLink(SPWrapper.GetTrackingMapping(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string UploadTrackingMapping(List<TrackingLinkMapping> cloned, string Enviornment)
        {
            string res;

            DataTable DealerTable = ConvertDataTable.ToDataTable(cloned);
            try
            {

                res = SPWrapper.UpdateTrackingMappingList(DealerTable, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }

        public ServiceResponse<MenusAccess> GetRoleMenuAccess(int UserId, string Enviornment)
        {
            return Mapper.GetRoleMenuAccess(SPWrapper.GetRoleMenuAccess(UserId, Enviornment));
        }

        //public void InsertUsername(string Usernmae)
        //{
        //    SPWrapper.InsertTokenUsername(Usernmae);
        //}

        public string GetEnviroment(string Username)
        {
            return SPWrapper.GetEnviornmant(Username);
        }
        public void InsertTransaction(string Userid, string Transaction, string Enviornment)
        {
            try
            {
                SPWrapper.InesrtTransaction(Userid, Transaction, Enviornment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserProfile> GetRoleMaster(string Enviornment)
        {
            return Mapper.GetRoleMaster(SPWrapper.GetRoleMaster(Enviornment));
        }
        public int SaveUser(UserProfile User)
        {
            return SPWrapper.SaveGatePass(User);
        }


        public List<ShippingStatus> GetShippingStatus(string Enviornment)
        {
            List<ShippingStatus> codes = new List<ShippingStatus>();
            try
            {
                return codes = Mapper.GetShippingMaster(SPWrapper.GetShippingStatusMaster(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string UpdateShippingStatusMaster(List<ShippingStatus> cloned, string Enviornment)
        {
            string res;

            DataTable DealerTable = ConvertDataTable.ToDataTable(cloned);
            try
            {

                res = SPWrapper.UpdateShippingStatusMaster(DealerTable, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }
        public string ResetPassword(UserProfile userProfile, string Enviornment)
        {
            string res;

            try
            {

                res = SPWrapper.ResetPassword(userProfile, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }
        public string GetSpecialCharacter(string Enviornment)
        {
            //var id = GenerateNumeric();
            return SPWrapper.GetSpecialCharacter(Enviornment);
        }
        public string UpdateSpecialCharacterMaster(string Characters, string Enviornment)
        {
            string res;
            try
            {
                res = SPWrapper.UpdateSpecialCharacterMaster(Characters, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }


        public ServiceResponse<DashboardsLists> GetDashboardDetails(string Enviornment)
        {
            ServiceResponse<DashboardsLists> List = new ServiceResponse<DashboardsLists>();
            try
            {
                return List = Mapper.GetDashBoardDetails(SPWrapper.GetDashboardDetails(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public ServiceResponse<DashboardsLists> GetDashboardDetailsByName(string Enviornment, string Name)
        //{
        //    ServiceResponse<DashboardsLists> List = new ServiceResponse<DashboardsLists>();
        //    try
        //    {
        //        return List = Mapper.GetDashBoardDetailsByName(SPWrapper.GetDashboardDetails(Enviornment));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}


        public List<TDashboardDetails> GetTrackingDetailsByName(string Enviornment, string Name)
        {
            List<TDashboardDetails> List = new List<TDashboardDetails>();
            try
            {
                return List = Mapper.GetDashBoardDetailsByName(SPWrapper.GetTrackingDetailsByName(Enviornment, Name));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<TDashboardDetails> GetTrackingLink(string Enviornment, string SearchBy, string trackingNo)
        {
            List<TDashboardDetails> List = new List<TDashboardDetails>();
            try
            {
                return List = Mapper.GetTrackingDetailsByName(SPWrapper.GetTrackingLink(Enviornment, SearchBy, trackingNo));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void TrackingStatusError(bool status, string Reason, TrackingStatus elements, string Enviornment,string FacilityCode)
        {
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




                //for (int i = 0; i < elements.Count; i++)
                //{
                    DataRow dr = dtinstcode.NewRow();
                    dr["Id"] = 0;
                    dr["providerCode"] = elements.providerCode;
                    dr["trackingNumber"] = elements.trackingNumber;
                    dr["trackingStatus"] = elements.trackingStatus;
                    dr["statusDate"] = elements.statusDate;
                    dr["shipmentTrackingStatusName"] = elements.shipmentTrackingStatusName;
                    dr["facilitycode"] = null;
                    dr["Instance"] = null;

                    dtinstcode.Rows.Add(dr);
                //}



                SPWrapper.TrackingStatusErrorUpdate(status, Reason, dtinstcode, Enviornment,FacilityCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TDashboardDetails> GetHistoryData(string Enviornment, string FromDate, string ToDate)
        {
            List<TDashboardDetails> codes = new List<TDashboardDetails>();
            try
            {
                return codes = Mapper.GetHistoryData(SPWrapper.GetHistoryData(Enviornment,FromDate,ToDate));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<AllocateshippingPando> GetAllcoaetData(string Enviornment)
        {
            List<AllocateshippingPando> codes = new List<AllocateshippingPando>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.getallocateshippingpost(SPWrapper.Getallocateshippingdemo(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public List<TrackingStatusDb> GetTrackingstatusFailedData(string Enviornment)
        {
            List<TrackingStatusDb> codes = new List<TrackingStatusDb>();

            try
            {
                //CreateLog($"get SKU Code From DB DB");
                return codes = Mapper.getTrackingstatuspost(SPWrapper.GetTrackingstatusdemo(Enviornment));
                //CreateLog($"get SKU Code From DB DB{codes}");
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }

        }
        public List<CityMasterEntity> GetCityMaster(string Enviornment)
        {
            List<CityMasterEntity> codes = new List<CityMasterEntity>();
            try
            {
                return codes = Mapper.GetCityList(SPWrapper.GetCityList(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string UploadCityMaster(List<CityMasterEntity> cloned, string Enviornment)
        {
            string res;
            try
            {
                DataTable TruckDetails = new DataTable();

                TruckDetails.Columns.Add("ReferenceName");
                TruckDetails.Columns.Add("ActualName");
                for (var i = 0; i < cloned.Count; i++)
                {
                    DataRow SOrow = TruckDetails.NewRow();

                    SOrow["ReferenceName"] = cloned[i].ReferenceName;
                    SOrow["ActualName"] = cloned[i].ActualName;
                    TruckDetails.Rows.Add(SOrow);
                }
                res = SPWrapper.UpdateCityMaster(TruckDetails, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public List<DashboardStatusMasterEntity> GetDashboardStatusMaster(string Enviornment)
        {
            List<DashboardStatusMasterEntity> codes = new List<DashboardStatusMasterEntity>();
            try
            {
                return codes = Mapper.GetDashboardStatusList(SPWrapper.GetDashboardStatusList(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string UploadDashboardMaster(List<DashboardStatusMasterEntity> cloned, string Enviornment)
        {
            string res;
            try
            {
                DataTable TruckDetails = new DataTable();

                TruckDetails.Columns.Add("Tracking_Status");
                TruckDetails.Columns.Add("Dashboard_Status");
                for (var i = 0; i < cloned.Count; i++)
                {
                    DataRow SOrow = TruckDetails.NewRow();

                    SOrow["Tracking_Status"] = cloned[i].TrackingStatus;
                    SOrow["Dashboard_Status"] = cloned[i].DashboardStatus;
                    TruckDetails.Rows.Add(SOrow);
                }
                res = SPWrapper.UpdateDashboardStatus(TruckDetails, Enviornment);
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw;
            }
            return res;
        }
        public bool InsertFTLShipmentMain(FTLShipment itemDatun, string Enviornment)
        {
            //string res;
            bool res;

            try
            {
               
                res = SPWrapper.IsertFTLShipmentsMain(itemDatun, Enviornment);
              

            }
            catch (Exception ex)
            {                
                res = false;
            }
            return res;

        }

        public bool InsertFTLShipment(List<Shipment> itemDatun, string ShipmentId,string Enviornment)
        {
            //string res;
            bool res;

            try
            {
                //var dataTable = ConvertDataTable.ToDataTable(itemDatun);
                DataTable FTLShipment = new DataTable();

                FTLShipment.Columns.Add("Shipment_Id");
                FTLShipment.Columns.Add("DeliveryNumbers");
                FTLShipment.Columns.Add("BusinessDivisions");
                FTLShipment.Columns.Add("ship_to_ref_id");
                FTLShipment.Columns.Add("ship_to_type");
                FTLShipment.Columns.Add("sold_to_ref_id");
                FTLShipment.Columns.Add("sold_to_type");
                FTLShipment.Columns.Add("pick_up_ref_id");
                FTLShipment.Columns.Add("Pick_up_Type");
                FTLShipment.Columns.Add("Pod_Available");
                FTLShipment.Columns.Add("Pod_Attachments");

                FTLShipment.Columns.Add("LR_Number");
                FTLShipment.Columns.Add("Consignment_Number");
                FTLShipment.Columns.Add("Tracking_Link");
                for (var i = 0; i < itemDatun.Count; i++)
                {
                    DataRow SOrow = FTLShipment.NewRow();

                    SOrow["Shipment_Id"] = ShipmentId;
                    StringBuilder deliveryno=new StringBuilder();
                    for (int j = 0; j < itemDatun[i].delivery_numbers.Count; j++)
                    {
                        if(j>0)
                        {
                            deliveryno.Append(",");
                        }
                        deliveryno.Append( itemDatun[i].delivery_numbers[j]);
                    }
                    SOrow["DeliveryNumbers"] = deliveryno;
                    StringBuilder businessdivision = new StringBuilder();
                    for (int j = 0; j < itemDatun[i].business_divisions.Count; j++)
                    {
                        if (j > 0)
                        {
                            businessdivision.Append(",");
                        }
                        businessdivision.Append(itemDatun[i].business_divisions[j]);
                    }
                    SOrow["BusinessDivisions"] = businessdivision;
                    SOrow["ship_to_ref_id"] = itemDatun[i].ship_to_ref_id;
                    SOrow["ship_to_type"] = itemDatun[i].ship_to_type;
                    SOrow["sold_to_ref_id"] = itemDatun[i].sold_to_ref_id;
                    SOrow["sold_to_type"] = itemDatun[i].sold_to_type;
                    SOrow["pick_up_ref_id"] = itemDatun[i].pick_up_ref_id;
                    SOrow["Pick_up_Type"] = itemDatun[i].pick_up_type;
                    SOrow["Pod_Available"] = itemDatun[i].pod_available;
                    StringBuilder podattachment = new StringBuilder();
                    for (int j = 0; j < itemDatun[i].pod_attachments.Count; j++)
                    {
                        if (j > 0)
                        {
                            podattachment.Append(",");
                        }
                        podattachment.Append(itemDatun[i].pod_attachments[j]);
                    }
                    SOrow["Pod_Attachments"] = podattachment;
                    SOrow["LR_Number"] = itemDatun[i].lr_number;
                    SOrow["Consignment_Number"] = itemDatun[i].consignment_number;
                    SOrow["Tracking_Link"] = itemDatun[i].tracking_link;
                    FTLShipment.Rows.Add(SOrow);
                }          
                 res = SPWrapper.IsertFTLShipments(FTLShipment, Enviornment);
                //CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Success to Pando {JsonConvert.SerializeObject(reversePickupResponse)}");


            }
            catch (Exception ex)
            {
                //successResponse.status = false;
                //successResponse.waybill = $"No Data Received with Error {ex.Message}";
                //successResponse.shippingLabel = "";
                res=false;
            }
            return res;

        }
        public List<TrackingStatusDb> GetLast30daysStatus(string Enviornment)
        {
            List<TrackingStatusDb> codes = new List<TrackingStatusDb>();

            try
            {
                return codes = Mapper.GetLast30daysStatus(SPWrapper.GeLast30DaysStatus(Enviornment));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}




