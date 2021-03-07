using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AddReceiver
    {
        [Key]
        public String ID { get; set; }

        [Display(Name = "Listening time in nsec")]
        [Required(ErrorMessage = "Please enter the receiver's listening time in nsec")]
        public float listening_time { get; set; }

        [Display(Name = "Rest time in nsec")]
        [Required(ErrorMessage = "Please enter the receiver's rest time in nsec")]
        public float rest_time { get; set; }

        [Display(Name = "Recovery time in nsec")]
        [Required(ErrorMessage = "Please enter the receiver's recovery time in nsec")]
        public float recovery_time { get; set; }
    }
}
