using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ASPNETAOP.Models;
using ASPNETAOP.Aspect;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;

namespace ASPNETAOP.Controllers
{
    [Guid("18020B1D-DB0B-4600-9443-8ACA5C6CF4FE")]
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        [IsAuthenticated]
        public IActionResult Profile()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });

            //Get the current user from WebApi
            foreach (Pair pair in SessionList.listObject.Pair)
            {
                if (HttpContext.Session.Id.Equals(pair.getSessionID()))
                {
                    HttpClient client = new HttpClient();
                    String connectionString = "https://localhost:44316/api/SessionItems/" + pair.getRequestID();
                    Task<SessionItem> userSession = GetJsonHttpClient(connectionString, client); ;

                    ViewData["message"] = "User name: " + userSession.Result.Username + "\r\n Mail: " + userSession.Result.Usermail;
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult Profile(UserLogin ur)
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });

            //Get the current user from WebApi
            foreach (Pair pair in SessionList.listObject.Pair)
            {
                if (HttpContext.Session.Id.Equals(pair.getSessionID()))
                {
                    HttpClient client = new HttpClient();
                    String connectionString = "https://localhost:44316/api/SessionItems/" + SessionList.listObject.count;
                    Task<SessionItem> userSession = GetJsonHttpClient(connectionString, client); ;

                    ViewData["message"] = "User name: " + userSession.Result.Username + "\r\n Mail: " + userSession.Result.Usermail;
                }
            }

            return View(ur);
        }

        //Used to extract user information from retrieved json file
        private static async Task<SessionItem> GetJsonHttpClient(string uri, HttpClient httpClient)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<SessionItem>(uri);
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
