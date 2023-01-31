namespace WebClient
{
    public class TextMessage : IMessage
    {
        private readonly string _text;

        public TextMessage(string text)
        {
            _text = text;
        }

        public Task SendMessage(string chatId, IMessanger client) =>
            client.SendMessage(chatId, _text);
    }
}
