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

        public async System.Threading.Tasks.Task<IActionResult> Edit(Guid id)
        {
            Radar r = await _session.Radars.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
            Transmitter transmitter_temp = await _session.Transmitters.Where(b => b.ID.Equals(r.transmitter_id)).FirstOrDefaultAsync();
            Receiver receiver_temp = await _session.Receivers.Where(b => b.ID.Equals(r.receiver_id)).FirstOrDefaultAsync();
            Location location_temp = await _session.Location.Where(b => b.ID.Equals(r.location_id)).FirstOrDefaultAsync();
            
            //List<Antenna> AntennaList = await _session.Antennas.Where(b => (b.duty.Equals("receiver") && b.receiver_id.Equals(receiver_temp.ID)) || (b.duty.Equals("receiver")! && b.transmitter_id.Equals(transmitter_temp.ID)) ).ToListAsync();
            List<Mode> ModeList = await _session.Modes.Where(b => b.radar_id.Equals(r.ID)).ToListAsync();
            List<Submode> SubModeList = new List<Submode>();
            foreach (Mode mod in ModeList)
            {
                List<Submode> list_temp = await _session.Submode.Where(b => b.mode_id.Equals(mod.ID)).ToListAsync();

                foreach (Submode s in list_temp)
                {
                    SubModeList.Add(s);
                }
            }
            RadarInfo radar = new RadarInfo(r, transmitter_temp, receiver_temp, location_temp);
            //radar.ListOfAntennas = AntennaList;
            radar.Mode = ModeList;
            radar.Submode = SubModeList;

            return View(radar);
        }

        public IActionResult RadarEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Radar", new { id = id });
        }
    }
}