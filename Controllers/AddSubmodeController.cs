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
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Radar(ID, mode_id, PW, PRI, min_frequency, max_frequency, scan_id) 
                            VALUES(@ID, @mode_id, @PW, @PRI, @min_frequency, @max_frequency, @scan_id)";
                    Guid key = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@mode_id", sm.mode_id);
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
    }
}
