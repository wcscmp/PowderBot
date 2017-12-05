using Data;
using Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class CheckStrategy : ICommandStrategy
    {
        private readonly SubscriptionRepository _repo;
        private readonly SnowfallChecker _snowfallChecker;

        public CheckStrategy(SubscriptionRepository repo,
                             SnowfallChecker snowfallChecker)
        {
            _repo = repo;
            _snowfallChecker = snowfallChecker;
        }

        async public Task<(string, UserModel)> Process(UserModel user)
        {
            var subscriptions = await _repo.GetByUser(user.Id);
            var snowfall = await _snowfallChecker
                .Check(new UserModel[] { user }, subscriptions, null);
            if (!snowfall.Any())
            {
                return ("Nothing good", user);
            }
            var uris = snowfall.First().Subscriptions.Select(s => s.Uri);
            return ("Check this out:\n" + string.Join("\n", uris), user);
        }

        public const string Usage = "check\n" +
                                    "    check snowfall for all subscriptions";
    }
}
