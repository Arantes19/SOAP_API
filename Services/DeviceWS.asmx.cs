using DeviceService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        /// <returns>A List containing all devices.</returns>
        public List<Device> GetAllDevices()
        {
            List<Device> devices = new List<Device>();

            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Use parameterized query to prevent SQL injection
                    string q = "SELECT * FROM Devices";

                    SqlCommand cmd = new SqlCommand(q, con);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            devices.Add(new Device
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                // Check for DBNull before retrieving boolean value
                                State = reader.IsDBNull(2) ? false : reader.GetBoolean(2),
                                // Check for DBNull before retrieving double value
                                Value = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3),
                                HouseId = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log or throw)
                throw ex;
            }

            return devices;
        }

        [WebMethod(Description = "Retrieves a device from the Devices table based on the ID of the User.")]
        /// <summary>
        /// Retrieves a device from the Devices table based on the ID of the User.
        /// </summary>
        /// <param name="userId">The ID of the User to retrieve.</param>
        /// <returns>A List containing the device information.</returns>
        public List<Device> GetDeviceByUserId(int userId)
        {
            List<Device> devices = new List<Device>();

            try
            {
                string cs = ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Use parameterized query to prevent SQL injection
                    string q = "SELECT d.* " +
                        "FROM Users u " +
                        "JOIN Houses h ON u.Id = h.UserId " +
                        "JOIN Devices d ON h.Id = d.HouseId " +
                        "WHERE u.Id = @userId;";

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            devices.Add(new Device
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                // Check for DBNull before retrieving boolean value
                                State = reader.IsDBNull(2) ? false : reader.GetBoolean(2),
                                // Check for DBNull before retrieving double value
                                Value = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3),
                                HouseId = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log or throw)
                throw ex;
            }

            return devices;
        }


        [WebMethod(Description = "Inserts a new device into the Devices table.")]
        /// <summary>
        /// Inserts a new device into the Devices table.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        /// <param name="state">The state of the device.</param>
        /// <param name="value">The value of the device.</param>
        /// <param name="houseId">The ID of the house to which the device belongs.</param>
        /// <returns>The number of rows affected by the insertion operation.</returns>
        public int InsertDevice(string name, bool? state, double? value, int houseId)
        {
            int rowsAffected = 0;
            state = state is null ? false : state;
            value = value is null ? 0 : value;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DevicesConnectionString"].ConnectionString))
                {
                    con.Open();

                    // parameterized queries for better security and to prevent SQL injection.
                    string query = "INSERT INTO Devices(Name, State, Value, HouseId) " +
                                   "VALUES(@Name, @State, @Value, @HouseId)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@State", state);
                        cmd.Parameters.AddWithValue("@Value", value);
                        cmd.Parameters.AddWithValue("@HouseId", houseId);

                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
                return rowsAffected;
            }
            catch (Exception)
            {
                return -1;
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
        /// Deletes devices from the Devices table based on their Id.
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
