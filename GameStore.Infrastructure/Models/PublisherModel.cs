namespace GameStore.Infrastructure.Models
{
    // видавець ігор (зв'язок один-до-багатьох з GameModel)
    public class PublisherModel : BaseModel
    {
        public string Name { get; set; } = "";
        public string Country { get; set; } = "";
        public int FoundedYear { get; set; }

        public List<GameModel> Games { get; set; } = new List<GameModel>();
    }
}
