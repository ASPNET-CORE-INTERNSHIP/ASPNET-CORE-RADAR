using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ASPNETAOP.Controllers
{
    public class ReceiverController : Controller
    {
        private readonly NHibernateMapperSession _session;

        public ReceiverController(NHibernateMapperSession session)
        {
            _session = session;

            //new list of antennas for new Radar
            Data.ListOfAntennas = new List<Antenna>();
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewReceiver()
        {
            return View();
        }


        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> NewReceiverAsync(Receiver receiver)
        {
            //In antenna table we have both receiver_id and transmitter_id attributes with foreign key constraint
            //Because of foreign key constraint we cannot create a receiver antenna without using a transmitter_id. (Tried but DBNull.Value did not work as I want)
            //So create a useless transmitter and receiver, so we can fill the emptiness in antenna's receiver_id and transmitter_id attributes.
            Receiver useless_r = new Receiver(Guid.NewGuid(), "useless", 0, 0, 0);
            Transmitter useless_t = new Transmitter(Guid.NewGuid(), "useless", "AM-amplitude modulation", 0, 0, 0);
            Data.uselessReceiver = useless_r;
            Data.uselessTransmitter = useless_t;
            try
            {
                _session.BeginTransaction();
                await _session.SaveTransmitter(useless_t);
                await _session.SaveReceiver(useless_r);
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

            //If the receiver name is null we give a default name that specifies its number
            //because we change the default name after the Radar added we we should keep it in mind to it is a default given name
            bool IsNamed = false;
            String rec_name = null;
            if (String.IsNullOrEmpty(receiver.name))
            {
                int count = 0;
                try
                {
                    count = await _session.GetTransmitterNumber();
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
                rec_name = "Receiver " + count;
                //keep it in mind
                IsNamed = true;
            }
            else
            {
                rec_name = receiver.name;
            }

            Guid key = Guid.NewGuid();
            Receiver r = new Receiver(key, rec_name, receiver.listening_time, receiver.rest_time, receiver.recovery_time);
            r.Isnamed = IsNamed;
            Data.Receiver = r;
            Data.Transmitter = new Transmitter();
            Data.Submode = new Submode();
            Data.Scan = new Scan();
            Data.Radar = new Radar();

            Data.newProgram = "yes";
            try
            {
                _session.BeginTransaction();
                
                await _session.SaveReceiver(r);
                await _session.Commit();
                ViewData["Message"] = "New Receiver added";
                return RedirectToAction("NewAntenna", "Antenna");
            }
            catch (Exception e)
            {
                // log exception here
                ViewData["Message"] = e.Message.ToString() + " Error";
                await _session.Rollback();
                return View(receiver);
            }
            finally
            {
                _session.CloseTransaction();
            }
            return View(receiver);
            
        }
    }
}
