using Data;
using Data.Models;
using WebClient;

namespace BusinessLogic
{
    public class CommandFactory
    {
        private readonly SubscriptionRepository _subscriptionRepo;
        private readonly ISnowForecastClient _snowForecastClient;

        public CommandFactory(SubscriptionRepository subscriptionRepo,
                              ISnowForecastClient snowForecastClient)
        {
            _subscriptionRepo = subscriptionRepo;
            _snowForecastClient = snowForecastClient;
        }

        public ICommandStrategy Create(UserModel user, string message)
        {
            var lastCommand = user.LastCommand;
            user.LastCommand = null;
            var words = message
                .Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToLowerInvariant());
            switch (words.First())
            {
            case "/check":
                return new CheckStrategy(user, _subscriptionRepo, _snowForecastClient);
            case "/subscribe":
                return new SubscribeStrategy(user, words.ToArray(), _subscriptionRepo);
            case "/unsubscribe":
                return new UnsubscribeStrategy(user, words.ToArray(), _subscriptionRepo);
            case "/list":
                return new ListStrategy(user, _subscriptionRepo);
            }
            return lastCommand != null
                ? Create(user, $"{lastCommand} {message}")
                : new UsageStrategy(user);
        }
    }
}
