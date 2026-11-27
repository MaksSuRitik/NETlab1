namespace GameStore.Infrastructure.Models
{
    // гра для ПК (окрема таблиця PcGames у підході Table-per-Type)
    public class PcGameModel : GameModel
    {
        public string Platform { get; set; } = "";
        public int MinRamGb { get; set; }
        public double DiskSizeGb { get; set; }
    }
}
