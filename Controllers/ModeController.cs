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
    public class ModeController : Controller
    {
        private readonly NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

        public ModeController(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewMode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewModeAsync(Mode mod)
        {
            Guid key = Guid.NewGuid();
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            Mode m = new Mode(key, mod.name, current.Radar.ID);
            ModeInfo mi = new ModeInfo(m);
            mi.ListOfSubmodes = new List<SubModeInfo>();
            current.LastMode = mi;

            try
            {
                _session.BeginTransaction();
                await _session.SaveMode(m);
                await _session.Commit();
                current.message = "New Mode added";
            }
            catch (Exception e)
            {
                // log exception here
                current.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                return View(mod);
            }
            finally
            {
                _session.CloseTransaction();
            }
            return RedirectToAction("NewSubmode", "Submode");
        }

        public async Task<IActionResult> BeforeEdit(Guid id)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            if (current != null)
            {
                //Because we use the same view before and after edit process we should handle the view messages with the following conditions
                if (current.edited)
                {
                    ViewData["Message"] = "Update completed successfully";
                    current.edited = false;
                }
                if (current.message != null)
                {
                    ViewData["Message"] = current.message;
                    current.message = null;
                }

                //Get mode's informations and shows it in edit page
                Mode mod = await _session.Modes.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                List<SubModeInfo> INFOLIST = new List<SubModeInfo>();
                List <Submode> sbm_list = await _session.Submode.Where(b => b.mode_id.Equals(id)).ToListAsync();
                foreach (Submode sbm in sbm_list)
                {
                    Scan s = await _session.Scan.Where(b => b.ID.Equals(sbm.scan_id)).FirstOrDefaultAsync();
                    SubModeInfo subModeInfo = new SubModeInfo(sbm, s);
                    INFOLIST.Add(subModeInfo);
                }
                ModeInfo INFO = new ModeInfo();
                INFO.Mode = mod;
                INFO.ListOfSubmodes = INFOLIST;
                //it is necessary when we are going to submode pages
                current.LastMode = INFO;
                return View(INFO);
            }
            return RedirectToAction("RadarList", "AdminRadarList");
        }
        public async Task<IActionResult> Edit(ModeInfo newValues)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            if (current!=null)
            {
                //update the class
                for(int i = 0; i < current.ListOfModes.Count; i++)
                {
                    if (current.ListOfModes[i].Mode.ID.Equals(newValues.Mode.ID))
                        current.ListOfModes[i].Mode = newValues.Mode;
                }
                //update the db
                try
                {
                    await _session.EditMode(newValues.Mode);
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
                current.edited = true;
            }
            return RedirectToAction("BeforeEdit", "Mode", new { id = newValues.Mode.ID });
        }

        public async Task<RedirectToActionResult> DeleteModeAsync(Guid id)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            Mode mode = new Mode();
            try
            {
                _session.BeginTransaction();
                mode = await _session.Modes.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                List<Submode> sbmList = await _session.Submode.Where(b => b.mode_id.Equals(id)).ToListAsync();
                foreach(Submode s in sbmList)
                {
                    Scan scan = await _session.Scan.Where(b => b.ID.Equals(s.scan_id)).FirstOrDefaultAsync();
                    await _session.DeleteScanbyID(scan.ID);
                }
                await _session.DeleteMode(id);
                await _session.Commit();
                if(current!=null)
                    current.message = "Mode removed from database";
            }
            catch (Exception e)
            {
                // log exception here
                if (current != null)
                    current.message = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
                _session.CloseTransaction();
            }

            if (current!=null)
            {
                for (int i = 0; i < current.ListOfModes.Count; i++)
                {
                    if (current.ListOfModes[i].Mode.ID.Equals(id))
                    {
                        current.ListOfModes.RemoveAt(i);
                    }
                }
            }
            return RedirectToAction("Edit", "EditRadar", new { id = mode.radar_id });
        }

        public IActionResult SubmodeEdit(Guid id)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            return RedirectToAction("BeforeEdit", "Submode", new { id = id });
        }

        public async Task<RedirectToActionResult> AddSubModeAsync()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            if (current!=null)
            {
                current.ComeFromAdd = true;
                return RedirectToAction("NewSubmode", "Submode");
            }
            return RedirectToAction("RadarList", "AdminRadarList");
        }

        public async Task<RedirectToActionResult> DeleteSubmode(Guid id)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            if (current != null)
            {
                Mode m = current.LastMode.Mode;
                try
                {
                    _session.BeginTransaction();
                    Guid scan_id = await _session.GetScanID(id);
                    await _session.DeleteAntennaScanUsingScanID(scan_id);
                    await _session.DeleteScan(scan_id);
                    await _session.DeleteSubMode(id);
                    await _session.Commit();
                    current.message = "Submode " + id + "removed from database";
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

                return RedirectToAction("BeforeEdit", "Mode", new { id = m.ID });
            }
            return RedirectToAction("RadarList", "AdminRadarList");
        }

        public async Task<IActionResult> GoBack()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            //we may came to there from edit page
            current.ComeFromAdd = false;
            Radar r = current.Radar;
            return RedirectToAction("Edit", "EditRadar", new { id = r.ID });
        }
        
    }
}
