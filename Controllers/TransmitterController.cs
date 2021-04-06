using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ASPNETAOP.Controllers
{
    public class TransmitterController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public TransmitterController(IConfiguration Configuration) { _configuration = Configuration; }

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
        public IActionResult NewTransmitter(Transmitter transmitter)
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
            Transmitter transmitter_temp = new Transmitter(key, def_name,transmitter.modulation_type, transmitter.max_frequency, transmitter.min_frequency, transmitter.power);
            Datas.Transmitter = transmitter_temp;

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
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }
                }
                //if the duty of antenna is both receiver and transmitter we do not need to add new antenna for transmitter and directly go to radar.
                if (Datas.Antenna.duty.Equals("both"))
                {
                    Antenna antenna = Datas.Antenna;
                    using (SqlCommand cmd1 = new SqlCommand())
                    {
                        cmd1.Connection = con;
                        cmd1.CommandType = CommandType.Text;
                        cmd1.CommandText = @"INSERT INTO Antenna(ID, name, type, horizontal_beamwidth, vertical_beamwidth, polarization, number_of_feed, horizontal_dimension, vertical_dimension, duty, transmitter_id, receiver_id, location) 
                            VALUES(@ID, @name, @type, @horizontal_beamwidth, @vertical_beamwidth, @polarization, @number_of_feed, @horizontal_dimension, @vertical_dimension, @duty, @transmitter_id, @receiver_id, @location)";
                        cmd1.Parameters.AddWithValue("@ID", antenna.ID);
                        cmd1.Parameters.AddWithValue("@name", antenna.name);
                        cmd1.Parameters.AddWithValue("@type", antenna.type);
                        cmd1.Parameters.AddWithValue("@horizontal_beamwidth", antenna.horizontal_beamwidth);
                        cmd1.Parameters.AddWithValue("@vertical_beamwidth", antenna.vertical_beamwidth);
                        cmd1.Parameters.AddWithValue("@polarization", antenna.polarization);
                        cmd1.Parameters.AddWithValue("@number_of_feed", antenna.number_of_feed);
                        cmd1.Parameters.AddWithValue("@horizontal_dimension", antenna.horizontal_dimension);
                        cmd1.Parameters.AddWithValue("@vertical_dimension", antenna.vertical_dimension);
                        cmd1.Parameters.AddWithValue("@duty", antenna.duty);
                        cmd1.Parameters.AddWithValue("@transmitter_id", key);
                        cmd1.Parameters.AddWithValue("@receiver_id", Datas.Receiver.ID);
                        cmd1.Parameters.AddWithValue("@location", antenna.location);

                        try
                        {
                            con.Open();
                            int i = cmd1.ExecuteNonQuery();
                            if (i != 0)
                                ViewData["Message"] = "New Antenna added";
                            con.Close();
                        }
                        catch (SqlException e)
                        {
                            ViewData["Message"] = e.Message.ToString() + " Error";
                        }
                    }
                    return RedirectToAction("NewRadar", "Radar");
                }
                else
                {
                    return RedirectToAction("NewAntenna", "Antenna");
                }
            }
            return View(transmitter);

        }
    }
}