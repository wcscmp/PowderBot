using Data;
using Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebClient;

namespace BusinessLogic
{
    public class ListStrategy : ICommandStrategy
    {
        private readonly UserModel _user;
        private readonly SubscriptionRepository _repo;

        public ListStrategy(UserModel user, SubscriptionRepository repo)
        {
            _user = user;
            _repo = repo;
        }

        async public Task<(IMessage, UserModel)> Process()
        {
            var subscriptionsByUser = await _repo.GetByUser(_user.Id);
            var subscriptions = subscriptionsByUser.Any()
                ? string.Join("\n", subscriptionsByUser.Select(s => s.Uri))
                : "You have no subsctiptions";
            return (new TextMessage(subscriptions), _user);
        }

        public const string Usage = "ls/list - show your subscriptions";
    }
}
