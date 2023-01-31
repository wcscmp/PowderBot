using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Data
{
    public class AzureRepository<T> : IGenericRepository<T> where T : TableEntity, new()
    {
        private const string DefaultPartition = "";

        private readonly CloudTable _table;

        public AzureRepository(IOptions<StorageConfiguration> storageOptions)
        {
            var storageAccount = CloudStorageAccount.Parse(storageOptions.Value.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var tableName = GetType().GenericTypeArguments[0].Name;
            _table = tableClient.GetTableReference(tableName);
        }

        private async Task<IEnumerable<T>> ExecuteQuery(TableQuery<T> query)
        {
            TableContinuationToken token = null;
            var result = new List<T>();
            do
            {
                var queryResult = await _table.ExecuteQuerySegmentedAsync(query, token);
                result.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);
            return result;
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return ExecuteQuery(new TableQuery<T>());
        }

        public async Task<T> Get(string id)
        {
            var retrieveOperation = TableOperation.Retrieve<T>(DefaultPartition, id);
            var retrievedResult = await _table.ExecuteAsync(retrieveOperation);

            return (T)retrievedResult?.Result;
        }

        public Task<IEnumerable<T>> GetByCustomField(string fieldName, string value)
        {
            var query = new TableQuery<T>().Where(
                TableQuery.GenerateFilterCondition(fieldName, QueryComparisons.Equal, value));
            return ExecuteQuery(query);
        }

        public async Task InsertOrReplace(T entity)
        {
            await _table.CreateIfNotExistsAsync();

            var insertOperation = TableOperation.InsertOrReplace(entity);
            await _table.ExecuteAsync(insertOperation);
        }

        public async Task<bool> Delete(string id)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                return false;
            }

            var insertOperation = TableOperation.Delete(entity);
            await _table.ExecuteAsync(insertOperation);
            return true;
        }

        public async Task DropTable()
        {
            await _table.DeleteIfExistsAsync();
        }
    }
}
