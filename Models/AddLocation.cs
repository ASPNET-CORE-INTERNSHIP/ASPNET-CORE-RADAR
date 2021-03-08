using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AddLocation
    {

        [Key]
        public String ID { get; set; }

        [Display(Name = "If it is a ground located radar please enter the location informations")]
        [Required(ErrorMessage = "Please enter the location informations")]
        public String ground_location { get; set; }

        [Display(Name = "If it is an airborne radar please enter the airborne name")]
        [Required(ErrorMessage = "Please enter the airborne name")]
        public String airborne { get; set; }


    }
}
