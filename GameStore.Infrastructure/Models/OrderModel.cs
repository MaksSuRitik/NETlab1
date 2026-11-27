namespace GameStore.Infrastructure.Models
{
    // замовлення (1:1 з PaymentReceiptModel і M:N з GameModel)
    public class OrderModel : BaseModel
    {
        public string CustomerName { get; set; } = "";
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // зв'язок один-до-одного з чеком оплати
        public PaymentReceiptModel? Receipt { get; set; }

        // зв'язок багато-до-багатьох з іграми
        public List<GameModel> Games { get; set; } = new List<GameModel>();
    }
}
