namespace GameStore.Infrastructure.Models
{
    // базова модель гри для БД (наслідування Table-per-Type)
    public class GameModel : BaseModel
    {
        public string Title { get; set; } = "";
        public string Genre { get; set; } = "";
        public double Price { get; set; }

        // зовнішній ключ на видавця (1:N)
        public int PublisherId { get; set; }
        public PublisherModel? Publisher { get; set; }

        // зв'язок багато-до-багатьох із замовленнями
        public List<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
