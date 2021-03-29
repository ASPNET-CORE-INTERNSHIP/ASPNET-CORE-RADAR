﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class Receiver
    {
        public Receiver() { }
        public Receiver(Guid key, string rec_name, float listening_time, float rest_time, float recovery_time)
        {
            this.ID = key;
            this.name = rec_name;
            this.listening_time = listening_time;
            this.rest_time = rest_time;
            this.recovery_time = recovery_time;
        }

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