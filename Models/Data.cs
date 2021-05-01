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

        //I use this value in BeforeEdit functions.
        //So after complete editing process the program routes you to BeforeEdit function again (I let the user exit from edit page by her/his-self, with this solution the user can see the edited values before turning back) but with your "edit completed" message and edited values
        public static bool edited { get; set; } = false;

        //when we are done with adding process we should not continue as normal creating radar process
        //so I created a variable named ComeFromAdd which keeps the information where we reached current create page
        public static bool ComeFromAdd { get; set; } = false;

        //with message variable I send my messages from controllers to different views
        public static String message { get; set; }

        public static List<Antenna> ListOfAntennas { get; set; }
    }
}
