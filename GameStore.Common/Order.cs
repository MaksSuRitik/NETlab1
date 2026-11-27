namespace GameStore.Common
{
    // замовлення в магазині
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = "";
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
