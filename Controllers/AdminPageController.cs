using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ASPNETAOP.Aspect;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace ASPNETAOP.Controllers
{
    [Guid("45EADA4A-CFB8-46A9-8DDB-5A1ACCC89D2A")]
    public class AdminPageController : Controller
    {
        private IConfiguration _configuration;
        public AdminPageController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Index()
        {
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        [IsAuthenticated]
        [IsAuthorized]
        public IActionResult UserList()
        {
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });

            String connection = _configuration.GetConnectionString("localDatabase");

            var model = new List<AdminPage>();
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select AI.Username, AI.Usermail, AcS.LoginDate, AcS.IsLoggedIn from AccountInfo AI, AccountSessions AcS WHERE AI.Usermail=AcS.Usermail";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var user = new AdminPage();
                            user.Username = (string)reader["Username"];
                            user.Usermail = (string)reader["Usermail"];
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
