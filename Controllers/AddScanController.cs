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
    public class AddScanController : Controller
    {
        private IConfiguration _configuration;
        public AddScanController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewScan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewScan(AddScan scan, AddSubmode sm)
        {
            if (TempData.ContainsKey("radar_id") && TempData.ContainsKey("mode_id"))
            {
                Guid mode_id = (Guid)TempData["mode_id"];

                Guid radar_id = (Guid)TempData["radar_id"];

                Guid receiver_id = (Guid)TempData["rec_id"];

                Guid transmitter_id = (Guid)TempData["tra_id"];

                Console.WriteLine(mode_id + " /---0_0---/ " + radar_id + " /---0_0---/ " + receiver_id + " /---0_0---/ " +transmitter_id);

                //generate a list for antennas to display in view
                //so the user can select antennas which empolys current scan type
                List<AddAntenna> AntennaList = new List<AddAntenna>();

                //view'e taşı
                using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"SELECT * FROM Antenna WHERE receiver_id = @receiver_id or transmitter_id = @transmitter_id";
                        cmd.Parameters.AddWithValue("@receiver_id", receiver_id);
                        cmd.Parameters.AddWithValue("@transmitter_id", transmitter_id);

                        try
                        {
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                AddAntenna antenna = new AddAntenna();
                                antenna.ID = reader.GetGuid("ID");
                                antenna.name = reader.GetString("name");
                                antenna.type = reader.GetString("type");
                                antenna.horizontal_beamwidth = (float)reader.GetDouble("horizontal_beamwidth");
                                antenna.vertical_beamwidth = (float)reader.GetDouble("vertical_beamwidth");
                                antenna.polarization = reader.GetString("polarization");
                                antenna.number_of_feed = reader.GetInt32("number_of_feed");
                                antenna.horizontal_dimension = (float)reader.GetDouble("horizontal_dimension");
                                antenna.vertical_dimension = (float)reader.GetDouble("vertical_dimension");
                                antenna.duty = reader.GetString("duty");
                                antenna.location = reader.GetString("location");
                                AntennaList.Add(antenna);
                            }
                            //TempData["Antennas"] = AntennaList;
                            con.Close();
                        }
                        catch (SqlException e)
                        {
                            ViewData["Message"] = e.Message.ToString() + " Error";
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
                        command.CommandText = @"INSERT INTO Scan(ID, name, type, main_aspect, scan_angle, scan_rate, hits_per_scan) 
                            VALUES(@ID, @name, @type, @main_aspect, @scan_angle, @scan_rate, @hits_per_scan)";
                        Guid key = Guid.NewGuid();
                        command.Parameters.AddWithValue("@ID", key);
                        command.Parameters.AddWithValue("@name", scan.name);
                        command.Parameters.AddWithValue("@type", scan.type);
                        command.Parameters.AddWithValue("@main_aspect", scan.main_aspect);
                        command.Parameters.AddWithValue("@scan_angle", scan.scan_angle);
                        command.Parameters.AddWithValue("@scan_rate", scan.scan_rate);
                        command.Parameters.AddWithValue("@hits_per_scan", scan.hits_per_scan);
                        command.ExecuteNonQuery();

                        command.CommandText = @"INSERT INTO Submode(ID, name, mode_id, PW, PRI, min_frequency, max_frequency, scan_id) 
                            VALUES(@id, @name_sm, @mode_id, @PW, @PRI, @min_frequency, @max_frequency, @scan_id)";
                        Guid key_submode = Guid.NewGuid();
                        command.Parameters.AddWithValue("@id", key_submode);
                        command.Parameters.AddWithValue("@name_sm", sm.name);
                        command.Parameters.AddWithValue("@mode_id", mode_id);
                        command.Parameters.AddWithValue("@PW", sm.PW);
                        command.Parameters.AddWithValue("@PRI", sm.PW);
                        command.Parameters.AddWithValue("@min_frequency", sm.min_frequency);
                        command.Parameters.AddWithValue("@max_frequency", sm.max_frequency);
                        command.Parameters.AddWithValue("@scan_id", key);
                        command.ExecuteNonQuery();

                        // Attempt to commit the transaction.
                        transaction.Commit();
                        Console.WriteLine("Both records are written to database.");
                        //return RedirectToAction("AddAntennaScans", "AntennaScans", new { id = key});
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
            return View(scan);
        }

    }
}
