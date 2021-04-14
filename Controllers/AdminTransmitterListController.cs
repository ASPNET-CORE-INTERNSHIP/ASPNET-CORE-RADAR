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
    public class AdminTransmitterListController : Controller
    {
        private readonly Session.NHibernateMapperSession _session;

        public AdminTransmitterListController(NHibernateMapperSession session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }   

        public IActionResult TransmitterList()
        {
            //Used to display the appropriate layout
            //in the upcoming versions, this part will be updated for the admins with the additional role controls
            TempData["ResultMessage"] = "Regular";

            List<object> result = _session.GetTransmitterName();

            //Adding a model to a list to access them in the view
            var model = new List<AdminTransmitterList>();

            for (int i=0; i<result.Count; i++)
            {
                AdminTransmitterList tempList = new AdminTransmitterList();
                tempList.transmitterName = result[i];
                Console.WriteLine(i + "th transmitter is " + result[i]);
                model.Add(tempList);
            }


            return View(model);
        }

        public IActionResult TransmitterEdit(AdminTransmitterEdit ae) 
        {
            _session.UpdateTransmitter(ae.name, ae.newName, ae.max_frequency, ae.min_frequency, ae.power);

            return View(ae);
        }
    }
}
