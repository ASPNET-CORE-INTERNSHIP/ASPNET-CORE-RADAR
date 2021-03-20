using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AddSubmode
    {
        public AddSubmode()
        {
            this.name = "0submode0";
            PW = 0;
            PRI = 0;
            this.min_frequency = 0;
            this.max_frequency = 0;
        }
        public AddSubmode(string name, float pW, float pRI, int min_frequency, int max_frequency)
        {
            this.name = name;
            PW = pW;
            PRI = pRI;
            this.min_frequency = min_frequency;
            this.max_frequency = max_frequency;
        }

        [Key]
        public Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
#nullable enable
        public String? name { get; set; }

        [Display(Name = "Mode ID")]
        [Required(ErrorMessage = "Please enter the Mode ID")]
        public Guid mode_id { get; set; }

        [Display(Name = "PW -pulse width-")]
        [Required(ErrorMessage = "Please enter the PW value")]
        public float PW { get; set; }

        [Display(Name = "PRI -pulse receive interval-")]
        [Required(ErrorMessage = "Please enter the PRI value")]
        public float PRI { get; set; }

        [Display(Name = "Min Frequency")]
        [Required(ErrorMessage = "Please enter the geogmin frequency")]
        public int min_frequency { get; set; }

        [Display(Name = "Max Frequency")]
        [Required(ErrorMessage = "Please enter the max frequency")]
        public int max_frequency { get; set; }

        [Display(Name = "scan_id")]
        [Required(ErrorMessage = "Please enter the scan_id")]
        public Guid scan_id { get; set; }
    }
}
