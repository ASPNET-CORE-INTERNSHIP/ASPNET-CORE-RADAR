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
            return View();
        }


        [HttpPost]
        public IActionResult NewMode(AddMode mod)
        {
            Guid radar_id = (Guid)TempData["radar_id"];

            Guid? key = null;

            if (TempData.ContainsKey("mode_id"))
            {
                Guid mode_id = (Guid)TempData["mode_id"];
            }
            else
            {
                key = Guid.NewGuid(); //id for mode
                TempData["mode_id"] = key;
            }
            
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Mode(ID, name, radar_id) 
                            VALUES(@ID, @name, @radar_id)";
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@name", mod.name);
                    cmd.Parameters.AddWithValue("@radar_id", radar_id);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New mode added for " + radar_id + " radar";
                        con.Close();

                        //send it to submode because submode needs mode_id
                        TempData["mode_id"] = key;
                        return RedirectToAction("NewSubmode", "AddSubmode");
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
