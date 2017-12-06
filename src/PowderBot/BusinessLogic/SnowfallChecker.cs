using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient;

namespace BusinessLogic
{
    public class SnowfallChecker
    {
        public SnowfallChecker(ISnowForecastClient snowForecastClient)
        {
            _snowForecastClient = snowForecastClient;
        }

        private readonly ISnowForecastClient _snowForecastClient;

        async public Task<IEnumerable<(string UserId, IEnumerable<SubscriptionModel> Subscriptions)>> Check(
            IEnumerable<UserModel> users,
            IEnumerable<SubscriptionModel> subscriptions)
        {
            var userDict = users.ToDictionary(u => u.Id);
            var subscriptionsForUsers = subscriptions.Where(s => userDict.ContainsKey(s.UserId));
            var urls = subscriptionsForUsers
                .GroupBy(s => s.Uri)
                .Select(g => g.Key);
            var snowfall = (await _snowForecastClient.GetSnowfall(urls))
                .ToDictionary(s => s.Uri, s => s.Snowfall);
            return subscriptionsForUsers
                .Where(s => snowfall.TryGetValue(s.Uri, out int forecast) && s.Snowfall <= forecast)
                .GroupBy(s => s.UserId)
                .Select(g => (g.Key, (IEnumerable<SubscriptionModel>)g));
        }
    }
}
