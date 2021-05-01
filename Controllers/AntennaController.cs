using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NHibernate.Linq;
using System;
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

        public async Task<IActionResult> AddNewAntenna(Guid id)
        {
            Radar r = new Radar();
            try
            {
                r = await _session.Radars.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
                Data.Radar = r;
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
            return RedirectToAction("NewAntenna", "Antenna");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> NewAntennaAsync(Antenna antenna)
        {
            //handling user-may-occur mistakes
            if (antenna.type.StartsWith("Select"))
            {
                return View(antenna);
                Data.message = "Please select Type";
            }

            Guid receiver_id = Data.Receiver.ID;

            //if our antenna does not have a user friendly name we give it a default name with the code below.
            String def_name = null;
            //because we change the default name after the Radar added we we should keep it in mind to it is a default given name
            bool isNamed = false;
            if (String.IsNullOrEmpty(antenna.name))
            {
                String transmitter_or_receiver_name = null;
                if (antenna.duty.ToLower().Equals("transmitter"))
                {
                    Guid transmitter_id = Data.Transmitter.ID;
                    try
                    {
                        transmitter_or_receiver_name = await _session.SelectTransmitter(transmitter_id);
                    }
                    catch (Exception e)
                    {
                        // log exception here
                        Data.message = e.Message.ToString() + " Error";
                        await _session.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        transmitter_or_receiver_name = await _session.SelectReceiver(receiver_id);
                    }
                    catch (Exception e)
                    {
                        // log exception here
                        Data.message = e.Message.ToString() + " Error";
                        await _session.Rollback();
                    }
                }

                if (antenna.duty.ToLower().Equals("both"))
                {
                    def_name = "Monostatic radar antenna with receiver name: " + transmitter_or_receiver_name;
                }
                else
                    def_name = transmitter_or_receiver_name + "s antenna";
                isNamed = true;
            }
            else
                def_name = antenna.name;
            //end of naming

            Guid key = Guid.NewGuid();
            //if the antenna is both receiver and transmitter antenna give it a receiver and a transmitter id 
            //Because we need a transmitter before adding an antenna which serves as transmitter antenna we should create its transmitter first.
            //After create a transmitter we can insert our antenna to database
            if (antenna.duty.Equals("both"))
            {
                Antenna a = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, Data.Transmitter.ID, receiver_id, antenna.location);
                a.Isnamed = isNamed;
                Data.ListOfAntennas.Add(a);
            }
            //if the antenna is a receiver antenna give it a receiver id to build a relationship between antenna and its receiver
            else if (antenna.duty.Equals("receiver"))
            {
                Antenna a = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, 1, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, null, receiver_id, antenna.location);
                a.Isnamed = isNamed;
                Data.ListOfAntennas.Add(a);
            }
            //if the antenna is a transmitter antenna define its a transmitter with giving an attribute transmitter id
            else
            {
                Guid transmitter_id = Data.Transmitter.ID;
                Antenna a = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, transmitter_id, null, antenna.location);
                a.Isnamed = isNamed;
                //Save this antenna to Datas. So we can handle the problem that the user may add a receiver antenna instead of a transmitter antenna.
                Data.ListOfAntennas.Add(a);
            }
            return View(antenna);
        }

        public async Task<IActionResult> BeforeEdit(Guid id)
        {
            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            if (Data.edited)
            {
                Data.message = "Update completed successfully";
                Data.edited = false;
            }

            //Get antenna's informations and shows it in edit page
            Antenna antenna = await _session.Antennas.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();

            return View(antenna);
        }

        public async Task<IActionResult> Edit(Antenna antenna)
        {
            try
            {
                await _session.EditAntenna(antenna.ID, antenna.name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.location);
                Data.message = "Antenna updated succesfully";
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
            Data.edited = true;
            return RedirectToAction("BeforeEdit", "Antenna", new { id = antenna.ID });
        }

        //hiç ekleme yapmayınca sorun
        public async Task<IActionResult> GoBack(Guid id)
        {
            Radar radar = new Radar();
            try
            {
                Antenna a = await _session.Antennas.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
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
                Data.message = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
                _session.CloseTransaction();
            }
            return RedirectToAction("Edit", "EditRadar", new { id = radar.ID });
        }

        public IActionResult GoToTransmitter()
        {
            return RedirectToAction("NewTransmitter", "Transmitter");
        }

        public async System.Threading.Tasks.Task<IActionResult> GoToRadarAsync()
        {
            //first save all antennas that created before
            try
            {
                _session.BeginTransaction();
                for (int i = 0; i < Data.ListOfAntennas.Count; i++)
                {
                    Antenna antenna = Data.ListOfAntennas[i];
                    if (antenna.duty.Equals("both"))
                    {
                        antenna.transmitter_id = Data.Transmitter.ID;
                    }
                    _session.SaveAntenna(antenna);
                    Data.message = "New Antenna added";
                }

                await _session.Commit();
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
            Data.message = "Antennas added succesfully";
            return RedirectToAction("NewRadar", "Radar");
        }

        public async System.Threading.Tasks.Task<IActionResult> Done()
        {

            //first save all antennas that created before
            try
            {
                _session.BeginTransaction();
                for (int i = 0; i < Data.ListOfAntennas.Count; i++)
                {
                    Antenna antenna = Data.ListOfAntennas[i];
                    if (antenna.duty.Equals("both"))
                    {
                        antenna.transmitter_id = Data.Transmitter.ID;
                    }
                    _session.SaveAntenna(antenna);
                }

                await _session.Commit(); 
                Data.message = "New Antennas added";
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
            Data.ComeFromAdd = false;
            return RedirectToAction("Edit", "EditRadar", new { id = Data.Radar.ID });
        }

    }
}
