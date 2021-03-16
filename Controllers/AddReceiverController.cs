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
            Guid key = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Receiver(ID, name, listening_time, rest_time, recovery_time) 
                            VALUES(@ID, @name, @listening_time, @rest_time, @recovery_time)";
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@name", receiver.name);
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
                        if (ModelState.IsValid)
                        {
                            //this receiver id goes to controller of antenna
                            TempData["receiver_id"] = key;
                            //this receiver id goes to view of antenna
                            TempData["ReceiverID"] = key;

                            //I use newProgram variable because when i try to add second (or more) receiver antenna
                            //the transmitter antenna id which i used in previous antenna comes from antenna view to antenna controller
                            //and the program automaticly determines that it is an transmitter antenna. So when we add new receiver
                            //it means we create new radar.
                            TempData["newProgram"] = "yes";
                            Console.WriteLine("newReceiver***********************************");
                            return RedirectToAction("NewAntenna", "AddAntenna");
                        }
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
