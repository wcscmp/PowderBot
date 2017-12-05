using Common.Converters;
using Data;
using Data.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class SubscribeStrategy : ICommandStrategy
    {
        private readonly string[] _words;
        private readonly SubscriptionRepository _repo;
        private static readonly Regex _snowfallRe
            = new Regex(@"^(\d+)(inch|cm)$", RegexOptions.IgnoreCase);

        public SubscribeStrategy(string[] words, SubscriptionRepository repo)
        {
            _words = words;
            _repo = repo;
        }

        async public Task<(string, UserModel)> Process(UserModel user)
        {
            var (snowfall, errorMessage) = parse();
            if (errorMessage != string.Empty)
            {
                return (errorMessage, user);
            }
            await _repo.Save(new SubscriptionModel(user.Id, _words[1])
                             {
                                 Snowfall = snowfall
                             });
            return ("Subscribtion added", user);
        }

        private (int, string) parse()
        {
            if (_words.Length != 3)
            {
                return (0, Parameters);
            }
            var m = _snowfallRe.Match(_words.Last());
            if (!m.Success)
            {
                return (0, "Snowfall should be a number followed by inch or cm without a space");
            }
            var snowfall = int.Parse(m.Groups[1].Captures[0].Value);
            if (m.Groups[2].Captures[0].Value == "inch")
            {
                return (snowfall.InchToCm(), string.Empty);
            }
            return (snowfall, string.Empty);
        }

        public const string Parameters
            = "subscribe <snow forecast url> <snowfall threshold>\n" +
              "    For example:\n" +
              "    subscribe http://www.snow-forecast.com/resorts/Gulmarg/6day/mid 40cm\n" +
              "    subscribe http://www.snow-forecast.com/resorts/Snowbird/6day/mid 10inch";

        public const string Usage = "subscribe <snow forecast url> <snowfall threshold>\n" +
                                    "    get notification about snowfall bigger then threshold";
    }
}
