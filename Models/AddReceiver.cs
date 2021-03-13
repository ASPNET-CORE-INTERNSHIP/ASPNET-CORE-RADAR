using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class AddReceiver
    {
        [Key]
        public Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public String? name { get; set; }

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
