using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class RadarInfo
    {
        public RadarInfo() { }

        public RadarInfo(Radar radar, Transmitter transmitter, Receiver receiver, Location loc)
        {
            this.Radar = radar;
            this.Transmitter = transmitter;
            this.Receiver = receiver;
            this.Location = loc;
        }

        public Radar Radar { get; set; }

        public Receiver Receiver { get; set; }

        public Transmitter Transmitter { get; set; }

        public Location Location { get; set; }

        public List<Mode> Mode { get; set; }

        public List<Submode> Submode { get; set; }

        public Scan Scan { get; set; }

        public List<Antenna> ListOfAntennas { get; set; }
    }
}
