using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task InsertOrReplace(T entity)
        {
            _table[entity.RowKey] = entity;
            return Task.CompletedTask;
        }

        public Task Delete(string id)
        {
            _table.Remove(id);
            return Task.CompletedTask;
        }

        public Task DropTable()
        {
            _table.Clear();
            return Task.CompletedTask;
        }
    }
}
