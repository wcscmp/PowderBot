using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T: TableEntity, new()
    {
        private readonly IGenericRepository<T> _impl;

        public GenericRepository(IOptions<StorageConfiguration> storageOptions)
        {
            if (storageOptions.Value.Type == "Memory")
            {
                _impl = new MemoryRepository<T>();
            }
            else
            {
                _impl = new AzureRepository<T>(storageOptions);
            }
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return _impl.GetAll();
        }

        public Task<T> Get(string id)
        {
            return _impl.Get(id);
        }

        public Task InsertOrReplace(T entity)
        {
            return _impl.InsertOrReplace(entity);
        }

        public Task Delete(string id)
        {
            return _impl.Delete(id);
        }

        public Task DropTable()
        {
            return _impl.DropTable();
        }
    }
}
