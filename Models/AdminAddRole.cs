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
    }
}
