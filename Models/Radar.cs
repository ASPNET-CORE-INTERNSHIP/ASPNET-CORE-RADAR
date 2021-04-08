using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNETAOP.Models
{
    public class Radar
    {
        public Radar() { }

        public Radar(string def_name, string system, string configuration)
        {
            this.name = def_name;
            this.system = system;
            this.configuration = configuration;
        }

        public Radar(Guid key, string def_name, string system, string configuration, Guid transmitter_id, Guid receiver_id, Guid location_id)
        {
            this.ID = key;
            this.name = def_name;
            this.system = system;
            this.configuration = configuration;
            this.transmitter_id = transmitter_id;
            this.receiver_id = receiver_id;
            this.location_id = location_id;
        }

        [Key]
        public virtual Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
        public virtual String? name { get; set; }

        [Display(Name = "System")]
        [Required(ErrorMessage = "Please enter the radar's system")]
        public virtual String system { get; set; }

        [Display(Name = "Configuration")]
        [Required(ErrorMessage = "Please enter the radar's configuration")]
        public virtual String configuration { get; set; }

        [Display(Name = "Add Transmitter")]
        [Required(ErrorMessage = "Please add a transmitter to this radar")]
        public virtual Guid transmitter_id { get; set; }

        [Display(Name = "Add Receiver")]
        [Required(ErrorMessage = "Please add a receiver to this radar")]
        public virtual Guid receiver_id { get; set; }

        [Display(Name = "Add Location")]
        [Required(ErrorMessage = "Please add a receiver to this radar")]
        public virtual Guid location_id { get; set; }

        public virtual bool Isnamed { get; set; } = false;
    }
}
