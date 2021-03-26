using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class Datas
    {
        public static Guid ReceiverID { get; set; }

        public static String newProgram { get; set; } = "yes";

        public static Guid TransmitterID { get; set; }

        public static String AntennaDuty { get; set; }

        public static String RadarName { get; set; }

        public static String RadarSystem { get; set; }

        public static String RadarConfiguration { get; set; }

        public static Guid RadarID { get; set; }

        public static Guid ModeID { get; set; }

        //below for submode
        public static String SubmodeName { get; set; }

        public static float PW { get; set; }

        public static float PRI { get; set; }

        public static int min_frequency { get; set; }

        public static int max_frequency { get; set; }

        public static Guid ScanID { get; set; }
    }
}
