namespace GameStore.Common
{
    // гра на консоль, теж наслідується від Game
    public class ConsoleGame : Game
    {
        public string ConsoleType { get; set; } = "";
        public bool IsPhysicalDisc { get; set; }
        public int MaxPlayers { get; set; }
    }
}
