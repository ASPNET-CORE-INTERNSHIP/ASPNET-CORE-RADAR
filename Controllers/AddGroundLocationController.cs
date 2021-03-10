using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ASPNETAOP.Controllers
{
    public class AddGroundLocationController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddGroundLocationController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewGround()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewGround(AddGroundLocation loc)
        {
            SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true");
            SqlCommand cmd = new SqlCommand("GroundLocation_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@country", loc.country);
            cmd.Parameters.AddWithValue("@city", loc.city);
            cmd.Parameters.AddWithValue("@geographic_latitude", loc.geographic_latitude);
            cmd.Parameters.AddWithValue("@geographic_longitude", loc.geographic_longitude);
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
