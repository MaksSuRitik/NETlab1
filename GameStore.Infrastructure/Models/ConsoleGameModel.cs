namespace GameStore.Infrastructure.Models
{
    // консольна гра (окрема таблиця ConsoleGames у підході Table-per-Type)
    public class ConsoleGameModel : GameModel
    {
        public string ConsoleType { get; set; } = "";
        public bool IsPhysicalDisc { get; set; }
        public int MaxPlayers { get; set; }
    }
}
