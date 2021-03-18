using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class AddRadarController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddRadarController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewRadar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewRadar(AddRadar radar)
        {
            //IF you want to start addaing radar and do not want to add specific transmitter or receiver use below code and  comment other
            //For testing routing between scan and submode i use this code and comment the code which is 
            //not commented for now :\
            /*Guid receiver_id = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    String name = "receiver";
                    float listening_time = 3;
                    float recovery_time = 3;
                    float rest_time = 3;
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Receiver(ID, name, listening_time, rest_time, recovery_time) 
                            VALUES(@ID, @name, @listening_time, @rest_time, @recovery_time)";
                    cmd.Parameters.AddWithValue("@ID", receiver_id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@listening_time", listening_time);
                    cmd.Parameters.AddWithValue("@rest_time", rest_time);
                    cmd.Parameters.AddWithValue("@recovery_time", recovery_time);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            Guid transmitter_id = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    String name = "transmitter";
                    int max_frequency = 3;
                    String modulation = "FM-frequency modulation";
                    int min_frequency = 3;
                    int power = 3;
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Transmitter(ID, name, modulation_type, max_frequency, min_frequency, power) 
                            VALUES(@ID, @name, @modulation_type, @max_frequency, @min_frequency, @power)";
                    cmd.Parameters.AddWithValue("@ID", transmitter_id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@modulation_type", modulation);
                    cmd.Parameters.AddWithValue("@max_frequency", max_frequency);
                    cmd.Parameters.AddWithValue("@min_frequency", min_frequency);
                    cmd.Parameters.AddWithValue("@power", power);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            TempData["rec_id"] = receiver_id;
            TempData["tra_id"] = transmitter_id;
            TempData["radar_name"] = radar.name;
            TempData["radar_config"] = radar.configuration;
            TempData["radar_sys"] = radar.system;
            return RedirectToAction("NewLocation", "AddLocation");*/

            if (TempData.ContainsKey("ReceiverID") && TempData.ContainsKey("TransmitterID"))
            {
                Guid receiver_id = (Guid)TempData["ReceiverID"];
                Guid transmitter_id = (Guid)TempData["TransmitterID"];
                TempData["rec_id"] = receiver_id;
                TempData["tra_id"] = transmitter_id;
                Console.WriteLine(receiver_id + " " + transmitter_id + "----------------------------------");

                //If the radar name is null we give a default name that specifies its number
                if (String.IsNullOrEmpty(radar.name))
                {
                    String def_name;
                    string stmt = "SELECT COUNT(*) FROM Radar";
                    int count = 0;

                    using (SqlConnection thisConnection = new SqlConnection("Data Source=DATASOURCE"))
                    {
                        using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                        {
                            thisConnection.Open();
                            count = (int)cmdCount.ExecuteScalar();
                        }
                    }
                    count = count + 1;
                    def_name = "Radar " + count;
                    TempData["radar_name"] = def_name;
                }
                else
                {
                    TempData["radar_name"] = radar.name;
                }

                TempData["radar_config"] = radar.configuration;
                TempData["radar_sys"] = radar.system;
                return RedirectToAction("NewLocation", "AddLocation");
            }
            else {
                //GO BACK
                if(!TempData.ContainsKey("ReceiverID"))
                    return RedirectToAction("NewReceiver", "AddReceiver");
                else
                    return RedirectToAction("NewTransmitter", "AddTransmitter");
            }

        }

    }
}
