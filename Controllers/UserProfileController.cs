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
            return View();
        }

        [IsAuthenticated]
        public IActionResult Profile()
        {
            long sessionId = Hash.CurrentHashed(AppHttpContext.Current.Session.Id);

            // Retrieve the user information from the ASPNETAOP-WebServer
            HttpClient client = new HttpClient();
            String connectionString = "https://localhost:44316/api/UserLoginItems/" + sessionId;
            Task<UserLoginItem> userProfile = GetJsonHttpClient(connectionString, client); ;

            if(userProfile != null)
            {
                int userRole = GetUserRole(sessionId);

                //Change the layout according to user role
                if (userRole == 1) { TempData["ResultMessage"] = "Admin"; }
                else { TempData["ResultMessage"] = "Regular"; }

                ViewData["message"] = "User name: " + userProfile.Result.Username + "\r\n Mail: " + userProfile.Result.Usermail;
            }
            

            return View();
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
            try { return await httpClient.GetFromJsonAsync<UserLoginItem>(uri); }
            catch (HttpRequestException) { Console.WriteLine("An error occurred."); }
            catch (NotSupportedException) { Console.WriteLine("The content type is not supported."); }
            catch (JsonException) { Console.WriteLine("Invalid JSON."); }

            return null;
        }
    }
}
