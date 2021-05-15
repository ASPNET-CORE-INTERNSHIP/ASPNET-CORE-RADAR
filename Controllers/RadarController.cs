using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NHibernate.Linq;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class RadarController : Controller
    {
        private readonly NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

        public RadarController(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewRadar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewRadarAsync(Radar radar)
        {
            //handling user-may-occur mistakes
            if (radar.system.StartsWith("Select") | radar.configuration.StartsWith("Select"))
            {
                ViewData["Message"] = "Please select System and Configuration";
                return View(radar);
            }

            //If the radar name is null we give a default name that specifies its number and change when the location added
            String def_name;
            bool isNamed = false;
            if (String.IsNullOrEmpty(radar.name))
            {
                int count = 0;

                try
                {
                    count = await _session.GetRadarNumber();
                }
                catch (Exception e)
                {
                    // log exception here
                    ViewData["Message"] = e.Message.ToString() + " Error";
                    await _session.Rollback();
                    return View(radar);
                }
                finally
                {
                    _session.CloseTransaction();
                }
                count = count + 1;
                def_name = "Radar " + count;
                isNamed = true;
            }
            else
            {
                def_name = radar.name;
            }

            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data d = new Data();
            Program.data.TryGetValue(sessionID, out d);

            d.Radar = new Radar(def_name, radar.system, radar.configuration);
            d.Radar.Isnamed = isNamed;
            return RedirectToAction("NewLocation", "Location"); 
        }

        public async Task<RedirectToActionResult> DeleteRadar()
        {
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            try
            {
                _session.BeginTransaction();
                await _session.DeleteScan(current.Radar.ID);
                await _session.DeleteLocation(current.Location.ID);
                await _session.DeleteReceiver(current.Receiver.ID);
                await _session.DeleteTransmitter(current.Transmitter.ID);
                await _session.DeleteLocation(current.Location.ID);
                await _session.Commit();
                current.message = "Radar " + current.Radar.ID + " removed From Database";
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
            return RedirectToAction("RadarList", "AdminRadarList");
        }

         public async Task<IActionResult> BeforeEdit()
        {
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data d = new Data();
            Program.data.TryGetValue(sessionID, out d);

            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            if (d.edited)
            {
                ViewData["Message"] = "Update completed successfully";
                d.edited = false;
            }
            if(d.message != null)
            {
                ViewData["Message"] = d.message;
                d.message = null;
            }
           
            //Get radar's informations and shows it in edit page
            //Radar r = await _session.Radars.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
            Radar r = d.Radar;
            return View(r);
        }

        public async Task<IActionResult> Edit(Radar newValues)
        {
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data d = new Data();
            Program.data.TryGetValue(sessionID, out d);
            d.Radar = newValues;
            try
            {
                await _session.EditRadar(newValues.ID, newValues.name, newValues.system, newValues.configuration);
            }
            catch (Exception e)
            {
                // log exception here
                d.message = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
                _session.CloseTransaction();
            }
            d.edited = true;
            return RedirectToAction("BeforeEdit", "Radar");
        }

    }
}