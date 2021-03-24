using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
            if (TempData["rec_id"] == null && TempData["tra_id"] == null) return RedirectToAction("Login", "UserLogin");

            Guid receiver_id = (Guid)TempData.Peek("rec_id");
            Guid transmitter_id = (Guid)TempData.Peek("tra_id");

            //New model consisting of a list
            //so the user can select antennas which empolys current scan type
            var model = new List<AddAntenna>();
            
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
                           
                            //Adding the antenna information to the newly created model
                            model.Add(antenna);
                        }
                        con.Close();
                    }
                    catch (SqlException e)
                    {
                        ViewData["Message"] = e.Message.ToString() + " Error";
                    }

                }
            }
            
            //returning the model for the cshmtl page to access it
            return View(model);
        }

        public IActionResult NewAntennaScanParam(AddAntennaScan ascans)
        {
            if (TempData.ContainsKey("scan_id"))
            {

                Guid id = (Guid)TempData.Peek("scan_id");

                using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"INSERT INTO AntennaScan(antenna_id, scan_id) 
                            VALUES(@antenna_id, @scan_id)";
                        cmd.Parameters.AddWithValue("@antenna_id", ascans.antenna_id);
                        cmd.Parameters.AddWithValue("@scan_id", id);

                        try
                        {
                            con.Open();
                            int i = cmd.ExecuteNonQuery();
                            if (i != 0)
                                ViewData["Message"] = "New Relationship between antenna and scans added";
                            con.Close();
                            if (ModelState.IsValid)
                            {
                                return RedirectToAction("NewSubmode", "AddSubmode");
                            }
                        }
                        catch (SqlException e)
                        {
                            ViewData["Message"] = e.Message.ToString() + " Error";
                        }

                    }
                }
            }
            else
            {
                Console.WriteLine("ERROR123");
            }
            return View(ascans);
        }
    }
}
