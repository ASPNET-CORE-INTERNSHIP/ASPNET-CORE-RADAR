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
        public IActionResult NewSubmode(Submode sm)
        {
            for (int i = 0; i < Data.ListOfAntennas.Count; i++)
            {
                Data.ListOfAntennas[i].IsChecked = false;
            }
            Submode sbm = new Submode(sm.name, sm.PRI, sm.PW, sm.max_frequency, sm.min_frequency);
            Data.Submode = sbm;
            return RedirectToAction("NewScan", "Scan");
        }

        public async void DeleteSubModeAsync(Guid id)
        {
            try
            {
                _session.BeginTransaction();
                Guid scan_id = await _session.GetScanID(id);
                await _session.DeleteAntennaScanUsingScanID(scan_id);
                await _session.DeleteSubMode(id);
                await _session.Commit();
                ViewData["Message"] = "SubMode removed from database";
            }
            catch (Exception e)
            {
                // log exception here
                ViewData["Message"] = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
                _session.CloseTransaction();
            }
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