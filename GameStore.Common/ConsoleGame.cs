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
    }
}
