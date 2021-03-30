using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace ASPNETAOP.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IConfiguration _configuration;
        public UserRegistrationController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserRegister ur)
        {
            //Make the SessionId smaller
            long sessionId = Hash.CurrentHashed(AppHttpContext.Current.Session.Id);

            String[] registerInfo = { ur.Username, ur.Usermail, ur.Userpassword };
            SendUserRegister(registerInfo, sessionId);

            return View(ur);
        }
        
        //Post request to Web Api with the user information
        public async void SendUserRegister(String[] registerInfo, long sessionId)
        {
            HttpClient client = new HttpClient();
            var postUser = new UserRegisterItem { Id = sessionId, Username = registerInfo[0], Usermail = registerInfo[1], Userpassword = registerInfo[2] };
            var postResponse = await client.PostAsJsonAsync("https://localhost:44316/api/UserRegisterItems", postUser);
            postResponse.EnsureSuccessStatusCode();
        }
    }
}
