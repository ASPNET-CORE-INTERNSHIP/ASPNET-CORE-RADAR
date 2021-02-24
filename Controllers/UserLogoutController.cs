using ASPNETAOP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace ASPNETAOP.Controllers
{
    public class UserLogoutController : Controller
    {
        private IConfiguration _configuration;
        public UserLogoutController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Logout()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });

            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "UPDATE AccountSessions SET IsLoggedIn = 0 WHERE IsLoggedIn = 1;";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }
            }
            
            //removes the records of the currently logged in user from the global currentUserInfo array
            for(int i=0; i<3; i++)
            {
                Models.CurrentUser.currentUser.CurrentUserInfo[i] = null;
            }

            //sends a Delete HTTP request
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44316/api/");

                foreach (Pair pair in SessionList.listObject.Pair)
                {
                    if (HttpContext.Session.Id.Equals(pair.getSessionID()))
                    {
                        var deleteTask = client.DeleteAsync("SessionItems/" + pair.getRequestID());
                        deleteTask.Wait();

                        var result = deleteTask.Result;

                        if (!result.IsSuccessStatusCode) {}

                        SessionList.listObject.Pair.Remove(pair);
                        break;
                    }
                }
            }

            return RedirectToAction("Login", "UserLogin");
        }
    }
}
