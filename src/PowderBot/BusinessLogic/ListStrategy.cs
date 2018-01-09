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
            if (!subscriptionsByUser.Any())
            {
                return (new TextMessage("You have no subsctiptions"), _user);
            }
            return (new MultiTextMessage(subscriptionsByUser.Select(s => s.Uri)), _user);
        }

        public const string Usage = "ls/list - show your subscriptions";
    }
}
