using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ASPNETAOP.Controllers
{
    public class AddTransmitterController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddTransmitterController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewTransmitter()
        {
            return View();
        }


        [HttpPost]
        public IActionResult NewTransmitter(AddTransmitter transmitter)
        {
            Guid? receiver_id = null;
            String? AntennaDuty = null;
            TempData["newProgram"] = "no";
            if (TempData.ContainsKey("receiver_id"))
            {
                receiver_id = (Guid)TempData.Peek("receiver_id");
                //tempdata that we redirect to antenna controller again or pass the radar controller
                TempData["receiver_id"] = receiver_id;
                //tempdata that we may direct it to antenna view if the receiver has its own antenna(s)
                TempData["ReceiverID"] = receiver_id;
            }
            if (TempData.ContainsKey("AntennaDuty"))
            {
                AntennaDuty = TempData["AntennaDuty"] as string;
            }

            //If the transmitter name is null we give a default name that specifies its number
            String def_name = null;
            if (String.IsNullOrEmpty(transmitter.name))
            {
                string stmt = "SELECT COUNT(*) FROM Transmitter";
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
                def_name = "Transmitter " + count;
            }
            else
            {
                def_name = transmitter.name;
            }

            Guid key = Guid.NewGuid();

            //tempdata that we direct to antenna controller or pass the radar directly
            TempData["transmitter_id"] = key;

            //tempdata that we direct to antenna view so we can handle the issue that users should not select receiver duty for a transmitter antenna
            //Also manage the next button to direct to antenna controller or direct to radar controller
            TempData["TransmitterID"] = key;

            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Transmitter(ID, name, modulation_type, max_frequency, min_frequency, power) 
                            VALUES(@ID, @name, @modulation_type, @max_frequency, @min_frequency, @power)";
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@name", def_name);
                    cmd.Parameters.AddWithValue("@modulation_type", transmitter.modulation_type);
                    cmd.Parameters.AddWithValue("@max_frequency", transmitter.max_frequency);
                    cmd.Parameters.AddWithValue("@min_frequency", transmitter.min_frequency);
                    cmd.Parameters.AddWithValue("@power", transmitter.power);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New Transmitter added";
                        con.Close();
                        //if the duty of antenna is both receiver and transmitter we do not need to add new antenna for transmitter and directly go to radar.
                        if (AntennaDuty.ToString().Equals("both"))
                        {
                            return RedirectToAction("NewRadar", "AddRadar");
                        }
                        else
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
            return View(transmitter);

        }
    }
}