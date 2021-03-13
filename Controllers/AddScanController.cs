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
        //We need configuration for calling db.
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
        public IActionResult NewScan(AddScan scan)
        {
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Radar(ID, name, type, main_aspect, scan_angle, scan_rate, hits_per_scan) 
                            VALUES(@ID, @name, @type, @main_aspect, @scan_angle, @scan_rate, @hits_per_scan)";
                    Guid key = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@name", scan.name);
                    cmd.Parameters.AddWithValue("@type", scan.type);
                    cmd.Parameters.AddWithValue("@main_aspect", scan.main_aspect);
                    cmd.Parameters.AddWithValue("@scan_angle", scan.scan_angle);
                    cmd.Parameters.AddWithValue("@scan_rate", scan.scan_rate);
                    cmd.Parameters.AddWithValue("@hits_per_scan", scan.hits_per_scan);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New scan added";
                        con.Close();
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }

                }
            }

            return View(scan);
        }
    }
}
