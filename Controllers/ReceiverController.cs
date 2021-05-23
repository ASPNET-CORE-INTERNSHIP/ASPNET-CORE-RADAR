using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ASPNETAOP.Controllers
{
    public class ReceiverController : Controller
    {
        private readonly NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

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
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            
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
                    Program.data.Remove(sessionID);
                    await _session.Rollback();
                    return RedirectToAction("NewReceiver", "Receiver");
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
                ViewData["Message"] = e.Message.ToString() + " Error";
                Program.data.Remove(sessionID);
                await _session.Rollback();
                return RedirectToAction("NewReceiver", "Receiver");
            }
            finally
            {
                _session.CloseTransaction();
            }
            Program.data.Remove(sessionID);
            Program.data.Add(sessionID, current);
            return RedirectToAction("Preliminary", "Antenna");
        }

        //Below functions are for edit pages
        public async Task<IActionResult> BeforeEdit()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

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
            //Receiver r = await _session.Receivers.Where(b => b.ID.Equals(current.Receiver.ID)).FirstOrDefaultAsync();
            Receiver r = current.Receiver;
            return View(r);
        }

        public async Task<IActionResult> Edit(Receiver newValues)
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            current.Receiver = newValues;
            try
            {
                await _session.EditReceiver(newValues.ID, newValues.name, newValues.listening_time, newValues.rest_time, newValues.recovery_time);
            }
            catch (Exception e)
            {
                // log exception here
                current.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                return RedirectToAction("BeforeEdit", "Receiver");
            }
            finally
            {
                _session.CloseTransaction();
            }
            current.edited = true;
            return RedirectToAction("BeforeEdit", "Receiver");
        }

        public async Task<IActionResult> GoBack()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            return RedirectToAction("Edit", "EditRadar", new { id = current.Radar.ID});
        }

        //below functions are for normal user display pages
        public async Task<IActionResult> Show()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            Receiver r = current.Receiver;
            return View(r);
        }

        //In case we do not need to write this code again and again in all controllers
        //we can use redirecttoaction method to reach this method
        //but i chose write it again and again because i do not know what we need if more user roles added 
        public async Task<IActionResult> GoBackToRadar()
        {
            //get session id (we will use it when updating data and handling errors)
            String sessionID_s = HttpContext.Session.GetString("Session");
            Guid sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);
            return RedirectToAction("Show", "UserRadarScreen", new { id = current.Radar.ID });
        }

    }
}
