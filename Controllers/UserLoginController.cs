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

        [IsAuthenticated]
        public IActionResult ConfirmAction(UserLogin ur)
        {
            return RedirectToAction("Profile", "UserProfile", new { ur });
        }

        public IActionResult Login(UserLogin ur)
        {
            /*
               Send the user mail & password with Http Post Request 
               -> WebServer will add a new entitty to AccountSessions if the given information is correct
               Call the method above
            */

            //Make the SessionId smaller
            long sessionId = Hash.CurrentHashed(AppHttpContext.Current.Session.Id);
            HttpContext.Session.SetString("SessID", new Guid().ToString());
            Console.WriteLine("Login regularid, hashed, new " + AppHttpContext.Current.Session.Id + ", " + sessionId + ", " + HttpContext.Session.Id);

            if (ur.Usermail != null && ur.Userpassword != null) {
                //Send the current login information to the ASPNETAOP-WebServer project
                String[] loginInfo = { ur.Usermail, ur.Userpassword };
                SendUserLogin(loginInfo, sessionId);

                return RedirectToAction("ConfirmAction", "UserLogin", new { ur });
            }

               

            return View(ur);
        }

        //Post request to Web Api with the given user credentials
        public async void SendUserLogin(String[] loginInfo, long sessionId)
        {
            HttpClient client = new HttpClient();

            await PostUserLogin("https://localhost:44316/api/UserLoginItems", client, loginInfo, sessionId);
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
            try{ return await httpClient.GetFromJsonAsync<UserLoginItem>(uri); }
            catch (HttpRequestException){ Console.WriteLine("An error occurred."); }
            catch (NotSupportedException){ Console.WriteLine("The content type is not supported."); }
            catch (JsonException){ Console.WriteLine("Invalid JSON."); }

            return null;
        }
    }
}
