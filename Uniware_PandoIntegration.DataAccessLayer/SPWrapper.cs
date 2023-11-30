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

namespace Uniware_PandoIntegration.DataAccessLayer
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
                using (con = GetConnection())
                {
                    //con = GetConnection();
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Sp_InsertCode";
                    com.CommandTimeout = 1000;
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@tablesearch", dt);
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }

                //con = GetConnection();
                //com = new SqlCommand();
                //com.Connection = con;
                //com.CommandText = "Sp_InsertCode";
                //com.CommandType = CommandType.StoredProcedure;
                //com.Parameters.AddWithValue("@tablesearch", dt);
                //con.Open();
                //com.ExecuteNonQuery();
                //res = true;
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            ////finally { con.Close(); }
            return res;

        }

        public static DataSet GetCodeDB()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    //con = GetConnection();
                    //com = new SqlCommand();
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetCode",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            ////finally { con.Close(); }
            return ds;
        }
        public static bool InsertSaleOrderDTO(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    //con = GetConnection();
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertSaleOrderDTO";
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 1000;
                    com.Parameters.AddWithValue("@tbltype", dt);
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;

            }
            ////finally { con.Close(); }
            return res;

        }
        public static bool Insertaddress(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertAddresses";
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 1000;

                    com.Parameters.AddWithValue("@tbltypes", dt);
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
                //con = GetConnection();

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;

            }
            ////finally { con.Close(); }
            return res;

        }
        public static bool InsertShippingDetails(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertShippingPackage";
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 1000;
                    com.Parameters.AddWithValue("@tbltypes", dt);
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
                //con = GetConnection();

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw;
            }
            ////finally { con.Close(); }
            return res;

        }
        public static bool InsertsalesorderItems(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    //con = GetConnection();
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertSaleOrderItem";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@tbltypes", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            ////finally { con.Close(); }
            return res;

        }

        public static bool InsertItems(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertItems";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@tbltypes", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
                //con = GetConnection();

            }
            catch (Exception ex)
            {

                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            ////finally { con.Close(); }
            return res;
        }
        public static DataSet GetSkuCodeDB()
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetSkuCode",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //CreateLog($"Error: {ex.Message}");
            }
            ////finally { con.Close(); }
            return ds;
        }
        public static bool IsertItemtypes(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertItemtypes";
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 1000;
                    com.Parameters.AddWithValue("@tbltypes", dt);
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
                //con = GetConnection();

            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetAllSendRecords()
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetAllRecords",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        //public static DataSet PostStatus()
        //{
        //    con = GetConnection();
        //    com = new SqlCommand();
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    try
        //    {
        //        com = new SqlCommand()
        //        {
        //            Connection = con,
        //            CommandText = "select TriggerId from TriggerDataRecords where Status=1"
        //        };
        //        con.Open();
        //        da = new SqlDataAdapter(com);
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        //CreateLog(ex.Message);
        //        throw ex;
        //    }
        //    //finally { con.Close(); }
        //    return ds;
        //}
        public static string IsertAllsendingrec(DataTable dt)
        {
            string res;
            try
            {
                //con = GetConnection();
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Sp_InsertSendingData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@Trigger_id", SqlDbType.VarChar, 100);
                    com.Parameters["@Trigger_id"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@tbltype", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Trigger_id"].Value);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }

        public static bool UpdateSalesorderDetails(DataTable dt, int type)
        {
            bool res;
            try
            {
                //con = GetConnection();
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateSalesorderSearch";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Codes", dt);
                    com.Parameters.AddWithValue("@type", type);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }

            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static void Updatedetailspostdata(bool status, string reason, string Triggerid)
        {

            try
            {
                //con = GetConnection();
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateStatusOfPostdata";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.Parameters.AddWithValue("@trigger_id", Triggerid);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }


            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetCoderetrigger()
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_retriggersaleSearch",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetSendCode()
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetsendData",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetSkuCodeforRetrigger()
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_RetriggeritemSku",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }

        public static DataSet GetFailedCode()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetFailedCodes",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
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
                //con = GetConnection();
                com = new SqlCommand();
                using (con = GetConnection())
                {
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
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = res = Convert.ToString(com.Parameters["@Primary_id"].Value); ;
                }

            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool WaybillShipment(OmsToPandoRoot root, string primaryid)
        {
            bool res = false;
            try
            {
                //con = GetConnection();
                com = new SqlCommand();
                using (con = GetConnection())
                {
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
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            //finally { con.Close(); }
            return res;

        }

        public static bool Waybillinsertitems(DataTable ds)
        {
            bool res = false;
            try
            {

                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertWaybillItems";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Ustypes", ds);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool WaybillinsertCustomfield(DataTable ds)
        {
            bool res = false;
            try
            {

                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertwaybillCustomField";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UStypes", ds);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool WaybillPickupAddress(PickupAddressDetails root, string primaryid)
        {
            bool res = false;
            try
            {
                using (con = GetConnection())
                {
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
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool WaybillreturnAddress(ReturnAddressDetails root, string primaryid)
        {
            bool res = false;
            try
            {
                using (con = GetConnection())
                {
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
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //finally { con.Close(); }
            return res;

        }
        public static bool WaybilldeliveryAddress(DeliveryAddressDetails root, string primaryid)
        {
            bool res = false;
            try
            {
                using (con = GetConnection())
                {
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
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //finally { con.Close(); }
            return res;

        }

        public static DataSet GetWaybillSendData()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_SendWaybillDetails",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static string IsertwaybillPostData(DataTable dt)
        {
            string res;
            try
            {
                using (con = GetConnection())

                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertwaybillPostData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@Trigger_id", SqlDbType.VarChar, 100);
                    com.Parameters["@Trigger_id"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@Alldata", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Trigger_id"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }

        public static bool InsertReturnOrderCode(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "returnorderinsertcode";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typecode", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                    //con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }

        public static DataSet GetReturnOrderCode()
        {

            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetReturnOrderCode",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static bool InsertReturnSaleOrderitem(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "insertReturnSaleorder";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typesaleorder", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool InsertReturnaddress(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "insertReturnaddress";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeaddress", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetReturnOrderSkuCode()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {


                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetSkuCodeReturnOrder",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);

                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static bool InsertReturnOrderItemtypes(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "insertReturnOrderItemtypes";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeitems", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetReturnOrderSendData()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_sendReturnOrderAPIData",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static void UpdateWaybillError(bool status, string reason, string Triggerid)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "UpdateErrorStatus";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@triggerid", Triggerid);
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static bool UpdateReurnOrdercodeError(DataTable dt, int type)
        {
            bool status = false;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReturnOrderErrorCode";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ErrorlIst", dt);
                    com.Parameters.AddWithValue("@type", type);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        //public static bool UpdateReurnOrderSKUError(DataTable dt)
        //{
        //    bool status = false;
        //    try
        //    {
        //        con = GetConnection();
        //        com = new SqlCommand();
        //        com.Connection = con;
        //        com.CommandText = "sp_ErrorReturnSaleOrder";
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@ErrorlIst", dt);

        //        con.Open();
        //        com.ExecuteNonQuery();
        //        status = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //CreateLog($"Error: {ex.Message}");
        //        throw ex;
        //    }
        //    //finally { con.Close(); }
        //    return status;
        //}

        public static void UpdateSaleOrderSearchError(string reason)
        {
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_Firsterror";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void UpdateReturnOrderError(string reason)
        {
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReturnOrderCodeError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static string IsertReturnOrderPostData(DataTable dt)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InertReturnOrderSendData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@Trigger_id", SqlDbType.VarChar, 100);
                    com.Parameters["@Trigger_id"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@AllRecords", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Trigger_id"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static void UpdateReturnOrderPostDataError(bool status, string reason, string Triggerid)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateErrorStatusPostData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.Parameters.AddWithValue("@trigger_id", Triggerid);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static bool InsertGetPassCode(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertGetpassCode";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Getpasscode", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static DataSet GetWaybillgatepassCode()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetWaybillSTOCode",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static bool InsertGetPassElements(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertSTOWaybillEmelents";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Elements", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool InsertItemTypeDTO(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertSTOWayItemTypeDTO";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@itemtypeDTO", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetWaybillSKUCde()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetSTOSkucode",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static bool InsertWaybillItemsType(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertWaybillItemtypes";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Itemtypes", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetWaybillSTOSendData()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Sp_sendDataToWaybillSTO",
                        CommandTimeout = 1000
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static string IsertWaybillPostData(DataTable dt)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertWaybillSTOPostData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@Trigger_id", SqlDbType.VarChar, 100);
                    com.Parameters["@Trigger_id"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@SendRecords", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Trigger_id"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet CheckLoginCredentials(string UserName, string Password)
        {
            //con = GetConnection();
            // con = GetConnection();
            // con = new SqlConnection(connectionString);
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = null;
            DataTable dtConfig = new DataTable();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_GetUsernamePassword"
                    };
                    com.Parameters.AddWithValue("@Username", UserName);
                    com.Parameters.AddWithValue("@Password", Password);
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();

            }

            //  int lintId = 0;

            return ds;
        }
        public static bool InsertSTOAPIGetPassCode(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertSTOAPiGatePass";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@GatePasses", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }

        public static DataSet GetSTOAPIgatepassCode()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_STOAPIGetgatepass"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static bool InsertSTOAPIGetPassElements(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertSTOAPIEmelents";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Elements", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool InsertSTOAPIItemTypeDTO(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertSTOAPIItemTypeDTO";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@itemtypeDTO", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetSTOAPISKUCde()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_getSTOAPIItemSku"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    //con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public static bool InsertSTOAPiItemsType(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertSTOAPIItemtypes";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Itemtypes", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetSTOAPiSendData()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetallDataSTOApi"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static string IsertSTOAPIAllData(DataTable dt)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_GetSTOAPIAllData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@Trigger_id", SqlDbType.VarChar, 100);
                    com.Parameters["@Trigger_id"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@AllRecords", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Trigger_id"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }

        public static void UpdateSTOWaybillErrorCodesError(string reason)
        {
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_STOWaybillCodeError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static bool UpdateErrorWaybill(DataTable dt, int type)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_StoWaybillGatepassError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Codes", dt);
                    com.Parameters.AddWithValue("@type", type);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static void UpdateSTOwaybillErrorpostdata(bool status, string reason, string Triggerid)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateSTOWaybillErrorPostData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.Parameters.AddWithValue("@trigger_id", Triggerid);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void UpdateSTOAPIError(string reason)
        {
            try
            {
                using (con = GetConnection())
                {
                    con = GetConnection();
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_STOAPIError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static bool UpdateSTOAPI(DataTable dt, int type)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateSTOAPIError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Codes", dt);
                    com.Parameters.AddWithValue("@type", type);
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }

        public static void UpdateSTOAPIErrorpostdata(bool status, string reason, string Triggerid)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateSTOAPIErrorPostData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.Parameters.AddWithValue("@trigger_id", Triggerid);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetSTOAPIFailedCode()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_STOAPiErrorStatus"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetSTOErrorstatusCode()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_WaybillStoStatus"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetWaybillPoststatus()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetWaybillposterror"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet ReturnOrderStatus()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_ReturnOrderStatus"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }

        public static DataSet GetWaybillgatepassCodeRetrigger()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_waybillGetGatePassForretrigger"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetSTOAPIgatepassCodeRetrigger()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_getSToGatepassforretrigger"
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }

        public static DataSet GetReturnOrderCodeRetrigger()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = con;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "sp_returnorderCodeforretrigger";
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(sqlCommand);
                    da.Fill(ds);
                }
                //com = new SqlCommand()
                //{
                //    Connection = con,
                //    CommandType = CommandType.StoredProcedure,
                //    CommandText = "sp_returnorderCodeforretrigger"
                //};
                //con.Open();
                //da = new SqlDataAdapter(com);
                //da.Fill(ds);

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static TokenEntity Tokencheck(string UserName, string Password)
        {

            //con = GetConnection();
            TokenEntity tokenEntity = new TokenEntity();
            // con = GetConnection();
            // con = new SqlConnection(connectionString);
            com = new SqlCommand();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    //con.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = con;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //CommandType = CommandType.StoredProcedure,
                    sqlCommand.CommandText = "sp_tokenvalidate";

                    sqlCommand.CommandTimeout = 1000;
                    sqlCommand.Parameters.AddWithValue("@username", UserName);
                    sqlCommand.Parameters.AddWithValue("@password", Password);
                    con.Open();
                    SqlDataReader dr = sqlCommand.ExecuteReader();
                    //da.Fill(ds);
                    while (dr.Read())
                    {
                        tokenEntity.username = dr["username"].ToString();
                        tokenEntity.password = dr["password"].ToString();
                    }
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return tokenEntity;
        }

        public static void IsertshippingUpdate(DataTable dt)
        {
            string res;
            try
            {
                using (SqlConnection cons = new SqlConnection(ConnectionString))
                {
                    //con = GetConnection();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = cons;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //CommandType = CommandType.StoredProcedure,
                    sqlCommand.CommandText = "sp_UpdateShippingpackage";
                    sqlCommand.Parameters.AddWithValue("@records", dt);
                    com.CommandTimeout = 1000;
                    cons.Open();
                    sqlCommand.ExecuteNonQuery();

                }
                //con = GetConnection();
                //com = new SqlCommand();
                //com.Connection = con;
                //com.CommandText = "sp_UpdateShippingpackage";
                //com.CommandType = CommandType.StoredProcedure;
                //com.Parameters.AddWithValue("@records", dt);
                //con.Open();
                //com.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void IsertShippingBox(DataTable dt)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ShipingBox";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Records", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void IsertCustomFields(DataTable dt)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_CustomFiels";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Records", dt);
                    com.CommandTimeout=1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetUpdateShippingData()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_postUpdateshipping"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static void IsertAllocate_Shipping(DataTable dt)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_Allocate_Shipping";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Records", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }

        }

        public static DataSet GetAllocateShippingData()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_getAllocate_Shipping"
                    };
                    com.CommandTimeout= 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetAllocateShippingDataForRetrigger()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Retrigger_AllocateShipProvider"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }


        public static string IsertUpdateShippingrecords(UpdateShippingpackage dt, string triggerid, string FacilityCode)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertUpdateshippingFullData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@shippingPackageCode", dt.shippingPackageCode);
                    //com.Parameters.AddWithValue("@shippingProviderCode", dt.shippingProviderCode);
                    //com.Parameters.AddWithValue("@trackingNumber", dt.trackingNumber);
                    //com.Parameters.AddWithValue("@shippingPackageTypeCode", dt.shippingPackageTypeCode);
                    //com.Parameters.AddWithValue("@shippingPackageTypeCode", "");
                    //com.Parameters.AddWithValue("@actualWeight", dt.actualWeight);
                    //com.Parameters.AddWithValue("@noOfBoxes", dt.noOfBoxes);
                    //com.Parameters.AddWithValue("@length", dt.shippingBox.length);
                    //com.Parameters.AddWithValue("@width", dt.shippingBox.width);
                    //com.Parameters.AddWithValue("@height", dt.shippingBox.height);
                    com.Parameters.AddWithValue("@name", dt.customFieldValues[0].name);
                    com.Parameters.AddWithValue("@value", dt.customFieldValues[0].value);
                    com.Parameters.AddWithValue("@Trigger_Id", triggerid);
                    com.Parameters.AddWithValue("@FacilityCode", FacilityCode);
                    com.Parameters.Add("@Triggerid", SqlDbType.VarChar, 100);
                    com.Parameters["@Triggerid"].Direction = ParameterDirection.Output;
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Triggerid"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }

        public static void UpdateShippingError(bool status, string reason, string Triggerid)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "UpdateShippingErrorStatus";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@triggerid", Triggerid);
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout=1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetUpdateShippingStatus()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetUpateShippingerror"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }

        public static DataSet GetUpdateShippingRetrigger()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_retriggerPostdata"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }


        public static string IsertAllocateShippingrecords(Allocateshipping dt, string triggerid)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_AllocateShippingPostData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@shippingPackageCode", dt.shippingPackageCode);
                    com.Parameters.AddWithValue("@shippingLabelMandatory", dt.shippingLabelMandatory);
                    com.Parameters.AddWithValue("@shippingProviderCode", dt.shippingProviderCode);
                    com.Parameters.AddWithValue("@shippingCourier", dt.shippingCourier);
                    com.Parameters.AddWithValue("@trackingNumber", dt.trackingNumber);
                    //com.Parameters.AddWithValue("@generateUniwareShippingLabel", dt.generateUniwareShippingLabel);
                    com.Parameters.AddWithValue("@Trigger_Id", triggerid);

                    com.Parameters.Add("@Triggerid", SqlDbType.VarChar, 100);
                    com.Parameters["@Triggerid"].Direction = ParameterDirection.Output;
                    com.CommandTimeout=1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Triggerid"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static void AllocateShippingError(bool status, string reason, string Triggerid)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "AllocateShippingErrorStatus";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@triggerid", Triggerid);
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }


        public static DataSet GetAlocateShippingStatus()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetAllocateShippingerror"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static void UpdateShippingErrorDetais(string Shippingpck)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_GetUpateShippingDataForRetrigger";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@shippingpackagecode", Shippingpck);
                    com.CommandTimeout=1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void WaybillCancelId(string waybillId)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_CancelWaybill";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@waybillId", waybillId);
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetWaybillCancelData()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_WaybillCancelData"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static bool ReversePickupMain(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReversePickupMain";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeitems", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool ReversePickUpAddress(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReversePickUpAddress";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeitems", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool ReverseDimension(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReverseDimension";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeitems", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool ReverseCustomField(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReverseCustomField";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeitems", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetReverseAllData()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetReversePickUpDetails"
                    };
                    com.CommandTimeout=1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }

        public static string IsertRevrserePickUprecords(ReversePickup dt, string triggerid)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertReversePickupData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@reversePickupCode", dt.reversePickupCode);
                    com.Parameters.AddWithValue("@pickupInstruction", dt.pickupInstruction);
                    com.Parameters.AddWithValue("@trackingLink", dt.trackingLink);
                    com.Parameters.AddWithValue("@shippingCourier", dt.shippingCourier);
                    com.Parameters.AddWithValue("@trackingNumber", dt.trackingNumber);
                    com.Parameters.AddWithValue("@shippingProviderCode", dt.shippingProviderCode);
                    com.Parameters.AddWithValue("@id", dt.pickUpAddress.id);
                    com.Parameters.AddWithValue("@Adrsname", dt.pickUpAddress.name);
                    com.Parameters.AddWithValue("@addressLine1", dt.pickUpAddress.addressLine1);
                    com.Parameters.AddWithValue("@addressLine2", dt.pickUpAddress.addressLine2);
                    com.Parameters.AddWithValue("@city", dt.pickUpAddress.city);
                    com.Parameters.AddWithValue("@state", dt.pickUpAddress.state);
                    com.Parameters.AddWithValue("@phone", dt.pickUpAddress.phone);
                    com.Parameters.AddWithValue("@pincode", dt.pickUpAddress.pincode);
                    com.Parameters.AddWithValue("@boxLength", dt.dimension.boxLength);
                    com.Parameters.AddWithValue("@boxWidth", dt.dimension.boxWidth);
                    com.Parameters.AddWithValue("@boxHeight", dt.dimension.boxHeight);
                    com.Parameters.AddWithValue("@boxWeight", dt.dimension.boxWeight);
                    com.Parameters.AddWithValue("@name", dt.customFields[0].name);
                    com.Parameters.AddWithValue("@value", dt.customFields[0].value);
                    com.Parameters.AddWithValue("@Trigger_Id", triggerid);
                    com.Parameters.Add("@Triggerid", SqlDbType.VarChar, 100);
                    com.Parameters["@Triggerid"].Direction = ParameterDirection.Output;
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Triggerid"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static void ReversePickUpError(bool status, string reason, string Triggerid)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "ReversePickUpErrorStatus";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@triggerid", Triggerid);
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void UpdateErrorDetailsReversePickup(string reversepickupcode)
        {

            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateReversePickUpdata";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@reversePickupCode", reversepickupcode);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetReversePickUpErrorStatus()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_getReversePickUpErrorList"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetReversePickupDataForRetrigger()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_RetriggerReversePickUpCode"
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
            //finally { con.Close(); }
            return ds;
        }

        public static DataSet GetFacilityCode()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Sp_GetFacilityCode"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetFacilityMaintainData()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetFacilityData"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }

        public static string UpdateFaciitymaster(DataTable dt)
        {
            string status;
            try
            {

                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertandUpdateFacility";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@status", SqlDbType.VarChar, 100);
                    com.Parameters["@status"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@FacilityDetails", dt);
                    com.CommandTimeout=1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = Convert.ToString(com.Parameters["@status"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        public static bool InsertTrackingDetails(DataTable dt)
        {
            bool res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertTrackingDetails";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Trackdetails", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetTrackingDetails()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_getTrackingDetails"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static void InsertTrackingDetailsPostData(TrackingStatus dt, string triggerid, string Facility)
        {
            string res;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertTrackingtatusData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@providerCode", dt.providerCode);
                    com.Parameters.AddWithValue("@trackingNumber", dt.trackingNumber);
                    com.Parameters.AddWithValue("@trackingStatus", dt.trackingStatus);
                    com.Parameters.AddWithValue("@statusDate", dt.statusDate);
                    com.Parameters.AddWithValue("@shipmentTrackingStatusName", dt.shipmentTrackingStatusName);
                    com.Parameters.AddWithValue("@facilitycode", Facility);
                    com.Parameters.AddWithValue("@TriggerId", triggerid);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                }
                //res = Convert.ToString(com.Parameters["@Triggerid"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            ////return res;
        }
        public static bool STOWaybillCustField(DataTable ds)
        {
            bool res = false;
            try
            {

                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertSTOCustFiled";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@CustDetails", ds);
                    com.CommandTimeout=1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static string UpdateTruckDetails(DataTable dt)
        {
            string status;
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertTruckDetails";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@status", SqlDbType.VarChar, 100);
                    com.Parameters["@status"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@Records", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = Convert.ToString(com.Parameters["@status"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        public static DataSet GetTruckDetails()
        {
            //con = GetConnection();
            com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (con = GetConnection())
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetTruckDetails"
                    };
                    com.CommandTimeout=1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }

    }


}
