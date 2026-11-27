namespace GameStore.Common
{
    // гра на ПК, наслідується від Game
    public class PcGame : Game
    {
        public string Platform { get; set; } = "";
        public int MinRamGb { get; set; }
        public double DiskSizeGb { get; set; }
    }
}
