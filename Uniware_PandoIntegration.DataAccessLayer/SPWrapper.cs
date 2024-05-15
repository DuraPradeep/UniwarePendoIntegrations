using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.DataAccessLayer
{
    public class SPWrapper
    {
        public static string ConnectionString { get; set; }
        public static string ConnectionStringProd { get; set; }

        public SPWrapper(string connectionString, IConfiguration IConfiguration)
        {

            ConnectionString = IConfiguration.GetConnectionString("DBConnection");
            ConnectionStringProd = IConfiguration.GetConnectionString("DBConnectionProd");
            //ConnectionStringProd = IConfiguration.GetConnectionString("DBConnection");
        }
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        private static SqlConnection GetConnectionProd()
        {
            return new SqlConnection(ConnectionStringProd);
        }
        //private static SqlConnection con = null;
        //private static SqlCommand com = null;
        public static bool InsertCodeInUniware(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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

        public static DataSet GetCodeDB(string Instance, string Enviornment)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    com.Parameters.AddWithValue("@instance", Instance);
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static bool InsertSaleOrderDTO(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static bool Insertaddress(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertAddresses";
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 1000;

                    com.Parameters.AddWithValue("@tbltypes", dt);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static bool InsertShippingDetails(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertShippingPackage";
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 1000;
                    com.Parameters.AddWithValue("@tbltypes", dt);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static bool InsertsalesorderItems(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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

        public static bool InsertItems(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertItems";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@tbltypes", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static DataSet GetSkuCodeDB(string Instance, string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetSkuCode",
                        CommandTimeout = 1000
                    };
                    com.Parameters.AddWithValue("@Instance", Instance);

                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static bool IsertItemtypes(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertItemtypes";
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 1000;
                    com.Parameters.AddWithValue("@tbltypes", dt);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static DataSet GetAllSendRecords(string Instance, string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetAllRecords",
                        CommandTimeout = 1000
                    };
                    com.Parameters.AddWithValue("@Instance", Instance);

                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static string IsertAllsendingrec(DataTable dt, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                //con = GetConnection();
                using (con)
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
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }

        public static bool UpdateSalesorderDetails(DataTable dt, int type, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                //con = GetConnection();
                using (con)
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
                    con.Close();
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
        public static void Updatedetailspostdata(bool status, string reason, string Triggerid, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                //con = GetConnection();
                using (con)
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
                    con.Close();
                }


            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }

        public static DataSet GetFailedSendRecords(string Instance, string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_RetriggerFailedData",
                        CommandTimeout = 1000
                    };
                    com.Parameters.AddWithValue("@instance", Instance);

                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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

        public static DataSet GetInstanceFromTriggerTable(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_GetInstanceFromTriggerTable",
                        CommandTimeout = 1000
                    };

                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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

        public static void UpdateStatusinTriggerTable(string Triggerid, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                //con = GetConnection();
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_UpdateTriggerDataStatus";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TriggerId", Triggerid);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }


            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }

        public static DataSet GetCoderetrigger(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static DataSet GetSendCode(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static DataSet GetSkuCodeforRetrigger(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static DataSet GetFailedCode(string Enviornment)
        {

            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static string WaybillinsertMain(OmsToPandoRoot Mainres, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                //con = GetConnection();
                com = new SqlCommand();
                using (con)
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
                    res = res = Convert.ToString(com.Parameters["@Primary_id"].Value);
                    con.Close();
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
        public static bool WaybillShipment(OmsToPandoRoot root, string primaryid, string FacilityCode, string Enviornment)
        {
            bool res = false;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                //con = GetConnection();
                com = new SqlCommand();
                using (con)
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
                    com.Parameters.AddWithValue("@FacilityCode", FacilityCode);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true;
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            //finally { con.Close(); }
            return res;

        }
        public static bool Waybillinsertitems(DataTable ds, string Enviornment)
        {
            bool res = false;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool WaybillinsertCustomfield(DataTable ds, string Enviornment)
        {
            bool res = false;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertwaybillCustomField";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UStypes", ds);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = true; con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool WaybillPickupAddress(PickupAddressDetails root, string primaryid, string Enviornment)
        {
            bool res = false;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    res = true; con.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool WaybillreturnAddress(ReturnAddressDetails root, string primaryid, string Enviornment)
        {
            bool res = false;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    res = true; con.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //finally { con.Close(); }
            return res;

        }
        public static bool WaybilldeliveryAddress(DeliveryAddressDetails root, string primaryid, string Enviornment)
        {
            bool res = false;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    res = true; con.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //finally { con.Close(); }
            return res;

        }
        public static DataSet GetWaybillSendData(string Instance, string Enviornment)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_SendWaybillDetails",
                        CommandTimeout = 1000
                    };
                    com.Parameters.AddWithValue("@instance", Instance);
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static string IsertwaybillPostData(DataTable dt, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)

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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool InsertReturnOrderCode(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static DataSet GetReturnOrderCode(string Instacne, string Enviornment)
        {

            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetReturnOrderCode",
                        CommandTimeout = 1000
                    };
                    com.Parameters.AddWithValue("@Instance", Instacne);
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static bool InsertReturnSaleOrderitem(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static bool InsertReturnaddress(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetReturnOrderSkuCode(string Enviornment)
        {
            //con = GetConnection();
            DataSet ds = new DataSet();

            try
            {
                SqlDataAdapter da;
                SqlCommand com;
                SqlConnection con;

                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();

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
        public static bool InsertReturnOrderItemtypes(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet GetReturnOrderSendData(string Instance, string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_sendReturnOrderAPIData",
                        CommandTimeout = 1000
                    };
                    com.Parameters.AddWithValue("@Instance", Instance);
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static void UpdateWaybillError(bool status, string reason, string Triggerid, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static bool UpdateReurnOrdercodeError(DataTable dt, int type, string Enviornment)
        {
            bool status = false;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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

        public static void UpdateSaleOrderSearchError(string reason, string Enviornment)
        {
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_Firsterror";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                }

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void UpdateReturnOrderError(string reason, string Enviornment)
        {
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReturnOrderCodeError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                }
            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static string IsertReturnOrderPostData(DataTable dt, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static void UpdateReturnOrderPostDataError(bool status, string reason, string Triggerid, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    com.ExecuteNonQuery(); con.Close();
                }
            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static bool InsertGetPassCode(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertGetpassCode";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Getpasscode", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        public static DataSet GetWaybillgatepassCode(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    da.Fill(ds); con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static bool InsertGetPassElements(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertSTOWaybillEmelents";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Elements", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
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
        public static bool InsertItemTypeDTO(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertSTOWayItemTypeDTO";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@itemtypeDTO", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
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
        public static DataSet GetWaybillSKUCde(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    da.Fill(ds); con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static bool InsertWaybillItemsType(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertWaybillItemtypes";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Itemtypes", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
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
        public static DataSet GetWaybillSTOSendData(string Enviornment)
        {

            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    da.Fill(ds); con.Close();
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
        public static string IsertWaybillPostData(DataTable dt, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    res = Convert.ToString(com.Parameters["@Trigger_id"].Value); con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static DataSet CheckLoginCredentials(string UserName, string Password/*,string Enviornment*/)
        {
            //com = new SqlCommand();

            SqlCommand com;
            SqlConnection con = new SqlConnection(ConnectionStringProd);
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            //DataTable dtConfig = new DataTable();
            try
            {
                using (con)
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
                    da.Fill(ds); con.Close();
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
        public static bool InsertSTOAPIGetPassCode(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertSTOAPiGatePass";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@GatePasses", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
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
        public static DataSet GetSTOAPIgatepassCode(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    da.Fill(ds); con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static bool InsertSTOAPIGetPassElements(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertSTOAPIEmelents";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Elements", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
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
        public static bool InsertSTOAPIItemTypeDTO(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertSTOAPIItemTypeDTO";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@itemtypeDTO", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
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
        public static DataSet GetSTOAPISKUCde(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public static bool InsertSTOAPiItemsType(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertSTOAPIItemtypes";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Itemtypes", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
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
        public static DataSet GetSTOAPiSendData(string Instance, string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetallDataSTOApi"
                    };
                    com.Parameters.AddWithValue("@Instance", Instance);
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds); con.Close();
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
        public static string IsertSTOAPIAllData(DataTable dt, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    res = Convert.ToString(com.Parameters["@Trigger_id"].Value); con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static void UpdateSTOWaybillErrorCodesError(string reason, string Enviornment)
        {
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_STOWaybillCodeError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                }
            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static bool UpdateErrorWaybill(DataTable dt, int type, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_StoWaybillGatepassError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Codes", dt);
                    com.Parameters.AddWithValue("@type", type);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
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
        public static void UpdateSTOwaybillErrorpostdata(bool status, string reason, string Triggerid, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    com.ExecuteNonQuery(); con.Close();
                }
            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void UpdateSTOAPIError(string reason, string Enviornment)
        {
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    //con = GetConnection();
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_STOAPIError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                }

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static bool UpdateSTOAPI(DataTable dt, int type, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateSTOAPIError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Codes", dt);
                    com.Parameters.AddWithValue("@type", type);
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
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
        public static void UpdateSTOAPIErrorpostdata(bool status, string reason, string Triggerid, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    com.ExecuteNonQuery(); con.Close();
                }

            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetSTOAPIFailedCode(string Enviornment)
        {
            //con = GetConnection();
            //SqlCommand com ;
            //SqlConnection con;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    da.Fill(ds); con.Close();
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
        public static DataSet GetSTOErrorstatusCode(string Enviornment)
        {
            //con = GetConnection();
            SqlCommand com;
            SqlConnection con;
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_WaybillStoStatus"
                    };
                    com.CommandTimeout = 1000;
                    //con.Open();
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
        public static DataSet GetWaybillPoststatus(string Enviornment)
        {
            //con = GetConnection();
            //SqlCommand com ;
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        //public static string GetWaybillInstanceName(string Enviornment)
        //{
        //    string InstanceName = string.Empty;
        //    try
        //    {
        //        if (Enviornment == "Prod")
        //        {
        //            con = new SqlConnection(ConnectionStringProd);
        //        }
        //        else
        //        {
        //            con = new SqlConnection(ConnectionString);
        //        }
        //        using (con)
        //        {
        //            com = new SqlCommand();
        //            com.Connection = con;
        //            com.CommandText = "Pro_GetWaybillInstance";
        //            com.CommandType = CommandType.StoredProcedure;
        //            con.Open();
        //            SqlDataReader dr;
        //            dr = com.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                InstanceName = dr["Instance"].ToString();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //CreateLog($"Error: {ex.Message}");
        //        throw ex;
        //    }
        //    //finally { con.Close(); }
        //    return InstanceName;

        //}
        public static DataSet GetWaybillInstanceName(string Enviornment)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_GetWaybillInstance",
                        CommandTimeout = 1000
                    };

                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static void UpdateStatusinWaybillTriggerTable(string Triggerid, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                //con = GetConnection();
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_UpdateWaybillFailedTriggerData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@triggerid", Triggerid);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                }


            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void UpdateStatusinUpdateShippingTriggerTable(string ShippingPackageCode, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                //con = GetConnection();
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_UpdatePostDataFailedrecord";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ShippingPackCode", ShippingPackageCode);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                }


            }

            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetWaybillFailedData(string Instance, string Enviornment)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_GetWaybillFailedData",
                        CommandTimeout = 1000
                    };
                    com.Parameters.AddWithValue("@instance", Instance);
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds); con.Close();
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
        public static DataSet ReturnOrderStatus(string Enviornment)
        {
            //con = GetConnection();
            SqlCommand comd;
            SqlConnection cons;
            DataSet ds = new DataSet();
            SqlDataAdapter Sda;
            try
            {

                if (Enviornment == "Prod")
                {
                    cons = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    cons = new SqlConnection(ConnectionString);
                }
                using (cons)
                {
                    comd = new SqlCommand()
                    {
                        Connection = cons,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_ReturnOrderStatus"
                    };
                    comd.CommandTimeout = 1000;
                    cons.Open();
                    Sda = new SqlDataAdapter(comd);
                    Sda.Fill(ds);
                    cons.Close();
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
        public static DataSet GetWaybillgatepassCodeRetrigger(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    da.Fill(ds); con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetSTOAPIgatepassCodeRetrigger(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_getSToGatepassforretrigger"
                    };
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds); con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetReturnOrderCodeRetrigger(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "sp_returnorderCodeforretrigger";
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds); con.Close();
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
            //com = new SqlCommand();
            try
            {
                SqlCommand com;
                SqlConnection con;
                using (con = GetConnectionProd())
                {
                    //con.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = con;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //CommandType = CommandType.StoredProcedure,
                    sqlCommand.CommandText = "sp_tokenvalidate";

                    sqlCommand.Parameters.AddWithValue("@username", UserName);
                    sqlCommand.Parameters.AddWithValue("@password", Password);
                    con.Open();
                    SqlDataReader dr = sqlCommand.ExecuteReader();

                    //da.Fill(ds);
                    while (dr.Read())
                    {
                        tokenEntity.username = dr["username"].ToString();
                        tokenEntity.password = dr["password"].ToString();
                        tokenEntity.Environment = dr["Environment"].ToString();
                    }
                    con.Close();
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return tokenEntity;
        }
        public static void IsertshippingUpdate(DataTable dt, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    //con = GetConnection();
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    //CommandType = CommandType.StoredProcedure,
                    com.CommandText = "sp_UpdateShippingpackage";
                    com.Parameters.AddWithValue("@records", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();

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
        public static void IsertShippingBox(DataTable dt, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ShipingBox";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Records", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void IsertCustomFields(DataTable dt, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_CustomFiels";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Records", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetUpdateShippingData(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    da.Fill(ds); con.Close();
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
        public static bool IsertAllocate_Shipping(DataTable dt, string Enviornment)
        {
            string res;
            bool result = false;

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_Allocate_Shipping";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Records", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery(); con.Close();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return result;

        }
        public static DataSet GetAllocateShippingData(string Enviornment,DataTable dt)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_getAllocate_Shipping",

                    };

                    com.Parameters.AddWithValue("@Records", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds); con.Close();
                }
            }
            catch (Exception ex)
            {
                CreateLog("Allocate Shipping DB Insert " + ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetAllocateShippingDataForRetrigger(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        //public static string IsertUpdateShippingrecords(UpdateShippingpackage dt, string triggerid, string FacilityCode, string Enviornment)
        //{
        //    string res;
        //    try
        //    {
        //        if (Enviornment == "Prod")
        //        {
        //            con = new SqlConnection(ConnectionStringProd);
        //        }
        //        else
        //        {
        //            con = new SqlConnection(ConnectionString);
        //        }
        //        using (con)
        //        {
        //            com = new SqlCommand();
        //            com.Connection = con;
        //            com.CommandText = "sp_insertUpdateshippingFullData";
        //            com.CommandType = CommandType.StoredProcedure;
        //            com.Parameters.AddWithValue("@shippingPackageCode", dt.shippingPackageCode);
        //            com.Parameters.AddWithValue("@name", dt.customFieldValues[0].name);
        //            com.Parameters.AddWithValue("@value", dt.customFieldValues[0].value);
        //            com.Parameters.AddWithValue("@Trigger_Id", triggerid);
        //            com.Parameters.AddWithValue("@FacilityCode", FacilityCode);
        //            com.Parameters.Add("@Triggerid", SqlDbType.VarChar, 100);
        //            com.Parameters["@Triggerid"].Direction = ParameterDirection.Output;
        //            com.CommandTimeout = 1000;
        //            con.Open();
        //            com.ExecuteNonQuery();
        //            res = Convert.ToString(com.Parameters["@Triggerid"].Value);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //finally { con.Close(); }
        //    return res;
        //}

        public static bool IsertUpdateShippingrecords(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_insertUpdateshippingFullData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UpdateList", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static void UpdateShippingError(bool status, string reason, string Triggerid, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "UpdateShippingErrorStatus";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@triggerid", Triggerid);
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }
        }
        public static DataSet GetUpdateShippingStatus(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static DataSet GetUpdateShippingRetrigger(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        //public static string IsertAllocateShippingrecords(Allocateshipping dt, string triggerid, string Enviornment)
        //{
        //    string res;
        //    try
        //    {
        //        if (Enviornment == "Prod")
        //        {
        //            con = new SqlConnection(ConnectionStringProd);
        //        }
        //        else
        //        {
        //            con = new SqlConnection(ConnectionString);
        //        }
        //        using (con)
        //        {
        //            com = new SqlCommand();
        //            com.Connection = con;
        //            com.CommandText = "sp_AllocateShippingPostData";
        //            com.CommandType = CommandType.StoredProcedure;
        //            com.Parameters.AddWithValue("@shippingPackageCode", dt.shippingPackageCode);
        //            com.Parameters.AddWithValue("@shippingLabelMandatory", dt.shippingLabelMandatory);
        //            com.Parameters.AddWithValue("@shippingProviderCode", dt.shippingProviderCode);
        //            com.Parameters.AddWithValue("@shippingCourier", dt.shippingCourier);
        //            com.Parameters.AddWithValue("@trackingNumber", dt.trackingNumber);
        //            com.Parameters.AddWithValue("@trackingLink", dt.trackingLink);
        //            //com.Parameters.AddWithValue("@generateUniwareShippingLabel", dt.generateUniwareShippingLabel);
        //            com.Parameters.AddWithValue("@Trigger_Id", triggerid);

        //            com.Parameters.Add("@Triggerid", SqlDbType.VarChar, 100);
        //            com.Parameters["@Triggerid"].Direction = ParameterDirection.Output;
        //            com.CommandTimeout=1000;
        //            con.Open();
        //            com.ExecuteNonQuery();
        //            res = Convert.ToString(com.Parameters["@Triggerid"].Value);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //finally { con.Close(); }
        //    return res;
        //}

        public static string IsertAllocateShippingrecords(Allocateshipping dt, string triggerid, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    com.Parameters.AddWithValue("@trackingLink", dt.trackingLink);
                    //com.Parameters.AddWithValue("@generateUniwareShippingLabel", dt.generateUniwareShippingLabel);
                    com.Parameters.AddWithValue("@Trigger_Id", triggerid);

                    com.Parameters.Add("@Triggerid", SqlDbType.VarChar, 100);
                    com.Parameters["@Triggerid"].Direction = ParameterDirection.Output;
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Triggerid"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static void AllocateShippingError(bool status, string reason, string shippingPackageCode, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "AllocateShippingErrorStatus";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@triggerid", shippingPackageCode);
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetAlocateShippingStatus(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static void UpdateShippingErrorDetais(string Shippingpck, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_GetUpateShippingDataForRetrigger";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@shippingpackagecode", Shippingpck);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void WaybillCancelId(string waybillId, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_CancelWaybill";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@waybillId", waybillId);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetWaybillCancelData(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static bool ReversePickupMain(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReversePickupMain";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeitems", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static bool ReversePickUpAddress(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReversePickUpAddress";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeitems", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static bool ReverseDimension(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReverseDimension";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeitems", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static bool ReverseCustomField(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_ReverseCustomField";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@typeitems", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static DataSet GetReverseAllData(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetReversePickUpDetails"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static string IsertRevrserePickUprecords(ReversePickup dt, string triggerid, string FacilityCode, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    com.Parameters.AddWithValue("@FacilityCode", FacilityCode);
                    com.Parameters.Add("@Triggerid", SqlDbType.VarChar, 100);
                    com.Parameters["@Triggerid"].Direction = ParameterDirection.Output;
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    res = Convert.ToString(com.Parameters["@Triggerid"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return res;
        }
        public static void ReversePickUpError(bool status, string reason, string Triggerid, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static void UpdateErrorDetailsReversePickup(string reversepickupcode, string Enviornment)
        {

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateReversePickUpdata";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@reversePickupCode", reversepickupcode);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
        public static DataSet GetReversePickUpErrorStatus(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static DataSet GetReversePickupDataForRetrigger(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                com = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_RetriggerReversePickUpCode"
                };
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                //CreateLog(ex.Message);
                throw ex;
            }
            //finally { con.Close(); }
            return ds;
        }
        public static DataSet GetFacilityCode(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static DataSet GetFacilityMaintainData(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
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
        public static string UpdateFaciitymaster(DataTable dt, string Enviornment)
        {
            string status;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertandUpdateFacility";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@status", SqlDbType.VarChar, 100);
                    com.Parameters["@status"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@FacilityDetails", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = Convert.ToString(com.Parameters["@status"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        public static bool InsertTrackingDetails(DataTable dt, string Enviornment)
        {
            bool res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertTrackingDetails";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Trackdetails", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static DataSet GetTrackingDetails(string Enviornment, DataTable dt)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_getTrackingDetails"

                    };
                    com.Parameters.AddWithValue("@Trackdetails", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        //public static void InsertTrackingDetailsPostData(TrackingStatus dt, string triggerid, string Facility, string Enviornment)
        public static void InsertTrackingDetailsPostData(DataTable dt, string Enviornment)
        {
            string res;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertTrackingtatusData";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TrackingDetails", dt);
                    //com.Parameters.AddWithValue("@providerCode", dt.providerCode);
                    //com.Parameters.AddWithValue("@trackingNumber", dt.trackingNumber);
                    //com.Parameters.AddWithValue("@trackingStatus", dt.trackingStatus);
                    //com.Parameters.AddWithValue("@statusDate", dt.statusDate);
                    //com.Parameters.AddWithValue("@shipmentTrackingStatusName", dt.shipmentTrackingStatusName);
                    //com.Parameters.AddWithValue("@facilitycode", Facility);
                    //com.Parameters.AddWithValue("@TriggerId", triggerid);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static string GetInstanceName(string TrackingNo, string Enviornment)
        {
            string InstanceName = string.Empty;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_GetInstanceName";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@TrackingNo", TrackingNo);
                    con.Open();
                    SqlDataReader dr;
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        InstanceName = dr["name"].ToString();

                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }
            return InstanceName;

        }
        public static bool STOWaybillCustField(DataTable ds, string Enviornment)
        {
            bool res = false;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_InsertSTOCustFiled";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@CustDetails", ds);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
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
        public static string UpdateTruckDetails(DataTable dt, string Enviornment)
        {
            string status;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
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
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        public static DataSet GetTruckDetails(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "sp_GetTruckDetails"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static string UpdateRegionMaster(DataTable dt, string Enviornment)
        {
            string status;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Sp_InsertRegionMaster";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@status", SqlDbType.VarChar, 100);
                    com.Parameters["@status"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@RgionList", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = Convert.ToString(com.Parameters["@status"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        public static DataSet GetRegionDetails(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Sp_GetRegionDetails"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static DataSet GetTrackingStatusDetails(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Sp_GetTrackingStatusMaster"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static string UpdateTrackingStatus(DataTable dt, string Enviornment)
        {
            string status;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "sp_UpdateTrackingMaster";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@status", SqlDbType.VarChar, 100);
                    com.Parameters["@status"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@MasterList", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = Convert.ToString(com.Parameters["@status"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        public static DataSet GetCourierNameDetails(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Sp_getCourierName"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static string UpdateCourierList(DataTable dt, string Enviornment)
        {
            string status;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Sp_InsertCourierName";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@status", SqlDbType.VarChar, 100);
                    com.Parameters["@status"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@CourierList", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = Convert.ToString(com.Parameters["@status"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        public static DataSet GetTrackingMapping(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Sp_GetTrackingMappingList"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static string UpdateTrackingMappingList(DataTable dt, string Enviornment)
        {
            string status;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Sp_UploadTrackingmapping";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@status", SqlDbType.VarChar, 100);
                    com.Parameters["@status"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@TrackingMappingList", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = Convert.ToString(com.Parameters["@status"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        public static DataSet GetRoleMenuAccess(int UserId, string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            //xMLCreator = new XMLCreator();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                com = new SqlCommand
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Proc_GetRoleMenuAccess"
                };
                com.Parameters.AddWithValue("@USERID", UserId);
                con.Open();
                da = new SqlDataAdapter(com);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        //public static bool InsertTokenUsername(string Username)
        //{
        //    bool res;
        //    try
        //    {                
        //        using (con=GetConnection())
        //        {
        //            com = new SqlCommand();
        //            com.Connection = con;
        //            com.CommandText = "Sp_Insertuser";
        //            com.CommandType = CommandType.StoredProcedure;
        //            com.Parameters.AddWithValue("@username", Username);
        //            com.CommandTimeout = 1000;
        //            con.Open();
        //            com.ExecuteNonQuery();
        //            res = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //finally { con.Close(); }
        //    return res;
        //}
        public static string GetEnviornmant(string Username)
        {
            bool res;
            string InstanceName = string.Empty;
            SqlCommand cmd;
            SqlConnection cons;
            try
            {

                using (cons = GetConnectionProd())
                {
                    cmd = new SqlCommand();
                    cmd.Connection = cons;
                    cmd.CommandText = "Sp_GetEnviornment";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1000;
                    cmd.Parameters.AddWithValue("@Username", Username);
                    cons.Open();
                    SqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        InstanceName = dr["Environment"].ToString();

                    }
                    cons.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return InstanceName;
        }
        public static void InesrtTransaction(string Userid, string Transaction, string Enviornment)
        {
            //bool res;
            string InstanceName = string.Empty;

            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_InsertTransaction";
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 1000;
                    com.Parameters.AddWithValue("@UserId", Userid);
                    com.Parameters.AddWithValue("@TransactionName", Transaction);
                    con.Open();
                    //SqlDataReader dr;
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            //return InstanceName;
        }
        public static DataSet GetRoleMaster(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_GetRoleMaster"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static int SaveGatePass(UserProfile userProfile)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            int getid = 0;

            //xMLCreator = new XMLCreator();
            //DataSet ds = new DataSet();
            SqlDataAdapter da;
            SqlCommand com;
            SqlConnection con;
            //DataTable dtConfig = new DataTable();
            try
            {

                if (userProfile.Environment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "Pro_SaveUser";
                    com.CommandTimeout = 0;
                    com.Parameters.AddWithValue("@username", userProfile.Username);
                    com.Parameters.AddWithValue("@Password", userProfile.Password);
                    com.Parameters.AddWithValue("@Firstname", userProfile.FirstName);
                    com.Parameters.AddWithValue("@Lastname", userProfile.Lastname);
                    com.Parameters.AddWithValue("@Email", userProfile.Email);
                    com.Parameters.AddWithValue("@Roleid", userProfile.Roleid);
                    com.Parameters.AddWithValue("@MobileNo", userProfile.MobileNumber);
                    com.Parameters.AddWithValue("@Environment", userProfile.Environment);
                    con.Open();
                    getid = (int)com.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                getid = 0;
            }
            //finally
            //{
            //    con.Close();
            //    con = null;
            //}



            return getid;
        }
        public static DataSet GetShippingStatusMaster(string Enviornment)
        {
            //con = GetConnection();
            //com = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand()
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_GetShippingStatus"
                    };
                    com.CommandTimeout = 1000;
                    con.Open();
                    da = new SqlDataAdapter(com);
                    da.Fill(ds);
                    con.Close();
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
        public static string UpdateShippingStatusMaster(DataTable dt, string Enviornment)
        {
            string status;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_InsertShippingStatus";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@status", SqlDbType.VarChar, 100);
                    com.Parameters["@status"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@StatusList", dt);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = Convert.ToString(com.Parameters["@status"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }
        public static string ResetPassword(UserProfile userProfile, string Enviornment)
        {
            string status;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_Resetpassword";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    com.Parameters["@Result"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@username", userProfile.Username);
                    com.Parameters.AddWithValue("@OldPassword", userProfile.Password);
                    com.Parameters.AddWithValue("@NewPassword", userProfile.NewPassword);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    status = Convert.ToString(com.Parameters["@Result"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return status;
        }

        public static string GetSpecialCharacter(string Enviornment)
        {
            string InstanceName = string.Empty;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_GetSpecialCharacter";
                    com.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr;
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        InstanceName = dr["Name"].ToString();

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }
            return InstanceName;

        }

        public static string UpdateSpecialCharacterMaster(string Characters, string Enviornment)
        {
            string RuturnCharacters;
            try
            {
                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Proc_InsertSpecialCharacter";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add("@finalcharacter", SqlDbType.VarChar, 100);
                    com.Parameters["@finalcharacter"].Direction = ParameterDirection.Output;
                    com.Parameters.AddWithValue("@Characters", Characters);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    RuturnCharacters = Convert.ToString(com.Parameters["@finalcharacter"].Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally { con.Close(); }
            return RuturnCharacters;
        }


        public static DataSet GetDashboardDetails(string Enviornment)
        {
            //con = GetConnection();
            SqlCommand comds;
            SqlConnection conss;
            DataSet ds = new DataSet();
            SqlDataAdapter Gda;
            try
            {

                if (Enviornment == "Prod")
                {
                    conss = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    conss = new SqlConnection(ConnectionString);
                }
                using (conss)
                {
                    comds = new SqlCommand()
                    {
                        Connection = conss,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_GetDashboardDetails"
                    };
                    comds.CommandTimeout = 1000;
                    //conss.Open();
                    Gda = new SqlDataAdapter(comds);
                    Gda.Fill(ds);
                    //conss.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public static DataSet GetTrackingDetailsByName(string Enviornment, string Name)
        {
            //con = GetConnection();
            SqlCommand comds;
            SqlConnection conss;
            DataSet ds = new DataSet();
            SqlDataAdapter Gda;
            try
            {
                if (Enviornment == "Prod")
                {
                    conss = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    conss = new SqlConnection(ConnectionString);
                }
                using (conss)
                {
                    comds = new SqlCommand()
                    {
                        Connection = conss,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_GetStatusDetailsByParameter"
                    };
                    comds.Parameters.AddWithValue("@StatusName", Name);
                    comds.CommandTimeout = 1000;
                    //conss.Open();
                    Gda = new SqlDataAdapter(comds);
                    Gda.Fill(ds);
                    //conss.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public static DataSet GetTrackingLink(string Enviornment, string SearchBy, string trackingNo)
        {
            //con = GetConnection();
            SqlCommand comds;
            SqlConnection conss;
            DataSet ds = new DataSet();
            SqlDataAdapter Gda;
            try
            {
                if (Enviornment == "Prod")
                {
                    conss = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    conss = new SqlConnection(ConnectionString);
                }
                using (conss)
                {
                    comds = new SqlCommand()
                    {
                        Connection = conss,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "Pro_getTackingLink"
                    };
                    comds.Parameters.AddWithValue("@searchBy", SearchBy);
                    comds.Parameters.AddWithValue("@number", trackingNo);
                    comds.CommandTimeout = 1000;
                    //conss.Open();
                    Gda = new SqlDataAdapter(comds);
                    Gda.Fill(ds);
                    //conss.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public static void TrackingStatusErrorUpdate(bool status, string reason, DataTable DTData, string Enviornment,string FacilityCode)
        {

            try
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error Table Execute Details. {DTData}, reason {reason}");

                SqlCommand com;
                SqlConnection con;
                if (Enviornment == "Prod")
                {
                    con = new SqlConnection(ConnectionStringProd);
                }
                else
                {
                    con = new SqlConnection(ConnectionString);
                }
                using (con)
                {
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "Pro_InsertTrackingStatusError";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Trackdetails", DTData);
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@Reason", reason);
                    com.Parameters.AddWithValue("@Facility", FacilityCode);
                    com.CommandTimeout = 1000;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            //finally { con.Close(); }

        }
    }


}
