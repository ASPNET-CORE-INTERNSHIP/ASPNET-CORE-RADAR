using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class AdminRolePermission
    {
        [Key]
        [Display(Name = "Roleid")]
        public int Roleid { get; set; }
        
        [Display(Name = "Roleallow")]
        public String Roleallow { get; set; }

        [Display(Name = "Roledeny")]
        public String Roledeny { get; set; }
    }
}
