using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class AddAntennaScan
    {
        public Guid antenna_id { get; set; }

        public Guid scan_id { get; set; }
    }
}
