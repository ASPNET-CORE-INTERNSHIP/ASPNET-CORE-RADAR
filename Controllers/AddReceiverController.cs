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
            //If the receiver name is null we give a default name that specifies its number
            String rec_name = null;
            if (String.IsNullOrEmpty(receiver.name))
            {
                string stmt = "SELECT COUNT(*) FROM Receiver";
                int count = 0;

                using (SqlConnection thisConnection = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
                {
                    using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                    {
                        thisConnection.Open();
                        count = (int)cmdCount.ExecuteScalar();
                    }
                }
                count = count + 1;
                rec_name = "Receiver " + count;
            }
            else
            {
                rec_name = receiver.name;
            }

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
                    cmd.Parameters.AddWithValue("@name", rec_name);
                    cmd.Parameters.AddWithValue("@listening_time", receiver.listening_time);
                    cmd.Parameters.AddWithValue("@rest_time", receiver.rest_time);
                    cmd.Parameters.AddWithValue("@recovery_time", receiver.recovery_time);
                    Datas.ReceiverID = key;
                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New Receiver added";
                        con.Close();
                        if (ModelState.IsValid)
                        {
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
