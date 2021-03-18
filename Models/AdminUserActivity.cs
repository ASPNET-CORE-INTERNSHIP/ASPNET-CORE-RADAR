using System;

namespace ASPNETAOP.Models
{
    public class AdminUserActivity
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Usermail { get; set; }
        public string Rolename { get; set; }
        public DateTime LoginDate { get; set; }
        public int IsLoggedIn { get; set; }

    }
}
