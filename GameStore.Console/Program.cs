using System.Text;
using GameStore.Common;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Лабораторна робота №2. Багатопотоковість, асинхронність, LINQ (GameStore)");
Console.WriteLine();

string savePath = Path.Combine(AppContext.BaseDirectory, "games.json");
ICrudServiceAsync<PcGame> gameService = new CrudServiceAsync<PcGame>(savePath);

// 1. Паралельна генерація 1200 ігор
int count = 1200;
Console.WriteLine($"1. Parallel.For: паралельно створюємо {count} об'єктів PcGame");
Parallel.For(0, count, i =>
{
    PcGame randomGame = PcGame.CreateNew();
    gameService.CreateAsync(randomGame).GetAwaiter().GetResult();
});

var allGames = (await gameService.ReadAllAsync()).ToList();
Console.WriteLine($"Створено і додано в сервіс: {allGames.Count} ігор");
Console.WriteLine();

// 2. LINQ: мінімум, максимум і середнє по числових полях
Console.WriteLine("2. LINQ: статистика по числових полях");
Console.WriteLine($"Ціна (грн): мін = {allGames.Min(g => g.Price):F0}, макс = {allGames.Max(g => g.Price):F0}, середнє = {allGames.Average(g => g.Price):F2}");
Console.WriteLine($"ОЗП (ГБ):   мін = {allGames.Min(g => g.MinRamGb)}, макс = {allGames.Max(g => g.MinRamGb)}, середнє = {allGames.Average(g => g.MinRamGb):F2}");
Console.WriteLine($"Диск (ГБ):  мін = {allGames.Min(g => g.DiskSizeGb):F0}, макс = {allGames.Max(g => g.DiskSizeGb):F0}, середнє = {allGames.Average(g => g.DiskSizeGb):F2}");
Console.WriteLine();

// 3. Пагінація
Console.WriteLine("3. Пагінація ReadAllAsync(page: 1, amount: 3)");
var firstPage = await gameService.ReadAllAsync(1, 3);
foreach (var game in firstPage)
{
    Console.WriteLine("  " + game.GetInfo());
}
Console.WriteLine();

// 4. Перевірка IEnumerable (foreach напряму по сервісу)
Console.WriteLine("4. IEnumerable: підрахунок ігор дешевше 500 грн через foreach");
int cheapCount = 0;
foreach (var game in gameService)
{
    if (game.Price < 500)
    {
        cheapCount++;
    }
}
Console.WriteLine($"Ігор дешевше 500 грн: {cheapCount}");
Console.WriteLine();

// 5. Асинхронні ReadAsync, UpdateAsync, RemoveAsync
Console.WriteLine("5. Перевірка ReadAsync, UpdateAsync, RemoveAsync");
PcGame sample = allGames[0];
PcGame found = await gameService.ReadAsync(sample.Id);
found.Price = 999;
bool updated = await gameService.UpdateAsync(found);
bool removed = await gameService.RemoveAsync(allGames[1]);
Console.WriteLine($"Оновлено першу гру: {(updated ? "так" : "ні")}, нова ціна: {(await gameService.ReadAsync(sample.Id)).Price} грн");
Console.WriteLine($"Видалено другу гру: {(removed ? "так" : "ні")}, залишилось у сервісі: {(await gameService.ReadAllAsync()).Count()}");
Console.WriteLine();

// 6. Асинхронне збереження у файл
Console.WriteLine("6. SaveAsync: зберігаємо колекцію у файл");
bool saved = await gameService.SaveAsync();
var fileInfo = new FileInfo(savePath);
Console.WriteLine($"Збережено: {(saved ? "так" : "ні")}, файл: {fileInfo.Name} ({fileInfo.Length / 1024} КБ)");
Console.WriteLine();

// 7. Приклади CreateNew для інших класів
Console.WriteLine("7. Статичні CreateNew() для ConsoleGame та Order");
Console.WriteLine("  " + ConsoleGame.CreateNew().GetInfo());
Console.WriteLine("  " + Order.CreateNew().GetOrderInfo());
Console.WriteLine();

// 8. Примітиви синхронізації
Console.WriteLine("8. Примітиви синхронізації (lock, Semaphore, AutoResetEvent)");
SyncExamples.RunLockExample();
SyncExamples.RunSemaphoreExample();
SyncExamples.RunAutoResetEventExample();
