namespace GameStore.Common
{
    // гра на ПК, наслідується від Game
    public class PcGame : Game
    {
        public string Platform { get; set; } = "";
        public int MinRamGb { get; set; }
        public double DiskSizeGb { get; set; }

        // конструктор без параметрів
        public PcGame() : base()
        {
        }

        // конструктор з параметрами
        public PcGame(string title, string genre, double price, string platform, int minRamGb, double diskSizeGb)
            : base(title, genre, price)
        {
            Platform = platform;
            MinRamGb = minRamGb;
            DiskSizeGb = diskSizeGb;
        }

        // метод
        public override string GetInfo()
        {
            return $"{Title} ({Genre}) - {Price} {Currency}, платформа: {Platform}, ОЗП: {MinRamGb} ГБ, диск: {DiskSizeGb} ГБ";
        }
    }
}
