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

            if (userLogin == null || userLogin.Result == null) throw new UserSessionExpired(); 

            //Get the current time
            //Compare it with LoginItem 
            //If the time difference is more than 30 minutes, throw a new UserSessionExpired() error
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

    public class UserSessionExpired : Exception
    {
        public UserSessionExpired() { }

        public UserSessionExpired(String message) : base(message) { }
    }
}
