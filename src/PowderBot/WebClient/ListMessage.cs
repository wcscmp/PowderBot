using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PowderBot.ApiTypes.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient
{
    public class ListMessage : IMessage
    {
        private readonly string _header;
        private readonly string _baseText;
        private readonly IEnumerable<(string, string)> _elements;

        public ListMessage(string header, string baseText, IEnumerable<string> elements)
        {
            _header = header;
            _baseText = baseText;
            _elements = elements.Select(e => (e, e));
        }

        public ListMessage(string header,
                           string baseText,
                           IEnumerable<string> elements,
                           IEnumerable<string> texts)
        {
            _header = header;
            _baseText = baseText;
            _elements = elements.Zip(texts, (e, t) => (e, t));
        }

        public async Task SendMessage(string userId, IMessanger client)
        {
            await client.SendMessage(userId,
                new QuickReply
                {
                    Text = _header,
                    QuickReplies = _elements.Select(e => new QuickReplyBody
                    {
                        Title = e.Item2,
                        Payload = _baseText + e.Item1
                    }).ToArray()
                });
        }
    }
}
