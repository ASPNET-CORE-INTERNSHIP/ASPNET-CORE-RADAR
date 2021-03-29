using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class Datas
    {
        public static AddReceiver Receiver { get; set; }

        public static String newProgram { get; set; } = "yes";

        public static AddTransmitter Transmitter { get; set; }

        public static AddAntenna Antenna { get; set; }

        public static AddRadar Radar { get; set; }

        public static AddMode Mode { get; set; }

        public static AddSubmode Submode { get; set; }

        public static AddScan Scan { get; set; }
    }
}
