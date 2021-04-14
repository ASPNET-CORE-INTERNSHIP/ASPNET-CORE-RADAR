using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class RadarController : Controller
    {
        private readonly NHibernateMapperSession _session;

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

            //handling user-may-occur mistakes
            if (radar.system.StartsWith("Select") | radar.configuration.StartsWith("Select"))
            {
                return View(radar);
                ViewData["Message"] = "Please select System and Configuration";
            }
            else
            {
                Data.Radar = new Radar(def_name, radar.system, radar.configuration);
                Data.Radar.Isnamed = isNamed;
                return RedirectToAction("NewLocation", "Location");
            }
            
        }

        public async void DeleteRadar(Guid id)
        {
            try
            {
                _session.BeginTransaction();
                Guid receiver_id = await _session.GetReceiverID(id);
                Guid transmitter_id = await _session.GetTransmitterID(id);
                await _session.DeleteReceiver(receiver_id);
                await _session.DeleteTransmitter(transmitter_id);
                await _session.DeleteScan(id);
                await _session.Commit();
                ViewData["Message"] = "Radar " + id + " removed From Database";
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
}
