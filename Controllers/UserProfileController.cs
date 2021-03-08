using ASPNETAOP.Aspect;
using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace ASPNETAOP.Controllers
{
    [Guid("18020B1D-DB0B-4600-9443-8ACA5C6CF4FE")]
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        [IsAuthenticated]
        public IActionResult Profile()
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            long sessionId = Hash.CurrentHashed(HttpContext.Session.Id);

            // Retrieve the user information from the ASPNETAOP-WebServer
            HttpClient client = new HttpClient();
            String connectionString = "https://localhost:44316/api/UserLoginItems/" + sessionId;
            Task<UserLoginItem> userProfile = GetJsonHttpClient(connectionString, client); ;

            int userRole = GetUserRole(sessionId);

            //Change the layout according to user
            if (userRole == 1) { TempData["ResultMessage"] = "Admin"; }
            else { TempData["ResultMessage"] = "Regular"; }

            ViewData["message"] = "User name: " + userProfile.Result.Username + "\r\n Mail: " + userProfile.Result.Usermail;

            return View();
        }

        [HttpPost]
        public IActionResult Profile(UserLogin ur)
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            long sessionId = Hash.CurrentHashed(HttpContext.Session.Id);

            HttpClient client = new HttpClient();
            String connectionString = "https://localhost:44316/api/UserLoginItems/" + sessionId;
            Task<UserLoginItem> userProfile = GetJsonHttpClient(connectionString, client); ;

            ViewData["message"] = "User name: " + userProfile.Result.Username + "\r\n Mail: " + userProfile.Result.Usermail;

            return View(ur);
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

        // Used to extract user information from retrieved json file
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
