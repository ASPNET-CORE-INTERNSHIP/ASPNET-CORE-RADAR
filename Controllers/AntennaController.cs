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
    public class AntennaController : Controller
    {
        private readonly NHibernateMapperSession _session;
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

        public async Task<IActionResult> begin()
        {
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data d = new Data();
            foreach (KeyValuePair<Guid, Data> sds in Program.data)
            {
                if (sds.Key.Equals(sessionID))
                {
                    d = sds.Value;
                }
            }
            
            return View(d);
        }

        [HttpPost]
        public async Task<IActionResult> NewAntennaAsync(Data current)
        {
            //handling user-may-occur mistakes
            if (current.LastAntenna.type.StartsWith("Select"))
            {
                current.message = "Please select Type";
                String sessionID_string = HttpContext.Session.GetString("Session");
                Guid sessionIDguid = Guid.Parse(sessionID_string);
                foreach (KeyValuePair<Guid, Data> sds in Program.data)
                {
                    if (sds.Key.Equals(sessionIDguid))
                    {
                        Program.data.Remove(sessionIDguid);
                        Program.data.Add(sessionIDguid, current);
                    }
                }
                return RedirectToAction("begin", "Antenna");
            }

            //if our antenna does not have a user friendly name we give it a default name with the code below.
            String def_name = null;
            //because we change the default name after the Radar added we we should keep it in mind to it is a default given name
            bool isNamed = false;
            if (String.IsNullOrEmpty(current.LastAntenna.name))
            {
                String transmitter_or_receiver_name = null;
                if (current.LastAntenna.duty.ToLower().Equals("transmitter"))
                {
                    Guid transmitter_id = (Guid)current.Transmitter.ID;
                    try
                    {
                        transmitter_or_receiver_name = await _session.SelectTransmitter(transmitter_id);
                    }
                    catch (Exception e)
                    {
                        // log exception here
                        current.message = e.Message.ToString() + " Error";
                        await _session.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        transmitter_or_receiver_name = await _session.SelectReceiver((Guid)current.Receiver.ID);
                    }
                    catch (Exception e)
                    {
                        // log exception here
                        current.message = e.Message.ToString() + " Error";
                        await _session.Rollback();
                    }
                }

                if (current.LastAntenna.duty.ToLower().Equals("both"))
                {
                    def_name = "Monostatic radar Antenna with receiver name: " + transmitter_or_receiver_name;
                }
                else
                    def_name = transmitter_or_receiver_name + "s Antenna";
                isNamed = true;
            }
            else
                def_name = current.LastAntenna.name;
            //end of naming

            Guid key = Guid.NewGuid();
            //if the antenna is both receiver and transmitter antenna give it a receiver and a transmitter id 
            //Because we need a transmitter before adding an antenna which serves as transmitter antenna we should create its transmitter first.
            //After create a transmitter we can insert our antenna to database
            if (current.LastAntenna.duty.Equals("both"))
            {
                Antenna antenna = new Antenna(key, def_name, current.LastAntenna.type, current.LastAntenna.horizontal_beamwidth, current.LastAntenna.vertical_beamwidth, current.LastAntenna.polarization, current.LastAntenna.number_of_feed, current.LastAntenna.horizontal_dimension, current.LastAntenna.vertical_dimension, current.LastAntenna.duty, current.Transmitter.ID, current.Receiver.ID, current.LastAntenna.location);
                antenna.Isnamed = isNamed;
                try
                {
                    _session.BeginTransaction();
                    _session.SaveAntenna(antenna);

                    await _session.Commit();
                    current.message = "New Antenna added";
                    current.ListOfAntennas.Add(antenna);
                }
                catch (Exception e)
                {
                    // log exception here
                    current.message = e.Message.ToString() + " Error";
                    await _session.Rollback();
                }
                finally
                {
                    current.LastAntenna = antenna;
                    _session.CloseTransaction();
                }
            }
            //if the antenna is a receiver antenna give it a receiver id to build a relationship between antenna and its receiver
            else if (current.LastAntenna.duty.Equals("receiver"))
            {
                Antenna antenna = new Antenna(key, def_name, current.LastAntenna.type, current.LastAntenna.horizontal_beamwidth, current.LastAntenna.vertical_beamwidth, current.LastAntenna.polarization, 1, current.LastAntenna.horizontal_dimension, current.LastAntenna.vertical_dimension, current.LastAntenna.duty, null, current.Receiver.ID, current.LastAntenna.location);
                antenna.Isnamed = isNamed;
                try
                {
                    _session.BeginTransaction();
                    _session.SaveAntenna(antenna);

                    await _session.Commit();
                    current.message = "New Antenna added";
                    current.ListOfAntennas.Add(antenna);
                }
                catch (Exception e)
                {
                    // log exception here
                    current.message = e.Message.ToString() + " Error";
                    await _session.Rollback();
                }
                finally
                {
                    current.LastAntenna = antenna;
                    _session.CloseTransaction();
                }
            }
            //if the antenna is a transmitter antenna define its a transmitter with giving an attribute transmitter id
            else
            {
                Guid transmitter_id = (Guid)current.Transmitter.ID;
                Antenna antenna = new Antenna(key, def_name, current.LastAntenna.type, current.LastAntenna.horizontal_beamwidth, current.LastAntenna.vertical_beamwidth, current.LastAntenna.polarization, current.LastAntenna.number_of_feed, current.LastAntenna.horizontal_dimension, current.LastAntenna.vertical_dimension, current.LastAntenna.duty, transmitter_id, null, current.LastAntenna.location);
                antenna.Isnamed = isNamed;
                try
                {
                    _session.BeginTransaction();
                    _session.SaveAntenna(antenna);

                    await _session.Commit();
                    current.message = "New Antenna added";
                    current.ListOfAntennas.Add(antenna);
                }
                catch (Exception e)
                {
                    // log exception here
                    current.message = e.Message.ToString() + " Error";
                    await _session.Rollback();
                }
                finally
                {
                    current.LastAntenna = antenna;
                    _session.CloseTransaction();
                }
                //Save this antenna to Datas. So we can handle the problem that the user may add a receiver antenna instead of a transmitter antenna.
                //current.ListOfAntennas.Add(key);
            }
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);

            //Data d = new Data();
            //Program.data.TryGetValue(sessionID, out d);

            Program.data.Remove(sessionID);
            Program.data.Add(sessionID, current);
            
            return RedirectToAction("begin", "Antenna");
        }

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

        public async Task<IActionResult> BeforeEdit(Data current)
        {
            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            if (current.edited)
            {
                current.message = "Update completed successfully";
                current.edited = false;
            }

            //Get antenna's informations and shows it in edit page
            Antenna antenna = await _session.Antennas.Where(b => b.ID.Equals(current.LastAntenna.ID)).FirstOrDefaultAsync();

            return View(current);
        }

        public async Task<IActionResult> Edit(Data current)
        {
            try
            {
                await _session.EditAntenna(current.LastAntenna.ID, current.LastAntenna.name, current.LastAntenna.type, current.LastAntenna.horizontal_beamwidth, current.LastAntenna.vertical_beamwidth, current.LastAntenna.polarization, current.LastAntenna.number_of_feed, current.LastAntenna.horizontal_dimension, current.LastAntenna.vertical_dimension, current.LastAntenna.location);
                current.message = "Antenna updated succesfully";
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
            return RedirectToAction("BeforeEdit", "Antenna", new { id = current.LastAntenna.ID });
        }

        //hiç ekleme yapmayınca sorun
        public async Task<IActionResult> GoBack(Data current)
        {
            /*Radar radar = new Radar();
            try
            {
                Antenna a = await _session.Antennas.Where(b => b.ID.Equals(current.LastAntenna.ID)).FirstOrDefaultAsync();
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

        public IActionResult GoToTransmitter(Data current)
        {
            return RedirectToAction("NewTransmitter", "Transmitter", current = current);
        }

        public async System.Threading.Tasks.Task<IActionResult> GoToRadarAsync(Data current)
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
                    await _session.SaveAntenna(antenna);
                    current.message = "New Antenna added";
                }

                await _session.Commit();
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
            current.message = "Antennas added succesfully";
            return RedirectToAction("NewRadar", "Radar");
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
            current.ComeFromAdd = false;
            return RedirectToAction("Edit", "EditRadar", new { current = current });
        }

    }
}
