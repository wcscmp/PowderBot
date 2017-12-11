using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public interface IGenericRepository<T> where T: TableEntity, new()
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(string id);
        Task InsertOrReplace(T entity);
        Task<bool> Delete(string id);
        Task DropTable();
    }
}
