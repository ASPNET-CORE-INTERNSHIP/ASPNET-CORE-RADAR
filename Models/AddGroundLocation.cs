using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class AddGroundLocation
    {
        [Key]
        public String ID { get; set; }

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
    }
}
