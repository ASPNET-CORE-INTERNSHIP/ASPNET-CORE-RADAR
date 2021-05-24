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
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            Guid key = Guid.NewGuid();
            //if our scan does not have a user friendly name we give it a default name with the code below.
            String def_name = null;
            if (String.IsNullOrEmpty(scan.name))
            {
                def_name = current.LastMode.LastSubmode.Submode.name + "'s scan";
            }
            else
                def_name = scan.name;
            Scan s = new Scan(key, def_name, scan.type, scan.main_aspect, scan.scan_angle, scan.scan_rate, scan.hits_per_scan);


            current.LastMode.LastSubmode.Scan = s;
            //remember we did not give a scan id to this submode, so we did not add it to db
            current.LastMode.LastSubmode.Submode.scan_id = key;
            Submode sbm = current.LastMode.LastSubmode.Submode;

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
        
        //below functions are for edit pages
        public async Task<IActionResult> BeforeEdit(Guid id)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            if (current.Receiver != null)
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

                //Get scan's informations and shows it in edit page
                Scan scan = await _session.Scan.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                Submode sbm = await _session.Submode.Where(b => b.scan_id.Equals(id)).FirstOrDefaultAsync();
                SubModeInfo info = new SubModeInfo();
                info.Scan = scan;
                info.Submode = sbm;
                info.ListOfAntennas = new List<Antenna>();
                //because we do not have an attribute that specifies if the antenna is checked
                //(because an antenna might be used from a lot of scans)
                //we should find if the antenna is working with this scan using AntennaScan tables
                for (int i = 0; i < current.ListOfAntennas.Count; i++)
                {
                    Guid temp = Guid.Empty;
                    temp = await _session.SelectAntennaScan(current.ListOfAntennas[i].ID, id);
                    if (!temp.Equals(Guid.Empty))
                    {
                        info.ListOfAntennas.Add(current.ListOfAntennas[i]);
                    }
                }
                current.LastMode.LastSubmode = info;
                return View(info);
            }
            //else
            String message = "Update cannot be completed, please restart the program to solve this issue. If it continues please report it";
            return RedirectToAction("BeforeEdit", "Submode", new { id = Guid.Empty, message = message });
        }

        public async Task<IActionResult> Edit(SubModeInfo newValues)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            //not necessary to edit current data bc in submode page we will retrieve all scan informations from db again
            try
            {
                await _session.EditScan(newValues.Scan);
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
            if (current != null)
                current.edited = true;
            return RedirectToAction("BeforeEdit", "Scan", new { id = newValues.Scan.ID });
        }

        public async Task<RedirectToActionResult> EditRelationship(Guid id)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            if (current != null)
            {
                current.ComeFromAdd = true;

                //because we do not have an attribute that specifies if the antenna is checked
                //(because an antenna might be used from a lot of scans)
                //we should find if the antenna is working with this scan using AntennaScan tables
                for (int i=0; i<current.ListOfAntennas.Count; i++)
                {
                    Guid temp = Guid.Empty;
                    temp = await _session.SelectAntennaScan(current.ListOfAntennas[i].ID, id);
                    if (!temp.Equals(Guid.Empty))
                    {
                        current.ListOfAntennas[i].IsChecked = true;
                    }
                    else
                    {
                        current.ListOfAntennas[i].IsChecked = false;
                    }
                }
                return RedirectToAction("Preliminary", "AntennaScan");
            }
            //else
            String message = "Update cannot be completed, please restart the program to solve this issue. If it continues please report it";
            return RedirectToAction("BeforeEdit", "Submode", new { id = Guid.Empty, message = message });
        }

        public async Task<IActionResult> GoBack()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            if (current.Receiver != null)
            {
                return RedirectToAction("BeforeEdit", "Mode", new { id = current.LastMode.Mode.ID });
            }
            String message = "Update cannot be completed, please restart the program to solve this issue. If it continues please report it";
            return RedirectToAction("BeforeEdit", "Submode", new { id = Guid.Empty, message = message });
        }

        //below functions are for display pages
        public async Task<IActionResult> Show(Guid id)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            if (current.Receiver != null)
            {
                //Get scan's informations and shows it in edit page
                Scan scan = await _session.Scan.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                Submode sbm = await _session.Submode.Where(b => b.scan_id.Equals(id)).FirstOrDefaultAsync();
                SubModeInfo info = new SubModeInfo();
                info.Scan = scan;
                info.Submode = sbm;
                info.ListOfAntennas = new List<Antenna>();
                //because we do not have an attribute that specifies if the antenna is checked
                //(because an antenna might be used from a lot of scans)
                //we should find if the antenna is working with this scan using AntennaScan tables
                for (int i = 0; i < current.ListOfAntennas.Count; i++)
                {
                    Guid temp = Guid.Empty;
                    temp = await _session.SelectAntennaScan(current.ListOfAntennas[i].ID, id);
                    if (!temp.Equals(Guid.Empty))
                    {
                        info.ListOfAntennas.Add(current.ListOfAntennas[i]);
                    }
                }
                current.LastMode.LastSubmode = info;
                return View(info);
            }
            //else
            String message = "The process cannot be completed, please restart the program to solve this issue. If it continues please report it";
            return RedirectToAction("RadarList", "UserRadarList", new { message = message });
        }

        public async Task<IActionResult> Back()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            if (current.Receiver != null)
            {
                return RedirectToAction("Show", "Submode", new { id = current.LastMode.LastSubmode.Submode.ID });
            }
            String message = "The process cannot be completed, please restart the program to solve this issue. If it continues please report it";
            return RedirectToAction("Show", "Submode", new { id = Guid.Empty, message = message });
        }

    }
}
