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

        public ICommandStrategy Create(string chatId, UserModel user, string message)
        {
            var lastCommand = user.LastCommand;
            user.LastCommand = null;
            var words = message
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim().ToLowerInvariant());
            switch (words.First())
            {
                case "/check":
                    return new CheckStrategy(user, _subscriptionRepo, _snowForecastClient);
                case "/subscribe":
                    return new SubscribeStrategy(chatId, user, words.ToArray(), _subscriptionRepo);
                case "/unsubscribe":
                    return new UnsubscribeStrategy(chatId, user, words.ToArray(), _subscriptionRepo);
                case "/list":
                    return new ListStrategy(user, _subscriptionRepo);
            }

            return lastCommand != null
                ? Create(chatId, user, $"{lastCommand} {message}")
                : new UsageStrategy(user);
        }
    }
}
