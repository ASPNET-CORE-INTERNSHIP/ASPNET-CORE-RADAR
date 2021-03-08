using ASPNETAOP.Aspect;
using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace ASPNETAOP.Controllers
{
    public class AdminAddRoleController : Controller
    {
        private IConfiguration _configuration;
        public AdminAddRoleController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        [IsAuthenticated]
        [IsAuthorized]
        public IActionResult AddRole(AdminAddRole ur)
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            TempData["ResultMessage"] = "Admin";

            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                // Number 2 for the Roleid indicates RegularUser
                string sqlquery = "insert into AccountRoles(Rolename, Roleallow, Roledeny) values ('" + ur.Rolename + "', '" + ur.Roleallow + "', '" + ur.Roledeny + "')";
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
