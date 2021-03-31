using System;

namespace ASPNETAOP.Models
{
    public class UserLoginItem
    {
        public long Id { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Usermail { get; set; }
        public string Userpassword { get; set; }
        public int UserRole { get; set; }

        public DateTime LoginDate { get; set; }
        public Guid SessionID { get; set; }
        public int isUserLoggedIn { get; set; }
    }
}
