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
    public class AddAntennaScanController : Controller
    {
        private IConfiguration _configuration;
        public AddAntennaScanController(IConfiguration Configuration) { _configuration = Configuration; }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewAntennaScan()
        {
            return View();
        }

        public IActionResult NewAntennaScan(AddAntennaScan ascans, Guid id)
        {
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO AntennaScan(antenna_id, scan_id) 
                            VALUES(@antenna_id, @scan_id)";
                    cmd.Parameters.AddWithValue("@antenna_id", ascans.antenna_id);
                    cmd.Parameters.AddWithValue("@scan_id", id);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New Relationship between antenna and scans added";
                        con.Close();
                        if (ModelState.IsValid)
                        {
                            return RedirectToAction("NewSubmode", "AddSubmode");
                        }
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }

                }
            }
            return View(ascans);
        }
    }
}
