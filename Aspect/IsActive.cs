using ASPNETAOP.Models;
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
    // Used for checking if the user session is active
    [PSerializable]
    public sealed class IsActive : OnMethodBoundaryAspect
    {
        public override async void OnEntry(MethodExecutionArgs args)
        {
            long sessionId = Hash.CurrentHashed(AppHttpContext.Current.Session.Id);


            List<UserLoginItem> reservationList = new List<UserLoginItem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44316/api/UserLoginItems/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservationList = JsonConvert.DeserializeObject<List<UserLoginItem>>(apiResponse);
                }
            }

            foreach (UserLoginItem item in reservationList)
            {
                if (item.Id == sessionId)
                {
                    //Compare current time with the last accessed time
                    DateTime timeAccessed = DateTime.Now;
                    TimeSpan span = timeAccessed.Subtract(item.LoginDate);

                    using (var clientGet = new HttpClient())
                    {
                        clientGet.BaseAddress = new Uri("https://localhost:44316/api/");

                        var deleteTask = clientGet.DeleteAsync("UserLoginItems/" + sessionId);
                        deleteTask.Wait();

                        var result = deleteTask.Result;
                    }

                    //If the session has been only active for less than 30 minutes, update the last accessed item
                    if (span.Minutes <= 30)
                    {
                        String[] loginInfo = { item.Usermail, item.Userpassword };
                        SendUserLogin(loginInfo, sessionId);
                    }
                    //otherwise, throw an appropiote exception
                    else{ throw new UserSessionExpiredException(); }
                }
            }
        }

        public async void SendUserLogin(String[] loginInfo, long sessionId)
        {
            HttpClient client = new HttpClient();

            var postUser = new UserLoginItem { Id = sessionId, Usermail = loginInfo[0], Userpassword = loginInfo[1], isUserLoggedIn = 0 };
            var postResponse = await client.PostAsJsonAsync("https://localhost:44316/api/UserLoginItems", postUser);
            postResponse.EnsureSuccessStatusCode();
        }
    }

    public class UserSessionExpiredException : Exception
    {
        public UserSessionExpiredException() { }

        public UserSessionExpiredException(String message) : base(message) { }
    }
}



