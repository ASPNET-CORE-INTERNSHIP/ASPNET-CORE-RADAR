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

namespace ASPNETAOP.Controllers
{
    [Guid("151E61F1-1FE3-4FAD-B8EC-9034B676579E")]
    public class AdminRadarListController : Controller
    {
        private readonly Session.NHibernateMapperSession _session;

        public AdminRadarListController(NHibernateMapperSession session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async System.Threading.Tasks.Task<IActionResult> RadarListAsync()
        {
            //Used to display the appropriate layout
            //in the upcoming versions, this part will be updated for the admins with the additional role controls
            TempData["ResultMessage"] = "Regular";

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
            Console.WriteLine("**********************");
            return View(model);
        }

        public IActionResult RadarEdit(Guid id)
        {
            Console.WriteLine(id);
            return RedirectToAction( "Edit", "EditRadar", new { id = id });
        }
    }
}
