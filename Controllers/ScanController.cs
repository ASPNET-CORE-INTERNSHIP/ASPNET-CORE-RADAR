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

namespace ASPNETAOP.Controllers
{
    public class ScanController : Controller
    {
        private readonly NHibernateMapperSession _session;

        public ScanController(NHibernateMapperSession session)
        {
            _session = session;
        }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewScan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewScanAsync(Scan scan)
        {
            Guid key = Guid.NewGuid();
            Scan s = new Scan(key, scan.name, scan.type, scan.main_aspect, scan.scan_angle, scan.scan_rate, scan.hits_per_scan);
            Data.Scan = s;

            Guid key_submode = Guid.NewGuid();
            Submode sbm = new Submode(key_submode, Data.Submode.name, Data.Mode.ID, Data.Submode.PW, Data.Submode.PRI, Data.Submode.min_frequency, Data.Submode.max_frequency, key);
            Data.Submode = sbm;

            try
            {
                _session.BeginTransaction();
                await _session.SaveScan(s);
                await _session.SaveSubMode(sbm);
                await _session.Commit();
                Data.message = "Both records (Submode and Scan) added to db";
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

    }
}
