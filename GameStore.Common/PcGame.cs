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

        // генерація випадкової гри для ПК
        public static new PcGame CreateNew()
        {
            string[] titles = { "S.T.A.L.K.E.R.", "The Witcher", "Cyberpunk", "Metro", "Factorio", "Hades", "Doom", "CS" };
            string[] genres = { "Шутер", "RPG", "Стратегія", "Екшн", "Інді", "Симулятор" };
            string[] platforms = { "Steam", "Epic Games", "GOG", "Origin" };
            int[] ramOptions = { 4, 8, 16, 32 };

            string title = titles[Random.Shared.Next(titles.Length)] + " #" + Random.Shared.Next(1, 1000);
            string genre = genres[Random.Shared.Next(genres.Length)];
            double price = Random.Shared.Next(199, 2499);
            string platform = platforms[Random.Shared.Next(platforms.Length)];
            int minRam = ramOptions[Random.Shared.Next(ramOptions.Length)];
            double diskSize = Random.Shared.Next(5, 181);

            return new PcGame(title, genre, price, platform, minRam, diskSize);
        }
    }
}
