﻿using Microsoft.WindowsAzure.Storage.Table;

namespace Data
{
    public interface IGenericRepository<T> where T: TableEntity, new()
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(string id);
        Task<IEnumerable<T>> GetByCustomField(string fieldName, string value);
        Task InsertOrReplace(T entity);
        Task<bool> Delete(string id);
        Task DropTable();
    }
}
