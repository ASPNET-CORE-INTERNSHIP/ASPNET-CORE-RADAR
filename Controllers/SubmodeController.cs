using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class SubmodeController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public SubmodeController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewSubmode()
        {
            return View();
        }


        [HttpPost]
        public IActionResult NewSubmode(Submode sm)
        {
            Submode sbm = new Submode(sm.name, sm.PRI, sm.PW, sm.max_frequency, sm.min_frequency);
            Datas.Submode = sbm;
            return RedirectToAction("NewScan", "Scan");
        }

    }
/*SELECT* FROM Transmitter;
SELECT* FROM Receiver;
SELECT* FROM Antenna;
SELECT* FROM Radar;
SELECT* FROM Location;
SELECT* FROM Mode;
SELECT* FROM Submode;
SELECT* FROM Scan;
DELETE FROM Antenna WHERE number_of_feed < 6000;
DELETE FROM Receiver WHERE rest_time < 6000;
DELETE FROM Transmitter WHERE max_frequency < 6000;
DELETE FROM Location WHERE city = 'DAKAR';
DELETE FROM Radar WHERE name = 'Friendly ';
DELETE FROM Mode WHERE name ='Friendly ';
DELETE FROM Scan WHERE scan_rate<6000;
*/
}