using System.Text;
using GameStore.Common;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Лабораторна робота №1. Магазин ігор (GameStore)");
Console.WriteLine();

ICrudService<PcGame> pcGameService = new CrudService<PcGame>();

// 1. Create
Console.WriteLine("1. Create: додаємо дві гри для ПК");
PcGame game1 = new PcGame("S.T.A.L.K.E.R. 2", "Шутер", 1399, "Steam", 16, 160);
PcGame game2 = new PcGame("The Witcher 3", "RPG", 729, "GOG", 8, 50);

pcGameService.Create(game1);
pcGameService.Create(game2);
Console.WriteLine("Додано 2 гри.");
Console.WriteLine();

// 2. ReadAll
Console.WriteLine("2. ReadAll: усі додані ігри");
foreach (var game in pcGameService.ReadAll())
{
    Console.WriteLine(game.GetInfo());
}
Console.WriteLine();

// 3. Read
Console.WriteLine("3. Read: пошук першої гри за Id");
PcGame foundGame = pcGameService.Read(game1.Id);
if (foundGame != null)
{
    Console.WriteLine(foundGame.GetInfo());
}
Console.WriteLine();

// 4. Update
Console.WriteLine("4. Update: змінюємо платформу та ціну першої гри");
game1.Platform = "Epic Games";
game1.Price = 1199;
pcGameService.Update(game1);

PcGame updatedGame = pcGameService.Read(game1.Id);
Console.WriteLine(updatedGame.GetInfo());
Console.WriteLine();

// 5. Remove
Console.WriteLine("5. Remove: видаляємо другу гру");
pcGameService.Remove(game2);
foreach (var game in pcGameService.ReadAll())
{
    Console.WriteLine(game.GetInfo());
}
Console.WriteLine();

// 6. Той самий CRUD-сервіс для іншого типу (Order)
Console.WriteLine("6. CRUD-сервіс для замовлень (Order)");
ICrudService<Order> orderService = new CrudService<Order>();
Order order1 = new Order("Максим", 1199);
orderService.Create(order1);
foreach (var order in orderService.ReadAll())
{
    Console.WriteLine(order.GetOrderInfo());
}
Console.WriteLine();

// 7. Наслідування
Console.WriteLine("7. Наслідування: консольна гра як Game");
Game consoleGame = new ConsoleGame("God of War Ragnarok", "Екшн", 1999, "PS5", true, 1);
Console.WriteLine(consoleGame.GetInfo());
Console.WriteLine();

// 8. Делегат і подія
Console.WriteLine("8. Делегат і подія: зміна ціни на гру");
game1.OnPriceChanged += ShowPriceChange;
game1.ChangePrice(999);
Console.WriteLine();

// 9. Статичне поле, статичний метод і метод розширення
Console.WriteLine("9. Статика і метод розширення");
Console.WriteLine("Всього створено ігор: " + Game.TotalGamesCount);
Console.WriteLine("Ціна 500 коректна: " + (Game.IsValidPrice(500) ? "так" : "ні"));
Console.WriteLine("Ціна -100 коректна: " + (Game.IsValidPrice(-100) ? "так" : "ні"));
Console.WriteLine("Короткий тег: " + consoleGame.ToShortTag());

void ShowPriceChange(string title, double oldPrice, double newPrice)
{
    Console.WriteLine($"Подія: ціна на \"{title}\" змінилась з {oldPrice} на {newPrice} {Game.Currency}");
}
