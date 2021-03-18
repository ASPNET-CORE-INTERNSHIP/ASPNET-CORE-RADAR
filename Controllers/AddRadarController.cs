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
            if (TempData.ContainsKey("ReceiverID") && TempData.ContainsKey("TransmitterID"))
            {
                Guid receiver_id = (Guid)TempData["ReceiverID"];
                Guid transmitter_id = (Guid)TempData["TransmitterID"];
                TempData["rec_id"] = receiver_id;
                TempData["tra_id"] = transmitter_id;
                Console.WriteLine(receiver_id + " " + transmitter_id + "----------------------------------");
            }
            else { 
                //GO BACK
            }


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

    }
}
