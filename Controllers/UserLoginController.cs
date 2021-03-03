using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;

namespace ASPNETAOP.Controllers
{
    public class UserLoginController : Controller
    {
        private IConfiguration _configuration;
        public UserLoginController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Index()
        {
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        public IActionResult Login()
        {
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }


        //Used to log user activies
        public void SaveCookie(UserLogin ur)
        {
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                DateTime thisDay = DateTime.Now;
                //Date format is 30/3/2020 12:00 AM
                //Number at the end indicates 0 for Logged Out & 1 for Logged in
                string sqlQuerySession = "insert into AccountSessions(Usermail, LoginDate, IsLoggedIn) values ('" + ur.Usermail + "', '" + thisDay.ToString("g") + "', 1 )";
                using (SqlCommand sqlcommCookie = new SqlCommand(sqlQuerySession, sqlconn))
                {
                    sqlconn.Open();
                    sqlcommCookie.ExecuteNonQuery();
                }
            }
        }

        // Used for sessions by sending the current user information to Web Api
        public void SendRequest(String[] ur)
        {
            HttpClient client = new HttpClient();

            // Adds a new Pair class containing the sessionId, Http Request ID and role id of the current user to the arraylist inside SessionList
            SessionList.listObject.Pair.Add(new Pair(HttpContext.Session.Id, SessionList.listObject.count, Int32.Parse(ur[4])));

            PostJsonHttpClient("https://localhost:44316/api/SessionItems", client, ur);
        }

        private static async Task PostJsonHttpClient(string uri, HttpClient httpClient, String[] userInfo)
        {
            var postUser = new SessionItem { Id = SessionList.listObject.count++, UserID = Int32.Parse(userInfo[0]), Username = userInfo[1], Usermail = userInfo[2], Roleid = Int32.Parse(userInfo[3]) };

            var postResponse = await httpClient.PostAsJsonAsync(uri, postUser);

            postResponse.EnsureSuccessStatusCode();
        }

        //When user is redirected to login page, user's info is 
        //1. saved in the database (for logging)
        //2. sent to ASPNETAOP-WebServer (for sessions)
        [HttpPost]
        public IActionResult Login(UserLogin ur)
        {
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            /*
             * String[] UserLoggedIn = { userID, username, usermail, roleID, userID };
             * SendRequest(UserLoggedIn);
             * 
             * 
             * 
             * 
             */


            String connection = _configuration.GetConnectionString("localDatabase");

            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select AI.Userpassword, AI.UserID, AI.Username, UR.Roleid  from AccountInfo AI, UserRoles UR where AI.UserID = UR.UserID AND AI.Usermail = '" + ur.Usermail + "' ";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0).Equals(ur.Userpassword))
                            {
                                ViewData["Message"] = "Welcome: " + ur.Usermail;

                                String userID = reader.GetInt32(1).ToString();  
                                String username = reader.GetString(2);        
                                String usermail = ur.Usermail;
                                String roleID = reader.GetInt32(3).ToString();

                                reader.Close();
                                sqlconn.Close();

                                // 1. Stores user activity in AccountDb
                                SaveCookie(ur);

                                // 2. Sends the user information to ASPNETAOP-WebServer for sessions
                                String[] UserLoggedIn = { userID, username, usermail, roleID, userID };
                                SendRequest(UserLoggedIn);

                                ViewData["Message"] = "Successfully logged in";
                                reader.Close();

                                return RedirectToAction("Profile", "UserProfile", new { ur });
                            }
                            else
                            {
                                ViewData["Message"] = "Incorrect password";
                            }
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "No user with this email address has been found";
                        reader.Close();

                        return RedirectToAction("Create", "UserRegistration");
                    }
                    reader.Close();
                }
            }
            return View(ur);
        }
    }
}
