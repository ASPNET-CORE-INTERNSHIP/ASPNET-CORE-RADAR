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

        //To build relationships with the last inserted submode, we should know last inserted submode's id information
        public SubModeInfo LastSubmode { get; set; }

        public List<SubModeInfo> ListOfSubmodes { get; set; }
    }
}
