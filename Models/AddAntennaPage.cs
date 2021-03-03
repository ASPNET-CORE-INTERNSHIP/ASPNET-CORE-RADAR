using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AddAntennaPage
    {
        [Key]
        public int antenna_id { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Please enter the antenna type")]
        public String type { get; set; }

        [Display(Name = "Horizontal Beamwidth in xx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's beam's max horizontal beamwidth in xx.xx format")]
        public double horizontal_beamwidth { get; set; }

        [Display(Name = "Vertical Beamwidth in xx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's beam's max vertical beamwidth in xx.xx format")]
        public double vertical_beamwidth { get; set; }

        [Display(Name = "Polarization")]
        [Required(ErrorMessage = "Please enter the antenna's polarization")]
        public String polarization { get; set; }

        [Display(Name = "Number Of Feed")]
        [Required(ErrorMessage = "Please enter the antenna's number of feed")]
        public int number_of_feed { get; set; }

        [Display(Name = "Horizontal Dimension in xxxx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's horizontal dimension in xxxx.xx format")]
        public double horizontal_dimension { get; set; }

        [Display(Name = "Vertical Dimension in xxxx.xx format")]
        [Required(ErrorMessage = "Please enter the antenna's beam's max vertical dimension in xxxx.xx format")]
        public double vertical_dimension { get; set; }
        //numericToDouble = Convert.ToDouble32(reader[0].ToString());

    }
}
