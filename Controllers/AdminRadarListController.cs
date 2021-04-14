using ASPNETAOP.Aspect;
using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using ASPNETAOP.Session;

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

        [IsAuthenticated]
        [IsAuthorized("151E61F1-1FE3-4FAD-B8EC-9034B676579E")]
        public IActionResult RadarListAsync()
        {
            //Used to display the appropriate layout
            //in the upcoming versions, this part will be updated for the admins with the additional role controls
            TempData["ResultMessage"] = "Regular";

            //Adding a model to a list to access them in the view
            AdminRadarList tempList = new AdminRadarList();
            tempList.transmitterName = Datas.Transmitter.name;
            tempList.transmitter = Datas.Transmitter;
            tempList.radar = Datas.Radar;
            
            var model = new List<AdminRadarList>();

            return View(model);
        }
    }
}
