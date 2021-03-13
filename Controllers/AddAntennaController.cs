using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ASPNETAOP.Controllers
{
    public class AddAntennaController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddAntennaController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewAntenna()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAntenna(AddAntenna antenna, Guid id, SqlCommand? cmd_transmitter_receiver)
        {
            //Guid id = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Antenna(ID, type, horizontal_beamwidth, vertical_beamwidth, polarization, number_of_feed, horizontal_dimension, vertical_dimension, duty, transmitter_id, receiver_id, location) 
                            VALUES(@ID, @type, @horizontal_beamwidth, @vertical_beamwidth, @polarization, @number_of_feed, @horizontal_dimension, @vertical_dimension, @duty, @transmitter_id, @receiver_id, @location)";
                    Guid key = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@type", antenna.type);
                    cmd.Parameters.AddWithValue("@horizontal_beamwidth", antenna.horizontal_beamwidth);
                    cmd.Parameters.AddWithValue("@vertical_beamwidth", antenna.vertical_beamwidth);
                    cmd.Parameters.AddWithValue("@polarization", antenna.polarization);
                    cmd.Parameters.AddWithValue("@number_of_feed", antenna.number_of_feed);
                    cmd.Parameters.AddWithValue("@horizontal_dimension", antenna.horizontal_dimension);
                    cmd.Parameters.AddWithValue("@vertical_dimension", antenna.vertical_dimension);
                    cmd.Parameters.AddWithValue("@duty", antenna.duty);
                    //if the antenna is both receiver and transmitter antenna give it a receiver and a transmitter id
                    if (antenna.duty.Equals("both"))
                    {
                        cmd.Parameters.AddWithValue("@transmitter_id", id);
                        cmd.Parameters.AddWithValue("@receiver_id", id);
                    }
                    //if the antenna is a receiver antenna give it a receiver id
                    else if (antenna.duty.Equals("receiver"))
                    {
                        cmd.Parameters.AddWithValue("@transmitter_id", DBNull.Value);
                        cmd.Parameters.AddWithValue("@receiver_id", id);
                    }
                    //if the antenna is a transmitter antenna attach it a transmitter id
                    else
                    {
                        cmd.Parameters.AddWithValue("@transmitter_id", id);
                        cmd.Parameters.AddWithValue("@receiver_id", DBNull.Value);
                    }
                    
                    cmd.Parameters.AddWithValue("@location", antenna.location);

                    try
                    {
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                            ViewData["Message"] = "New Antenna added";
                        con.Close();
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }

                }
            }

            return View(antenna);
        }
    }
}
