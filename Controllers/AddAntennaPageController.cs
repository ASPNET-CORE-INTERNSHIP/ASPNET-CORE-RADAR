using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace ASPNETAOP.Controllers
{
    public class AddAntennaPageController : Controller
    {
        private IConfiguration _configuration;
        public AddAntennaPageController(IConfiguration Configuration) { _configuration = Configuration; }

        public IActionResult Add(AddAntennaPage antenna)
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            Console.WriteLine("Enter");

            // Add a new user to the database
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "insert into Antenna(type, horizontal_beamwidth, vertical_beamwidth, polarization, number_of_feed, horizontal_dimension, vertical_dimension) values ('" + antenna.type + "', '" + antenna.horizontal_beamwidth + "', '" + antenna.vertical_beamwidth + "', '" + antenna.polarization + "', '" + antenna.number_of_feed + "', '" + antenna.horizontal_dimension + "', '" + antenna.vertical_dimension + "' )";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();

                    ViewData["Message"] = "Antenna has been successfully";

                    sqlconn.Close();
                }
            }

            Console.WriteLine("Exit");

            return View(antenna);
        }
    }
}
