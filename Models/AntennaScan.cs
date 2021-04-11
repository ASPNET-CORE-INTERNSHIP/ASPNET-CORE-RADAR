using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace ASPNETAOP.Models
{
    public class AntennaScan
    {
        public AntennaScan(Guid antenna_id, Guid scan_id)
        {
            this.antenna_id = antenna_id;
            this.scan_id = scan_id;
        }
        public Guid antenna_id { get; set; }

        public Guid scan_id { get; set; }
    }
}
