using ASPNETAOP.Aspect;
using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Runtime.InteropServices;

namespace ASPNETAOP.Controllers
{
    [Guid("54CABEE8-BDED-447D-BBFB-AFB2859797DF")]
    public class AdminRolePermissionController : Controller
    {
        private IConfiguration _configuration;
        public AdminRolePermissionController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [IsAuthenticated]
        [IsAuthorized]
        public IActionResult RolePermission(AdminRolePermission ur)
        {
            TempData["ResultMessage"] = "Admin";

            String connection = _configuration.GetConnectionString("localDatabase");

            if(ur.Roleid != null)
            {
                if (ur.Roleallow != null)
                {
                    using (SqlConnection sqlconn = new SqlConnection(connection))
                    {
                        // Number 2 for the Roleid indicates RegularUser
                        string sqlquery = "insert into RoleAllow(Roleid, Roleallow) values ('" + ur.Roleid + "', '" + ur.Roleallow + "')";
                        using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                        {
                            sqlconn.Open();
                            sqlcomm.ExecuteNonQuery();

                            ViewData["Message"] = "GUID " + ur.Roleallow + " has been allowed for " + ur.Roleid;
                        }
                    }
                }

                if (ur.Roledeny != null)
                {
                    using (SqlConnection sqlconn = new SqlConnection(connection))
                    {
                        // Number 2 for the Roleid indicates RegularUser
                        string sqlquery = "insert into RoleDeny(Roleid, Roledeny) values ('" + ur.Roleid + "', '" + ur.Roledeny + "')";
                        using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                        {
                            sqlconn.Open();
                            sqlcomm.ExecuteNonQuery();

                            ViewData["Message"] = "GUID " + ur.Roledeny + " has been denied for " + ur.Roleid;
                        }
                    }
                }
            }

            return View(ur);
        }

    }
}
