using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using NHibernate.Linq;
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
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            for (int i = 0; i < current.ListOfAntennas.Count; i++)
            {
                current.ListOfAntennas[i].IsChecked = false;
            }
            Guid key_submode = Guid.NewGuid();
            Submode sbm = new Submode(key_submode ,sm.name, current.LastMode.Mode.ID, sm.PRI, sm.PW, sm.max_frequency, sm.min_frequency);
            SubModeInfo sbmINFO = new SubModeInfo(sbm);
            current.LastMode.ListOfSubmodes.Add(sbmINFO);
            return RedirectToAction("NewScan", "Scan");
        }

        /*public async Task<IActionResult> BeforeEdit(Guid id)
        {
            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            if (Data.edited)
            {
                Data.message = "Update completed successfully";
                Data.edited = false;
            }

            //Get mode's informations and shows it in edit page
            Submode sbm = await _session.Submode.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
            Scan scan = await _session.Scan.Where(b => b.ID.Equals(sbm.scan_id)).FirstOrDefaultAsync();

            List<Submode> SubModeList = new List<Submode>();
            List<Submode> list_temp = await _session.Submode.Where(b => b.mode_id.Equals(id)).ToListAsync();
            foreach (Submode s in list_temp)
            {
                SubModeList.Add(s);
            }

            SubModeInfo modal = new SubModeInfo();
            modal.Submode = sbm;
            modal.Scan = scan;
            modal.ListOfAntennas = Data.ListOfAntennas;
            return View(modal);
        }

        public async Task<IActionResult> Edit(SubModeInfo newValues)
        {
            try
            {
                await _session.EditSubmode(newValues.Submode);
            }
            catch (Exception e)
            {
                // log exception here
                Data.message = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
                _session.CloseTransaction();
            }
            Data.edited = true;
            return RedirectToAction("BeforeEdit", "Submode", new { id = newValues.Submode.ID });
        }

        public IActionResult ScanEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Scan", new { id = id });
        }

        public async Task<IActionResult> GoBack(Guid id)
        {
            Radar r = new Radar();
            Mode m = new Mode();
            Submode sm = new Submode();
            try
            {
                sm = await _session.Submode.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                m = await _session.Modes.Where(b => b.ID.Equals(sm.mode_id)).FirstOrDefaultAsync();
                r = await _session.Radars.Where(b => b.ID.Equals(m.radar_id)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                // log exception here
                Data.message = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
                _session.CloseTransaction();
            }
            return RedirectToAction("BeforeEdit", "Mode", new { id = m.ID });
            //return RedirectToAction("Edit", "EditRadar", new { id = r.ID });
        }*/

    }
    /*
SELECT* FROM Transmitter;
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