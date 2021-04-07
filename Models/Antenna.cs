using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ASPNETAOP.Models
{
    public class Antenna
    {
        public Antenna() { }
        public Antenna(Guid key, string def_name, string type, float horizontal_beamwidth, float vertical_beamwidth, string polarization, int number_of_feed, float horizontal_dimension, float vertical_dimension, string duty, Guid? transmitter_id, Guid? receiver_id, string location)
        {
            this.ID = key;
            this.name = def_name;
            this.type = type;
            this.horizontal_beamwidth = horizontal_beamwidth;
            this.vertical_beamwidth = vertical_beamwidth;
            this.polarization = polarization;
            this.number_of_feed = number_of_feed;
            this.horizontal_dimension = horizontal_dimension;
            this.vertical_dimension = vertical_dimension;
            this.duty = duty;
            this.transmitter_id = transmitter_id;
            this.receiver_id = receiver_id;
            this.location = location;
        }

        [Key]
        public virtual Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
#nullable enable
        public virtual String? name { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Please enter the antenna type")]
        public virtual String type { get; set; }

        [Display(Name = "Horizontal Beamwidth in xx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's beam's max horizontal beamwidth in xx.xx format")]
        public virtual float horizontal_beamwidth { get; set; }

        [Display(Name = "Vertical Beamwidth in xx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's beam's max vertical beamwidth in xx.xx format")]
        public virtual float vertical_beamwidth { get; set; }

        [Display(Name = "Polarization")]
        [Required(ErrorMessage = "Please enter the antenna's polarization")]
        public virtual String polarization { get; set; }

        [Display(Name = "Number Of Feed")]
        public virtual int number_of_feed { get; set; }

        [Display(Name = "Horizontal Dimension in xxx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's horizontal dimension in xxxx.xx format")]
        public virtual float horizontal_dimension { get; set; }

        [Display(Name = "Vertical Dimension in xxx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's beam's max vertical dimension in xxxx.xx format")]
        public virtual float vertical_dimension { get; set; }

        [Display(Name = "Duty Ex: Transmitter, Receiver, Both")]
        [Required(ErrorMessage = "Please enter the antenna's duyt as transmitter or receiver or both")]
        public virtual String duty { get; set; }

        [Display(Name = "If this antenna is works for transmitting please enter Transmitter ID")]
        public virtual Guid? transmitter_id { get; set; }

        [Display(Name = "If this antenna is works for receiving please enter Receiver ID")]
        public virtual Guid? receiver_id { get; set; }

        [Display(Name = "Please enter antenna's location")]
        [Required(ErrorMessage = "Please enter antenna's location")]
        public virtual String location { get; set; }

        //we will use it when building relationships between antenna and scans
        public virtual bool IsChecked { get; set; } = false;

        public class AntennaList
        {
           public List<Antenna>? antennas { get; set; }
        }
    }
}
