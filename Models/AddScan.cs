using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AddScan
    {
        [Key]
        public Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public String? name { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Please enter the scan type")]
        public String type { get; set; }

        [Display(Name = "Aspect")]
        [Required(ErrorMessage = "Please enter the main aspect of scan")]
        public String main_aspect { get; set; }

        [Display(Name = "Scanning Angle")]
        [Required(ErrorMessage = "Please enter the angle of scan")]
        public float scan_angle { get; set; }

        [Display(Name = "Scan Rate")]
        [Required(ErrorMessage = "Please enter the scan rate")]
        public float scan_rate { get; set; }

        [Display(Name = "Scan Duration (Hits-per-scan)")]
        [Required(ErrorMessage = "Please enter the scan duration")]
        public int hits_per_scan { get; set; }
    }
}
