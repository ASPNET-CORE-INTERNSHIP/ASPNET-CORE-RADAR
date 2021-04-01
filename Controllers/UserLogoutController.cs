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
            return View();
        }

        public IActionResult Logout()
        {
            //Retrieve the user guid from the cookie
            String cookie = Request.Cookies["UserSession"];
            Guid guid = new Guid(cookie);

            //sends a Delete HTTP request
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44316/api/");

                var deleteTask = client.DeleteAsync("UserInfoItems/" + guid);
                deleteTask.Wait();
            }

            //deletes the cookie
            Response.Cookies.Delete("UserSession");

            return RedirectToAction("Login", "UserLogin");
        }
    }
}
