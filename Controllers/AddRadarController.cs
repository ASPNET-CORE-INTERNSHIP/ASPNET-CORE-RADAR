using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class AddRadarController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddRadarController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewRadar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewRadar(AddRadar radar)
        {
            /*using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Radar(ID, name,system, configuration, receiver_id, transmitter_id, location_id) 
                            VALUES(@ID, @name, @system, @configuration, @receiver_id, @transmitter_id, @location_id)";
                    Guid key = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@name", radar.name);
                    cmd.Parameters.AddWithValue("@system", radar.system);
                    cmd.Parameters.AddWithValue("@configuration", radar.configuration);
                    cmd.Parameters.AddWithValue("@receiver_id", radar.receiver_id);
                    cmd.Parameters.AddWithValue("@receiver_id", radar.transmitter_id);
                    cmd.Parameters.AddWithValue("@location_id", radar.location_id);

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
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }

                }
            }

            return View(radar);*/
            //Console.WriteLine(radar.name+ " " + radar.configuration+" "+ "000000000000000000000000000000000000000000");
            return RedirectToAction("NewLocation", "AddLocation", new { @radar_name = radar.name,  @radar_system = radar.system, @radar_configuration = radar.configuration }) ;
        }

    }
}
