using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.APIs
{
    public class SPWrapper
    {
        public static string ConnectionString { get; set; }

        public SPWrapper(string connectionString, IConfiguration IConfiguration)
        {

            ConnectionString = IConfiguration.GetConnectionString("DBConnection");
        }
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        private static SqlConnection con = null;
        private static SqlCommand com = null;
        public static bool InsertCodeInUniware(DataTable dt)
        {
            bool res;
            try
            {
                
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "Sp_InsertCode";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@tablesearch", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return res;

        }

        public static DataSet GetCodeDB()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_GetCode"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static bool InsertSaleOrderDTO(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_InsertSaleOrderDTO";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@tbltype", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
                 
            }
           return res;

        }
        public static bool Insertaddress(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertAddresses";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@tbltypes", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
               
            }
            return res;
            
        }
        public static bool InsertShippingDetails(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertShippingPackage";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@tbltypes", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw;
            }
            return res;
            
        }
        public static bool InsertsalesorderItems(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertSaleOrderItem";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@tbltypes", dt);
                con.Open();
                com.ExecuteNonQuery();
                res= true;
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return res;
           
        }

        public static bool InsertItems(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertItems";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@tbltypes", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }
        public static DataSet GetSkuCodeDB()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_GetSkuCode"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
                //CreateLog($"Error: {ex.Message}");
            }
            return ds;
        }
        public static bool IsertItemtypes(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_InsertItemtypes";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@tbltypes", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }
        public static DataSet GetAllSendRecords()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_GetAllRecords"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static DataSet PostStatus()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandText = "select TriggerId from TriggerDataRecords where Status=1"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static string IsertAllsendingrec(DataTable dt)
        {
            string res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "Sp_InsertSendingData";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@Trigger_id", SqlDbType.VarChar, 100);
                com.Parameters["@Trigger_id"].Direction = ParameterDirection.Output;
                com.Parameters.AddWithValue("@tbltype", dt);                
                con.Open();
                com.ExecuteNonQuery();
                res = Convert.ToString(com.Parameters["@Trigger_id"].Value);
            }
            catch (Exception ex)
            {               
                throw ex;
            }
            finally { con.Close(); }
            return res;
        }

        public static bool UpdateSalesorderDetails(DataTable dt,int type)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_UpdateSalesorderSearch";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Codes", dt);
                com.Parameters.AddWithValue("@type", type);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            finally { con.Close(); }
            return res;
        }
        public static void Updatedetailspostdata(bool status,string reason,string Triggerid)
        {
           
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_UpdateStatusOfPostdata";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@status", status);
                com.Parameters.AddWithValue("@Reason", reason);
                com.Parameters.AddWithValue("@trigger_id", Triggerid);
                con.Open();
                com.ExecuteNonQuery();
               
            }
            
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            finally { con.Close(); }
            
        }
        public static DataSet GetCoderetrigger()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_retriggersaleSearch"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static DataSet GetSendCode()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_GetsendData"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static DataSet GetSkuCodeforRetrigger()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_RetriggeritemSku"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }

        public static DataSet GetFailedCode()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_GetFailedCodes"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static void CreateLog(string message)
        {
            Log.Information(message);
        }


        public static string WaybillinsertMain(OmsToPandoRoot Mainres)
        {
            string res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_InsertWaybillmain";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@Primary_id", SqlDbType.VarChar, 100);
                com.Parameters["@Primary_id"].Direction = ParameterDirection.Output;
                com.Parameters.AddWithValue("@serviceType", Mainres.serviceType);
                com.Parameters.AddWithValue("@handOverMode", Mainres.handOverMode);
                com.Parameters.AddWithValue("@returnShipmentFlag", Mainres.returnShipmentFlag);
                com.Parameters.AddWithValue("@deliveryAddressId", Mainres.deliveryAddressId);
                com.Parameters.AddWithValue("@pickupAddressId", Mainres.pickupAddressId);
                com.Parameters.AddWithValue("@returnAddressId", Mainres.returnAddressId);
                com.Parameters.AddWithValue("@currencyCode", Mainres.currencyCode);
                com.Parameters.AddWithValue("@paymentMode", Mainres.paymentMode);
                com.Parameters.AddWithValue("@totalAmount", Mainres.totalAmount);
                com.Parameters.AddWithValue("@collectableAmount", Mainres.collectableAmount);
                com.Parameters.AddWithValue("@courierName", Mainres.courierName);
                con.Open();
                com.ExecuteNonQuery();
                res = res = Convert.ToString(com.Parameters["@Primary_id"].Value); ;
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return res;
        }
        public static bool WaybillShipment(OmsToPandoRoot root,string primaryid)
        {
            bool res = false;
            try
            {                
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_InsertShipmentwaybill";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", primaryid);
                com.Parameters.AddWithValue("@Code", root.Shipment.code);
                com.Parameters.AddWithValue("@SaleOrderCode", root.Shipment.SaleOrderCode);
                com.Parameters.AddWithValue("@orderCode", root.Shipment.orderCode);
                com.Parameters.AddWithValue("@channelCode", root.Shipment.channelCode);
                com.Parameters.AddWithValue("@channelName", root.Shipment.channelName);
                com.Parameters.AddWithValue("@invoiceCode", root.Shipment.invoiceCode);
                com.Parameters.AddWithValue("@orderDate", root.Shipment.orderDate);
                com.Parameters.AddWithValue("@fullFilllmentTat", root.Shipment.fullFilllmentTat);
                com.Parameters.AddWithValue("@weight", root.Shipment.weight);
                com.Parameters.AddWithValue("@length", root.Shipment.length);
                com.Parameters.AddWithValue("@height", root.Shipment.height);
                com.Parameters.AddWithValue("@breadth", root.Shipment.breadth);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex) 
            {

                throw;
            }
            return res;

        }

        public static bool Waybillinsertitems(DataTable ds)
        {
            bool res = false;
            try
            {
                
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertWaybillItems";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Ustypes", ds);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static bool WaybillinsertCustomfield(DataTable ds)
        {
            bool res = false;
            try
            {
                
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertwaybillCustomField";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UStypes", ds);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static bool WaybillPickupAddress(PickupAddressDetails root, string primaryid)
        {
            bool res = false;
            try
            {                
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertWaybillPickupAddres";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", primaryid);
                com.Parameters.AddWithValue("@name", root.name);
                com.Parameters.AddWithValue("@email", root.email);
                com.Parameters.AddWithValue("@phone", root.phone);
                com.Parameters.AddWithValue("@address1", root.address1);
                com.Parameters.AddWithValue("@address2", root.address2);
                com.Parameters.AddWithValue("@pincode", root.pincode);
                com.Parameters.AddWithValue("@city", root.city);
                com.Parameters.AddWithValue("@state", root.state);
                com.Parameters.AddWithValue("@country", root.country);
                com.Parameters.AddWithValue("@gstin", root.gstin);   
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return res;
        }
        public static bool WaybillreturnAddress(ReturnAddressDetails root, string primaryid)
        {
            bool res = false;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertreturnAddressDetails";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", primaryid);
                com.Parameters.AddWithValue("@name", root.name);
                com.Parameters.AddWithValue("@email", root.email);
                com.Parameters.AddWithValue("@phone", root.phone);
                com.Parameters.AddWithValue("@address1", root.address1);
                com.Parameters.AddWithValue("@address2", root.address2);
                com.Parameters.AddWithValue("@pincode", root.pincode);
                com.Parameters.AddWithValue("@city", root.city);
                com.Parameters.AddWithValue("@state", root.state);
                com.Parameters.AddWithValue("@country", root.country);
         
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return res;

        }
        public static bool WaybilldeliveryAddress(DeliveryAddressDetails root, string primaryid)
        {
            bool res = false;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertdeliveryadress";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", primaryid);
                com.Parameters.AddWithValue("@name", root.name);
                com.Parameters.AddWithValue("@email", root.email);
                com.Parameters.AddWithValue("@phone", root.phone);
                com.Parameters.AddWithValue("@address1", root.address1);
                com.Parameters.AddWithValue("@address2", root.address2);
                com.Parameters.AddWithValue("@pincode", root.pincode);
                com.Parameters.AddWithValue("@city", root.city);
                com.Parameters.AddWithValue("@state", root.state);
                com.Parameters.AddWithValue("@country", root.country);
                com.Parameters.AddWithValue("@gstin", root.gstin);

                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return res;

        }

        public static DataSet GetWaybillSendData()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_SendWaybillDetails"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }


        public static bool InsertReturnOrderCode(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "returnorderinsertcode";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@typecode", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return res;
        }

        public static DataSet GetReturnOrderCode()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_GetReturnOrderCode"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static bool InsertReturnSaleOrderitem(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "insertReturnSaleorder";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@typesaleorder", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static bool InsertReturnaddress(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "insertReturnaddress";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@typeaddress", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static DataSet GetReturnOrderSkuCode()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_GetSkuCodeReturnOrder"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static bool InsertReturnOrderItemtypes(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "insertReturnOrderItemtypes";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@typeitems", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static DataSet GetReturnOrderSendData()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_sendReturnOrderAPIData"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static void UpdateWaybillError(bool status, string reason)
        {

            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "UpdateErrorStatus";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@status", status);
                com.Parameters.AddWithValue("@Reason", reason);           
                con.Open();
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            finally { con.Close(); }

        }
        public static bool UpdateReurnOrdercodeError(DataTable dt )
        {
            bool status = false;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_ReturnOrderErrorCode";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ErrorlIst", dt);
                
                con.Open();
                com.ExecuteNonQuery();
                status = true;
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            finally { con.Close(); }
            return status;
        }
        public static bool UpdateReurnOrderSKUError(DataTable dt)
        {
            bool status = false;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_ErrorReturnSaleOrder";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ErrorlIst", dt);

                con.Open();
                com.ExecuteNonQuery();
                status = true;
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            finally { con.Close(); }
            return status;
        }

        public static void UpdateSaleOrderSearchError( string reason)
        {
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_Firsterror";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Reason", reason);
              
                con.Open();
                com.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            finally { con.Close(); }

        }
        public static void UpdateReturnOrderError(string reason)
        {
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_ReturnOrderCodeError";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Reason", reason);

                con.Open();
                com.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            finally { con.Close(); }

        }
        public static string IsertReturnOrderPostData(DataTable dt)
        {
            string res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_InertReturnOrderSendData";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@Trigger_id", SqlDbType.VarChar, 100);
                com.Parameters["@Trigger_id"].Direction = ParameterDirection.Output;
                com.Parameters.AddWithValue("@AllRecords", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = Convert.ToString(com.Parameters["@Trigger_id"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { con.Close(); }
            return res;
        }
        public static void UpdateReturnOrderPostDataError(bool status, string reason, string Triggerid)
        {

            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_UpdateErrorStatusPostData";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@status", status);
                com.Parameters.AddWithValue("@Reason", reason);
                com.Parameters.AddWithValue("@trigger_id", Triggerid);
                con.Open();
                com.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            finally { con.Close(); }

        }
        public static bool InsertGetPassCode(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertGetpassCode";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Getpasscode", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static DataSet GetWaybillgatepassCode()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_GetWaybillSTOCode"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {               
                throw ex;
            }
            return ds;
        }
        public static bool InsertGetPassElements(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertSTOWaybillEmelents";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Elements", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static bool InsertItemTypeDTO(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertSTOWayItemTypeDTO";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@itemtypeDTO", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static DataSet GetWaybillSKUCde()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_GetSTOSkucode"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public static bool InsertWaybillItemsType(DataTable dt)
        {
            bool res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_InsertWaybillItemtypes";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Itemtypes", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static DataSet GetWaybillSTOSendData()
        {
            con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Sp_sendDataToWaybillSTO"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            return ds;
        }
        public static string IsertWaybillPostData(DataTable dt)
        {
            string res;
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_insertWaybillSTOPostData";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@Trigger_id", SqlDbType.VarChar, 100);
                com.Parameters["@Trigger_id"].Direction = ParameterDirection.Output;
                com.Parameters.AddWithValue("@SendRecords", dt);
                con.Open();
                com.ExecuteNonQuery();
                res = Convert.ToString(com.Parameters["@Trigger_id"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { con.Close(); }
            return res;
        }
        public static DataSet CheckLoginCredentials(string UserName, string Password)
        {
            con = GetConnection();
            // con = GetConnection();
            // con = new SqlConnection(connectionString);
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = null;
            DataTable dtConfig = new DataTable();
            try
            {
                com = new SqlCommand
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Pro_GetUsernamePassword"
                };
                com.Parameters.AddWithValue("@Username", UserName);
                com.Parameters.AddWithValue("@Password", Password);
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //log.Error($"ServiceResponse Object {JsonConvert.SerializeObject(ex)}");

                // LoggingAdapter.WriteLog("Insert Country Details" + ex.Message + Environment.NewLine);
            }
            finally
            {
                con.Close();
                con = null;
            }

            //  int lintId = 0;

            return ds;
        }

        public static void UpdateSTOWaybillErrorCodesError(string reason)
        {
            try
            {
                con = GetConnection();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "Pro_STOWaybillCodeError";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Reason", reason);

                con.Open();
                com.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            finally { con.Close(); }

        }
    }

}
