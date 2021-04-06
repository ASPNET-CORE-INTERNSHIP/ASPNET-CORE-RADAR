using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class RadarController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public RadarController(IConfiguration Configuration) { _configuration = Configuration; }

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
        public IActionResult NewRadar(Radar radar)
        {
            //If the radar name is null we give a default name that specifies its number
            String def_name;
            if (String.IsNullOrEmpty(radar.name))
            {
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
            }
            else
            {
                def_name = radar.name;
            }
            Datas.Radar = new Radar(def_name, radar.system, radar.configuration);
            return RedirectToAction("NewLocation", "Location");
        }

    }
}
