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

            //if our submode does not have a user friendly name we give it a default name with the code below.
            String def_name = null;
            if (String.IsNullOrEmpty(sm.name))
            {
                def_name = current.LastMode.Mode.name + "'s submode " + current.LastMode.ListOfSubmodes.Count;
            }
            else
                def_name = sm.name;

            Submode sbm = new Submode(key_submode, def_name, current.LastMode.Mode.ID, sm.PRI, sm.PW, sm.max_frequency, sm.min_frequency);
            SubModeInfo sbmINFO = new SubModeInfo(sbm);
            current.LastMode.ListOfSubmodes.Add(sbmINFO);
            //specify current submode as lastSubmode. So it helps us find scanID during creation of antenna scan relationship
            current.LastMode.LastSubmode = sbmINFO;
            return RedirectToAction("NewScan", "Scan");
        }

        public async Task<IActionResult> BeforeEdit(Guid id, String? message)
        {
            //error case
            if (!String.IsNullOrEmpty(message as string))
            {
                SubModeInfo modal = new SubModeInfo();
                modal.Submode = new Submode();
                modal.Scan = new Scan();
                modal.ListOfAntennas = new List<Antenna>();
                ViewData["Message"] = message as string;
                return View(modal);
            }

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

                //Get submode's informations and shows it in edit page
                Submode sbm = await _session.Submode.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                Scan scan = await _session.Scan.Where(b => b.ID.Equals(sbm.scan_id)).FirstOrDefaultAsync();

                for (int j = 0; j < current.LastMode.ListOfSubmodes.Count; j++)
                {
                    if (current.LastMode.ListOfSubmodes[j].Submode.ID.Equals(id))
                    {
                        //specify current submode as last submode. It helps us in scan pages
                        current.LastMode.LastSubmode = current.LastMode.ListOfSubmodes[j];
                    }
                }

                SubModeInfo modal = new SubModeInfo();
                modal.Submode = sbm;
                modal.Scan = scan;
                modal.ListOfAntennas = current.ListOfAntennas;
                return View(modal);
            }
            return RedirectToAction("RadarList", "AdminRadarList");
            
        }

        public async Task<IActionResult> Edit(SubModeInfo newValues)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            if (current != null)
            {
                //update the class
                for (int j = 0; j < current.LastMode.ListOfSubmodes.Count; j++)
                {
                    if (current.LastMode.ListOfSubmodes[j].Submode.ID.Equals(newValues.Submode.ID))
                    {
                        current.LastMode.ListOfSubmodes[j].Submode = newValues.Submode;
                        //specify current submode as last submode
                        current.LastMode.LastSubmode = current.LastMode.ListOfSubmodes[j];
                    }
                }
                //update the db
                try
                {
                    await _session.EditSubmode(newValues.Submode);
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
                return RedirectToAction("BeforeEdit", "Submode", new { id = newValues.Submode.ID });
            }
            String message = "Update cannot be completed, please restart the program to solve this issue. If it continues please report it";
            return RedirectToAction("BeforeEdit", "Submode", new { id = newValues.Submode.ID, message = message });
        }

        public IActionResult ScanEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Scan", new { id = id });
        }

        public async Task<IActionResult> GoBack(Guid id)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            return RedirectToAction("BeforeEdit", "Mode", new { id = current.LastMode.Mode.ID });
        }

    }
}