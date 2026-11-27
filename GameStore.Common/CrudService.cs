namespace GameStore.Common
{
    public class CrudService<T> : ICrudService<T> where T : IEntity
    {
        private List<T> _items = new List<T>();

        public void Create(T element)
        {
            if (element.Id == Guid.Empty)
            {
                element.Id = Guid.NewGuid();
            }

            _items.Add(element);
        }

        public T Read(Guid id)
        {
            foreach (var item in _items)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            return default!;
        }

        public IEnumerable<T> ReadAll()
        {
            return _items;
        }

        public void Update(T element)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == element.Id)
                {
                    _items[i] = element;
                    break;
                }
            }
        }

        public void Remove(T element)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == element.Id)
                {
                    _items.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
