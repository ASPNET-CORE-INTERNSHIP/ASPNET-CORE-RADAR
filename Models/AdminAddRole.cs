using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class AdminAddRole
    {
        [Key]
        public int Roleid { get; set; }
        
        [Display(Name = "Rolename")]
        [Required(ErrorMessage = "Please enter role name")]
        public String Rolename { get; set; }
        
        [Display(Name = "Roleallow")]
        public String Roleallow { get; set; }

        [Display(Name = "Roledeny")]
        public String Roledeny { get; set; }
    }
}
