using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class ReceiverController : Controller
    {
        private readonly NHibernateMapperSession _session;

        public ReceiverController(NHibernateMapperSession session)
        {
            _session = session;

            //empty data class for our new Radar
            Data.Receiver = new Receiver();
            Data.Transmitter = new Transmitter();
            Data.Submode = new Submode();
            Data.Scan = new Scan();
            Data.Radar = new Radar();
            Data.newProgram = "yes";
            Data.message = null;
            Data.edited = false;
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
            //If the receiver name is null we give a default name that specifies its number
            //because we change the default name after the Radar added we we should keep it in mind to it is a default given name
            bool IsNamed = false;
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
        }

        public async Task<IActionResult> BeforeEdit(Guid id)
        {
            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            if (Data.edited)
            {
                ViewData["Message"] = "Update completed successfully";
                Data.edited = false;
            }
            if (Data.message != null)
            {
                ViewData["Message"] = Data.message;
                Data.message = null;
            }
            //Get radar's informations and shows it in edit page
            Receiver r = await _session.Receivers.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
            return View(r);
        }

        public async Task<IActionResult> Edit(Receiver newValues)
        {
            try
            {
                await _session.EditReceiver(newValues.ID, newValues.name, newValues.listening_time, newValues.rest_time, newValues.recovery_time);
            }
            catch (Exception e)
            {
                // log exception here
                Data.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                return RedirectToAction("BeforeEdit", "Receiver", new { id = newValues.ID });
            }
            finally
            {
                _session.CloseTransaction();
            }
            Data.edited = true;
            return RedirectToAction("BeforeEdit", "Receiver", new { id = newValues.ID });
        }

        public async Task<IActionResult> GoBack(Guid id)
        {
            Radar r = new Radar();
            try
            {
                r = await _session.Radars.Where(b => b.receiver_id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                // log exception here
                Data.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                return RedirectToAction("BeforeEdit", "Receiver", new { id = id });
            }
            finally
            {
                _session.CloseTransaction();
            }
            return RedirectToAction("Edit", "EditRadar", new { id = r.ID });
        }
    }
}
