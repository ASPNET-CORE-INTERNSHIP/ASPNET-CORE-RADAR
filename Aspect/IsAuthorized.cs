using ASPNETAOP.Models;
using ASPNETAOP.Session;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Net.Http;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ASPNETAOP.Aspect
{
    // Used for checking the permission of the currently logged in user
    // Requires Admin-level access
    [PSerializable]
    public sealed class IsAuthorizedAttribute : OnMethodBoundaryAspect
    {
        private String GUID = "";
        public IsAuthorizedAttribute(string GUID)
        {
            this.GUID = GUID;
        }

        public override async void OnEntry(MethodExecutionArgs args)
        {
            long sessionId = Hash.CurrentHashed(AppHttpContext.Current.Session.Id);

            Console.WriteLine("GUID -> " + GUID);

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
                if (item.Id.Equals(sessionId)) 
                {
                    if (item.UserRole != 1) throw new UserPermissionNotEnoughException();
                }
            }

        }
    }

    // Special type of error to indicate that the current user is not an admin
    public class UserPermissionNotEnoughException : Exception
    {
        public UserPermissionNotEnoughException() { }

        public UserPermissionNotEnoughException(String message) : base(message) { }
    }
}
