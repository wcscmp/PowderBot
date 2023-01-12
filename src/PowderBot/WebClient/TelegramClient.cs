using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WebClient
{
    public class TelegramClient : IMessanger
    {
        private const double _userTimezone = 2d;

        private readonly ILogger<TelegramClient> _logger;
        private readonly TelegramConfiguration _telegramConfiguration;

        public TelegramClient(ILogger<TelegramClient> logger, IOptions<TelegramConfiguration> config)
        {
            _logger = logger;
            _telegramConfiguration = config.Value;
        }

        public Task<double> QueryUserTimezone(string userId)
        {
            return Task.FromResult(_userTimezone);
        }

        public async Task SendMessage(string chatId, string message)
        {
            var client = new TelegramBotClient(_telegramConfiguration.ApiKey);

            try
            {
                await client.SendTextMessageAsync(
                    new ChatId(chatId),
                    RemoveMarkdownChars(message),
                    ParseMode.Markdown);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

        public Task SendMessage<T>(string userId, T message)
        {
            throw new NotImplementedException();
        }

        string RemoveMarkdownChars(string input)
        {
            return input
                .Replace("*", string.Empty)
                .Replace("_", string.Empty)
                .Replace("<", string.Empty)
                .Replace(">", string.Empty)
                .Replace("`", string.Empty);
        }
    }
}
