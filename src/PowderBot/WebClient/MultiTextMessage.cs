using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient
{
    public class MultiTextMessage : IMessage
    {
        private readonly IMessage _title;
        private readonly IEnumerable<IMessage> _texts;

        public MultiTextMessage(IEnumerable<string> texts, string title = null)
        {
            _title = title == null ? null : new TextMessage(title);
            _texts = texts.Select(t => new TextMessage(t));
        }

        public async Task SendMessage(string userId, IMessanger client)
        {
            if (_title != null)
            {
                await _title.SendMessage(userId, client);
            }

            foreach (var text in _texts)
            {
                await text.SendMessage(userId, client);
            }
        }
    }
}
