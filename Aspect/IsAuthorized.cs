using ASPNETAOP.Models;
using ASPNETAOP.Session;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Net.Http;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data.SqlClient;

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
                    String connection = "Server=DESKTOP-II1M7LK;Database=AccountDb;Trusted_Connection=True;MultipleActiveResultSets=true";

                    bool isAllowed = false;
                    
                    using (SqlConnection sqlconn = new SqlConnection(connection))
                    {
                        string sqlquery = "SELECT RA.Roleallow, RD.Roledeny FROM RoleAllow RA, RoleDeny RD, UserRoles UR WHERE RA.Roleid = UR.Roleid AND RD.Roleid = UR.Roleid AND UR.UserID = '" + item.UserID + "'";
                        using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                        {
                            sqlconn.Open();
                            SqlDataReader reader = sqlcomm.ExecuteReader();

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var user = new RolePermission();

                                    String Roleallow = (string)reader["Roleallow"];
                                    String Roledeny = (string)reader["Roledeny"];

                                    Console.WriteLine("Allowed: " + Roleallow);
                                    Console.WriteLine("Denied: " + Roledeny);

                                    Console.WriteLine("GUID: " + GUID);

                                    if (Roleallow.Equals(GUID)) isAllowed = true;
                                    if (Roledeny.Equals(GUID)) throw new UserPermissionNotEnoughException();
                                }
                                reader.Close();
                            }
                        }
                    }

                    if (!isAllowed) throw new UserPermissionNotEnoughException();
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
