using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNETAOP.Models
{
    public class Radar
    {
        public Radar() { }
        public Radar(string def_name, string system, string configuration)
        {
            this.name = def_name;
            this.system = system;
            this.configuration = configuration;
        }

        [Key]
        public Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public String? name { get; set; }

        [Display(Name = "System")]
        [Required(ErrorMessage = "Please enter the radar's system")]
        public String system { get; set; }

        [Display(Name = "Configuration")]
        [Required(ErrorMessage = "Please enter the radar's configuration")]
        public String configuration { get; set; }

        [Display(Name = "Add Transmitter")]
        [Required(ErrorMessage = "Please add a transmitter to this radar")]
        public Guid transmitter_id { get; set; }

        [Display(Name = "Add Receiver")]
        [Required(ErrorMessage = "Please add a receiver to this radar")]
        public Guid receiver_id { get; set; }

        [Display(Name = "Add Location")]
        [Required(ErrorMessage = "Please add a receiver to this radar")]
        public Guid location_id { get; set; }
    }
}
