using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ASPNETAOP.Controllers
{
    public class AntennaController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AntennaController(IConfiguration Configuration) { _configuration = Configuration; }

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
        public IActionResult NewAntenna(Antenna antenna)
        {
            Guid receiver_id = Datas.Receiver.ID;
            String def_name = null;
            if (String.IsNullOrEmpty(antenna.name))
            {
                String transmitter_or_receiver_name = null;
                using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        if (antenna.duty.Equals("transmitter"))
                        {
                            Guid transmitter_id = Datas.Transmitter.ID;
                            cmd.CommandText = @"SELECT name FROM Transmitter WHERE ID = @id";
                            cmd.Parameters.AddWithValue("@id", transmitter_id);
                        }
                        else
                        {
                            cmd.CommandText = @"SELECT name FROM Receiver WHERE ID = @id";
                            cmd.Parameters.AddWithValue("@id", receiver_id);
                        }

                        try
                        {
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                transmitter_or_receiver_name = reader.GetString("name");
                            }
                            con.Close();
                        }
                        catch (SqlException e)
                        {
                            ViewData["Message"] = e.Message.ToString() + " Error";
                            def_name = "an error occured";
                        }

                    }
                }

                if (antenna.duty.Equals("both"))
                    def_name = "Monostatic radar antenna with receiver name: " + transmitter_or_receiver_name;
                else
                    def_name = transmitter_or_receiver_name + "s antenna";
            }
            else
                def_name = antenna.name;

            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Antenna(ID, name, type, horizontal_beamwidth, vertical_beamwidth, polarization, number_of_feed, horizontal_dimension, vertical_dimension, duty, transmitter_id, receiver_id, location) 
                            VALUES(@ID, @name, @type, @horizontal_beamwidth, @vertical_beamwidth, @polarization, @number_of_feed, @horizontal_dimension, @vertical_dimension, @duty, @transmitter_id, @receiver_id, @location)";
                    Guid key = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("@ID", key);
                    cmd.Parameters.AddWithValue("@name", def_name);
                    cmd.Parameters.AddWithValue("@type", antenna.type);
                    cmd.Parameters.AddWithValue("@horizontal_beamwidth", antenna.horizontal_beamwidth);
                    cmd.Parameters.AddWithValue("@vertical_beamwidth", antenna.vertical_beamwidth);
                    cmd.Parameters.AddWithValue("@polarization", antenna.polarization);
                    if (antenna.duty.Equals("receiver"))
                        cmd.Parameters.AddWithValue("@number_of_feed", 1);
                    else
                        cmd.Parameters.AddWithValue("@number_of_feed", antenna.number_of_feed);
                    cmd.Parameters.AddWithValue("@horizontal_dimension", antenna.horizontal_dimension);
                    cmd.Parameters.AddWithValue("@vertical_dimension", antenna.vertical_dimension);
                    cmd.Parameters.AddWithValue("@duty", antenna.duty);

                    //if the antenna is both receiver and transmitter antenna give it a receiver and a transmitter id
                    if (antenna.duty.Equals("both"))
                    {
                        Datas.Antenna = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, receiver_id, receiver_id, antenna.location);
                        cmd.Parameters.AddWithValue("@transmitter_id", receiver_id);
                        cmd.Parameters.AddWithValue("@receiver_id", receiver_id);
                    }
                    //if the antenna is a receiver antenna give it a receiver id
                    else if (antenna.duty.Equals("receiver"))
                    {
                        Datas.Antenna = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, null, receiver_id, antenna.location);
                        cmd.Parameters.AddWithValue("@transmitter_id", DBNull.Value);
                        cmd.Parameters.AddWithValue("@receiver_id", receiver_id);
                    }
                    //if the antenna is a transmitter antenna attach it a transmitter id
                    else
                    {
                        Datas.Antenna = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, receiver_id, null, antenna.location);
                        Guid transmitter_id = Datas.Transmitter.ID;
                        cmd.Parameters.AddWithValue("@transmitter_id", transmitter_id);
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

        public IActionResult GoToTransmitter()
        {
            return RedirectToAction("NewTransmitter", "Transmitter");
        }

        public IActionResult GoToRadar()
        {
            return RedirectToAction("NewRadar", "Radar");
        }
    }
}
