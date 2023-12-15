using DeviceService.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;

namespace DeviceService.Services
{
    /// <summary>
    /// Summary description for DeviceWS
    /// </summary>
    [WebService(Namespace = "http://homeautomation.org/", Description = "CRUD of Devices Database table")]
    public class DeviceWS : WebService
    {
        [WebMethod(Description = "")]
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllDevices()
        {
            DataSet ds = new DataSet();

            //1º ConnectionString no WebConfig
            string cs = ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString;

            //2º OpenConnection
            SqlConnection con = new SqlConnection(cs);

            //3º Query
            string q = "SELECT * FROM Devices";

            //4º Execute
            SqlDataAdapter da = new SqlDataAdapter(q, con);

            da.Fill(ds, "Devices");

            //5º Result
            return ds;
        }

        [WebMethod(Description = "")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public int InsertDevice(Device device)
        {
            int tot = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString))
            {
                con.Open();
                string query = @"INSERT INTO Devices(Name, State, Value, HouseId) 
                                VALUES(@Name, @State, @Value, @HouseId)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@Name", SqlDbType.NChar).Value = device.Name;
                cmd.Parameters.Add("@State", SqlDbType.Bit).Value = device.State;
                cmd.Parameters.Add("@Value", SqlDbType.Float).Value = device.Value;
                cmd.Parameters.Add("@HouseId", SqlDbType.Int).Value = device.HouseId;

                tot = cmd.ExecuteNonQuery();

                con.Close();
            }
            return tot;
        }

        [WebMethod(Description = "")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public int UpdateDevice(Device device)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString);
                
                string cs = "UPDATE Devices SET State='" + 
                    device.State + "'WHERE Name='" + 
                    device.Name + "';";

                SqlCommand cmd = con.CreateCommand();

                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;

                DataSet ds = new DataSet();

                da.Fill(ds, "All");
                return 1;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [WebMethod(Description = "")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public int DeleteDevice(Device device)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString);
                string commandString = "DELETE * FROM Devices WHERE State='" + 
                    device.State + "';";
                
                SqlCommand command = con.CreateCommand();

                command.CommandText = commandString;

                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = command;

                DataSet ds = new DataSet();

                da.Fill(ds, "All");
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
