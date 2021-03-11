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

        //0 - Login Request
        //4 - User Registration Request
        public int isUserLoggedIn { get; set; }
    }
}
