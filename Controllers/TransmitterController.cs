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
    public class TransmitterController : Controller
    {
        private readonly NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

        public TransmitterController(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewTransmitter()
        {
            return View();
        }


        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> NewTransmitter(Transmitter transmitter_temp)
        {
            //handling user-may-occur mistakes
            if (transmitter_temp.modulation_type.StartsWith("Select"))
            {
                ViewData["message"] = "Please fill the modulation type";
                return View(transmitter_temp);
            }

            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            foreach (KeyValuePair<Guid, Data> sds in Program.data)
            {
                if (sds.Key.Equals(sessionID))
                {
                    current = sds.Value;
                }
            }

            //If the transmitter name is null we give a default name that specifies its number
            String def_name = null;
            //because we change the default name after the Radar added we we should keep it in mind to it is a default given name
            bool isNamed = false;
            if (String.IsNullOrEmpty(transmitter_temp.name))
            {
                int count = 0;

                try
                {
                    count = await _session.GetTransmitterNumber();
                }
                catch (Exception e)
                {
                    // log exception here
                    ViewData["message"] = e.Message.ToString() + " Error";
                    await _session.Rollback();
                    count = -1;
                    return View(transmitter_temp);
                }
                finally
                {
                    _session.CloseTransaction();
                }
                def_name = "Transmitter " + count;
                //keep it in mind
                isNamed = true;
            }
            else
            {
                def_name = transmitter_temp.name;
            }

            Guid key = Guid.NewGuid();
            Transmitter transmitter = new Transmitter(key, def_name, transmitter_temp.modulation_type, transmitter_temp.max_frequency, transmitter_temp.min_frequency, transmitter_temp.power);
            transmitter.Isnamed = isNamed;

            //save our transmitter to database
            try
            {
                _session.BeginTransaction();
                await _session.SaveTransmitter(transmitter);
                await _session.Commit();
            }
            catch (Exception e)
            {
                // log exception here
                ViewData["message"] = e.Message.ToString() + " Error";
                await _session.Rollback();
                return View(transmitter);
            }
            finally
            {
                _session.CloseTransaction();
            }

            //Add our transmitter to Data model and update the dictionary so we can use its id when we're adding Radar entity
            current.Transmitter = transmitter;

            return RedirectToAction("Preliminary", "Antenna");
        }

        public async Task<IActionResult> BeforeEdit(Data current)
        {
            //Because we use the same view before and after edit process we should handle the view messages with the following conditions
            if (current.edited)
            {
                current.message = "Update completed successfully";
                current.edited = false;
            }
            //Get radar's informations and shows it in edit page
            //Transmitter t = await _session.Transmitters.Where(b => b.ID.Equals(current.Transmitter.ID)).FirstOrDefaultAsync();
            return View(current);
        }

        public async Task<IActionResult> Edit(Transmitter transmitter_temp)
        {
            try
            {
                await _session.EditTransmitter(transmitter_temp.ID, transmitter_temp.name, transmitter_temp.modulation_type, transmitter_temp.max_frequency, transmitter_temp.min_frequency, transmitter_temp.power);
            }
            catch (Exception e)
            {
                // log exception here
                ViewData["message"] = e.Message.ToString() + " Error";
                await _session.Rollback();
                return RedirectToAction("BeforeEdit", "Transmitter", new { id = transmitter_temp.ID });
            }
            finally
            {
                _session.CloseTransaction();
            }
            //current.edited = true;
            return RedirectToAction("BeforeEdit", "Transmitter", new { id = transmitter_temp.ID });
        }

        /*public async Task<IActionResult> GoBack(Transmitter transmitter_temp)
        {
            Radar r = new Radar();
            try
            {
                r = await _session.Radars.Where(b => b.transmitter_id.Equals(transmitter_temp.ID)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                // log exception here
                current.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                //return RedirectToAction("BeforeEdit", "Transmitter", new { id = id });
            }
            finally
            {
                _session.CloseTransaction();
            }
            return RedirectToAction("Edit", "EditRadar", new { current = current });
        }*/
    }
}