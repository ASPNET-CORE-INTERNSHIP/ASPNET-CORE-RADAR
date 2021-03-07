using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AdminUserRoleEdit
    {
        public int UserID { get; set; }
        public int Roleid { get; set; }

        public String Allow { get; set; }

        public String Deny { get; set; }
    }
}
