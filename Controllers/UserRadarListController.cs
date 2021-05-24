using ASPNETAOP.Aspect;
using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using ASPNETAOP.Session;
using NHibernate.Linq;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ASPNETAOP.Controllers
{
    [Guid("151E61F1-1FE3-4FAD-B8EC-9034B676579E")]
    public class UserRadarListController : Controller
    {
        private readonly Session.NHibernateMapperSession _session;
        private String sessionID_s;
        private Guid sessionID;

        public UserRadarListController(NHibernateMapperSession session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async System.Threading.Tasks.Task<IActionResult> RadarList(String message)
        {
            List<Radar> RadarList = await _session.Radars.ToListAsync();

            var model = new List<RadarInfo>();

            for (int i = 0; i < RadarList.Count; i++)
            {
                Transmitter transmitter_temp = await _session.Transmitters.Where(b => b.ID.Equals(RadarList[i].transmitter_id)).FirstOrDefaultAsync();
                Receiver receiver_temp = await _session.Receivers.Where(b => b.ID.Equals(RadarList[i].receiver_id)).FirstOrDefaultAsync();
                Location location_temp = await _session.Location.Where(b => b.ID.Equals(RadarList[i].location_id)).FirstOrDefaultAsync();
                RadarInfo temp = new RadarInfo(RadarList[i], transmitter_temp, receiver_temp, location_temp);
                model.Add(temp);
            }
            //get session id (we will use it when updating data and handling errors)
            sessionID_s = HttpContext.Session.GetString("Session");
            sessionID = Guid.Parse(sessionID_s);
            Data current = new Data();
            Program.data.TryGetValue(sessionID, out current);

            if(!String.IsNullOrEmpty(message))
                ViewData["message"] = message;
            if (current != null && !String.IsNullOrEmpty(current.message))
                ViewData["message"] = current.message;
            return View(model);
        }

        public IActionResult GoToDisplayPage(Guid id)
        {
            return RedirectToAction("Display", "UserRadarScreen", new { id = id });
        }
    }
}
