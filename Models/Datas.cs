using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNETAOP.Models
{
    public class Datas
    {
        public static Receiver Receiver { get; set; }

        public static String newProgram { get; set; } = "yes";

        public static Transmitter Transmitter { get; set; }

        public static Antenna Antenna { get; set; }

        public static Radar Radar { get; set; }

        public static Mode Mode { get; set; }

        public static Submode Submode { get; set; }

        public static Scan Scan { get; set; }
    }
}
