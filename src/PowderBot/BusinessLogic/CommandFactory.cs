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

        public ICommandStrategy Create(string message)
        {
            var words = message
                .Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToLowerInvariant());
            switch (words.First())
            {
            case "check":
                return new CheckStrategy(_subscriptionRepo, _snowfallChecker);
            case "notify":
                return new NotifyStrategy(words.ToArray());
            case "subscribe":
                return new SubscribeStrategy(words.ToArray(), _subscriptionRepo);
            case "unsubscribe":
                return new UnsubscribeStrategy(words.ToArray(), _subscriptionRepo);
            case "list":
                return new ListStrategy(_subscriptionRepo);
            default:
                return new UsageStrategy();
            }
        }
    }
}
