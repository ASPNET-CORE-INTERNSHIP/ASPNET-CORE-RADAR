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
    public class AddAntennaController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddAntennaController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewAntenna()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        [HttpPost]
        public IActionResult NewAntenna(AddAntenna antenna)
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true");
            SqlCommand cmd = new SqlCommand("antenna_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", antenna.type);
            cmd.Parameters.AddWithValue("@horizontal_beamwidth", antenna.horizontal_beamwidth);
            cmd.Parameters.AddWithValue("@vertical_beamwidth", antenna.vertical_beamwidth);
            cmd.Parameters.AddWithValue("@polarization", antenna.polarization);
            cmd.Parameters.AddWithValue("@number_of_feed", antenna.number_of_feed);
            cmd.Parameters.AddWithValue("@horizontal_dimension", antenna.horizontal_dimension);
            cmd.Parameters.AddWithValue("@vertical_dimension", antenna.vertical_dimension);
            cmd.Parameters.AddWithValue("@duty", antenna.duty);
            cmd.Parameters.AddWithValue("@transmitter_id", antenna.transmitter_id);
            cmd.Parameters.AddWithValue("@receiver_id", antenna.receiver_id);
            cmd.Parameters.AddWithValue("@location", antenna.location);
            con.Open();
            int i = cmd.ExecuteNonQuery();

            con.Close();

            if (i != 0)
            {
                ViewData["Message"] = "New Antenna added";
            }

            return View(antenna);
        }
    }
}
