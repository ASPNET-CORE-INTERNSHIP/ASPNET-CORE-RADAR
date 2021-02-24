using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using Microsoft.Extensions.Configuration;

namespace ASPNETAOP.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IConfiguration _configuration;
        public UserRegistrationController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        public IActionResult Create()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserRegister ur)
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });

            //Add new user to the database
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "insert into AccountInfo(Username, Usermail, Userpassword) values ('" + ur.Username + "', '" + ur.Usermail + "', '" + ur.Userpassword + "' )";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                    ViewData["Message"] = "New User "+ur.Username+ " is saved successfully";
                }
            }

            return View(ur);
        }
    }
}
