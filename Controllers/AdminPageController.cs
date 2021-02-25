using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ASPNETAOP.Aspect;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;

namespace ASPNETAOP.Controllers
{
    [Guid("45EADA4A-CFB8-46A9-8DDB-5A1ACCC89D2A")]
    public class AdminPageController : Controller
    {
        private IConfiguration _configuration;
        public AdminPageController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Index()
        {
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        private void retrieveSessionInfo(int userID)
        {
            foreach (Pair pair in SessionList.listObject.Pair)
            {
                if (pair.getUserID().Equals(userID))
                {
                    HttpClient client = new HttpClient();
                    String connectionString = "https://localhost:44316/api/SessionItems/" + SessionList.listObject.count;
                    Task<SessionItem> userSession = GetJsonHttpClient(connectionString, client); ;

                    //ViewData["message"] = "User name: " + userSession.Result.Username + "\r\n Mail: " + userSession.Result.Usermail;
                }
            }
        }

        //Used to extract user information from retrieved json file
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

        [IsAuthenticated]
        [IsAuthorized]
        public IActionResult UserList()
        {
            HttpContext.Session.Set("What", new byte[] { 1, 2, 3, 4, 5 });

            String connection = _configuration.GetConnectionString("localDatabase");

            var model = new List<AdminPage>();
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select AI.UserID, AI.Username, AI.Usermail, AR.Rolename, AcS.LoginDate, AcS.IsLoggedIn from AccountInfo AI, AccountSessions AcS, UserRoles UR, AccountRoles AR where AI.Usermail=AcS.Usermail AND UR.UserID=AI.UserID AND AR.Roleid=UR.Roleid;";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var user = new AdminPage();
                            user.UserID = (int)reader["UserID"];
                            user.Username = (string)reader["Username"];
                            user.Usermail = (string)reader["Usermail"];
                            user.Rolename = (string)reader["Rolename"];
                            user.LoginDate = (string)reader["LoginDate"];
                            user.IsLoggedIn = (int)reader["IsLoggedIn"];

                            model.Add(user);
                        }
                        reader.Close();
                    }
                }
            }

            return View(model);
        }
    }
}
