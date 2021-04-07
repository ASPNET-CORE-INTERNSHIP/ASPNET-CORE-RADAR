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
            Guid receiver_id = Datas.Receiver.ID;
            String def_name = null;
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
            }
            else
                def_name = antenna.name;


            Guid key = Guid.NewGuid();
            //if the antenna is both receiver and transmitter antenna give it a receiver and a transmitter id 
            //Because we need a transmitter before adding an antenna which has a transmitter id we need a transmitter first 
            if (antenna.duty.Equals("both"))
            {
                Datas.Antenna = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, receiver_id, receiver_id, antenna.location);
                return GoToTransmitter();
            }
            //if the antenna is a receiver antenna give it a receiver id
            else if (antenna.duty.Equals("receiver"))
            {
                Antenna a = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, 1, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, receiver_id, Datas.uselessTransmitter.ID, antenna.location);
                try
                {
                    _session.BeginTransaction();

                    _session.SaveAntenna(a);
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
            }
            //if the antenna is a transmitter antenna attach it a transmitter id
            else
            {
                Guid transmitter_id = Datas.Transmitter.ID;
                Antenna a = new Antenna(key, def_name, antenna.type, antenna.horizontal_beamwidth, antenna.vertical_beamwidth, antenna.polarization, antenna.number_of_feed, antenna.horizontal_dimension, antenna.vertical_dimension, antenna.duty, Datas.uselessReceiver.ID, transmitter_id, antenna.location);
                try
                {
                    _session.BeginTransaction();

                    _session.SaveAntenna(a);
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
            }
            return View(antenna);
        }



        public IActionResult GoToTransmitter()
        {
            return RedirectToAction("NewTransmitter", "Transmitter");
        }

        public IActionResult GoToRadar()
        {
            return RedirectToAction("NewRadar", "Radar");
        }
    }
}
