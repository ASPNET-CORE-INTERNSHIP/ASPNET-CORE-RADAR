namespace ASPNETAOP.Models
{
    public class AdminRadarList
    {
        public Radar radar { get; set; }
        public Receiver receiver { get; set; }
        public Transmitter transmitter { get; set; }
        public Location location { get; set; }

        public Submode submode { get; set; }

    }
}
