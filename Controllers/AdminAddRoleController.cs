using ASPNETAOP.Aspect;
using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Runtime.InteropServices;

namespace ASPNETAOP.Controllers
{
    [Guid("1371A9F7-25FC-4EDC-B82B-ADB3CCEE485B")]
    public class AdminAddRoleController : Controller
    {
        private IConfiguration _configuration;
        public AdminAddRoleController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [IsAuthenticated]
        [IsAuthorized("1371A9F7-25FC-4EDC-B82B-ADB3CCEE485B")]
        public IActionResult AddRole(AdminAddRole ur)
        {
            TempData["ResultMessage"] = "Admin";

            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "insert into AccountRoles(Rolename) values ('" + ur.Rolename + "')";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();

                    ViewData["Message"] = "Role " + ur.Rolename + " has been added";
                }
            }

            return View(ur);
        }

    }
}
