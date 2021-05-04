using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NHibernate.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class TransmitterController : Controller
    {
        private readonly NHibernateMapperSession _session;

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
        public async System.Threading.Tasks.Task<IActionResult> NewTransmitterAsync(Data current)
        {
            current.newProgram = "no";

            //handling user may occur errors
            if (current.Transmitter.modulation_type.StartsWith("Select"))
            {
                ViewData["Message"] = "Please fill the modulation type";
                return View(current);
            }

            //If the transmitter name is null we give a default name that specifies its number
            String def_name = null;
            //because we change the default name after the Radar added we we should keep it in mind to it is a default given name
            bool isNamed = false;
            if (String.IsNullOrEmpty(current.Transmitter.name))
            {
                int count = 0;

                try
                {
                    count = await _session.GetTransmitterNumber();
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
                def_name = "Transmitter " + count;
                //keep it in mind
                isNamed = true;
            }
            else
            {
                def_name = current.Transmitter.name;
            }

            Guid key = Guid.NewGuid();
            Transmitter transmitter_temp = new Transmitter(key, def_name, current.Transmitter.modulation_type, current.Transmitter.max_frequency, current.Transmitter.min_frequency, current.Transmitter.power);
            transmitter_temp.Isnamed = isNamed;
            //Add our transmitter to Datas model so we can use its id when we add Radar entity
            current.Transmitter = transmitter_temp;

            //save our transmitter to database
            try
            {
                _session.BeginTransaction();
                await _session.SaveTransmitter(transmitter_temp);
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
            //Get radar's informations and shows it in edit page
            //Transmitter t = await _session.Transmitters.Where(b => b.ID.Equals(current.Transmitter.ID)).FirstOrDefaultAsync();
            return View(current);
        }

        public async Task<IActionResult> Edit(Data current)
        {
            try
            {
                await _session.EditTransmitter(current.Transmitter.ID, current.Transmitter.name, current.Transmitter.modulation_type, current.Transmitter.max_frequency, current.Transmitter.min_frequency, current.Transmitter.power);
            }
            catch (Exception e)
            {
                // log exception here
                current.message = e.Message.ToString() + " Error";
                await _session.Rollback();
                return RedirectToAction("BeforeEdit", "Transmitter", new { id = current.Transmitter.ID });
            }
            finally
            {
                _session.CloseTransaction();
            }
            current.edited = true;
            return RedirectToAction("BeforeEdit", "Transmitter", new { id = current.Transmitter.ID });
        }

        public async Task<IActionResult> GoBack(Data current)
        {
            Radar r = new Radar();
            try
            {
                r = await _session.Radars.Where(b => b.transmitter_id.Equals(current.Transmitter.ID)).FirstOrDefaultAsync();
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
        }
    }
}