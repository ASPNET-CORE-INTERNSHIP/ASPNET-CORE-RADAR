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

        /*public async Task<RedirectToActionResult> DeleteRadar(Guid id)
        {
            try
            {
                _session.BeginTransaction();
                Guid receiver_id = await _session.GetReceiverID(id);
                Guid transmitter_id = await _session.GetTransmitterID(id);
                await _session.DeleteScan(id);
                await _session.DeleteLocation(id);
                await _session.DeleteReceiver(receiver_id);
                await _session.DeleteTransmitter(transmitter_id);
                //look here and delete location 
                await _session.Commit();
                Data.message = "Radar " + id + " removed From Database";
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
            return RedirectToAction("RadarList", "AdminRadarList");
        }

         public async Task<IActionResult> BeforeEdit(Guid id)
        {
            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            if (Data.edited)
            {
                ViewData["Message"] = "Update completed successfully";
                Data.edited = false;
            }
            if(Data.message != null)
            {
                ViewData["Message"] = Data.message;
                Data.message = null;
            }
            //Get radar's informations and shows it in edit page
            Radar r = await _session.Radars.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
            return View(r);
        }

        public async Task<IActionResult> Edit(Radar newValues)
        {
            try
            {
                await _session.EditRadar(newValues.ID, newValues.name, newValues.system, newValues.configuration);
            }
            catch (Exception e)
            {
                // log exception here
                Data.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                return RedirectToAction("BeforeEdit", "Radar", new { id = newValues.ID });
            }
            finally
            {
                _session.CloseTransaction();
            }
            Data.edited = true;
            return RedirectToAction("BeforeEdit", "Radar", new { id = newValues.ID });
        }*/

    }
}