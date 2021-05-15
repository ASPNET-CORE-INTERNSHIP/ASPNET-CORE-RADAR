using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class SubModeInfo
    {
        public SubModeInfo() { }

        public SubModeInfo(Submode sbm)
        {
            this.Submode = sbm;
            this.ListOfAntennas = new List<Antenna>();
        }

        public SubModeInfo(Submode sbm, Scan s)
        {
            this.Submode = sbm;
            this.Scan = s;
            this.ListOfAntennas = new List<Antenna>();
        }

        public Submode Submode { get; set; }

        public Scan Scan { get; set; }

        public List<Antenna> ListOfAntennas { get; set; }
    }
}
