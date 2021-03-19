using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class RolePermission
    {
        public String Roleallow { get; set; }

        public String Roledeny { get; set; }
    }
}

