using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPNETAOP.Models
{
    public class AddAntenna
    {
        [Key]
        public Guid ID { get; set; }

        [Display(Name = "User Friendly Name")]
#nullable enable
        public String? name { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Please enter the antenna type")]
        public String type { get; set; }

        [Display(Name = "Horizontal Beamwidth in xx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's beam's max horizontal beamwidth in xx.xx format")]
        public float horizontal_beamwidth { get; set; }

        [Display(Name = "Vertical Beamwidth in xx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's beam's max vertical beamwidth in xx.xx format")]
        public float vertical_beamwidth { get; set; }

        [Display(Name = "Polarization")]
        [Required(ErrorMessage = "Please enter the antenna's polarization")]
        public String polarization { get; set; }

        [Display(Name = "Number Of Feed")]
        public int number_of_feed { get; set; }

        [Display(Name = "Horizontal Dimension in xxx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's horizontal dimension in xxxx.xx format")]
        public float horizontal_dimension { get; set; }

        [Display(Name = "Vertical Dimension in xxx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's beam's max vertical dimension in xxxx.xx format")]
        public float vertical_dimension { get; set; }

        [Display(Name = "Duty Ex: Transmitter, Receiver, Both")]
        [Required(ErrorMessage = "Please enter the antenna's duyt as transmitter or receiver or both")]
        public String duty { get; set; }

        [Display(Name = "If this antenna is works for transmitting please enter Transmitter ID")]
        public Guid? transmitter_id { get; set; }

        [Display(Name = "If this antenna is works for receiving please enter Receiver ID")]
        public Guid? receiver_id { get; set; }

        [Display(Name = "Please enter antenna's location")]
        [Required(ErrorMessage = "Please enter antenna's location")]
        public String location { get; set; }

        //we will use it when building relationships between antenna and scans
        public bool IsChecked { get; set; } = false;

        public class AntennaList
        {
           public List<AddAntenna>? antennas { get; set; }
        }
    }
}
