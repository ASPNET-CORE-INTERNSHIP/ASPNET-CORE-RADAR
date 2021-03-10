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
            SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true");
            SqlCommand cmd = new SqlCommand("submode_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode_id", sm.mode_id);
            cmd.Parameters.AddWithValue("@PW", sm.PW);
            cmd.Parameters.AddWithValue("@PRI", sm.PRI);
            cmd.Parameters.AddWithValue("@min_frequency", sm.min_frequency);
            cmd.Parameters.AddWithValue("@max_frequency", sm.max_frequency);
            cmd.Parameters.AddWithValue("@power", sm.power);
            cmd.Parameters.AddWithValue("@scan_id", sm.scan_id);
            con.Open();
            int i = cmd.ExecuteNonQuery();

            con.Close();

            if (i != 0)
            {
                ViewData["Message"] = "New Submode added";
            }

            return View(sm);
        }
    }
}
