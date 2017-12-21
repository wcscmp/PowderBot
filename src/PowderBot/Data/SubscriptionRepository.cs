using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class SubscriptionRepository
    {
        private readonly IGenericRepository<Models.SubscriptionModel> _repo;

        public SubscriptionRepository(IGenericRepository<Models.SubscriptionModel> repo)
        {
            _repo = repo;
        }

        public Task<Models.SubscriptionModel> Get(string userId, string uri)
        {
            return _repo.Get(new Models.SubscriptionModel(userId, uri).RowKey);
        }

        public Task<bool> Delete(string userId, string uri)
        {
            if (Models.SubscriptionModel.IsValidUri(uri))
            {
                return _repo.Delete(new Models.SubscriptionModel(userId, uri).RowKey);
            }
            else
            {
                return _repo.Delete(userId + uri.ToLower());
            }
        }

        public Task<IEnumerable<Models.SubscriptionModel>> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<IEnumerable<Models.SubscriptionModel>> GetByUser(string userId)
        {
            return _repo.GetByCustomField("UserId", userId);
        }

        public async Task Save(Models.SubscriptionModel subscription)
        {
            await _repo.InsertOrReplace(subscription);
        }

        public IEnumerable<Task> CreateSaveTasks(IEnumerable<Models.SubscriptionModel> subscriptions)
        {
            return subscriptions.Select(s => Save(s));
        }
    }
}