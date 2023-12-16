using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace DeviceService.Services
{
    /// <summary>
    /// Web service for CRUD operations on the Devices database table.
    /// </summary>
    [WebService(Namespace = "http://homeautomation.org/", Description = "CRUD of Devices database table")]
    public class DeviceWS : WebService
    {
        [WebMethod(Description = "Retrieves all devices from the Devices table.")]
        /// <summary>
        /// Retrieves all devices from the Devices table.
        /// </summary>
        /// <returns>A DataSet containing all devices.</returns>
        public DataSet GetAllDevices()
        {
            DataSet ds = new DataSet();

            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Use parameterized query to prevent SQL injection
                    string q = "SELECT * FROM Devices";

                    SqlCommand cmd = new SqlCommand(q, con);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds, "Devices");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log or throw)
                throw ex;
            }

            return ds;
        }

        [WebMethod(Description = "Retrieves a device from the Devices table based on its ID.")]
        /// <summary>
        /// Retrieves a device from the Devices table based on its ID.
        /// </summary>
        /// <param name="deviceId">The ID of the device to retrieve.</param>
        /// <returns>A DataSet containing the device information.</returns>
        public DataSet GetDeviceById(int Id)
        {
            DataSet ds = new DataSet();

            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Use parameterized query to prevent SQL injection
                    string q = "SELECT * FROM Devices WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@Id", Id);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds, "Devices");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log or throw)
                throw ex;
            }

            return ds;
        }


        [WebMethod(Description = "Inserts a new device into the Devices table.")]
        /// <summary>
        /// Inserts a new device into the Devices table.
        /// </summary>
        /// <param name="id">The ID of the device.</param>
        /// <param name="name">The name of the device.</param>
        /// <param name="state">The state of the device.</param>
        /// <param name="value">The value of the device.</param>
        /// <param name="houseId">The ID of the house to which the device belongs.</param>
        /// <returns>The number of rows affected by the insertion operation.</returns>
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

        [WebMethod(Description = "Updates the value of a device in the Devices table.")]
        /// <summary>
        /// Updates the value of a device in the Devices table.
        /// </summary>
        /// <param name="id">The id of the device to update.</param>
        /// <param name="value">The new value of the device.</param>
        /// <returns>The number of rows affected by the update operation.</returns>
        public int UpdateDevice(int id, double value)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString))
                {
                    con.Open();

                    //parameterized queries for better security and to prevent SQL injection.
                    string cs = "UPDATE Devices SET Value=@Value WHERE Id=@Id";

                    SqlCommand cmd = new SqlCommand(cs, con);
                    cmd.Parameters.AddWithValue("@Value", value);
                    cmd.Parameters.AddWithValue("@Id", id);

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

        [WebMethod(Description = "Deletes devices from the Devices table based on their state.")]
        /// <summary>
        /// Deletes devices from the Devices table based on their state.
        /// </summary>
        /// <param name="id">The id of the devices to delete.</param>
        /// <returns>The number of rows affected by the delete operation.</returns>
        public int DeleteDevice(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString))
                {
                    con.Open();

                    // Use parameterized query to prevent SQL injection
                    string commandString = "DELETE FROM Devices WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(commandString, con))
                    {
                        command.Parameters.AddWithValue("@Id", id);

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
