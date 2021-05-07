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

        public Transmitter Transmitter { get; set; }

        public Radar Radar { get; set; }

        //because we want to show last inserted antenna informations when creating a new antenna we use the last antenna variable
        public Antenna LastAntenna { get; set; }

        //To add submodes to last inserted mode, we should know last inserted mode's id information
        public ModeInfo LastMode { get; set; }

        //To build relationships with the last inserted submode, we should know last inserted submode's id information
        //public Submode LastSubmode { get; set; }

        //public Scan Scan { get; set; }

        //I use this value in BeforeEdit functions.
        //So after complete editing process the program routes you to BeforeEdit function again (I let the user exit from edit page by her/his-self, with this solution the user can see the edited values before turning back) but with your "edit completed" message and edited values
        public bool edited { get; set; } = false;

        //with message variable I send my messages from controllers to different views
        public String message { get; set; }

        public List<Antenna> ListOfAntennas { get; set; } = new List<Antenna>();

        public List<ModeInfo> ListOfModes { get; set; }
    }
}
