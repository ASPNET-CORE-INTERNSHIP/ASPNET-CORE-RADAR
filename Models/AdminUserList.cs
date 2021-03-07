using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AdminUserList
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Usermail { get; set; }
        public string Country { get; set; }

        public string City { get; set; }
        public string Rolename { get; set; }

    }
}
