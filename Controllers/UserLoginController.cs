using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using ASPNETAOP.Session;
using System.Text.Json;

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

        //Functions below are no longer used
        //Their actions are now handled by the WebServer
        /*
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

        //Used for sessions by sending the current user information to Web Api
        public void SendRequest(String[] ur)
        {
            HttpClient client = new HttpClient();

            //Adds a new Pair class containing the sessionId, Http Request ID and role id of the current user to the arraylist inside SessionList
            SessionList.listObject.Pair.Add(new Pair(HttpContext.Session.Id, SessionList.listObject.count, Int32.Parse(ur[0])));

            PostJsonHttpClient("https://localhost:44316/api/SessionItems", client, ur, HttpContext.Session.Id);
        }

        private static async Task PostJsonHttpClient(string uri, HttpClient httpClient, String[] userInfo, String SessionId)
        {
            var postUser = new SessionItem { Id = Hash.CurrentHashed(SessionId), UserID = Int32.Parse(userInfo[0]), Username = userInfo[1], Usermail = userInfo[2], Roleid = Int32.Parse(userInfo[3]), SessiondId = userInfo[4] };

            var postResponse = await httpClient.PostAsJsonAsync(uri, postUser);

            postResponse.EnsureSuccessStatusCode();
        }
        */

        [HttpPost]
        public IActionResult Login(UserLogin ur)
        {
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            //Make the SessionId smaller
            long sessionId = Hash.CurrentHashed(HttpContext.Session.Id);

            //Send the current login information to the ASPNETAOP-WebServer project
            String[] loginInfo = { ur.Usermail, ur.Userpassword};
            SendUserLogin(loginInfo, sessionId);

            //Get the result of the previous request
            int userLoginStatus = GetUserLogin(sessionId);

            if (userLoginStatus == 1)   //Given password matches the one in the database
            {
                ViewData["Message"] = "Welcome: " + ur.Usermail;
                ViewData["Message"] = "Successfully logged in";
                return RedirectToAction("Profile", "UserProfile", new { ur }); 
            }
            else if(userLoginStatus == 2)   //Given password does not match
            {
                ViewData["Message"] = "Incorrect password";
            }else if(userLoginStatus == 3)  //No user was found with the given mail address
            {
                ViewData["Message"] = "No user with this email address has been found";
                return RedirectToAction("Create", "UserRegistration");
            }

            return View(ur);
        }

        //Post request to Web Api with the given user credentials
        public void SendUserLogin(String[] loginInfo, long sessionId)
        {
            HttpClient client = new HttpClient();

            PostUserLogin("https://localhost:44316/api/UserLoginItems", client, loginInfo, sessionId);
        }

        //Helper method for the SendUserLogin
        private static async Task PostUserLogin(string uri, HttpClient httpClient, String[] loginInfo, long sessionId)
        {
            var postUser = new UserLoginItem { Id = sessionId, Usermail = loginInfo[0], Userpassword = loginInfo[1], isUserLoggedIn = 0 };

            var postResponse = await httpClient.PostAsJsonAsync(uri, postUser);

            postResponse.EnsureSuccessStatusCode();
        }

        //Get Request to check if the login is authorized
        private int GetUserLogin(long sessionId)
        {
            int isUserLoggedIn = 0;

            HttpClient client = new HttpClient();
            String connectionString = "https://localhost:44316/api/UserLoginItems/" + sessionId;
            Task<UserLoginItem> userLogin = GetJsonHttpClient(connectionString, client);

            if (userLogin.Result != null){ isUserLoggedIn = userLogin.Result.isUserLoggedIn; }

            return isUserLoggedIn;
        }

        //Used to extract user information from retrieved json file
        private static async Task<UserLoginItem> GetJsonHttpClient(string uri, HttpClient httpClient)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<UserLoginItem>(uri);
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }
    }
}
