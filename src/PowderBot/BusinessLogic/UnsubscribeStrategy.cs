using Data;
using Data.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PowderBot.ApiTypes.Facebook;
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
                var uris = (await _repo.GetByUser(_user.Id))
                    .Select(s => s.Uri)
                    .ToArray();
                if (uris.Any() || uris.Length <= 10)
                {
                    var options = (new string[] { "all" }).Concat(uris);
                    return (new ListMessage(_user.Id, $"{string.Join(" ", _words)} ", options),
                            _user);
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

        public const string Usage = "unsb/unsubscribe - stop following resort";
    }
}
