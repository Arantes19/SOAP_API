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
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="state"></param>
        /// <param name="value"></param>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public int InsertDevice(int id, string name, bool state, double value, int houseId)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString))
                {
                    con.Open();

                    // parameterized queries for better security and to prevent SQL injection.
                    string query = "INSERT INTO Devices(Id, Name, State, Value, HouseId) " +
                                   "VALUES(@Id, @Name, @State, @Value, @HouseId)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@State", state);
                        cmd.Parameters.AddWithValue("@Value", value);
                        cmd.Parameters.AddWithValue("@HouseId", houseId);

                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
                return rowsAffected;
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
        /// <param name="name"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int UpdateDevice(string name, bool state)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString))
                {
                    con.Open();

                    //parameterized queries for better security and to prevent SQL injection.
                    string cs = "UPDATE Devices SET State=@State WHERE Name=@Name";

                    SqlCommand cmd = new SqlCommand(cs, con);
                    cmd.Parameters.AddWithValue("@State", state);
                    cmd.Parameters.AddWithValue("@Name", name);

                    rowsAffected = cmd.ExecuteNonQuery();

                    con.Close();
                }
                return rowsAffected;
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
        /// <param name="state"></param>
        /// <returns></returns>
        public int DeleteDevice(bool state)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString))
                {
                    con.Open();

                    // Use parameterized query to prevent SQL injection
                    string commandString = "DELETE FROM Devices WHERE State = @State";

                    using (SqlCommand command = new SqlCommand(commandString, con))
                    {
                        command.Parameters.AddWithValue("@State", state);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
