using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Http;
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
    public class UserRadarScreen : Controller
    {
        private readonly NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

        public UserRadarScreen(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public async System.Threading.Tasks.Task<IActionResult> Show(Guid id)
        {
            Radar r = await _session.Radars.Where(b => b.ID.Equals(id)).FirstOrDefaultAsync();
            Transmitter transmitter_temp = await _session.Transmitters.Where(b => b.ID.Equals(r.transmitter_id)).FirstOrDefaultAsync();
            Receiver receiver_temp = await _session.Receivers.Where(b => b.ID.Equals(r.receiver_id)).FirstOrDefaultAsync();
            Location location_temp = await _session.Location.Where(b => b.ID.Equals(r.location_id)).FirstOrDefaultAsync();
            List<Antenna> AntennaList = await _session.Antennas.Where(b => (b.receiver_id != null && b.receiver_id.Value.Equals(receiver_temp.ID)) || (b.transmitter_id != null && b.transmitter_id.Value.Equals(transmitter_temp.ID))).ToListAsync();
            List<Mode> ModeList = await _session.Modes.Where(b => b.radar_id.Equals(r.ID)).ToListAsync();
            List<ModeInfo> Modes = new List<ModeInfo>();

            foreach (Mode m in ModeList)
            {
                ModeInfo INFO = new ModeInfo();
                INFO.Mode = m;
                Modes.Add(INFO);
            }

            //FILL THE DATA MODEL WITH NECESSARY VALUES so we can use DATA model in editing process
            //create new Data element for our current created radar
            Data radar = new Data();
            radar.Transmitter = transmitter_temp;
            radar.Receiver = receiver_temp;
            radar.Radar = r;
            radar.Location = location_temp;
            radar.ListOfAntennas = AntennaList;
            radar.ListOfModes = Modes;
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Program.data.Remove(sessionID);
            Program.data.Add(sessionID, radar);


            return View(radar);
        }

        public IActionResult DisplayRadar()
        {
            return RedirectToAction("Show", "Radar");
        }

        public IActionResult DisplayReceiver()
        {
            return RedirectToAction("Show", "Receiver");
        }

        public IActionResult DisplayTransmitter()
        {
            return RedirectToAction("Show", "Transmitter");
        }

        public IActionResult DisplayLocation()
        {
            return RedirectToAction("Show", "Location");
        }

        public IActionResult DisplayMode(Guid id)
        {
            return RedirectToAction("Show", "Mode", new { id = id });
        }

        public IActionResult DisplayAntenna(Guid id)
        {
            return RedirectToAction("Show", "Antenna", new { id = id });
        }

    }
}