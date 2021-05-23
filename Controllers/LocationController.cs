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
    public class LocationController : Controller
    {
        private readonly NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

        public LocationController(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewLocation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewLocationAsync(Location loc)
        {
            //handling user-may-occur mistakes
            if (loc.city == null && loc.country == null && loc.geographic_latitude == null && loc.geographic_longitude == null && loc.airborne == null)
            {
                return View(loc);
                ViewData["Message"] = "Please fill at least airborne area or Country, City, Geographic Latitude and Geographic Longitude areas";
            }

            //If the location name is null we give a default name that specifies its country, city and area number  
            String def_name = null;
            if (String.IsNullOrEmpty(loc.name))
            {
                if (loc.city == null && loc.country == null && loc.geographic_latitude == null && loc.geographic_longitude == null && loc.airborne != null)
                {
                    int count = 0;

                    try
                    {
                        count = await _session.GetLocationName(loc.airborne);
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
                    def_name = loc.airborne + " " + count;
                }
                else if (loc.city != null && loc.country != null && loc.geographic_latitude != null && loc.geographic_longitude != null)
                {
                    int count = 0;

                    try
                    {
                        count = await _session.GetLocationName(loc.country, loc.city);
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
                    def_name = loc.country + " " + loc.city + " " + count;
                }
                else
                {
                    ViewData["Message"] = "Please fill at least airborne area or Country, City, Geographic Latitude and Geographic Longitude areas";
                    return View(loc);
                }

            }
            else
            {
                def_name = loc.name;
            }
            //end of naming////////////////////////////////////////////////////////////////////////////////

            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            //control if current did not came
            if (current == null)
            {
                ViewData["Message"] = "Error occured please restart the program";
                return View(loc);
            }

            //defining Radar's and Location's key here
            Guid key_location = Guid.NewGuid();
            Guid key = Guid.NewGuid();
            current.Radar.ID = key;

            //rename Radar, Transmitter and Antennas again
            String radar_name = current.Radar.name;
            if (current.Radar.Isnamed == true)
            {
                radar_name = "Radar in " + def_name;
                current.Radar.name = radar_name;
            }

            if (current.Transmitter.Isnamed == true)
            {
                Guid id = current.Transmitter.ID;
                String newName = radar_name + "'s Transmitter";
                current.Transmitter.name = newName;
                try
                {
                    _session.BeginTransaction();
                    _session.RenameTransmitter(id, newName);
                    await _session.Commit();
                }
                catch (Exception e)
                {
                    // log exception here
                    ViewData["Message"] = e.Message.ToString() + " Error";
                    await _session.Rollback();
                    return View(loc);
                }
                finally
                {
                    _session.CloseTransaction();
                }
            }

            if (current.Receiver.Isnamed == true)
            {
                Guid id = current.Receiver.ID;
                String newName = radar_name + "'s Receiver";
                current.Receiver.name = newName;
                try
                {
                    _session.BeginTransaction();
                    _session.RenameReceiver(id, newName);
                    await _session.Commit();
                }
                catch (Exception e)
                {
                    // log exception here
                    ViewData["Message"] = e.Message.ToString() + " Error";
                    await _session.Rollback();
                    return View(loc);
                }
                finally
                {
                    _session.CloseTransaction();
                }
            }

            Location location_temp = new Location(key_location, def_name, loc.country, loc.city, loc.geographic_latitude, loc.geographic_longitude, loc.airborne);
            Radar radar_temp = new Radar(key, radar_name, current.Radar.system, current.Radar.configuration, current.Transmitter.ID, current.Receiver.ID, key_location);
            current.Radar = radar_temp;
            //we do not need location in current because we will not use its informations 

            try
            {
                _session.BeginTransaction();
                await _session.SaveLocation(location_temp);
                await _session.SaveRadar(radar_temp);
                await _session.Commit();
                current.message = "Both records (Location and Radar) saved to the db";
                Program.data.Remove(sessionID);
                Program.data.Add(sessionID, current);
                return RedirectToAction("NewMode", "Mode");
            }
            catch (Exception e)
            {
                // log exception here
                ViewData["Message"] = e.Message.ToString() + " Error";
                await _session.Rollback();
                return View(loc);
            }
            finally
            {
                _session.CloseTransaction();
            }
            
        }

        //Below functions are for edit pages
        public async Task<IActionResult> BeforeEdit()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            //control if current did not came
            if (current.Receiver == null)
            {
                ViewData["Message"] = "Error occured please restart the program";
                Location l = new Location();
                return View(l);
            }

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

            //Get receiver's informations and shows it in edit page
            //Transmitter t = await _session.Transmitters.Where(b => b.ID.Equals(current.Transmitter.ID)).FirstOrDefaultAsync();
            return View(current.Location);
        }

        public async Task<IActionResult> Edit(Location newValues)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            current.Location = newValues;
            try
            {
                await _session.EditLocation(newValues);
            }
            catch (Exception e)
            {
                // log exception here
                ViewData["message"] = e.Message.ToString() + " Error";
                await _session.Rollback();
                return RedirectToAction("BeforeEdit", "Location");
            }
            finally
            {
                _session.CloseTransaction();
            }
            current.edited = true;
            return RedirectToAction("BeforeEdit", "Location");
        }

        public async Task<IActionResult> GoBack()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            return RedirectToAction("Edit", "EditRadar", new { id = current.Radar.ID });
        }

        //Below methods are for normal user pages
        public async Task<IActionResult> Show()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            //control if current did not came
            if (current.Receiver == null)
            {
                ViewData["Message"] = "Error occured please restart the program";
                Location l = new Location();
                return View(l);
            }

            //Get receiver's informations and shows it in edit page
            //Transmitter t = await _session.Transmitters.Where(b => b.ID.Equals(current.Transmitter.ID)).FirstOrDefaultAsync();
            return View(current.Location);
        }

        //In case we do not need to write this code again and again in all controllers
        //we can use redirecttoaction method to reach this method
        //but i chose write it again and again because i do not know what we need if more user roles added 
        public async Task<IActionResult> GoBackToRadar()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            return RedirectToAction("Show", "UserRadarScreen", new { id = current.Radar.ID });
        }

    }
}
