namespace Data
{
    public class SubscriptionRepository
    {
        private readonly IGenericRepository<Models.SubscriptionModel> _repo;

        public SubscriptionRepository(IGenericRepository<Models.SubscriptionModel> repo)
        {
            _repo = repo;
        }

        public Task<bool> Delete(string chatId, string uri, string userId)
        {
            if (Models.SubscriptionModel.IsValidUri(uri))
            {
                return _repo.Delete(new Models.SubscriptionModel(chatId, uri, userId).RowKey);
            }
            else
            {
                return _repo.Delete(chatId + uri.ToLower());
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
    }
}
