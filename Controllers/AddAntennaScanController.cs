using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static ASPNETAOP.Models.AddAntenna;

namespace ASPNETAOP.Controllers
{
    public class AddAntennaScanController : Controller
    {
        private IConfiguration _configuration;
        public AddAntennaScanController(IConfiguration Configuration) { _configuration = Configuration; }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewAntennaScan()
        {
            if (TempData["rec_id"] == null && TempData["tra_id"] == null && TempData.ContainsKey("mode_id") && TempData.ContainsKey("radar_id")) return RedirectToAction("NewReceiver", "AddReceiver");

            Guid radar_id = (Guid)TempData.Peek("radar_id");
            //after adding submodes and scans we might want to add more modes to this radar so keep it in tempdata
            TempData["radar_id"] = radar_id;

            //after adding scans we might want to add more submodes to this radar so keep it in tempdata
            Guid mode_id = (Guid)TempData.Peek("mode_id");
            TempData["mode_id"] = mode_id;

            if (!TempData.ContainsKey("scan_id"))
            {
                return RedirectToAction("NewSubmode", "AddSubmode");
            }
            else
                TempData["scan_id"] = (Guid)TempData.Peek("scan_id");

            Guid receiver_id = (Guid)TempData.Peek("rec_id");
            Guid transmitter_id = (Guid)TempData.Peek("tra_id");
            TempData["rec_id"] = receiver_id;
            TempData["tra_id"] = transmitter_id;

            //New variable consisting of a list of antennas
            //so the user can select antennas which empolys current scan type
            List<AddAntenna> antennas = new List<AddAntenna>();
            
            using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT * FROM Antenna WHERE receiver_id = @receiver_id or transmitter_id = @transmitter_id";
                    cmd.Parameters.AddWithValue("@receiver_id", receiver_id);
                    cmd.Parameters.AddWithValue("@transmitter_id", transmitter_id);

                    try
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            AddAntenna antenna = new AddAntenna();
                            antenna.ID = reader.GetGuid("ID");
                            antenna.name = reader.GetString("name");
                            antenna.type = reader.GetString("type");
                            antenna.horizontal_beamwidth = (float)reader.GetDouble("horizontal_beamwidth");
                            antenna.vertical_beamwidth = (float)reader.GetDouble("vertical_beamwidth");
                            antenna.polarization = reader.GetString("polarization");
                            antenna.number_of_feed = reader.GetInt32("number_of_feed");
                            antenna.horizontal_dimension = (float)reader.GetDouble("horizontal_dimension");
                            antenna.vertical_dimension = (float)reader.GetDouble("vertical_dimension");
                            antenna.duty = reader.GetString("duty");
                            antenna.location = reader.GetString("location");
                            antenna.IsChecked = false;
                            //Adding the antenna information to the newly created model
                            antennas.Add(antenna);
                        }
                        con.Close();
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }

                }
            }
            AntennaList alist = new AntennaList();
            alist.antennas = antennas;
            //returning the model for the cshmtl page to access it
            return View(alist);
        }

        public IActionResult NewAntennaScanParam(AddAntenna.AntennaList ascans)
        {
            if (TempData.ContainsKey("mode_id") && TempData.ContainsKey("radar_id"))
            {
                Guid radar_id = (Guid)TempData.Peek("radar_id");

                //after adding submodes and scans we might want to add more modes to this radar so keep it in tempdata
                TempData["radar_id"] = radar_id;

                //after adding scans we might want to add more submodes to this radar so keep it in tempdata
                Guid mode_id = (Guid)TempData.Peek("mode_id");
                TempData["mode_id"] = mode_id;
            }

            if (TempData.ContainsKey("scan_id"))
            {
                Guid id = (Guid)TempData.Peek("scan_id");

                foreach (var antenna in ascans.antennas)
                {
                    if (antenna.IsChecked)
                    {
                        using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.Connection = con;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = @"INSERT INTO AntennaScan(antenna_id, scan_id) 
                            VALUES(@antenna_id, @scan_id)";
                                cmd.Parameters.AddWithValue("@antenna_id", antenna.ID);
                                cmd.Parameters.AddWithValue("@scan_id", id);

                                try
                                {
                                    con.Open();
                                    int i = cmd.ExecuteNonQuery();
                                    if (i != 0)
                                        ViewData["Message"] = "New Relationship between antenna and scans added";
                                    con.Close();
                                }
                                catch (SqlException e)
                                {
                                    ViewData["Message"] = e.Message.ToString() + " Error";
                                }

                            }
                        }
                    }

                    antenna.IsChecked = false;
                }
            }
            else
            {
                Console.WriteLine("ERROR123");
            }
            return View(ascans);
        }

        public IActionResult GoToSubmode(Guid radar_id, Guid mode_id)
        {
            TempData["radar_id"] = radar_id;
            TempData["mode_id"] = mode_id;
            return RedirectToAction("NewSubmode", "AddSubmode");
        }

        public IActionResult GoToMode(Guid radar_id)
        {
            TempData["radar_id"] = radar_id;
            TempData.Remove("mode_id");
            return RedirectToAction("NewMode", "AddMode");
        }

        public IActionResult Done(Guid radar_id)
        {
            TempData["radar_id"] = radar_id;
            TempData.Remove("mode_id");
            return View("~/Views/done.cshtml");
        }
    }
}
