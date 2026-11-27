namespace GameStore.Common
{
    // базовий клас для гри
    public class Game
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Genre { get; set; } = "";
        public double Price { get; set; }
    }
}
