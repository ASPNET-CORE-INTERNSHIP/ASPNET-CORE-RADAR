using ASPNETAOP.Models;
using ASPNETAOP.Controllers;
using Microsoft.Data.SqlClient;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using ASPNETAOP.Session;

namespace ASPNETAOP.Aspect
{
    // Used for checking the permission of the currently level
    // Requires Admin-level access
    [PSerializable]
    public sealed class IsAuthorizedAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            String sessionID = AppHttpContext.Current.Session.Id;

            //Get the current user from WebApi
            foreach (Pair pair in SessionList.listObject.Pair)
            {
                if (sessionID.Equals(pair.getSessionID()))
                {
                    HttpClient client = new HttpClient();
                    String connectionString = "https://localhost:44316/api/SessionItems/" + pair.getRequestID();
                    Task<SessionItem> userSession = GetJsonHttpClient(connectionString, client); ;

                    //check if the current user has an admin role
                    if (userSession.Result.Roleid != 1) throw new UserPermissionNotEnoughException();
                }
            }
        }

        private static async Task<SessionItem> GetJsonHttpClient(string uri, HttpClient httpClient)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<SessionItem>(uri);
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
