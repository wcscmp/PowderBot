namespace Data
{
    public class UserRepository
    {
        private readonly IGenericRepository<Models.UserModel> _repo;

        public UserRepository(IGenericRepository<Models.UserModel> repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Models.UserModel>> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<Models.UserModel> Get(string id)
        {
            var user = await _repo.Get(id);
            if (user == null)
            {
                return new Models.UserModel(id);
            }
            return user;
        }

        public async Task<IEnumerable<Models.UserModel>> GetUsersWhoCanBeNotified(DateTimeOffset now)
        {
            var all = await GetAll();
            return all.Where(u => u.CanBeNotified(now));
        }

        public async Task Save(Models.UserModel user)
        {
            await _repo.InsertOrReplace(user);
        }
    }
}