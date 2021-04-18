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
            return RedirectToAction("Edit", "EditRadar");
        }

   
        public async System.Threading.Tasks.Task<IActionResult> Edit(RadarGeneral RadarBase)
        {
            List<Antenna> AntennaList = await _session.Antennas.Where(b => b.receiver_id.Equals(RadarBase.Receiver.ID) || b.transmitter_id.Equals(RadarBase.Transmitter.ID)).ToListAsync();
            List<Mode> ModeList = await _session.Modes.Where(b => b.radar_id.Equals(RadarBase.Radar.ID)).ToListAsync();
            List<Submode> SubModeList = new List<Submode>();
            foreach (Mode mod in ModeList)
            {
                List<Submode> list_temp = await _session.Submode.Where(b => b.mode_id.Equals(mod.ID)).ToListAsync();

                foreach (Submode s in list_temp)
                {
                    SubModeList.Add(s);
                }
            }

            RadarBase.ListOfAntennas = AntennaList;
            RadarBase.Mode = ModeList;
            RadarBase.Submode = SubModeList;

            return View(RadarBase);
        }
    }
}
