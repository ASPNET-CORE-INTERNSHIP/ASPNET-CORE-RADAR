using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ASPNETAOP.Aspect;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

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
        public async System.Threading.Tasks.Task<IActionResult> NewReceiver(Receiver receiver)
        {
            //create new Data element for our current created radar
            Data current = new Data();
            //I think this method gives the current session id
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Console.WriteLine(sessionID_s);
            

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
            current.Receiver = r;

            try
            {
                _session.BeginTransaction();
                
                await _session.SaveReceiver(r);
                await _session.Commit();
                current.message = "New Receiver added";
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
            Program.data.Add(sessionID, current);
            return RedirectToAction("begin", "Antenna");
        }

        public async Task<IActionResult> BeforeEdit(Data current)
        {
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
            //Get radar's informations and shows it in edit page
            Receiver r = await _session.Receivers.Where(b => b.ID.Equals(current.Receiver.ID)).FirstOrDefaultAsync();
            return View(r);
        }

        public async Task<IActionResult> Edit(Data current)
        {
            try
            {
                await _session.EditReceiver(current.Receiver.ID, current.Receiver.name, current.Receiver.listening_time, current.Receiver.rest_time, current.Receiver.recovery_time);
            }
            catch (Exception e)
            {
                // log exception here
                current.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                return RedirectToAction("BeforeEdit", "Receiver", new { id = current.Receiver.ID });
            }
            finally
            {
                _session.CloseTransaction();
            }
            current.edited = true;
            return RedirectToAction("BeforeEdit", "Receiver", new { current = current });
        }

        public async Task<IActionResult> GoBack(Data current)
        {
            /*Radar r = new Radar();
            try
            {
                r = await _session.Radars.Where(b => b.receiver_id.Equals(current.Receiver.ID)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                // log exception here
                current.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                //return RedirectToAction("BeforeEdit", "Receiver", new { id = id });
            }
            finally
            {
                _session.CloseTransaction();
            }*/
            return RedirectToAction("Edit", "EditRadar", new { current = current });
        }
    }
}
