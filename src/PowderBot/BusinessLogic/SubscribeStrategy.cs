using Common.Converters;
using Data;
using Data.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebClient;

namespace BusinessLogic
{
    public class SubscribeStrategy : ICommandStrategy
    {
        private readonly UserModel _user;
        private readonly string[] _words;
        private readonly SubscriptionRepository _repo;
        private static readonly Regex _snowfallRe
            = new Regex(@"^(\d+)([a-z]*)$", RegexOptions.IgnoreCase);

        public SubscribeStrategy(UserModel user, string[] words, SubscriptionRepository repo)
        {
            _user = user;
            _words = words;
            _repo = repo;
        }

        async public Task<(IMessage, UserModel)> Process()
        {
            var (snowfall, errorMessage) = parse();
            if (errorMessage != null)
            {
                return (errorMessage, _user);
            }
            await _repo.Save(new SubscriptionModel(_user.Id, _words[1])
                             {
                                 Snowfall = snowfall
                             });
            return (new TextMessage(_user.Id, "Subscribtion added"), _user);
        }

        private (int, IMessage) parse()
        {
            if (_words.Length == 1)
            {
                _user.LastCommand = string.Join(" ", _words);
                return (0, new TextMessage(_user.Id,
                                           "Enter a http://www.snow-forecast.com link to follow"));
            }
            if (_words.Length == 2)
            {
                _user.LastCommand = string.Join(" ", _words);
                return (0, new TextMessage(_user.Id, "Enter a snowfall threshhold"));
            }
            var m = _snowfallRe.Match(_words.Last());
            if (!m.Success)
            {
                return (0, new TextMessage(_user.Id,
                                           "Snowfall should be a number followed by inch or cm"));
            }
            var snowfall = int.Parse(m.Groups[1].Captures[0].Value);
            if (m.Groups[2].Captures[0].Value == "inch")
            {
                return (snowfall.InchToCm(), null);
            }
            if (m.Groups[2].Captures[0].Value == "cm")
            {
                return (snowfall, null);
            }
            return (0, new ListMessage(_user.Id,
                                       "Choose measurement units",
                                       string.Join(" ", _words.Take(2)) +
                                           m.Groups[1].Captures[0].Value,
                                       new string[] { "cm", "inch" }));
        }

        public const string Usage = "sb/subscribe - follow resort's forecast";
    }
}
