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
    public class AddModeController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddModeController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewMode()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }


        [HttpPost]
        public IActionResult NewMode(AddMode mod)
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true");
            SqlCommand cmd = new SqlCommand("mode_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", mod.name);
            cmd.Parameters.AddWithValue("@radar_id", mod.radar_id);
            con.Open();
            int i = cmd.ExecuteNonQuery();

            con.Close();

            if (i != 0)
            {
                ViewData["Message"] = "New mode added for "+mod.radar_id+" radar";
            }

            return View(mod);
        }
    }
}
