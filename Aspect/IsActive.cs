using ASPNETAOP.Models;
using ASPNETAOP.Session;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace ASPNETAOP.Aspect
{
    // Used for checking if the user session is active
    [PSerializable]
    public sealed class IsActive : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Aspect regularid, hashed, new " + AppHttpContext.Current.Session.Id + ", " + Hash.CurrentHashed(AppHttpContext.Current.Session.Id));

            HttpClient client = new HttpClient();
            String connectionString = "https://localhost:44316/api/UserLoginItems/" + Hash.CurrentHashed(AppHttpContext.Current.Session.Id);
            Task<UserLoginItem> userLogin = GetJsonHttpClient(connectionString, client); ;

            //Compare current time with the last accessed time
            DateTime timeAccessed = DateTime.Now;
            TimeSpan span = timeAccessed.Subtract(userLogin.Result.LoginDate);

            //If the session had been inactive for more than 30 minutes, remove the session
            if (span.Minutes > 30)
            {
                long sessionId = Hash.CurrentHashed(AppHttpContext.Current.Session.Id);

                using (var clientGet = new HttpClient())
                {
                    clientGet.BaseAddress = new Uri("https://localhost:44316/api/");

                    var deleteTask = clientGet.DeleteAsync("UserLoginItems/" + sessionId);
                    deleteTask.Wait();

                    var result = deleteTask.Result;
                }

                throw new UserSessionExpired();
            }


            //Get the current time
            //Compare it with LoginItem 
            //If the time difference is more than 30 minutes, throw a new UserSessionExpired() error
        }

        //Post request to Web Api with the given user credentials
        public void SendUserRegister(String[] registerInfo, long sessionId)
        {
            HttpClient client = new HttpClient();

            PostUserLogin("https://localhost:44316/api/UserLoginItems", client, registerInfo, sessionId);
        }

        //Helper method for the SendUserLogin
        private static async Task PostUserLogin(string uri, HttpClient httpClient, String[] registerInfo, long sessionId)
        {
            var postUser = new UserLoginItem { Id = sessionId, Username = registerInfo[0], Usermail = registerInfo[1], Userpassword = registerInfo[2], isUserLoggedIn = 0 };

            var postResponse = await httpClient.PostAsJsonAsync(uri, postUser);

            postResponse.EnsureSuccessStatusCode();
        }
        private static async Task<UserLoginItem> GetJsonHttpClient(string uri, HttpClient httpClient)
        {
            try { return await httpClient.GetFromJsonAsync<UserLoginItem>(uri); }
            catch (HttpRequestException) { Console.WriteLine("An error occurred."); }
            catch (NotSupportedException) { Console.WriteLine("The content type is not supported."); }
            catch (JsonException) { Console.WriteLine("Invalid JSON."); }

            return null;
        }
    }
}


public class UserSessionExpired : Exception
{
    public UserSessionExpired() { }

    public UserSessionExpired(String message) : base(message) { }
}
