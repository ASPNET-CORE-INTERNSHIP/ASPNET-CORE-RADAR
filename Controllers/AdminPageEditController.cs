using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAOP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Aspect;

namespace ASPNETAOP.Controllers
{
    public class AdminPageEditController : Controller
    {
        private IConfiguration _configuration;
        public AdminPageEditController(IConfiguration Configuration) { _configuration = Configuration; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminPage()
        {
            return RedirectToAction("Admin", "UserList");
        }

        [IsAuthenticated]
        [IsAuthorized]
        public IActionResult UserEdit(AdminPageEdit ur)
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });

            //Add a new user to the database
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "Update UserRoles SET Roleid ='" + ur.Roleid + "' WHERE UserID = '" + ur.UserID + "'";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();

                    ViewData["Message"] = "Role id for " + ur.UserID + " has been changed to " + ur.Roleid;

                    sqlconn.Close();
                }
            }
            return View(ur);
        }
    }
}
