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
        public Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public String? name { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please enter the country")]
        public String country { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please enter the city")]
        public String city { get; set; }

        [Display(Name = "Geographic Latitude")]
        [Required(ErrorMessage = "Please enter the geographic latitude")]
        public String geographic_latitude { get; set; }

        [Display(Name = "Geographic Longitude")]
        [Required(ErrorMessage = "Please enter the geographic longitude")]
        public String geographic_longitude { get; set; }

        [Display(Name = "If it is an airborne radar please enter the airborne name")]
        [Required(ErrorMessage = "Please enter the airborne name")]
        public String airborne { get; set; }


    }
}
