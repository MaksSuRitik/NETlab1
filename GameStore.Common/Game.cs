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
            TotalGamesCount++;
        }

        // конструктор з параметрами
        public Game(string title, string genre, double price)
        {
            Id = Guid.NewGuid();
            Title = title;
            Genre = genre;
            Price = price;
            TotalGamesCount++;
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
    }
}
