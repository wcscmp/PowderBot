using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<Models.UserModel> Get(string id, int gmt)
        {
            var user = await _repo.Get(id);
            if (user == null)
            {
                user = new Models.UserModel(id)
                {
                    NotifyAfter = 7,
                    NeedToSave = true
                };
            }
            user.Gmt = gmt;
            return user;
        }

        public async Task Save(Models.UserModel user)
        {
            await _repo.InsertOrReplace(user);
        }
    }
}