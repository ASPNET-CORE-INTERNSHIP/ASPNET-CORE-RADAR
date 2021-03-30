using System;

namespace ASPNETAOP.Models
{
    public class UserRegisterItem
    {
        public long Id { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Usermail { get; set; }
        public string Userpassword { get; set; }
    }
}
