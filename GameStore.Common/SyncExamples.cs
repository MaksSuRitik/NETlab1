namespace GameStore.Common
{
    public static class SyncExamples
    {
        // приклад з lock: безпечне оновлення спільного лічильника з кількох потоків
        public static void RunLockExample()
        {
            object locker = new object();
            int salesCount = 0;

            Task[] tasks = new Task[5];
            for (int t = 0; t < tasks.Length; t++)
            {
                tasks[t] = Task.Run(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        lock (locker)
                        {
                            salesCount++;
                        }
                    }
                });
            }

            Task.WaitAll(tasks);
            Console.WriteLine($"[lock] 5 потоків по 1000 продажів -> разом: {salesCount}");
        }

        // приклад з Semaphore: одночасно гру можуть завантажувати не більше 2 потоків
        public static void RunSemaphoreExample()
        {
            Semaphore downloadSlots = new Semaphore(2, 2);
            int finished = 0;
            Task[] tasks = new Task[5];

            for (int i = 1; i <= 5; i++)
            {
                int userId = i;
                tasks[i - 1] = Task.Run(() =>
                {
                    downloadSlots.WaitOne();
                    try
                    {
                        Console.WriteLine($"[Semaphore] Користувач #{userId} почав завантаження гри");
                        Thread.Sleep(60);
                        Console.WriteLine($"[Semaphore] Користувач #{userId} завершив завантаження");
                        Interlocked.Increment(ref finished);
                    }
                    finally
                    {
                        downloadSlots.Release();
                    }
                });
            }

            Task.WaitAll(tasks);
            Console.WriteLine($"[Semaphore] Усі {finished} завантажень завершено (макс. 2 одночасно).");
        }

        // приклад з AutoResetEvent: другий потік чекає сигналу від першого
        public static void RunAutoResetEventExample()
        {
            AutoResetEvent orderReady = new AutoResetEvent(false);

            Task prepareOrder = Task.Run(() =>
            {
                Console.WriteLine("[AutoResetEvent] Потік 1: перевіряємо оплату замовлення...");
                Thread.Sleep(80);
                Console.WriteLine("[AutoResetEvent] Потік 1: оплату підтверджено, подаємо сигнал.");
                orderReady.Set();
            });

            Task sendKey = Task.Run(() =>
            {
                Console.WriteLine("[AutoResetEvent] Потік 2: чекаємо підтвердження оплати...");
                orderReady.WaitOne();
                Console.WriteLine("[AutoResetEvent] Потік 2: сигнал отримано, видаємо ключ активації.");
            });

            Task.WaitAll(prepareOrder, sendKey);
        }
    }
}
