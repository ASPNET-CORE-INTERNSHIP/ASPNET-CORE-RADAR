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

        [IsActive]
        public async Task<IActionResult> Profile()
        {
            long sessionId = Hash.CurrentHashed(AppHttpContext.Current.Session.Id);

            //Retrieve the user information
            List<UserLoginItem> userList = new List<UserLoginItem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44316/api/UserLoginItems/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<UserLoginItem>>(apiResponse);
                }
            }

            
            foreach (UserLoginItem item in userList)
            {
                if (item.Id == sessionId)
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
