using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

                Console.WriteLine(mode_id + " /---0_0---/ " + radar_id);

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
