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
    public class ModeController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public ModeController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewMode()
        {
            return View();
        }


        [HttpPost]
        public IActionResult NewMode(Mode mod)
        {
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Mode(ID, name, radar_id) 
                            VALUES(@ID, @name, @radar_id)";
                    Guid key = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@name", mod.name);
                    cmd.Parameters.AddWithValue("@radar_id", Datas.Radar.ID);

                    Datas.Mode = new Mode(key, mod.name, Datas.Radar.ID);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New mode added for " + Datas.Radar.ID + " radar";
                        con.Close();
                        return RedirectToAction("NewSubmode", "Submode");
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }

                }
            }
            return View(mod);
        }

    }
}
