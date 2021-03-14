using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class AddLocationController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddLocationController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewLocation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewLocation(AddLocation loc, String radar_name, String radar_system, String radar_configuration)
        {

             using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Radar(ID, name, system, configuration, receiver_id, transmitter_id, location_id) 
                            VALUES(@ID, @name, @system, @configuration, @receiver_id, @transmitter_id, @location_id)";
                    Guid key = Guid.NewGuid();
                    Guid receiver_id = Guid.NewGuid();
                    Guid transmitter_id = Guid.NewGuid();
                    Guid location_id = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@name", radar_name);
                    cmd.Parameters.AddWithValue("@system", radar_system);
                    cmd.Parameters.AddWithValue("@configuration", radar_configuration);
                    cmd.Parameters.AddWithValue("@receiver_id", receiver_id);
                    cmd.Parameters.AddWithValue("@transmitter_id", transmitter_id);
                    cmd.Parameters.AddWithValue("@location_id", location_id);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New Radar added";
                        con.Close();
                    }
                    catch (SqlException e)
                    {
                        //ViewData["Message"] = e.Message.ToString() + " Error";
                        ViewData["Message"] = radar_name + " Error";
                    }

                }
            }

            using (SqlConnection connection = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;
                
                try
                {
                    Guid key_location = Guid.NewGuid();
                    command.CommandText = @"INSERT INTO Radar(ID, name, system, configuration, receiver_id, transmitter_id, location_id) 
                            VALUES(@ID, @name, @system, @configuration, @receiver_id, @transmitter_id, @location_id)";
                    Guid key = Guid.NewGuid();
                    //theese variables are temporary
                    Guid transmitter_id = Guid.NewGuid();
                    Guid receiver_id = Guid.NewGuid();
                    command.Parameters.AddWithValue("@ID", key);
                    command.Parameters.AddWithValue("@name", radar_name);
                    command.Parameters.AddWithValue("@system", radar_system);
                    command.Parameters.AddWithValue("@configuration", radar_configuration);
                    command.Parameters.AddWithValue("@receiver_id", receiver_id);
                    command.Parameters.AddWithValue("@transmitter_id", transmitter_id);
                    command.Parameters.AddWithValue("@location_id", key_location);
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO Location(ID, name,country, city, geographic_latitude, geographic_longitude, airborne) 
                            VALUES(@ID, @name, @country, @city, @geographic_latitude,@geographic_longitude,@airborne)";
                    command.Parameters.AddWithValue("@ID", key);
                    command.Parameters.AddWithValue("@name", loc.name);
                    command.Parameters.AddWithValue("@country", loc.country);
                    command.Parameters.AddWithValue("@city", loc.city);
                    command.Parameters.AddWithValue("@geographic_latitude", loc.geographic_latitude);
                    command.Parameters.AddWithValue("@geographic_longitude", loc.geographic_longitude);
                    command.Parameters.AddWithValue("@airborne", loc.airborne);
                    command.ExecuteNonQuery();
                    

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
            /*using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Location(ID, name,country, city, geographic_latitude, geographic_longitude, airborne) 
                            VALUES(@ID, @name, @country, @city, @geographic_latitude,@geographic_longitude,@airborne)";
                    Guid key = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@ID", loc.name);
                    cmd.Parameters.AddWithValue("@country", loc.country);
                    cmd.Parameters.AddWithValue("@city", loc.city);
                    cmd.Parameters.AddWithValue("@geographic_latitude", loc.geographic_latitude);
                    cmd.Parameters.AddWithValue("@geographic_longitude", loc.geographic_longitude);
                    cmd.Parameters.AddWithValue("@airborne", loc.airborne);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New location added";
                        con.Close();
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString()+" Error";
                    }

                }
            }*/

            return View(loc);
        }
    }
}
