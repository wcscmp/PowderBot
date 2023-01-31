using Data;
using Data.Models;
using WebClient;

namespace BusinessLogic
{
    public class UnsubscribeStrategy : ICommandStrategy
    {
        private readonly string _chatId;
        private readonly UserModel _user;
        private readonly string[] _words;
        private readonly SubscriptionRepository _repo;

        public UnsubscribeStrategy(string chatId, UserModel user, string[] words, SubscriptionRepository repo)
        {
            _chatId = chatId;
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
                    var resorts = (new string[] { "/all" })
                        .Concat(subscriptions.Select(s => $"/{s.GetResortName()}"));

                    _user.LastCommand = "/unsubscribe";

                    return (new MultiTextMessage(resorts, "What from?"), _user);
                }
            }
            if (_words.Length != 2)
            {
                _user.LastCommand = string.Join(" ", _words);

                return (new TextMessage("Enter existing subscription link or all"), _user);
            }
            if (_words[1] != "/all")
            {
                bool deleted = await _repo.Delete(_chatId, _words[1].TrimStart('/'), _user.Id);
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
                    .Select(s => _repo.Delete(s.ChatId, s.Uri, s.UserId)));
            }
            return (new TextMessage("Done"), _user);
        }

        public const string Usage = "/unsubscribe - stop following resort";
    }
}
