using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace ASPNETAOP.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IConfiguration _configuration;
        public UserRegistrationController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        public IActionResult Create()
        {
            //Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });
            return View();
        }

        //Retrieve the UserID of the given User 
        private int GetUsedID(String Usermail)
        {
            int UserID = -1;

            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select UserID  from AccountInfo where Usermail = '" + Usermail + "' ";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            UserID = reader.GetInt32(0);
                        }
                    }

                    sqlconn.Close();
                }
            }
            return UserID;
        }

        // Add a new entity to the UserRoles with the given user
        private void AddUserRole(int UserID)
        {
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                // Number 2 for the Roleid indicates RegularUser
                string sqlquery = "insert into UserRoles(UserID, Roleid) values ('" + UserID + "', 2)";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }
            }
        }

        [HttpPost]
        public IActionResult Create(UserRegister ur)
        {
            // Necessary to prevent sessionID from changing with every request
            HttpContext.Session.Set("CurrentHTTPSession", new byte[] { 1, 2, 3, 4, 5 });

            //Make the SessionId smaller
            long sessionId = Hash.CurrentHashed(HttpContext.Session.Id);

            String[] registerInfo = { ur.Username, ur.Usermail, ur.Userpassword };
            SendUserRegister(registerInfo, sessionId);

            return View(ur);
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
            var postUser = new UserLoginItem { Id = sessionId, Username = registerInfo[0], Usermail = registerInfo[1], Userpassword = registerInfo[2], isUserLoggedIn = 4 };

            var postResponse = await httpClient.PostAsJsonAsync(uri, postUser);

            postResponse.EnsureSuccessStatusCode();
        }
    }
}
