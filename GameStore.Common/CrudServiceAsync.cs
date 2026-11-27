using System.Collections;
using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace GameStore.Common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : IEntity
    {
        // потокобезпечна колекція для зберігання даних
        private readonly ConcurrentDictionary<Guid, T> _items = new ConcurrentDictionary<Guid, T>();
        private readonly SemaphoreSlim _saveSemaphore = new SemaphoreSlim(1, 1);

        public string FilePath { get; set; }

        public CrudServiceAsync(string filePath = "games.json")
        {
            FilePath = filePath;
        }

        public Task<bool> CreateAsync(T element)
        {
            if (element.Id == Guid.Empty)
            {
                element.Id = Guid.NewGuid();
            }

            bool added = _items.TryAdd(element.Id, element);
            return Task.FromResult(added);
        }

        public Task<T> ReadAsync(Guid id)
        {
            _items.TryGetValue(id, out T? item);
            return Task.FromResult(item!);
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            IEnumerable<T> snapshot = _items.Values.ToList();
            return Task.FromResult(snapshot);
        }

        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (amount < 1)
            {
                amount = 10;
            }

            IEnumerable<T> pageItems = _items.Values
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToList();

            return Task.FromResult(pageItems);
        }

        public Task<bool> UpdateAsync(T element)
        {
            if (!_items.ContainsKey(element.Id))
            {
                return Task.FromResult(false);
            }

            _items[element.Id] = element;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(T element)
        {
            bool removed = _items.TryRemove(element.Id, out _);
            return Task.FromResult(removed);
        }

        public async Task<bool> SaveAsync()
        {
            await _saveSemaphore.WaitAsync();
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                string json = JsonSerializer.Serialize(_items.Values.ToList(), options);
                await File.WriteAllTextAsync(FilePath, json);
                return true;
            }
            finally
            {
                _saveSemaphore.Release();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
