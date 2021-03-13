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

            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
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
            }

            return View(loc);
        }
    }
}
