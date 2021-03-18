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
    public class AddSubmodeController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddSubmodeController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewSubmode()
        {
            return View();
        }


        [HttpPost]
        public IActionResult NewSubmode(AddSubmode sm)
        {
            Guid mode_id = (Guid)TempData["mode_id"];

            Guid radar_id = (Guid)TempData["RadarID"];

            //Send it to view. So we can control go back to add mode files again with a button.
            TempData["RadID"] = radar_id;

            //Because we can addmore than one submode to a mode we should pass mode id for each submode adding process
            TempData["mode_id"] = mode_id;

            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Radar(ID, name, mode_id, PW, PRI, min_frequency, max_frequency, scan_id) 
                            VALUES(@ID, @name, @mode_id, @PW, @PRI, @min_frequency, @max_frequency, @scan_id)";
                    Guid key = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@name", sm.name);
                    cmd.Parameters.AddWithValue("@mode_id", mode_id);
                    cmd.Parameters.AddWithValue("@PW", sm.PW);
                    cmd.Parameters.AddWithValue("@PRI", sm.PRI);
                    cmd.Parameters.AddWithValue("@min_frequency", sm.min_frequency);
                    cmd.Parameters.AddWithValue("@max_frequency", sm.max_frequency);
                    cmd.Parameters.AddWithValue("@scan_id", sm.scan_id);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New Submode added";
                        con.Close();
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }

                }
            }
           
            return View(sm);
        }

        public IActionResult GoToMode(Guid radar_id)
        {
            TempData["radar_id"] = radar_id;
            return RedirectToAction("NewMode", "AddMode");
        }

    }
}
/*SELECT* FROM Transmitter;
SELECT* FROM Receiver;
SELECT* FROM Antenna;
SELECT* FROM Radar;
SELECT* FROM Location;
SELECT* FROM Mode;
SELECT* FROM Submode;
DELETE FROM Antenna WHERE number_of_feed < 60;
DELETE FROM Receiver WHERE rest_time < 60;
DELETE FROM Transmitter WHERE max_frequency < 60;
DELETE FROM Location WHERE city = 'GANA';
DELETE FROM Radar WHERE name = 'Friendly ';
DELETE FROM Mode WHERE name ='Friendly ';
DELETE FROM Submode WHERE PW<60;
*/