namespace GameStore.Common
{
    public static class GameExtensions
    {
        // метод розширення
        public static string ToShortTag(this Game game)
        {
            return $"[{game.Genre}] {game.Title} — {game.Price} {Game.Currency}";
        }
    }
}
