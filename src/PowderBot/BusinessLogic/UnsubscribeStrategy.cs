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
            if (_words.Length != 2)
            {
                _user.LastCommand = string.Join(" ", _words);
                return (new WebClient.TextMessage(_user.Id, "Enter a snow-forecast url"), _user);
            }
            if (_words[1] != "all")
            {
                bool deleted = await _repo.Delete(_user.Id, _words[1]);
                if (!deleted)
                {
                    return (new WebClient.TextMessage(_user.Id, "Subscription not found"), _user);
                }
            }
            else
            {
                var subscriptions = await _repo.GetByUser(_user.Id);
                await Task.WhenAll(subscriptions
                    .ToArray()
                    .Select(s => _repo.Delete(s.UserId, s.Uri)));
            }
            return (new WebClient.TextMessage(_user.Id, "Done"), _user);
        }

        public const string Usage = "unsb/unsubscribe - stop following resort";
    }
}
