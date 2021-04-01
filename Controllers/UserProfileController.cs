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
using System.Collections.Generic;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Profile()
        {
            //Retrieve the user guid from the cookie
            String cookie = Request.Cookies["UserSession"];
            Guid guid = new Guid(cookie);

            //Retrieve the user information from the web server
            List<UserInfoItem> userList = new List<UserInfoItem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44316/api/UserInfoItems/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<UserInfoItem>>(apiResponse);
                }
            }

            //find the account information of the active session
            foreach (UserInfoItem item in userList)
            {
                if (item.Id == guid)
                {
                    //Display basic information about the current user
                    ViewData["message"] = "User name: " + item.Username + "\r\n Mail: " + item.Usermail;

                    //Change the layout according to the user role
                    if (item.UserRole == 1) { TempData["ResultMessage"] = "Admin"; }
                    else { TempData["ResultMessage"] = "Regular"; }
                }
            }

            return View();
        }
    }
}
