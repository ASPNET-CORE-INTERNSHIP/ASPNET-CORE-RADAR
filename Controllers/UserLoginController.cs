using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

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

                int userRole = GetUserRole(sessionId);

                //Change the layout according to user
                if (userRole == 1) { TempData["ResultMessage"] = "Admin"; }
                else { TempData["ResultMessage"] = "Regular"; }

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

        private int GetUserRole(long sessionId)
        {
            int UserRole = 0;

            HttpClient client = new HttpClient();
            String connectionString = "https://localhost:44316/api/UserLoginItems/" + sessionId;
            Task<UserLoginItem> userLogin = GetJsonHttpClient(connectionString, client);

            if (userLogin.Result != null) { UserRole = userLogin.Result.UserRole; }

            return UserRole;
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
