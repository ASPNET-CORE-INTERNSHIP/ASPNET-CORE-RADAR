using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
