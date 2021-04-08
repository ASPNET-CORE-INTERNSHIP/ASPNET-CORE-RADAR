using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class ModeController : Controller
    {
        private readonly NHibernateMapperSession _session;

        public ModeController(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewMode()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> NewModeAsync(Mode mod)
        {
            Guid key = Guid.NewGuid();
            Mode m = new Mode(key, mod.name, Datas.Radar.ID);
            Datas.Mode = m;

            try
            {
                _session.BeginTransaction();
                await _session.SaveMode(m);
                await _session.Commit();
                ViewData["Message"] = "New Receiver added";
                return RedirectToAction("NewSubmode", "Submode");
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
            return View(mod);
        }

    }
}
