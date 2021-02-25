using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AdminPage
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Usermail { get; set; }
        public string Rolename { get; set; }
        public string LoginDate { get; set; }
        public int IsLoggedIn { get; set; }

    }
}
