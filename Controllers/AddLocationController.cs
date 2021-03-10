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
            SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true");
            SqlCommand cmd = new SqlCommand("location_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ground_location", loc.ground_location);
            cmd.Parameters.AddWithValue("@airborne", loc.airborne);
            con.Open();
            int i = cmd.ExecuteNonQuery();

            con.Close();

            if (i != 0)
            {
                ViewData["Message"] = "New location added";
            }

            return View(loc);
        }
    }
}
