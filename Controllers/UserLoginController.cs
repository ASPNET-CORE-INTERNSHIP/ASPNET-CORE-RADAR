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
using System.Web;
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

        //First method
        public IActionResult Login(UserLogin userCredentials)
        {
            //Necessary to prevent HttpContext.Session.Id from changing with every request
            Guid guid = Guid.NewGuid();
            HttpContext.Session.SetString("Session", guid.ToString());

            //Create a cookie
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(30);
            Response.Cookies.Append("UserSession", guid.ToString(), option);

            //Check if the necessary boxes are filled
            //if so, send a POST Request to the Web Api
            if (userCredentials.Usermail != null && userCredentials.Userpassword != null) {
                String[] userCredentialsArray = { userCredentials.Usermail, userCredentials.Userpassword};

                SendUserLogin(userCredentialsArray, Hash.CurrentHashed(AppHttpContext.Current.Session.Id), guid);
                return RedirectToAction("Profile", "UserProfile");
            }

            return View(userCredentials);
        }

        //Additional method with aspect to ensure that user session has been added by Web Server
        //User is redirected to profile page only if authentication by IsAuthenticated aspect is succesfful
        [IsAuthenticated]
        public IActionResult ConfirmAction()
        {
            return RedirectToAction("Profile", "UserProfile");
        }

        //Post request to Web Api with the given user credentials
        public async void SendUserLogin(String[] userCredentials, long sessionId, Guid guid)
        {
            HttpClient client = new HttpClient();
            var postUser = new UserLoginItem { Id = sessionId, Usermail = userCredentials[0], Userpassword = userCredentials[1], SessionID = guid};
            var postResponse = await client.PostAsJsonAsync("https://localhost:44316/api/UserLoginItems", postUser);
            postResponse.EnsureSuccessStatusCode();
        }
    }
}
