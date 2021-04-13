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
            TempData["ResultMessage"] = "Regular";

            var model = new List<AdminRadarList>();

            return View(model);
        }
    }
}
