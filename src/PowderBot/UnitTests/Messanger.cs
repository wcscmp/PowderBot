using PowderBot.ApiTypes.Facebook;
using System;
using System.Threading.Tasks;
using WebClient;

namespace UnitTests
{
    public class TestMessanger : IMessanger
    {
        public Task<int> QueryUserTimezone(string userId)
        {
            return Task.FromResult(0);
        }

        public Task SendMessage<T>(string userId, T message)
        {
            Text = matchMessage(message);
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

        public string Text { get; private set; }
    }
}
