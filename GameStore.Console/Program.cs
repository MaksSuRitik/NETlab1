using System.Text;
using GameStore.Infrastructure;
using GameStore.Infrastructure.Models;
using GameStore.Infrastructure.Repositories;
using GameStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Лабораторна робота №3. Entity Framework Core та SQLite (GameStore)");
Console.WriteLine();

using var context = new GameStoreContext();
await context.Database.MigrateAsync();
Console.WriteLine($"База даних SQLite: {Path.GetFileName(context.DbPath)}");
Console.WriteLine();

// очищаємо старі тестові записи, щоб повторні запуски давали чисті дані
context.PaymentReceipts.RemoveRange(context.PaymentReceipts);
context.Orders.RemoveRange(context.Orders);
context.Games.RemoveRange(context.Games);
context.Publishers.RemoveRange(context.Publishers);
await context.SaveChangesAsync();

var publisherRepo = new Repository<PublisherModel>(context);
var pcGameRepo = new Repository<PcGameModel>(context);
var consoleGameRepo = new Repository<ConsoleGameModel>(context);
var orderRepo = new Repository<OrderModel>(context);

var publisherService = new DbCrudServiceAsync<PublisherModel>(publisherRepo, context);
var pcGameService = new DbCrudServiceAsync<PcGameModel>(pcGameRepo, context);
var consoleGameService = new DbCrudServiceAsync<ConsoleGameModel>(consoleGameRepo, context);
var orderService = new DbCrudServiceAsync<OrderModel>(orderRepo, context);

// 1. Створення видавців (для зв'язку 1:N)
Console.WriteLine("1. CreateAsync: додаємо видавців у БД");
var gsc = new PublisherModel { Name = "GSC Game World", Country = "Україна", FoundedYear = 1995 };
var cdpr = new PublisherModel { Name = "CD Projekt Red", Country = "Польща", FoundedYear = 2002 };
var sony = new PublisherModel { Name = "Santa Monica Studio", Country = "США", FoundedYear = 1999 };

await publisherService.CreateAsync(gsc);
await publisherService.CreateAsync(cdpr);
await publisherService.CreateAsync(sony);
await publisherService.SaveAsync();
Console.WriteLine($"Додано видавців: {(await publisherService.ReadAllAsync()).Count()}");
Console.WriteLine();

// 2. Створення ігор (Table-per-Type: таблиці Games, PcGames, ConsoleGames)
Console.WriteLine("2. CreateAsync: додаємо ПК та консольні ігри (Table-per-Type)");
var stalker = new PcGameModel
{
    Title = "S.T.A.L.K.E.R. 2",
    Genre = "Шутер",
    Price = 1399,
    Platform = "Steam",
    MinRamGb = 16,
    DiskSizeGb = 160,
    PublisherId = gsc.Id
};
var witcher = new PcGameModel
{
    Title = "The Witcher 3",
    Genre = "RPG",
    Price = 729,
    Platform = "GOG",
    MinRamGb = 8,
    DiskSizeGb = 50,
    PublisherId = cdpr.Id
};
var tempGame = new PcGameModel
{
    Title = "Test Demo Game",
    Genre = "Інді",
    Price = 199,
    Platform = "Steam",
    MinRamGb = 4,
    DiskSizeGb = 5,
    PublisherId = gsc.Id
};
var godOfWar = new ConsoleGameModel
{
    Title = "God of War Ragnarok",
    Genre = "Екшн",
    Price = 1999,
    ConsoleType = "PS5",
    IsPhysicalDisc = true,
    MaxPlayers = 1,
    PublisherId = sony.Id
};
var cyberpunk = new ConsoleGameModel
{
    Title = "Cyberpunk 2077",
    Genre = "RPG",
    Price = 1499,
    ConsoleType = "Xbox Series X",
    IsPhysicalDisc = false,
    MaxPlayers = 1,
    PublisherId = cdpr.Id
};

await pcGameService.CreateAsync(stalker);
await pcGameService.CreateAsync(witcher);
await pcGameService.CreateAsync(tempGame);
await consoleGameService.CreateAsync(godOfWar);
await consoleGameService.CreateAsync(cyberpunk);
await pcGameService.SaveAsync();

Console.WriteLine($"ПК ігор у таблиці PcGames: {(await pcGameService.ReadAllAsync()).Count()}");
Console.WriteLine($"Консольних ігор у таблиці ConsoleGames: {(await consoleGameService.ReadAllAsync()).Count()}");
Console.WriteLine();

