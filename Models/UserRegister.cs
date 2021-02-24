using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class UserRegister
    {
        [Key]
        public int UserID { get; set; }
        
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter your user name")]
        public String Username { get; set; }
        
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter your email")]
        public String Usermail { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public String Userpassword { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [Compare("Userpassword")]
        public String ConfirmUserpassword { get; set; }
    }
}
