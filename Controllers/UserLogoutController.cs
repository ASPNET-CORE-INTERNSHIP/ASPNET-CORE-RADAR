using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;

namespace ASPNETAOP.Controllers
{
    public class UserLogoutController : Controller
    {
        private IConfiguration _configuration;
        public UserLogoutController(IConfiguration Configuration) { _configuration = Configuration; }

        public IActionResult Index()
        {
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        public IActionResult Logout()
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            long sessionId = Hash.CurrentHashed(HttpContext.Session.Id);

            // sends a Delete HTTP request
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44316/api/");

                var deleteTask = client.DeleteAsync("UserLoginItems/" + sessionId);
                deleteTask.Wait();

                var result = deleteTask.Result;
            }

            return RedirectToAction("Login", "UserLogin");
        }
    }
}