// 3. Зв'язки 1:1 (Order <-> PaymentReceipt) та M:N (Order <-> Games)
Console.WriteLine("3. CreateAsync: створюємо замовлення з чеками (1:1) та іграми (M:N)");
var order1 = new OrderModel
{
    CustomerName = "Максим Сухарєв",
    TotalPrice = stalker.Price + witcher.Price,
    OrderDate = DateTime.Now,
    Receipt = new PaymentReceiptModel
    {
        ReceiptNumber = "CHK-2026-001",
        PaymentMethod = "Картка Visa",
        PaidAt = DateTime.Now
    },
    Games = new List<GameModel> { stalker, witcher }
};

var order2 = new OrderModel
{
    CustomerName = "Олег Коваленко",
    TotalPrice = godOfWar.Price + cyberpunk.Price,
    OrderDate = DateTime.Now,
    Receipt = new PaymentReceiptModel
    {
        ReceiptNumber = "CHK-2026-002",
        PaymentMethod = "Apple Pay",
        PaidAt = DateTime.Now
    },
    Games = new List<GameModel> { godOfWar, cyberpunk }
};

await orderService.CreateAsync(order1);
await orderService.CreateAsync(order2);
await orderService.SaveAsync();
Console.WriteLine($"Додано замовлень: {(await orderService.ReadAllAsync()).Count()}");
Console.WriteLine();

// 4. ReadAsync (по Guid) та GetByIdAsync (по int Id через репозиторій)
Console.WriteLine("4. ReadAsync (Guid) та GetByIdAsync (int): пошук у БД");
var foundByGuid = await pcGameService.ReadAsync(stalker.ExternalId);
var foundByInt = await pcGameService.ReadByIntIdAsync(witcher.Id);
Console.WriteLine($"Знайдено за Guid: {foundByGuid.Title} — {foundByGuid.Price} грн ({foundByGuid.Platform})");
Console.WriteLine($"Знайдено за int Id={witcher.Id}: {foundByInt.Title} — {foundByInt.Price} грн ({foundByInt.Platform})");
Console.WriteLine();

// 5. UpdateAsync та RemoveAsync
Console.WriteLine("5. UpdateAsync та RemoveAsync: оновлення і видалення з БД");
foundByGuid.Price = 1199;
await pcGameService.UpdateAsync(foundByGuid);
await pcGameService.RemoveAsync(tempGame);
await pcGameService.SaveAsync();
Console.WriteLine($"Нова ціна {foundByGuid.Title}: {(await pcGameService.ReadByIntIdAsync(foundByGuid.Id)).Price} грн");
Console.WriteLine($"Після видалення Test Demo Game залишилось ПК ігор: {(await pcGameService.ReadAllAsync()).Count()}");
Console.WriteLine();

// 6. Пагінація ReadAllAsync(page, amount)
Console.WriteLine("6. Пагінація ReadAllAsync(page: 1, amount: 2) для PcGames");
var page1 = await pcGameService.ReadAllAsync(1, 2);
foreach (var game in page1)
{
    Console.WriteLine($"  [{game.Id}] {game.Title} ({game.Genre}) — {game.Price} грн, {game.Platform}");
}
Console.WriteLine();

// 7. Перевірка зв'язків 1:N, 1:1 та M:N із бази даних
Console.WriteLine("7. Перевірка зв'язків у БД (1:N, 1:1, M:N)");
var publishersWithGames = await context.Publishers.Include(p => p.Games).ToListAsync();
foreach (var pub in publishersWithGames)
{
    string gamesList = string.Join(", ", pub.Games.Select(g => g.Title));
    Console.WriteLine($"  [1:N] Видавець {pub.Name} ({pub.Country}): {gamesList}");
}

var ordersWithDetails = await context.Orders
    .Include(o => o.Receipt)
    .Include(o => o.Games)
    .ToListAsync();
foreach (var ord in ordersWithDetails)
{
    string gamesList = string.Join(", ", ord.Games.Select(g => g.Title));
    Console.WriteLine($"  [1:1 + M:N] Замовлення #{ord.Id} ({ord.CustomerName}), чек {ord.Receipt?.ReceiptNumber} ({ord.Receipt?.PaymentMethod}) -> ігри: {gamesList}");
}
