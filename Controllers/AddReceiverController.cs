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
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        public string GetID(float listening_time, float rest_time, float recovery_time)
        {
            String receiver_id = "";
            SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true");
            SqlCommand cmd_receiverID = new SqlCommand("SelectIDReceiver", con);
            cmd_receiverID.CommandType = CommandType.StoredProcedure;
            cmd_receiverID.Parameters.AddWithValue("@listening_time", listening_time);
            cmd_receiverID.Parameters.AddWithValue("@rest_time", rest_time);
            cmd_receiverID.Parameters.AddWithValue("@recovery_time", recovery_time);
            con.Open();
            SqlDataReader dr = cmd_receiverID.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    receiver_id = dr.GetString(0);
                }
            }
            dr.Close();
            con.Close();
            return receiver_id;
        }

        [HttpPost]
        public IActionResult NewReceiver(AddReceiver receiver)
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true");
            SqlCommand cmd = new SqlCommand("receiver_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@listening_time", receiver.listening_time);
            cmd.Parameters.AddWithValue("@rest_time", receiver.rest_time);
            cmd.Parameters.AddWithValue("@recovery_time", receiver.recovery_time);
            con.Open();
            int i = cmd.ExecuteNonQuery();

            con.Close();

            if (i != 0)
            {
                ViewData["Message"] = "New Receiver added";
            }

            return View(receiver);
        }
    }
}
