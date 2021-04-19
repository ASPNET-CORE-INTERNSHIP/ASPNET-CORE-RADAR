using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNETAOP.Models
{
    public class Transmitter
    {
        public Transmitter() { }
        public Transmitter(Guid id, String name, String modulation_type, int max_frequency, int min_frequency, int power)
        {
            this.ID = id;
            this.name = name;
            this.modulation_type = modulation_type;
            this.max_frequency = max_frequency;
            this.min_frequency = min_frequency;
            this.power = power;
        }

        [Key]
        public virtual Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public virtual String? name { get; set; }

        [Display(Name = "Modulation type")]
        [Required(ErrorMessage = "Please enter the transmitter's modulation type")]
        public virtual String modulation_type { get; set; }

        [Display(Name = "Maximum Operating Frequency")]
        [Required(ErrorMessage = "Please enter the transmitter's maximum operating frequency")]
        public virtual int max_frequency { get; set; }

        [Display(Name = "Minimum Operating Frequency")]
        [Required(ErrorMessage = "Please enter the transmitter's minimum operating frequency")]
        public virtual int min_frequency { get; set; }

        [Display(Name = "Power")]
        [Required(ErrorMessage = "Please enter the power")]
        public virtual int power { get; set; }

        public virtual bool Isnamed { get; set; } = false;
       
    }
}
