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
        public IActionResult NewAntenna(AddAntenna antenna)
        {
            String NewProgram = TempData["newProgram"] as string;
            Guid? transmitter_id = null;
            Guid? receiver_id = null;
            if (TempData.ContainsKey("receiver_id"))
            {
                //this value comes from receiver controller
                receiver_id = (Guid)TempData["receiver_id"];
                //this receiver id should go to transmitter so we can carry it up to radar or go back
                //to there if we add more than one antenna to a receiver
                TempData["receiver_id"] = receiver_id;
                //Anddd this is for antenna view. Because the receiver can have more than one antenna and we set a 
                //control structure to duty select are of antenna view to prevent users to select wrong duty 
                //(ex when receiver directs user to antenna page users cannot select transmitter duty)
                //So we should send receiver id to antenna view when we add any type of antenna (again and again)
                TempData["ReceiverID"] = receiver_id;
                TempData["newProgram"] = "yes";
            }
            if (TempData.ContainsKey("transmitter_id") && NewProgram.Equals("no"))
            {
                //this value comes from transmitter controller and EVEN THE CONTROLLER WHICH WE USE BEFORE WE EXECUTE CURRENT!!!!
                transmitter_id = (Guid)TempData["transmitter_id"];
                TempData["transmitter_id"] = transmitter_id;
                //Because the transmitter may have more than one antenna and we have a control if-else structure in antenna view
                //we should send transmitter id to antenna view to prevent users wrong duty selection
                TempData["TransmitterID"] = transmitter_id;
                TempData["newProgram"] = "no";
            }

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

                    //tempdata that we send it to receiver. With this value we can manage not to add redundant transmitter antenna if the antenna duty is 'both (receiver and transmitter)' 
                    TempData["AntennaDuty"] = antenna.duty;

                    //if the antenna is both receiver and transmitter antenna give it a receiver and a transmitter id
                    if (antenna.duty.Equals("both"))
                    {
                        cmd.Parameters.AddWithValue("@transmitter_id", receiver_id);
                        cmd.Parameters.AddWithValue("@receiver_id", receiver_id);
                    }
                    //if the antenna is a receiver antenna give it a receiver id
                    else if (antenna.duty.Equals("receiver"))
                    {
                        cmd.Parameters.AddWithValue("@transmitter_id", DBNull.Value);
                        cmd.Parameters.AddWithValue("@receiver_id", receiver_id);
                    }
                    //if the antenna is a transmitter antenna attach it a transmitter id
                    else
                    {
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

        public IActionResult GoToTransmitter(Guid receiver_id, String antenna_duty)
        {
            TempData["ReceiverID"] = receiver_id; //id represents receiver id
            TempData["AntennaDuty"] = antenna_duty;
            return RedirectToAction("NewTransmitter", "AddTransmitter");
        }

        public IActionResult GoToRadar(Guid receiver_id, Guid transmitter_id)
        {
            TempData["ReceiverID"] = receiver_id;
            TempData["TransmitterID"] = transmitter_id;
            return RedirectToAction("NewRadar", "AddRadar");
        }
    }
}
