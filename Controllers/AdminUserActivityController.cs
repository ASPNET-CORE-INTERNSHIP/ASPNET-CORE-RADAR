using ASPNETAOP.Aspect;
using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace ASPNETAOP.Controllers
{
    [Guid("45EADA4A-CFB8-46A9-8DDB-5A1ACCC89D2A")]
    public class AdminUserActivityController : Controller
    {
        private IConfiguration _configuration;
        public AdminUserActivityController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Index()
        {
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        [IsAuthenticated]
        [IsAuthorized]
        public IActionResult UserActivity()
        {
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            //Changing the layout
            TempData["ResultMessage"] = "Admin";

            String connection = _configuration.GetConnectionString("localDatabase");

            var model = new List<AdminUserActivity>();
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select AI.UserID, AI.Username, AI.Usermail, AR.Rolename, AcS.LoginDate, AcS.IsLoggedIn from AccountInfo AI, AccountSessions AcS, UserRoles UR, AccountRoles AR where AI.Usermail=AcS.Usermail AND UR.UserID=AI.UserID AND AR.Roleid=UR.Roleid;";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var user = new AdminUserActivity();
                            user.UserID = (int)reader["UserID"];
                            user.Username = (string)reader["Username"];
                            user.Usermail = (string)reader["Usermail"];
                            user.Rolename = (string)reader["Rolename"];
                            user.LoginDate = (string)reader["LoginDate"];
                            user.IsLoggedIn = (int)reader["IsLoggedIn"];

                            model.Add(user);
                        }
                        reader.Close();
                    }
                }
            }

            return View(model);
        }
    }
}
