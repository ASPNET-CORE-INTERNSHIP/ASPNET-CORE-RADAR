using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AddMode
    {
        [Key]
        public Guid ID { get; set; }

        [Display(Name = "Mode name")]
        [Required(ErrorMessage = "Please enter the mode name")]
        public String name { get; set; }

        [Display(Name = "Radar ID")]
        [Required(ErrorMessage = "Please enter the radar which has ability to work with this mode")]
        public Guid radar_id { get; set; }

    }
}
