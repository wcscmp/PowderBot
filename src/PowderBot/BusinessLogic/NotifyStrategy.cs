using Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using PowderBot.ApiTypes.Facebook;

namespace BusinessLogic
{
    public class NotifyStrategy : ICommandStrategy
    {
        private readonly string[] _words;

        public NotifyStrategy(string[] words)
        {
            _words = words;
        }

        public Task<(string, UserModel)> Process(UserModel user)
        {
            var (time, rangeBound, errorMessage) = parse();
            if (errorMessage != string.Empty)
            {
                return Task.FromResult((errorMessage, user));
            }
            if (rangeBound == TimeRangeBound.After)
            {
                user.NotifyAfter = time == 24 ? 0 : time;
            }
            else
            {
                user.NotifyBefore = time == 0 ? 24 : time;
            }
            user.NeedToSave = true;
            var message = $"Notification range is [{user.NotifyAfter}, {user.NotifyBefore})";
            if (user.NotifyAfter >= user.NotifyBefore)
            {
                message += "- you won't see any notifications";
            }
            return Task.FromResult((message, user));
        }

        private (int, TimeRangeBound, string) parse()
        {
            var rangeBound = parseBound();
            if (_words.Length != 3 || rangeBound == TimeRangeBound.Invalid)
            {
                return (0, rangeBound, Parameters);
            }
            int time = 0;
            if (int.TryParse(_words.Last(), out time) && time >= 0 && time <= 24)
            {
                return (time, rangeBound, string.Empty);
            }
            return (0, rangeBound, "Time should be a number from 0 to 24");
        }

        private TimeRangeBound parseBound()
        {
            if (_words.Length != 3)
            {
                return TimeRangeBound.Invalid;
            }
            switch (_words[1])
            {
            case "after":
                return TimeRangeBound.After;
            case "before":
                return TimeRangeBound.Before;
            }
            return TimeRangeBound.Invalid;
        }

        private enum TimeRangeBound
        {
            After,
            Before,
            Invalid
        }

        public const string Usage
            = "notify <before or after> HH\n" +
              "    Allows you to chose time range at which it's ok to notify you";

        public const string Parameters = "notify <before or after> HH\n" +
                                         "    For example:\n" +
                                         "    notify after 08\n" +
                                         "    notify before 22\n";
    }
}
