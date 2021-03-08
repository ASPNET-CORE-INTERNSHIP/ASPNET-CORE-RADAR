using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AntennaScancs
    {

        [Key]
        public String ID { get; set; }

        public String antenna_id { get; set; }

        public String scan_id { get; set; }


    }
}
