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
            Datas.newProgram = "no";
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
            Datas.TransmitterID = key;

            Console.WriteLine(Datas.TransmitterID+" tra_id");

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
                        if (Datas.AntennaDuty.Equals("both"))
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