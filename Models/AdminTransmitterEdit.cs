namespace ASPNETAOP.Models
{
    public class AdminTransmitterEdit
    {
        public string name { get; set; }

        public string newName { get; set; }
        public string modulation_type { get; set; }

        public int max_frequency { get; set; }


        public int min_frequency { get; set; }

        public int power { get; set; }
    }
}
