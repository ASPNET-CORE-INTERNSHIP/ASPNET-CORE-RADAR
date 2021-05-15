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
using static ASPNETAOP.Models.Antenna;

namespace ASPNETAOP.Controllers
{
    public class AntennaScanController : Controller
    {
        private readonly NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

        public AntennaScanController(NHibernateMapperSession session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Preliminary()
        {
            //New variable consisting of a list of antennas
            //so the user can select antennas which empolys current scan type
            AntennaList alist = new AntennaList();

            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            //control if current did not came
            if (current != null)
            {
                alist.antennas = current.ListOfAntennas;

                if (current.ComeFromAdd)
                {
                    alist.ComeFromAdd = true;
                }

                ViewData["message"] = current.message;
                //returning the model for the cshmtl page to access it
                return View(alist);
            }

            ViewData["message"] = "Error occured please restart the program";
            //returning the model for the cshmtl page to access it
            return View(alist);
        }

        public async Task<IActionResult> NewAntennaScanParamAsync(Antenna.AntennaList ascans)
        {
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            //control if current did not came
            if (current == null)
            {
                ViewData["message"] = "Error occured please restart the program";
                return View(ascans);
            }

            //refresh the list to specify selected antennas
            current.ListOfAntennas = ascans.antennas;
            try
            {
                _session.BeginTransaction();

                for (int i = 0; i < ascans.antennas.Count; i++)
                {
                    Antenna antenna = ascans.antennas[i];

                    AntennaScan ascan = new AntennaScan(antenna.ID, current.LastMode.LastSubmode.Scan.ID);
                    if (antenna.IsChecked)
                    {
                        Guid temp = Guid.Empty;
                        temp = await _session.SelectAntennaScan(current.ListOfAntennas[i].ID, current.LastMode.LastSubmode.Scan.ID);
                        if (temp.Equals(Guid.Empty))
                        {
                            await _session.SaveAntennaScan(ascan);
                        }
                    }
                    else
                    {
                        Guid temp = Guid.Empty;
                        temp = await _session.SelectAntennaScan(current.ListOfAntennas[i].ID, current.LastMode.LastSubmode.Scan.ID);
                        if (!temp.Equals(Guid.Empty))
                        {
                            await _session.DeleteAntennaScan(ascan);
                        }
                    }
                }
                await _session.Commit();
                if (current != null)
                    current.message = "New Relationship between antennas and scan added";
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
            return RedirectToAction("Preliminary", "AntennaScan");
        }

        public IActionResult GoToSubmode()
        {
            return RedirectToAction("NewSubmode", "Submode");
        }

        public IActionResult GoToMode()
        {
            return RedirectToAction("NewMode", "Mode");
        }

        public IActionResult Done()
        {
            Program.data.Remove(sessionID);
            return View("~/Views/Shared/done.cshtml");
        }

        public IActionResult GoBack()
        {
            //After we have done with the ComeFromEdit, we give false (default) to this value
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            if (current != null)
            {
                current.ComeFromAdd = false;
                return RedirectToAction("BeforeEdit", "Mode", new { id = current.LastMode.Mode.ID });
            }
            else
            {
                return RedirectToAction("RadarList", "AdminRadarList");
            }
        }
        
    }
}