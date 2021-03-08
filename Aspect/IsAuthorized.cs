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
    // Used for checking the permission of the currently logged in user
    // Requires Admin-level access
    [PSerializable]
    public sealed class IsAuthorizedAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            HttpClient client = new HttpClient();
            String connectionString = "https://localhost:44316/api/UserLoginItems/" + Hash.CurrentHashed(AppHttpContext.Current.Session.Id);
            Task<UserLoginItem> userLogin = GetJsonHttpClient(connectionString, client); ;

            if (userLogin == null || userLogin.Result == null || userLogin.Result.Id == null) throw new UserPermissionNotEnoughException(); //check if the current user has an active session
            if (userLogin.Result.UserRole != 1) throw new UserPermissionNotEnoughException();  //check if the user has ann admin level authorization
        }

        private static async Task<UserLoginItem> GetJsonHttpClient(string uri, HttpClient httpClient)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<UserLoginItem>(uri);
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }
    }

    // Special type of error to indicate that the current user is not an admin
    public class UserPermissionNotEnoughException : Exception
    {
        public UserPermissionNotEnoughException() { }

        public UserPermissionNotEnoughException(String message) : base(message) { }
    }
}
