using Data;
using Data.Models;
using WebClient;

namespace BusinessLogic
{
    public class CheckStrategy : ICommandStrategy
    {
        private readonly UserModel _user;
        private readonly SubscriptionRepository _repo;
        private readonly ISnowForecastClient _snowForecastClient;

        public CheckStrategy(UserModel user,
                             SubscriptionRepository repo,
                             ISnowForecastClient snowForecastClient)
        {
            _user = user;
            _repo = repo;
            _snowForecastClient = snowForecastClient;
        }

        async public Task<(IMessage, UserModel)> Process()
        {
            var subscriptions = (await _repo.GetByUser(_user.Id)).ToDictionary(s => s.Uri);
            var snowfall = await _snowForecastClient.GetSnowfall(subscriptions.Keys);
            if (!snowfall.Any())
            {
                return (new TextMessage("Nothing good"), _user);
            }
            var uris = snowfall
                .Select(s => $"{subscriptions[s.Uri].GetResortName()}: {s.Snowfall}cm");
            return (new MultiTextMessage(uris), _user);
        }

        public const string Usage = "/check - check your subscriptions";
    }
}
