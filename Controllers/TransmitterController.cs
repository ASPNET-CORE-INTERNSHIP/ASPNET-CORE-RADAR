using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

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
        public async System.Threading.Tasks.Task<IActionResult> NewTransmitterAsync(Transmitter transmitter)
        {
            Datas.newProgram = "no";

            //If the transmitter name is null we give a default name that specifies its number
            String def_name = null;
            //because we change the default name after the Radar added we we should keep it in mind to it is a default given name
            bool isNamed = false;
            if (String.IsNullOrEmpty(transmitter.name))
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
                def_name = "Transmitter " + count;
                //keep it in mind
                isNamed = true;
            }
            else
            {
                def_name = transmitter.name;
            }

            Guid key = Guid.NewGuid();
            Transmitter transmitter_temp = new Transmitter(key, def_name,transmitter.modulation_type, transmitter.max_frequency, transmitter.min_frequency, transmitter.power);
            transmitter_temp.Isnamed = isNamed;
            //Add our transmitter to Datas model so we can use its id when we add Radar entity
            Datas.Transmitter = transmitter_temp;

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
    }
}