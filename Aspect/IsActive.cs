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

            if(userLogin != null || userLogin.Result != null || userLogin.Result.LoginDate != null)
            {
                //Compare current time with the last accessed time
                DateTime timeAccessed = DateTime.Now;
                TimeSpan span = timeAccessed.Subtract(userLogin.Result.LoginDate);

                //If the session had been inactive for more than 30 minutes, remove the session
                if (span.Minutes >= 0)
                {
                    long sessionId = Hash.CurrentHashed(AppHttpContext.Current.Session.Id);

                    using (var clientGet = new HttpClient())
                    {
                        clientGet.BaseAddress = new Uri("https://localhost:44316/api/");

                        var deleteTask = clientGet.DeleteAsync("UserLoginItems/" + sessionId);
                        deleteTask.Wait();

                        var result = deleteTask.Result;
                    }

                    String[] loginInfo = {userLogin.Result.Usermail, userLogin.Result.Userpassword };
                    SendUserLogin(loginInfo, Hash.CurrentHashed(AppHttpContext.Current.Session.Id));
                }
                else
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
