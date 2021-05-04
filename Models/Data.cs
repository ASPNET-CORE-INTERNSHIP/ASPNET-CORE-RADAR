using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ASPNETAOP.Models
{
    [BindProperties(SupportsGet = true)]
    public class Data
    {
        [BindProperty]
        public Receiver Receiver { get; set; }

        public String newProgram { get; set; } = "yes";

        public Transmitter Transmitter { get; set; }

        public Radar Radar { get; set; }

        public Antenna LastAntenna { get; set; }

        public Mode LastMode { get; set; }

        public  Mode LastSubmode { get; set; }

        public Mode LastScan { get; set; }

        //I use this value in BeforeEdit functions.
        //So after complete editing process the program routes you to BeforeEdit function again (I let the user exit from edit page by her/his-self, with this solution the user can see the edited values before turning back) but with your "edit completed" message and edited values
        public bool edited { get; set; } = false;

        //when we are done with adding process we should not continue as normal creating radar process
        //so I created a variable named ComeFromAdd which keeps the information where we reached current create page
        public bool ComeFromAdd { get; set; } = false;

        //with message variable I send my messages from controllers to different views
        public String message { get; set; }

        public List<Antenna> ListOfAntennas { get; set; } = new List<Antenna>();

        public List<ModeInfo> ListOfModes { get; set; }
    }
}
