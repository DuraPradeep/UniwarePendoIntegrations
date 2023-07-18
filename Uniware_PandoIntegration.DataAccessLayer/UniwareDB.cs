using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.DataAccessLayer
{
    public class UniwareDB
    {
        public static  string ConnectionString { get; set; }

        public UniwareDB(string connectionString, IConfiguration IConfiguration)
        {
            
            ConnectionString = IConfiguration.GetConnectionString("DBConnection");
        }
        private static  SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        private static SqlConnection con = null;
        private static SqlCommand com = null;
        public static void InsertCodeInUniware(DataTable dt)
        {
            
            con = GetConnection();
            com = new SqlCommand();
            com.Connection = con;
            com.CommandText = "Sp_InsertCode";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@tablesearch", dt);
            con.Open();
            com.ExecuteNonQuery();

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
                da=new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public static void InsertSaleOrderDTO(DataTable dt)
        {
            con = GetConnection();
            com = new SqlCommand();
            com.Connection = con;
            com.CommandText = "sp_InsertSaleOrderDTO";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@tbltype", dt);
            con.Open();
            com.ExecuteNonQuery();

        }       
        public static void Insertaddress(DataTable dt)
        {
            con = GetConnection();
            com = new SqlCommand();
            com.Connection = con;
            com.CommandText = "sp_insertAddresses";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@tbltypes", dt);
            con.Open();
            com.ExecuteNonQuery();
        }
        public static void InsertShippingDetails(DataTable dt)
        {
            con = GetConnection();
            com = new SqlCommand();
            com.Connection = con;
            com.CommandText = "sp_insertShippingPackage";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@tbltypes", dt);
            con.Open();
            com.ExecuteNonQuery();
        }
        public static void InsertsalesorderItems(DataTable dt)
        {
            con = GetConnection();
            com = new SqlCommand();
            com.Connection = con;
            com.CommandText = "sp_insertSaleOrderItem";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@tbltypes", dt);
            con.Open();
            com.ExecuteNonQuery();
        }
       
        public static void IsertItemtypes(DataTable dt)
        {
            con = GetConnection();
            com = new SqlCommand();
            com.Connection = con;
            com.CommandText = "sp_InsertItemtypes";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@tbltypes", dt);
            con.Open();
            com.ExecuteNonQuery();
        }
        public static void IsertItems(DataTable dt)
        {
            con = GetConnection();
            com = new SqlCommand();
            com.Connection = con;
            com.CommandText = "sp_insertItems";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@tbltypes", dt);
            con.Open();
            com.ExecuteNonQuery();
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
            }
            return ds;
        }
    }
}
