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
            Guid id_receiver = receiver_temp.ID;
            List<Antenna> AntennaList = await _session.Antennas.Where(b => (b.receiver_id!= null && b.receiver_id.Value.Equals(receiver_temp.ID)) || (b.transmitter_id != null && b.transmitter_id.Value.Equals(transmitter_temp.ID))).ToListAsync();
            List<Mode> ModeList = await _session.Modes.Where(b => b.radar_id.Equals(r.ID)).ToListAsync();
            
            RadarInfo radar = new RadarInfo(r, transmitter_temp, receiver_temp, location_temp);
            radar.ListOfAntennas = AntennaList;
            radar.ListOfModes = ModeList;

            //FILL THE DATA MODEL WITH NECESSARY VALUES so we can use DATA model in editing process
            Data.Transmitter = transmitter_temp;
            Data.Receiver = receiver_temp;
            Data.ListOfAntennas = AntennaList;

            return View(radar);
        }

        public IActionResult RadarEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Radar", new { id = id });
        }

        public IActionResult ReceiverEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Receiver", new { id = id });
        }

        public IActionResult TransmitterEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Transmitter", new { id = id });
        }

        public IActionResult LocationEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Location", new { id = id });
        }

        public IActionResult ModeEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Mode", new { id = id });
        }

        public IActionResult AntennaEdit(Guid id)
        {
            return RedirectToAction("BeforeEdit", "Antenna", new { id = id });
        }

        public async System.Threading.Tasks.Task<IActionResult> AddAntennaAsync(Guid id)
        {
            return RedirectToAction("AddNewAntenna", "Antenna", new { id = id});
        }

    }
}