using PowderBot.ApiTypes.Facebook;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient;

namespace UnitTests
{
    public class TestMessanger : IMessanger
    {
        private readonly List<string> _messages = new List<string>();

        public Task<double> QueryUserTimezone(string userId)
        {
            return Task.FromResult(0.0);
        }

        public Task SendMessage<T>(string userId, T message)
        {
            _messages.Add(matchMessage(message));
            return Task.CompletedTask;
        }

        private string matchMessage(object message)
        {
            if (message is TextData m)
            {
                return m.Text;
            }
            return string.Empty;
        }

        public Task SendMessage(string chatId, string message)
        {
            throw new NotImplementedException();
        }

        public string Text => string.Join("\n", _messages);
    }
}
