using Data;
using Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class ListStrategy : ICommandStrategy
    {
        SubscriptionRepository _repo;

        public ListStrategy(SubscriptionRepository repo)
        {
            _repo = repo;
        }

        async public Task<(string, UserModel)> Process(UserModel user)
        {
            var subscriptionsByUser = await _repo.GetByUser(user.Id);
            var subscriptions = string.Join("\n", subscriptionsByUser.Select(s => s.Uri));
            if (subscriptions == string.Empty)
            {
                subscriptions = "You have no subsctiptions";
            }
            var timeRange = $"Notification range is [{user.NotifyAfter}, {user.NotifyBefore})\n";
            return (timeRange + subscriptions, user);
        }

        public const string Usage = "list\n" +
                                    "    show info on your subscriptions";
    }
}
