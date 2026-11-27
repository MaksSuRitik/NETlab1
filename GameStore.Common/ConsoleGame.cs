namespace GameStore.Common
{
    // гра на консоль, теж наслідується від Game
    public class ConsoleGame : Game
    {
        public string ConsoleType { get; set; } = "";
        public bool IsPhysicalDisc { get; set; }
        public int MaxPlayers { get; set; }

        // конструктор без параметрів
        public ConsoleGame() : base()
        {
        }

        // конструктор з параметрами
        public ConsoleGame(string title, string genre, double price, string consoleType, bool isPhysicalDisc, int maxPlayers)
            : base(title, genre, price)
        {
            ConsoleType = consoleType;
            IsPhysicalDisc = isPhysicalDisc;
            MaxPlayers = maxPlayers;
        }

        // метод
        public override string GetInfo()
        {
            string format = IsPhysicalDisc ? "диск" : "цифрова";
            return $"{Title} ({Genre}) - {Price} {Currency}, консоль: {ConsoleType}, версія: {format}, гравців: до {MaxPlayers}";
        }

        // генерація випадкової консольної гри
        public static new ConsoleGame CreateNew()
        {
            string[] titles = { "God of War", "Spider-Man", "Gran Turismo", "Zelda", "Forza", "Halo" };
            string[] genres = { "Екшн", "Пригоди", "Гонки", "Шутер", "RPG" };
            string[] consoles = { "PS5", "Xbox Series X", "Nintendo Switch" };

            string title = titles[Random.Shared.Next(titles.Length)] + " #" + Random.Shared.Next(1, 500);
            string genre = genres[Random.Shared.Next(genres.Length)];
            double price = Random.Shared.Next(500, 2900);
            string console = consoles[Random.Shared.Next(consoles.Length)];
            bool isDisc = Random.Shared.Next(2) == 1;
            int maxPlayers = Random.Shared.Next(1, 5);

            return new ConsoleGame(title, genre, price, console, isDisc, maxPlayers);
        }
    }
}
