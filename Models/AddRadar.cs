using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AddRadar
    {
        [Key]
        public String ID { get; set; }

        [Display(Name = "System")]
        [Required(ErrorMessage = "Please enter the radar's system")]
        public String system { get; set; }

        [Display(Name = "Configuration")]
        [Required(ErrorMessage = "Please enter the radar's configuration")]
        public String configuration { get; set; }

        /*[Display(Name = "Add Transmitter")]
        [Required(ErrorMessage = "Please add a transmitter to this radar")]
        public String transmitter_id { get; set; }*/

        [Display(Name = "Add Receiver")]
        [Required(ErrorMessage = "Please add a receiver to this radar")]
        public string receiver_id { get; set; }

        [Display(Name = "Add Location")]
        [Required(ErrorMessage = "Please add a receiver to this radar")]
        public String location_id { get; set; }
    }
}
