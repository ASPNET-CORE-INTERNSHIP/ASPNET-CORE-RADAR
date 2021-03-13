using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AddTransmitter
    {
        [Key]
        public Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public String? name { get; set; }

        [Display(Name = "Oscillator type")]
        [Required(ErrorMessage = "Please enter the transmitter's oscillator type")]
        public String oscillator_type { get; set; }

        [Display(Name = "Modulation type")]
        [Required(ErrorMessage = "Please enter the transmitter's modulation type")]
        public String modulation_type { get; set; }

        [Display(Name = "Maximum Operating Frequency")]
        [Required(ErrorMessage = "Please enter the transmitter's maximum operating frequency")]
        public int max_frequency { get; set; }

        [Display(Name = "Minimum Operating Frequency")]
        [Required(ErrorMessage = "Please enter the transmitter's minimum operating frequency")]
        public int min_frequency { get; set; }

        [Display(Name = "Power")]
        [Required(ErrorMessage = "Please enter the power")]
        public int power { get; set; }
    }
}
