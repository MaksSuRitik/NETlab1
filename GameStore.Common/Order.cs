namespace GameStore.Common
{
    // замовлення в магазині
    public class Order : IEntity
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = "";
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        // конструктор без параметрів
        public Order()
        {
            Id = Guid.NewGuid();
            OrderDate = DateTime.Now;
        }

        // конструктор з параметрами
        public Order(string customerName, double totalPrice)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;
            TotalPrice = totalPrice;
            OrderDate = DateTime.Now;
        }

        // метод
        public string GetOrderInfo()
        {
            return $"Покупець: {CustomerName}, сума: {TotalPrice} {Game.Currency}, дата: {OrderDate:dd.MM.yyyy}";
        }

        // генерація випадкового замовлення
        public static Order CreateNew()
        {
            string[] customers = { "Максим", "Олег", "Ірина", "Дмитро", "Анна", "Богдан" };

            string customer = customers[Random.Shared.Next(customers.Length)];
            double total = Random.Shared.Next(300, 5001);

            return new Order(customer, total);
        }
    }
}
