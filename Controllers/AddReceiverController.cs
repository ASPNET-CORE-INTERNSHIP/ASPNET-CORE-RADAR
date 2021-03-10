using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ASPNETAOP.Controllers
{
    public class AddReceiverController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddReceiverController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewReceiver()
        {
            return View();
        }


        [HttpPost]
        public IActionResult NewReceiver(AddReceiver receiver)
        {
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Radar(ID, listening_time, rest_time, recovery_time) 
                            VALUES(@ID, @listening_time, @rest_time, @recovery_time)";
                    Guid key = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@listening_time", receiver.listening_time);
                    cmd.Parameters.AddWithValue("@rest_time", receiver.rest_time);
                    cmd.Parameters.AddWithValue("@recovery_time", receiver.recovery_time);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New Receiver added";
                        con.Close();
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }

                }
            }

            return View(receiver);
        }
    }
}
