using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class UserProfile
    {
        [Display(Name = "Name")]
        public String Username { get;}

        [Display(Name = "Mail")]
        public String Usermail { get; }
    }
}
