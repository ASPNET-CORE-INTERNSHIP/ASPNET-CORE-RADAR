using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNETAOP.Models
{
    public class Submode
    {
        public Submode() { }
        public Submode(Guid ID, String name, Guid mode_id, Double PW, Double PRI, int min_frequency, int max_frequency, Guid scan_id)
        {
            this.ID = ID;
            this.mode_id = mode_id;
            this.name = name;
            this.PW = PW;
            this.PRI = PRI;
            this.min_frequency = min_frequency;
            this.max_frequency = max_frequency;
            this.scan_id = scan_id;
        }
        public Submode(string name, Double pW, Double pRI, int min_frequency, int max_frequency)
        {
            this.name = name;
            PW = pW;
            PRI = pRI;
            this.min_frequency = min_frequency;
            this.max_frequency = max_frequency;
        }

        [Key]
        public virtual Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
#nullable enable
        public virtual String? name { get; set; }

        [Display(Name = "Mode ID")]
        [Required(ErrorMessage = "Please enter the Mode ID")]
        public virtual Guid mode_id { get; set; }

        [Display(Name = "PW -pulse width-")]
        [Required(ErrorMessage = "Please enter the PW value")]
        public virtual Double PW { get; set; }

        [Display(Name = "PRI -pulse receive interval-")]
        [Required(ErrorMessage = "Please enter the PRI value")]
        public virtual Double PRI { get; set; }

        [Display(Name = "Min Frequency")]
        [Required(ErrorMessage = "Please enter the geogmin frequency")]
        public virtual int min_frequency { get; set; }

        [Display(Name = "Max Frequency")]
        [Required(ErrorMessage = "Please enter the max frequency")]
        public virtual int max_frequency { get; set; }

        [Display(Name = "scan_id")]
        [Required(ErrorMessage = "Please enter the scan_id")]
        public virtual Guid scan_id { get; set; }
    }
}
