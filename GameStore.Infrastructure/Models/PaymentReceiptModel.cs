namespace GameStore.Infrastructure.Models
{
    // чек оплати замовлення (зв'язок один-до-одного з OrderModel)
    public class PaymentReceiptModel : BaseModel
    {
        public string ReceiptNumber { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public DateTime PaidAt { get; set; } = DateTime.Now;

        public int OrderId { get; set; }
        public OrderModel? Order { get; set; }
    }
}
