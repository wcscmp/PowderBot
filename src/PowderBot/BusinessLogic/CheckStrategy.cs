using Data;
using Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebClient;

namespace BusinessLogic
{
    public class CheckStrategy : ICommandStrategy
    {
        private readonly UserModel _user;
        private readonly SubscriptionRepository _repo;
        private readonly SnowfallChecker _snowfallChecker;

        public CheckStrategy(UserModel user,
                             SubscriptionRepository repo,
                             SnowfallChecker snowfallChecker)
        {
            _user = user;
            _repo = repo;
            _snowfallChecker = snowfallChecker;
        }

        async public Task<(IMessage, UserModel)> Process()
        {
            var subscriptions = await _repo.GetByUser(_user.Id);
            var snowfall = await _snowfallChecker.Check(subscriptions);
            if (!snowfall.Any())
            {
                return (new TextMessage("Nothing good"), _user);
            }
            var uris = snowfall.First().Subscriptions.Select(s => s.Uri);
            return (new MultiTextMessage(uris, "Check this out:"), _user);
        }

        public const string Usage = "check - check your subscriptions";
    }
}
