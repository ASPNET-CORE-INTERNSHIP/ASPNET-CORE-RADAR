﻿using ASPNETAOP.Models;
using ASPNETAOP.Session;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ASPNETAOP.Aspect
{
    // Used for checking if the user has a session
    [PSerializable]
    public sealed class IsAuthenticatedAttribute : OnMethodBoundaryAspect
    {
 
        public override async void OnEntry(MethodExecutionArgs args)
        {
            String cookie = AppHttpContext.Current.Request.Cookies["UserSession"];
            if (cookie == null || cookie.Length == 0) throw new UserNotLoggedInException();

            /*
            if (cookie == null) { Console.WriteLine("COOKIE -> " + cookie); }
            else { Console.WriteLine("COOKIE NOT NULL"); }

            List<UserLoginItem> reservationList = new List<UserLoginItem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44316/api/UserLoginItems/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservationList = JsonConvert.DeserializeObject<List<UserLoginItem>>(apiResponse);
                }
            }

            bool isPresent = false;

            foreach (UserLoginItem item in reservationList)
            {
                if (item.Id.Equals(sessionId)){ isPresent = true; }
            }

            if(!isPresent) throw new UserNotLoggedInException(); //check if the current user has an active session
            */
        }
    }

    public class UserNotLoggedInException : Exception
    {
        public UserNotLoggedInException() {}

        public UserNotLoggedInException(String message) : base(message) {}
    }
}
