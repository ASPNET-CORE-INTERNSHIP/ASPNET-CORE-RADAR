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
                    cmd.Parameters.AddWithValue("@receiver_id", Datas.ReceiverID);
                    cmd.Parameters.AddWithValue("@transmitter_id", Datas.TransmitterID);

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
                            cmd.Parameters.AddWithValue("@scan_id", Datas.ScanID);

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
            return View(ascans);
        }

        public IActionResult GoToSubmode()
        {
            return RedirectToAction("NewSubmode", "AddSubmode");
        }

        public IActionResult GoToMode()
        {
            return RedirectToAction("NewMode", "AddMode");
        }

        public IActionResult Done()
        {
            return View("~/Views/done.cshtml");
        }
    }
}
