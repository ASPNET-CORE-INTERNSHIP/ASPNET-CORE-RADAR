using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class SubModeInfo
    {
        public SubModeInfo() { }

        public Submode Submode { get; set; }

        public Scan Scan { get; set; }

        public List<Antenna> ListOfAntennas { get; set; }
    }
}
