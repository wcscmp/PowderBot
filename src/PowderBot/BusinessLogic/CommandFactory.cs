using Data;
using Data.Models;
using System;
using System.Linq;
using PowderBot.ApiTypes.Facebook;
using WebClient;

namespace BusinessLogic
{
    public class CommandFactory
    {
        private readonly SubscriptionRepository _subscriptionRepo;
        private readonly SnowfallChecker _snowfallChecker;

        public CommandFactory(SubscriptionRepository subscriptionRepo,
                              SnowfallChecker snowfallChecker)
        {
            _subscriptionRepo = subscriptionRepo;
            _snowfallChecker = snowfallChecker;
        }

        public ICommandStrategy Create(UserModel user, string message)
        {
            var words = message
                .Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToLowerInvariant());
            switch (words.First())
            {
            case "check":
                return new CheckStrategy(user, _subscriptionRepo, _snowfallChecker);
            case "sb":
            case "subscribe":
                return new SubscribeStrategy(user, words.ToArray(), _subscriptionRepo);
            case "unsb":
            case "unsubscribe":
                return new UnsubscribeStrategy(user, words.ToArray(), _subscriptionRepo);
            case "ls":
            case "list":
                return new ListStrategy(user, _subscriptionRepo);
            }
            if (user.LastCommand != null)
            {
                var newCommand = $"{user.LastCommand} {message}";
                user.LastCommand = null;
                return Create(user, newCommand);
            }
            return new UsageStrategy(user);
        }
    }
}
