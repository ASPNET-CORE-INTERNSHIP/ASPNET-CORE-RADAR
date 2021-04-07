using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NHibernate;
using System;
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
            //we add useless receiver and transmitter to db because in antenna we have both transmitter and receiver id attributes
            //but an transmitter antenna only needs valid transmitter id and receiver antennas only need valid receiver id.
            //however we cannot set a null value to a transmirrer antenna's receiver id (because of foreign key constraint)
            Receiver useless_r = new Receiver(Guid.NewGuid(), "useless", 0, 0, 0);
            Transmitter useless_t = new Transmitter(Guid.NewGuid(), "useless", "AM-amplitude modulation", 0, 0, 0);
            Datas.uselessReceiver = useless_r;
            Datas.uselessTransmitter = useless_t;
            try
            {
                _session.BeginTransaction();

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
            try
            {
                _session.BeginTransaction();

                await _session.SaveTransmitter(useless_t);
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
            String rec_name = null;
            if (String.IsNullOrEmpty(receiver.name))
            {
                int count = 0;
                try
                {
                    count = await _session.GetReceiverNumber();
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
            }
            else
            {
                rec_name = receiver.name;
            }

            Guid key = Guid.NewGuid();
            Receiver r = new Receiver(key, rec_name, receiver.listening_time, receiver.rest_time, receiver.recovery_time);
            Datas.Receiver = r;
            Datas.Transmitter = new Transmitter();
            Datas.Submode = new Submode();
            Datas.Scan = new Scan();
            Datas.Radar = new Radar();

            Datas.newProgram = "yes";
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
            }
            finally
            {
                _session.CloseTransaction();
            }
            return View(receiver);
            
        }
    }
}
