using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class ModeInfo
    {
        public ModeInfo() { }

        public Mode Mode { get; set; }

        public List<Submode> ListOfSubmodes { get; set; }
    }
}
