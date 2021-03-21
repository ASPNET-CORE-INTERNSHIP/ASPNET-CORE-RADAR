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
    public class AddSubmodeController : Controller
    {
        //We need configuration for calling db.
        private IConfiguration _configuration;
        public AddSubmodeController(IConfiguration Configuration) { _configuration = Configuration; }

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
        public IActionResult NewSubmode(AddSubmode sm)
        {
            return RedirectToAction("NewScan", "AddScan", new { sm = new AddSubmode (sm.name, sm.PW, sm.PRI, sm.min_frequency, sm.max_frequency) });
            //bu kısımda hata veriyor sebebini anlamadım
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
DELETE FROM Antenna WHERE number_of_feed < 600;
DELETE FROM Receiver WHERE rest_time < 600;
DELETE FROM Transmitter WHERE max_frequency < 600;
DELETE FROM Location WHERE city = 'DAKAR';
DELETE FROM Radar WHERE name = 'Friendly ';
DELETE FROM Mode WHERE name ='Friendly ';
DELETE FROM Scan WHERE scan_rate<600;
*/
}