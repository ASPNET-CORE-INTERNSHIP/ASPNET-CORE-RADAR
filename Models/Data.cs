using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static ASPNETAOP.Models.Antenna;

namespace ASPNETAOP.Models
{
    public class Data
    {
        public static Receiver Receiver { get; set; }

        public static String newProgram { get; set; } = "yes";

        public static Transmitter Transmitter { get; set; }

        public static Radar Radar { get; set; }

        public static Mode Mode { get; set; }

        public static Submode Submode { get; set; }

        public static Scan Scan { get; set; }

        public static bool edited { get; set; } = false;

        public static bool ComeFromAdd { get; set; } = false;

        public static String message { get; set; }

        public static List<Antenna> ListOfAntennas { get; set; }
    }
}
