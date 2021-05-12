using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class AntennaController : Controller
    {
        private readonly NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

        public AntennaController(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewAntenna()
        {
            return View();
        }

        public async Task<IActionResult> Preliminary()
        {
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);

            Data d = new Data();
            Program.data.TryGetValue(sessionID, out d);

            Antenna antenna = new Antenna();
            if (d.LastAntenna != null)
            {
                antenna = d.LastAntenna;
                if (d.ListOfAntennas.Count > 0)
                {
                    antenna.IsFirstAntenna = false;
                    if (d.Transmitter != null)
                    {
                        antenna.transmitter_id = d.Transmitter.ID;
                    }
                }
            }
            else
            {
                antenna.transmitter_id = Guid.Empty;
                antenna.IsFirstAntenna = true;
                antenna.receiver_id = d.Receiver.ID;
            }

            if(!string.IsNullOrEmpty(d.message))
                ViewData["message"] = d.message;

            return View(antenna);
        }

        [HttpPost]
        public async Task<IActionResult> NewAntennaAsync(Antenna antenna_temp)
        {
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);

            /*we can also use below lines to get correct data
            Data d = new Data();
            Program.data.TryGetValue(sessionID, out d);*/

            Data current = new Data();
            foreach (KeyValuePair<Guid, Data> sds in Program.data)
            {
                if (sds.Key.Equals(sessionID))
                {
                    current = sds.Value;
                }
            }

            //handling user-may-occur mistakes
            if (antenna_temp.type.StartsWith("Select"))
            {
                current.message = "Please select Type";
                current.LastAntenna = antenna_temp;
                Program.data.Remove(sessionID);
                Program.data.Add(sessionID, current);
                return RedirectToAction("Preliminary", "Antenna");
            }

            //if our antenna does not have a user friendly name we give it a default name with the code below.
            String def_name = null;
            //because we change the default name after the Radar added we we should keep it in mind to it is a default given name
            bool isNamed = false;
            if (String.IsNullOrEmpty(antenna_temp.name))
            {
                String transmitter_or_receiver_name = null;
                //if it is a transmitter antenna define it as a transmitter antenna in its name
                if (antenna_temp.duty.ToLower().Equals("transmitter"))
                {
                    transmitter_or_receiver_name = current.Transmitter.name;
                }
                //if it is a receiver or multi tasked antenna define it as a receiver in its name
                else
                {
                    transmitter_or_receiver_name = current.Receiver.name;
                }
                //this statement is for seperating receiver and multi-tasked antennas
                if (antenna_temp.duty.ToLower().Equals("both"))
                {
                    def_name = "Monostatic radar Antenna with receiver name: " + transmitter_or_receiver_name;
                }
                else
                    def_name = transmitter_or_receiver_name + "s Antenna";
                isNamed = true;
            }
            else
                def_name = antenna_temp.name;
            //end of naming

            Guid key = Guid.NewGuid();
            Antenna antenna = new Antenna();
            //if the antenna is both receiver and transmitter antenna give it a receiver and a transmitter id 
            //Because we need a transmitter before adding an antenna which serves as transmitter antenna we should create its transmitter first.
            //After create a transmitter we can insert our antenna to database
            //all of the insertions will execute in goToRadar and Done methods
            if (antenna_temp.duty.Equals("both"))
            {
                antenna = new Antenna(key, def_name, antenna_temp.type, antenna_temp.horizontal_beamwidth, antenna_temp.vertical_beamwidth, antenna_temp.polarization, antenna_temp.number_of_feed, antenna_temp.horizontal_dimension, antenna_temp.vertical_dimension, antenna_temp.duty, null, current.Receiver.ID, antenna_temp.location);
                antenna.Isnamed = isNamed;
                current.message = "New Antenna added to record list";
                current.ListOfAntennas.Add(antenna);
            }
            //if the antenna is a receiver antenna give it a receiver id to build a relationship between antenna and its receiver
            else if (antenna_temp.duty.Equals("receiver"))
            {
                antenna = new Antenna(key, def_name, antenna_temp.type, antenna_temp.horizontal_beamwidth, antenna_temp.vertical_beamwidth, antenna_temp.polarization, 1, antenna_temp.horizontal_dimension, antenna_temp.vertical_dimension, antenna_temp.duty, null, current.Receiver.ID, antenna_temp.location);
                antenna.Isnamed = isNamed; 
                current.message = "New Antenna added to record list";
                current.ListOfAntennas.Add(antenna);
            }
            //if the antenna is a transmitter antenna define it's transmitter with giving an attribute transmitter id
            else
            {
                Guid transmitter_id = current.Transmitter.ID;
                antenna = new Antenna(key, def_name, antenna_temp.type, antenna_temp.horizontal_beamwidth, antenna_temp.vertical_beamwidth, antenna_temp.polarization, antenna_temp.number_of_feed, antenna_temp.horizontal_dimension, antenna_temp.vertical_dimension, antenna_temp.duty, transmitter_id, null, antenna_temp.location);
                antenna.Isnamed = isNamed;
                current.message = "New Antenna added to record list";
                current.ListOfAntennas.Add(antenna);
            }

            current.LastAntenna = antenna_temp;
            return RedirectToAction("Preliminary", "Antenna");
        }

        public IActionResult GoToTransmitter()
        {
            return RedirectToAction("NewTransmitter", "Transmitter");
        }

        public async System.Threading.Tasks.Task<IActionResult> GoToRadar()
        {
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);

            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            //first save all antennas that created before
            try
            {
                _session.BeginTransaction();
                for (int i = 0; i < current.ListOfAntennas.Count; i++)
                {
                    Antenna antenna = current.ListOfAntennas[i];
                    if (antenna.duty.Equals("both"))
                    {
                        antenna.transmitter_id = current.Transmitter.ID;
                    }
                    await _session.SaveAntenna(antenna);
                }

                await _session.Commit();
            }
            catch (Exception e)
            {
                // log exception here
                current.message = e.Message.ToString() + " Error, plase check your database connection and restart your program, Do not forgett to delete uneccessary transmitter and receiver";
                await _session.Rollback();
                return RedirectToAction("Preliminary", "Antenna");
            }
            finally
            {
                _session.CloseTransaction();
            }
            current.message = "Recorded antennas added to database";
            return RedirectToAction("NewRadar", "Radar");
        }

        //This method is for edit page
        public async Task<IActionResult> AddNewAntenna(Data current)
        {
            Radar r = new Radar();
            try
            {
                r = await _session.Radars.Where(b => b.ID.Equals(current.Radar)).FirstOrDefaultAsync();
                current.Radar = r;
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
            return RedirectToAction("NewAntenna", "Antenna");
        }

        public async Task<IActionResult> BeforeEdit(Antenna antenna)
        {
            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            /*if (anntenna.edited)
            {
                ViewData["message"] = "Update completed successfully";
                current.edited = false;
            }*/

            //Get antenna's informations and shows it in edit page
            Antenna a = await _session.Antennas.Where(b => b.ID.Equals(antenna.ID)).FirstOrDefaultAsync();

            return View(a);
        }

        public async Task<IActionResult> Edit(Antenna antenna)
        {
            try
            {
                await _session.EditAntenna(antenna.ID, antenna.name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.location);
                ViewData["message"] = "Antenna updated succesfully";
            }
            catch (Exception e)
            {
                // log exception here
                ViewData["message"] = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
                _session.CloseTransaction();
            }
            //current.edited = true;
            return RedirectToAction("BeforeEdit", "Antenna", new { id = antenna.ID });
        }

        //hiç ekleme yapmayınca sorun
        public async Task<IActionResult> GoBack(Data current)
        {
            /*Radar radar = new Radar();
            try
            {
                Antenna a = await _session.Antennas.Where(b => b.ID.Equals(antenna_temp.ID)).FirstOrDefaultAsync();
                if (a.duty.Equals("receiver"))
                {
                    Receiver receiver = await _session.Receivers.Where(b => b.ID.Equals(a.ID)).FirstOrDefaultAsync();
                    radar = await _session.Radars.Where(b => b.receiver_id.Equals(receiver.ID)).FirstOrDefaultAsync();
                }
                else
                {
                    Transmitter transmitter = await _session.Transmitters.Where(b => b.ID.Equals(a.ID)).FirstOrDefaultAsync();
                    radar = await _session.Radars.Where(b => b.transmitter_id.Equals(transmitter.ID)).FirstOrDefaultAsync();
                }
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
            }*/
            return RedirectToAction("Edit", "EditRadar", new { current = current });
        }

        public async System.Threading.Tasks.Task<IActionResult> Done(Data current)
        {
            //first save all antennas that created before
            try
            {
                _session.BeginTransaction();
                for (int i = 0; i < current.ListOfAntennas.Count; i++)
                {
                    Antenna antenna = current.ListOfAntennas[i];
                    if (antenna.duty.Equals("both"))
                    {
                        antenna.transmitter_id = current.Transmitter.ID;
                    }
                    _session.SaveAntenna(antenna);
                }

                await _session.Commit(); 
                current.message = "New Antennas added";
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
            //current.ComeFromAdd = false;
            return RedirectToAction("Edit", "EditRadar", new { current = current });
        }

    }
}
