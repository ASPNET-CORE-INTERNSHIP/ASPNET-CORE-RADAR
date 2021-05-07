using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ScanController : Controller
    {
        private readonly NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

        public ScanController(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewScan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewScanAsync(Scan scan)
        {
            Guid key = Guid.NewGuid();
            Scan s = new Scan(key, scan.name, scan.type, scan.main_aspect, scan.scan_angle, scan.scan_rate, scan.hits_per_scan);
            
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            int i = current.LastMode.ListOfSubmodes.Count;
            i--;
            current.LastMode.ListOfSubmodes[i].Scan = s;
            //remember we did not give a scan id to this submode, so we did not add it to db
            current.LastMode.ListOfSubmodes[i].Submode.scan_id = key;

            Submode sbm = current.LastMode.ListOfSubmodes[i].Submode;

            try
            {
                _session.BeginTransaction();
                await _session.SaveScan(s);
                await _session.SaveSubMode(sbm);
                await _session.Commit();
                current.message = "Both records (Submode and Scan) added to db";
            }
            catch (Exception e)
            {
                // log exception here
                current.message = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
                _session.CloseTransaction();
            }

            return RedirectToAction("Preliminary", "AntennaScan");
        }
        /*
        public async Task<IActionResult> BeforeEdit(Guid id)
        {
            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            if (Data.edited)
            {
                Data.message = "Update completed successfully";
                Data.edited = false;
            }

            //Get scan's informations and shows it in edit page
            Scan scan = await _session.Scan.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
            Submode sbm = await _session.Submode.Where(b => b.scan_id.Equals(id)).FirstOrDefaultAsync();
            SubModeInfo info = new SubModeInfo();
            info.Scan = scan;
            info.Submode = sbm;
            info.ListOfAntennas = Data.ListOfAntennas;
            return View(info);
        }

        public async Task<IActionResult> Edit(SubModeInfo newValues)
        {
            try
            {
                await _session.EditScan(newValues.Scan);
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
            return RedirectToAction("BeforeEdit", "Scan", new { id = newValues.Scan.ID });
        }

        public async Task<RedirectToActionResult> EditRelationship(Guid id)
        {
            Scan s = await _session.Scan.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
            Data.Scan = s;
            Data.ComeFromAdd = true;
            return RedirectToAction("NewAntennaScan", "AntennaScan");
        }

        public async Task<IActionResult> GoBack(Guid id)
        {
            Radar r = new Radar();
            Mode m = new Mode();
            Submode sm = new Submode();
            try
            {
                sm = await _session.Submode.Where(b => b.scan_id.Equals(id)).FirstOrDefaultAsync();
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
        }
        */
    }
}
