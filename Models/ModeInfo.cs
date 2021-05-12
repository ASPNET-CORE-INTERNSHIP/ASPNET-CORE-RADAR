using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class ModeInfo
    {
        public ModeInfo() { }

        public ModeInfo(Mode m) 
        {
            this.Mode = m;
        }

        public Mode Mode { get; set; }

        public List<SubModeInfo> ListOfSubmodes { get; set; }
    }
}
