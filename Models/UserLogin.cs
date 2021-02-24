using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class UserLogin
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter your email")]
        public String Usermail { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public String Userpassword { get; set; }
    }
}

