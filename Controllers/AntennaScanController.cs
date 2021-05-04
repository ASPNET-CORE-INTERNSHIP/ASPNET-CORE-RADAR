/*using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NHibernate.Linq;
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
        
        public async Task<IActionResult> NewAntennaScanAsync()
        {
            //New variable consisting of a list of antennas
            //so the user can select antennas which empolys current scan type
            AntennaList alist = new AntennaList();
            List<AntennaScan> selected_list = await _session.AntennaScans.Where(b => b.scan_id.Equals(Data.Scan.ID)).ToListAsync();
            alist.antennas = Data.ListOfAntennas;
            //returning the model for the cshmtl page to access it
            return View(alist);
        }

        public async Task<IActionResult> NewAntennaScanParamAsync(Antenna.AntennaList ascans)
        {
            Data.message = null;
            Data.ListOfAntennas = new List<Antenna>();
            try
            {
                _session.BeginTransaction();

                for (int i = 0; i < ascans.antennas.Count; i++)
                {
                    Antenna antenna = ascans.antennas[i];
                    Data.ListOfAntennas.Add(antenna);
                    AntennaScan ascan = new AntennaScan(antenna.ID, Data.Scan.ID);
                    if (antenna.IsChecked)
                    {
                        await _session.SaveAntennaScan(ascan);
                    }
                    else
                    {
                        await _session.DeleteAntennaScan(ascan);
                    }
                }
                await _session.Commit();
                Data.message = "New Relationship between antennas and scan added";
            }
            catch (Exception e)
            {
                // log exception here
                Data.message = e.Message.ToString() + " Error";
                await _session.Rollback();
            }
            finally
            {
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

        public IActionResult GoBack()
        {
            //After we have done with the ComeFromEdit, we give false (default) to this value
            Data.ComeFromAdd = false;
            return RedirectToAction("BeforeEdit", "Mode", new { id = Data.Mode.ID});
        }
    }
}
            
*/