using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class SessionItem
    {
        public long Id { get; set; }

        //Information from AccountInfo table in AccountDb
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Usermail { get; set; }

        //Information from UserRoles table in AccountDb
        public int Roleid { get; set; }

    }
}
