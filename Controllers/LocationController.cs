using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
            if (loc.city == null | loc.country == null | loc.geographic_latitude == null | loc.geographic_longitude == null)
            {
                return View(loc);
                ViewData["Message"] = "Please do not leave empty Country, City, Geographic Latitude and Geographic Longitude areas";
            }

            //defining Radar's and Location's key here
            Guid key_location = Guid.NewGuid();
            Guid key = Guid.NewGuid();
            Datas.Radar.ID = key;

            //If the location name is null we give a default name that specifies its country, city and area number  
            String def_name = null;
            if (String.IsNullOrEmpty(loc.name))
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
                def_name = loc.name;
            }

            //rename Radar, Transmitter and Antennas again
            String radar_name = Datas.Radar.name;
            if (Datas.Radar.Isnamed == true)
            {
                radar_name = "Radar in " + def_name;
            }

            if (Datas.Transmitter.Isnamed == true)
            {
                Guid id = Datas.Transmitter.ID;
                String newName = radar_name + "'s Transmitter";
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
                }
                finally
                {
                    _session.CloseTransaction();
                }
            }

            if (Datas.Receiver.Isnamed == true)
            {
                Guid id = Datas.Receiver.ID;
                String newName = radar_name + "'s Receiver";
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
                }
                finally
                {
                    _session.CloseTransaction();
                }
            }

            /*int count_receiver = 0;

            int count_transmitter = 0;

            int count_both = 0;


            for (int i=0; i < Datas.ListOfAntennas.Count; i++)
            {
                Antenna antenna = Datas.ListOfAntennas[i];
                if (antenna.Isnamed == true)
                {
                    String newName = "";
                    if (antenna.duty.Equals("receiver"))
                    {
                        count_receiver = count_receiver + 1;
                        newName = radar_name + "'s receiver antenna " + count_receiver;
                    }

                    else if (antenna.duty.Equals("transmitter"))
                    {
                        count_transmitter = count_transmitter + 1;
                        newName = radar_name + "'s transmitter antenna " + count_transmitter;
                    }

                    else
                    {
                        count_both = count_both + 1;
                        newName = radar_name + "'s multiple role antenna " + count_both;
                    }

                    try
                    {
                        _session.BeginTransaction();
                        _session.RenameAntenna(antenna.ID, newName);
                        await _session.Commit();
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
            }*/

            Location location_temp = new Location(key_location, def_name, loc.country, loc.city, loc.geographic_latitude, loc.geographic_longitude, loc.airborne);
            Radar radar_temp = new Radar(key, radar_name, Datas.Radar.system, Datas.Radar.configuration, Datas.Transmitter.ID, Datas.Receiver.ID, key_location);
            Datas.Radar.location_id = key_location;
            Datas.Radar.name = radar_name;
            Datas.Radar.transmitter_id = Datas.Transmitter.ID;
            Datas.Radar.receiver_id = Datas.Receiver.ID;

            try
            {
                _session.BeginTransaction();
                _session.SaveLocation(location_temp);
                _session.SaveRadar(radar_temp);
                await _session.Commit();
                ViewData["Message"] = "Both records (Location and Radar) saved to the db";
                return RedirectToAction("NewMode", "Mode");
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
            

            return View(loc);
        }
    }
}
