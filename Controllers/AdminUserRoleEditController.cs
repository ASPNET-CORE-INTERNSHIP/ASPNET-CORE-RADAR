using ASPNETAOP.Aspect;
using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace ASPNETAOP.Controllers
{
    public class AdminUserRoleEditController : Controller
    {
        private IConfiguration _configuration;
        public AdminUserRoleEditController(IConfiguration Configuration) { _configuration = Configuration; }

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
        public IActionResult UserEdit(AdminUserRoleEdit ur)
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            TempData["ResultMessage"] = "Admin";

            String connection = _configuration.GetConnectionString("localDatabase");

            if (ur.Roleid != null)
            {

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
            }

            if(ur.Allow != null)
            {
                using (SqlConnection sqlconn = new SqlConnection(connection))
                {
                    string sqlquery = "Update UserRoles SET UserAllow ='" + ur.Allow + "' WHERE UserID = '" + ur.UserID + "'";
                    using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                    {
                        sqlconn.Open();
                        sqlcomm.ExecuteNonQuery();

                        ViewData["Message"] = "Method " + ur.Allow + " has been allowed for " + ur.UserID;

                        sqlconn.Close();
                    }
                }
            }

            if (ur.Deny != null)
            {
                using (SqlConnection sqlconn = new SqlConnection(connection))
                {
                    string sqlquery = "Update UserRoles SET UserDeny ='" + ur.Deny + "' WHERE UserID = '" + ur.UserID + "'";
                    using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                    {
                        sqlconn.Open();
                        sqlcomm.ExecuteNonQuery();

                        ViewData["Message"] = "Method " + ur.Deny + " has been denied for " + ur.UserID;

                        sqlconn.Close();
                    }
                }
            }

            return View(ur);
        }
    }
}
