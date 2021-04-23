using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNETAOP.Models
{
    public class Location
    {
        public Location() { }
        public Location(Guid key_location, string def_name, string country, string city, string geographic_latitude, string geographic_longitude, string airborne)
        {
            this.ID = key_location;
            this.name = def_name;
            this.country = country;
            this.city = city;
            this.geographic_latitude = geographic_latitude;
            this.geographic_longitude = geographic_longitude;
            this.airborne = airborne;
        }


        [Key]
        public virtual Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public virtual String? name { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please enter the country")]
        public virtual String? country { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please enter the city")]
        public virtual String? city { get; set; }

        [Display(Name = "Geographic Latitude")]
        [Required(ErrorMessage = "Please enter the geographic latitude")]
        public virtual String? geographic_latitude { get; set; }

        [Display(Name = "Geographic Longitude")]
        [Required(ErrorMessage = "Please enter the geographic longitude")]
        public virtual String? geographic_longitude { get; set; }

        [Display(Name = "If it is an airborne radar please enter the airborne name")]
        [Required(ErrorMessage = "Please enter the airborne name")]
        public virtual String? airborne { get; set; }


    }
}
