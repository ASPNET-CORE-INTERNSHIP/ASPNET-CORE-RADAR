/*using ASPNETAOP.Models;
using ASPNETAOP.Session;
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
            Mode m = new Mode(key, mod.name, Data.Radar.ID);
            Data.Mode = m;

            try
            {
                _session.BeginTransaction();
                await _session.SaveMode(m);
                await _session.Commit();
                Data.message = "New Mode added";
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
            return RedirectToAction("NewSubmode", "Submode");
        }

        public async Task<IActionResult> BeforeEdit(Guid id)
        {
            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            if (Data.edited)
            {
                Data.message = "Update completed successfully";
                Data.edited = false;
            }

            //Get mode's informations and shows it in edit page
            Mode mod = await _session.Modes.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();

            List<Submode> SubModeList = new List<Submode>();
            List<Submode> list_temp = await _session.Submode.Where(b => b.mode_id.Equals(id)).ToListAsync();
            foreach (Submode s in list_temp)
            {
                SubModeList.Add(s);
            }

            ModeInfo modal = new ModeInfo();
            modal.Mode = mod;
            modal.ListOfSubmodes = SubModeList;
            return View(modal);
        }

        public async Task<IActionResult> Edit(ModeInfo newValues)
        {
            try
            {
                await _session.EditMode(newValues.Mode);
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
            return RedirectToAction("BeforeEdit", "Mode", new { id = newValues.Mode.ID });
        }

        public async Task<RedirectToActionResult> DeleteModeAsync(Guid id)
        {
            Mode mode = new Mode();
            try
            {
                _session.BeginTransaction();
                mode = await _session.Modes.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                await _session.DeleteMode(id);
                await _session.Commit();
                Data.message = "Mode removed from database";
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

            return RedirectToAction("Edit", "EditRadar", new { id = mode.radar_id });
        }

        public IActionResult SubmodeEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Submode", new { id = id });
        }

        public async Task<RedirectToActionResult> AddSubModeAsync(Guid id)
        {
            Mode m = await _session.Modes.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
            Data.Mode = m;
            Data.ComeFromAdd = true;
            return RedirectToAction("NewSubmode", "Submode");
        }

        public async Task<RedirectToActionResult> DeleteSubmode(Guid id)
        {
            Mode m = new Mode();
            try
            {
                _session.BeginTransaction();
                Submode s = await _session.Submode.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                m = await _session.Modes.Where(b => b.ID.Equals(s.mode_id)).FirstOrDefaultAsync();
                Guid scan_id = await _session.GetScanID(id);
                await _session.DeleteAntennaScanUsingScanID(scan_id);
                await _session.DeleteScan(scan_id);
                await _session.DeleteSubMode(id);
                await _session.Commit();
                Data.message = "Submode " + id + "removed from database";
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
        }

        public async Task<IActionResult> GoBack(Guid id)
        {
            Radar r = new Radar();
            Mode m = new Mode();
            try
            {
                m = await _session.Modes.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                r = await _session.Radars.Where(b => b.ID.Equals(m.radar_id)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                // log exception here
                Data.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                return RedirectToAction("BeforeEdit", "Mode", new { id = m.radar_id });
            }
            finally
            {
                _session.CloseTransaction();
            }
            return RedirectToAction("Edit", "EditRadar", new { id = r.ID });
        }
    }
}*/
