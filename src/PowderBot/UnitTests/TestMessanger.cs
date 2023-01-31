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

        public Task SendMessage(string chatId, string message)
        {
            _messages.Add(message);

            return Task.CompletedTask;
        }

        public string Text => string.Join("\n", _messages);
    }
}
