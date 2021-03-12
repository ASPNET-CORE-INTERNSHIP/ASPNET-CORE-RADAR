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
            Console.WriteLine("5");

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

                    //If the session had been inactive for more than 30 minutes, remove the session
                    if (span.Minutes <= 30)
                    {
                        using (var clientGet = new HttpClient())
                        {
                            clientGet.BaseAddress = new Uri("https://localhost:44316/api/");

                            var deleteTask = clientGet.DeleteAsync("UserLoginItems/" + sessionId);
                            deleteTask.Wait();

                            var result = deleteTask.Result;
                        }

                        String[] loginInfo = { item.Usermail, item.Userpassword };
                        SendUserLogin(loginInfo, Hash.CurrentHashed(AppHttpContext.Current.Session.Id));
                    }
                    else
                    {
                        using (var clientGet = new HttpClient())
                        {
                            clientGet.BaseAddress = new Uri("https://localhost:44316/api/");

                            var deleteTask = clientGet.DeleteAsync("UserLoginItems/" + sessionId);
                            deleteTask.Wait();

                            var result = deleteTask.Result;
                        }

                        throw new UserSessionExpiredException();
                    }
                }
            }
        }


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
    }

    public class UserSessionExpiredException : Exception
    {
        public UserSessionExpiredException() { }

        public UserSessionExpiredException(String message) : base(message) { }
    }
}



