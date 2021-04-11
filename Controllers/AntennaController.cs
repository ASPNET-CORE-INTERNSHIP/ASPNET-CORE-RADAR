using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

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

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> NewAntennaAsync(Antenna antenna)
        {
            //handling user-may-occur mistakes
            if (antenna.type.StartsWith("Select"))
            {
                return View(antenna);
                ViewData["Message"] = "Please select Type";
            }

            Guid receiver_id = Datas.Receiver.ID;

            //if our antenna does not have a user friendly name we give it a default name with the code below.
            String def_name = null;
            //because we change the default name after the Radar added we we should keep it in mind to it is a default given name
            bool isNamed = false;
            if (String.IsNullOrEmpty(antenna.name))
            {
                String transmitter_or_receiver_name = null;
                if (antenna.duty.ToLower().Equals("transmitter"))
                {
                    Guid transmitter_id = Datas.Transmitter.ID;
                    try
                    {
                        transmitter_or_receiver_name = await _session.SelectTransmitter(transmitter_id);
                    }
                    catch (Exception e)
                    {
                        // log exception here
                        ViewData["Message"] = e.Message.ToString() + " Error";
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
                        ViewData["Message"] = e.Message.ToString() + " Error";
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
                Antenna a = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, Datas.Transmitter.ID, receiver_id, antenna.location);
                a.Isnamed = isNamed;
                Datas.ListOfAntennas.Add(a);
            }
            //if the antenna is a receiver antenna give it a receiver id to build a relationship between antenna and its receiver
            else if (antenna.duty.Equals("receiver"))
            {
                Antenna a = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, 1, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, Datas.uselessTransmitter.ID, receiver_id, antenna.location);
                a.Isnamed = isNamed;
                Datas.ListOfAntennas.Add(a);
            }
            //if the antenna is a transmitter antenna define its a transmitter with giving an attribute transmitter id
            else
            {
                Guid transmitter_id = Datas.Transmitter.ID;
                Antenna a = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, transmitter_id, Datas.uselessReceiver.ID, antenna.location);
                a.Isnamed = isNamed;
                //Save this antenna to Datas. So we can handle the problem that the user may add a receiver antenna instead of a transmitter antenna.
                Datas.ListOfAntennas.Add(a);
            }
            return View(antenna);
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
                for (int i = 0; i < Datas.ListOfAntennas.Count; i++)
                {
                    Antenna antenna = Datas.ListOfAntennas[i];
                    Console.WriteLine(antenna.ID + " " + antenna.name + " " + antenna.polarization + " " + antenna.receiver_id + " " + antenna.transmitter_id);
                    if (antenna.duty.Equals("both"))
                    {
                        antenna.transmitter_id = Datas.Transmitter.ID;
                    }
                    _session.SaveAntenna(antenna);
                    ViewData["Message"] = "New Antenna added";
                }

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
            /*for (int i = 0; i < Datas.ListOfAntennas.Count; i++)
            {
                Antenna antenna = Datas.ListOfAntennas[i];
                Console.WriteLine(antenna.ID + " " + antenna.name + " " + antenna.polarization + " " + antenna.receiver_id + " " + antenna.transmitter_id);
                if (antenna.duty.Equals("both"))
                {
                    antenna.transmitter_id = Datas.Transmitter.ID;
                }
                try
                {
                    _session.BeginTransaction();
                    _session.SaveAntenna(antenna);
                    await _session.Commit();
                    ViewData["Message"] = "New Antenna added";
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
            }*/
            return RedirectToAction("NewRadar", "Radar");
        }
    }
}
