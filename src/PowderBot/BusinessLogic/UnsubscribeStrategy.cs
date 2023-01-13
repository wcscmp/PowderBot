using Data;
using Data.Models;
using WebClient;

namespace BusinessLogic
{
    public class UnsubscribeStrategy : ICommandStrategy
    {
        private readonly UserModel _user;
        private readonly string[] _words;
        private readonly SubscriptionRepository _repo;

        public UnsubscribeStrategy(UserModel user, string[] words, SubscriptionRepository repo)
        {
            _user = user;
            _words = words;
            _repo = repo;
        }

        async public Task<(IMessage, UserModel)> Process()
        {
            if (_words.Length == 1)
            {
                var subscriptions = await _repo.GetByUser(_user.Id);
                if (subscriptions.Any() && subscriptions.Count() <= 10)
                {
                    var resorts = (new string[] { "all" })
                        .Concat(subscriptions.Select(s => s.GetResortName()));
                    return (new ListMessage("What from?", "/unsubscribe", resorts), _user);
                }
            }
            if (_words.Length != 2)
            {
                _user.LastCommand = string.Join(" ", _words);
                return (new TextMessage("Enter existing subscription link or all"), _user);
            }
            if (_words[1] != "all")
            {
                bool deleted = await _repo.Delete(_user.Id, _words[1]);
                if (!deleted)
                {
                    return (new TextMessage("Subscription not found"), _user);
                }
            }
            else
            {
                var subscriptions = await _repo.GetByUser(_user.Id);
                await Task.WhenAll(subscriptions
                    .ToArray()
                    .Select(s => _repo.Delete(s.UserId, s.Uri)));
            }
            return (new TextMessage("Done"), _user);
        }

        public const string Usage = "/unsubscribe - stop following resort";
    }
}
