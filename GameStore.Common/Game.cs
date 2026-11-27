namespace GameStore.Common
{
    // базовий клас для гри
    public class Game : IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Genre { get; set; } = "";
        public double Price { get; set; }

        // статичні поля
        public static int TotalGamesCount;
        public static string Currency;

        // статичний конструктор
        static Game()
        {
            TotalGamesCount = 0;
            Currency = "грн";
        }

        // делегат
        public delegate void PriceChangedHandler(string title, double oldPrice, double newPrice);

        // подія
        public event PriceChangedHandler? OnPriceChanged;

        // конструктор без параметрів
        public Game()
        {
            Id = Guid.NewGuid();
            Interlocked.Increment(ref TotalGamesCount);
        }

        // конструктор з параметрами
        public Game(string title, string genre, double price)
        {
            Id = Guid.NewGuid();
            Title = title;
            Genre = genre;
            Price = price;
            Interlocked.Increment(ref TotalGamesCount);
        }

        // метод для зміни ціни (викликає подію)
        public void ChangePrice(double newPrice)
        {
            if (!IsValidPrice(newPrice))
            {
                return;
            }

            double oldPrice = Price;
            Price = newPrice;

            if (OnPriceChanged != null)
            {
                OnPriceChanged(Title, oldPrice, newPrice);
            }
        }

        // метод для виводу інформації про гру
        public virtual string GetInfo()
        {
            return $"{Title} ({Genre}) - {Price} {Currency}";
        }

        // статичний метод
        public static bool IsValidPrice(double price)
        {
            return price >= 0;
        }

        // генерація випадкової гри
        public static Game CreateNew()
        {
            string[] titles = { "Horizon", "Metro", "Witcher", "Hades", "Portal", "Doom", "Hollow Knight", "Stalker" };
            string[] genres = { "Шутер", "RPG", "Екшн", "Стратегія", "Інді", "Пригоди" };

            string title = titles[Random.Shared.Next(titles.Length)] + " #" + Random.Shared.Next(1, 1000);
            string genre = genres[Random.Shared.Next(genres.Length)];
            double price = Random.Shared.Next(150, 2501);

            return new Game(title, genre, price);
        }
    }
}
