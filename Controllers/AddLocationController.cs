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
        public IActionResult NewLocation(AddLocation loc)
        {
            if (TempData.ContainsKey("rec_id") && TempData.ContainsKey("tra_id") && TempData.ContainsKey("radar_name") && TempData.ContainsKey("radar_config") && TempData.ContainsKey("radar_sys"))
            {
                Guid receiver_id = (Guid)TempData.Peek("rec_id");
                Guid transmitter_id = (Guid)TempData.Peek("tra_id");
                String radar_name = TempData.Peek("radar_name") as String;
                String radar_configuration = TempData.Peek("radar_config") as String;
                String radar_system = TempData.Peek("radar_sys") as String;

                //we will use them when we display antennas in addantennascan.
                TempData["rec_id"] = receiver_id;
                TempData["tra_id"] = transmitter_id;

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
                        command.CommandText = @"INSERT INTO Location(ID, name, country, city, geographic_latitude, geographic_longitude, airborne) 
                            VALUES(@ID, @name, @country, @city, @geographic_latitude,@geographic_longitude,@airborne)";
                        command.Parameters.AddWithValue("@ID", key_location);
                        command.Parameters.AddWithValue("@name", loc.name);
                        command.Parameters.AddWithValue("@country", loc.country);
                        command.Parameters.AddWithValue("@city", loc.city);
                        command.Parameters.AddWithValue("@geographic_latitude", loc.geographic_latitude);
                        command.Parameters.AddWithValue("@geographic_longitude", loc.geographic_longitude);
                        command.Parameters.AddWithValue("@airborne", loc.airborne);
                        command.ExecuteNonQuery();

                        command.CommandText = @"INSERT INTO Radar(ID, name, system, configuration, receiver_id, transmitter_id, location_id) 
                            VALUES(@id, @name_radar, @system, @configuration, @receiver_id, @transmitter_id, @location_id)";
                        Guid key = Guid.NewGuid();
                        command.Parameters.AddWithValue("@id", key);
                        command.Parameters.AddWithValue("@name_radar", radar_name);
                        command.Parameters.AddWithValue("@system", radar_system);
                        command.Parameters.AddWithValue("@configuration", radar_configuration);
                        command.Parameters.AddWithValue("@receiver_id", receiver_id);
                        command.Parameters.AddWithValue("@transmitter_id", transmitter_id);
                        command.Parameters.AddWithValue("@location_id", key_location);
                        command.ExecuteNonQuery();

                        //We will send it to mode table
                        TempData["radar_id"] = key;
                        // Attempt to commit the transaction.
                        transaction.Commit();
                        Console.WriteLine("Both records (radar and its location) are written to database.");
                        return RedirectToAction("NewMode", "AddMode");
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
            }
            else
            {
                //GO BACK
            }
            return View(loc);
        }
    }
}
