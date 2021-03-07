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
    public class AdminUserListController : Controller
    {
        private IConfiguration _configuration;
        public AdminUserListController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Index()
        {
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
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
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            String connection = _configuration.GetConnectionString("localDatabase");

            var model = new List<AdminUserActivity>();
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select AI.UserID, AI.Username, AI.Usermail, AR.Rolename from AccountInfo AI, UserRoles UR, AccountRoles AR where UR.UserID=AI.UserID AND AR.Roleid=UR.Roleid;";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var user = new AdminUserActivity();
                            user.UserID = (int)reader["UserID"];
                            user.Username = (string)reader["Username"];
                            user.Usermail = (string)reader["Usermail"];
                            user.Rolename = (string)reader["Rolename"];

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
