using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ASPNETAOP.Models
{
    [BindProperties(SupportsGet = true)]
    public class Receiver
    {
        public Receiver() { }

        public Receiver(Guid key, string rec_name, Double listening_time, Double rest_time, Double recovery_time)
        {
            this.ID = key;
            this.name = rec_name;
            this.listening_time = listening_time;
            this.rest_time = rest_time;
            this.recovery_time = recovery_time;
        }

        [Key]
        public virtual Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public virtual String? name { get; set; }

        [Display(Name = "Listening time in microsecond")]
        [Required(ErrorMessage = "Please enter the receiver's listening time in nsec")]
        public virtual Double listening_time { get; set; }

        [Display(Name = "Rest time in microsecond")]
        [Required(ErrorMessage = "Please enter the receiver's rest time in nsec")]
        public virtual Double rest_time { get; set; }

        [Display(Name = "Recovery time in microsecond")]
        [Required(ErrorMessage = "Please enter the receiver's recovery time in nsec")]
        public virtual Double recovery_time { get; set; }

        public virtual bool Isnamed { get; set; } = false;
    }
}
