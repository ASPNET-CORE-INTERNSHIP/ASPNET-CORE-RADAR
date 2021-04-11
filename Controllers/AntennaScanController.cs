using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static ASPNETAOP.Models.Antenna;

namespace ASPNETAOP.Controllers
{
    public class AntennaScanController : Controller
    {
        private readonly NHibernateMapperSession _session;

        public AntennaScanController(NHibernateMapperSession session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult NewAntennaScan()
        {
            //New variable consisting of a list of antennas
            //so the user can select antennas which empolys current scan type
            AntennaList alist = new AntennaList();
            alist.antennas = Datas.ListOfAntennas;
            //returning the model for the cshmtl page to access it
            return View(alist);
        }

        public async Task<IActionResult> NewAntennaScanParamAsync(Antenna.AntennaList ascans)
        {
            List<Antenna> alist = new List<Antenna>();
            try
            {
                _session.BeginTransaction();

                for (int i = 0; i < ascans.antennas.Count; i++)
                {
                    Antenna antenna = ascans.antennas[i];
                    AntennaScan ascan = new AntennaScan(antenna.ID, Datas.Scan.ID);

                    if (antenna.IsChecked)
                    {
                        await _session.SaveAntennaScan(ascan);
                        antenna.IsChecked = true;
                        alist.Add(antenna);
                    }

                    else
                    {
                        await _session.DeleteAntennaScan(ascan);
                        antenna.IsChecked = false;
                        alist.Add(antenna);
                    }
                }
                await _session.Commit();
                ViewData["Message"] = "New Relationship between antennas and scan added";
            }
            catch (Exception e)
            {
                // log exception here
                ViewData["Message"] = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
                Datas.ListOfAntennas = alist;
                _session.CloseTransaction();
            }
            return RedirectToAction("NewAntennaScan", "AntennaScan");
        }

        public IActionResult GoToSubmode()
        {
            return RedirectToAction("NewSubmode", "Submode");
        }

        public IActionResult GoToMode()
        {
            return RedirectToAction("NewMode", "Mode");
        }

        public IActionResult Done()
        {
            return View("~/Views/Shared/done.cshtml");
        }
    }
}
            
