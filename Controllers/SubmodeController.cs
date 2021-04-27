using ASPNETAOP.Models;
using ASPNETAOP.Session;
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
        private readonly NHibernateMapperSession _session;

        public SubmodeController(NHibernateMapperSession session)
        {
            _session = session;
        }

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
        public async Task<IActionResult> NewSubmodeAsync(Submode sm)
        {
            if (Data.message != null)
            {
                Data.message = null;
            }

            for (int i = 0; i < Data.ListOfAntennas.Count; i++)
            {
                Data.ListOfAntennas[i].IsChecked = false;
            }
            Submode sbm = new Submode(sm.name, sm.PRI, sm.PW, sm.max_frequency, sm.min_frequency);
            Data.Submode = sbm;
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