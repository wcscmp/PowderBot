using Common.Converters;
using Data;
using Data.Models;
using System.Text.RegularExpressions;
using WebClient;

namespace BusinessLogic
{
    public class SubscribeStrategy : ICommandStrategy
    {
        private readonly UserModel _user;
        private readonly string[] _words;
        private readonly SubscriptionRepository _repo;
        private static readonly Regex _snowfallRe
            = new Regex(@"^(\d+)([a-z]*)?$", RegexOptions.IgnoreCase);

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
            return (new TextMessage("Subscribtion added"), _user);
        }

        private (int, IMessage) parse()
        {
            if (_words.Length == 1 || !SubscriptionModel.IsValidUri(_words[1]))
            {
                _user.LastCommand = _words.First();
                return (0, new TextMessage("Enter a valid http://www.snow-forecast.com link"));
            }
            if (_words.Length == 2)
            {
                _user.LastCommand = string.Join(" ", _words);
                return (0, new TextMessage("Enter a snowfall threshold"));
            }
            if (!Uri.IsWellFormedUriString(_words[1], UriKind.Absolute))
            {
                _words[1] = "http://" + _words[1];
            }
            var m = _snowfallRe.Match(_words[2]);
            if (!m.Success)
            {
                _user.LastCommand = string.Join(" ", _words.Take(2));
                return (0, new TextMessage("Please enter a number for the threshold"));
            }
            var snowfall = int.Parse(m.Groups[1].Captures[0].Value);
            var units = _words.Length > 3 ? _words[3] : m.Groups[2].Captures[0].Value;
            switch (units)
            {
                case "inch":
                case "/inch":
                    return (snowfall.InchToCm(), null);
                case "cm":
                case "/cm":
                    return (snowfall, null);
            }
            var baseText = $"{string.Join(" ", _words.Take(2))} {snowfall}";
            return (0, new ListMessage("Choose measurement units", baseText,
                                       new string[] { "/cm", "/inch" }));
        }

        public const string Usage = "/subscribe - follow resort's forecast";
    }
}
