using Microsoft.WindowsAzure.Storage.Table;

namespace Data
{
    public class MemoryRepository<T> : IGenericRepository<T> where T : TableEntity, new()
    {
        private static readonly Dictionary<string, T> _table = new Dictionary<string, T>();

        public MemoryRepository()
        {
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return Task.FromResult<IEnumerable<T>>(_table.Values);
        }

        public Task<T> Get(string id)
        {
            if (_table.TryGetValue(id, out T value))
            {
                return Task.FromResult(value);
            }
            return Task.FromResult((T)null);
        }

        public Task<IEnumerable<T>> GetByCustomField(string fieldName, string value)
        {
            var property = typeof(T).GetProperty(fieldName);
            return Task.FromResult(
                _table.Values.Where(v => ((string)property.GetValue(v, null)) == value));
        }

        public Task InsertOrReplace(T entity)
        {
            _table[entity.RowKey] = entity;
            return Task.CompletedTask;
        }

        public Task<bool> Delete(string id)
        {
            return Task.FromResult(_table.Remove(id));
        }

        public Task DropTable()
        {
            _table.Clear();
            return Task.CompletedTask;
        }
    }
}
