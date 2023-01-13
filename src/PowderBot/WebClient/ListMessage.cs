namespace WebClient
{
    public class ListMessage : IMessage
    {
        private readonly string _header;
        private readonly string _baseText;
        private readonly IEnumerable<string> _elements;

        public ListMessage(string header, string baseText, IEnumerable<string> elements)
        {
            _header = header;
            _baseText = baseText;
            _elements = elements;
        }

        public async Task SendMessage(string chatId, IMessanger client)
        {
            await client.SendMessage(chatId, _header);
            await client.SendMessage(chatId, _baseText);

            foreach (var element in _elements)
            {
                await client.SendMessage(chatId, element);
            }
        }
    }
}
