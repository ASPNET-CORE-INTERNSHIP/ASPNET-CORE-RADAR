using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNETAOP.Models
{
    public class Scan
    {
        public Scan() { }
        public Scan(Guid key, string name, string type, string main_aspect, float scan_angle, float scan_rate, int hits_per_scan)
        {
            this.ID = key;
            this.name = name;
            this.type = type;
            this.main_aspect = main_aspect;
            this.scan_angle = scan_angle;
            this.scan_rate = scan_rate;
            this.hits_per_scan = hits_per_scan;
        }

        [Key]
        public virtual Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public virtual String? name { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Please enter the scan type")]
        public virtual String type { get; set; }

        [Display(Name = "Aspect")]
        [Required(ErrorMessage = "Please enter the main aspect of scan")]
        public virtual String main_aspect { get; set; }

        [Display(Name = "Scanning Angle")]
        [Required(ErrorMessage = "Please enter the angle of scan")]
        public virtual float scan_angle { get; set; }

        [Display(Name = "Scan Rate")]
        [Required(ErrorMessage = "Please enter the scan rate")]
        public virtual float scan_rate { get; set; }

        [Display(Name = "Scan Duration (Hits-per-scan)")]
        [Required(ErrorMessage = "Please enter the scan duration")]
        public virtual int hits_per_scan { get; set; }

        public virtual bool Isnamed { get; set; } = false;
    }
}
