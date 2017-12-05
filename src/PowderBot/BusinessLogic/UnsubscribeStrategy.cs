using Data;
using Data.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PowderBot.ApiTypes.Facebook;

namespace BusinessLogic
{
    public class UnsubscribeStrategy : ICommandStrategy
    {
        private readonly string[] _words;
        private readonly SubscriptionRepository _repo;

        public UnsubscribeStrategy(string[] words, SubscriptionRepository repo)
        {
            _words = words;
            _repo = repo;
        }

        async public Task<(string, UserModel)> Process(UserModel user)
        {
            if (_words.Length != 2)
            {
                return (Parameters, user);
            }
            if (_words[1] != "all")
            {
                await _repo.Delete(user.Id, _words[1]);
            }
            else
            {
                var subscriptions = await _repo.GetByUser(user.Id);
                await Task.WhenAll(subscriptions
                    .ToArray()
                    .Select(s => _repo.Delete(s.UserId, s.Uri)));
            }
            return ("Done", user);
        }

        public const string Parameters
            = "unsubscribe <snow forecast url>\n" +
              "    For example:\n" +
              "    unsubscribe http://www.snow-forecast.com/resorts/Snowbird/6day/mid\n" +
              "    unsubscribe all";

        public const string Usage = "unsubscribe <snow forecast url>\n" +
                                    "    Unsubscribe from notifications from this url";
    }
}
