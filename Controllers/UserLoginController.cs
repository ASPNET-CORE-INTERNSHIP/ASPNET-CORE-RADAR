using ASPNETAOP.Models;
using ASPNETAOP.Session;
using ASPNETAOP.Aspect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace ASPNETAOP.Controllers
{
    public class UserLoginController : Controller
    {
        private IConfiguration _configuration;
        public UserLoginController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(UserLogin ur)
        {
            //Necessary to prevent HttpContext.Session.Id from changing with every request
            HttpContext.Session.SetString("Session", new Guid().ToString());

            //Check if the necessary boxes are filled
            //if so, send a POST Request to the Web Api
            if (ur.Usermail != null && ur.Userpassword != null) {
                String[] loginInfo = { ur.Usermail, ur.Userpassword };
                SendUserLogin(loginInfo, Hash.CurrentHashed(AppHttpContext.Current.Session.Id));

                //if user credentials are correct, a new entitiy will be added to ActivitySession table
                return RedirectToAction("Profile", "UserProfile");
            }

            return View(ur);
        }

        [IsAuthenticated]
        public IActionResult ConfirmAction(UserLogin ur)
        {
            return RedirectToAction("Profile", "UserProfile");
        }

        //Post request to Web Api with the given user credentials
        public async void SendUserLogin(String[] loginInfo, long sessionId)
        {
            HttpClient client = new HttpClient();
            var postUser = new UserLoginItem { Id = sessionId, Usermail = loginInfo[0], Userpassword = loginInfo[1], isUserLoggedIn = 0 };
            var postResponse = await client.PostAsJsonAsync("https://localhost:44316/api/UserLoginItems", postUser);
            postResponse.EnsureSuccessStatusCode();
        }
    }
}
