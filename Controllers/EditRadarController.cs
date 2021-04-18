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

namespace ASPNETAOP.Controllers
{
    public class EditRadarController : Controller
    {
        private readonly NHibernateMapperSession _session;

        public EditRadarController(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        RadarGeneral currentRadar;

        public async System.Threading.Tasks.Task<IActionResult> EditParam(Guid id)
        {
            //When redirected from the AdminRadarList, the id of the current Radar is given
            //Using the id, RadarGeneral with appropiate Radar, Transmitter, Receiver and Location is retrieved
            currentRadar = await _session.GetRadarGeneralInfo(id);
            RadarGeneral.currentRadarGeneral.Radar = currentRadar.Radar;

            return RedirectToAction("Edit", "EditRadar");
        }

   
        public async System.Threading.Tasks.Task<IActionResult> Edit(Radar ur)
        {
            if(ur.name != null)
            {
                _session.UpdateRadar(ur.ID, ur.transmitter_id, ur.receiver_id, ur.location_id);
            }

            return View(ur);
        }
    }
}
