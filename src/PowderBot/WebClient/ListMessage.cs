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
        private readonly IEnumerable<string> _elements;

        public ListMessage(string header, string baseText, IEnumerable<string> elements)
        {
            _header = header;
            _baseText = baseText;
            _elements = elements;
        }

        public async Task SendMessage(string userId, IMessanger client)
        {
            await client.SendMessage(userId,
                new QuickReply
                {
                    Text = _header,
                    QuickReplies = _elements.Select(e => new QuickReplyBody
                    {
                        Title = e,
                        Payload = _baseText + e
                    }).ToArray()
                });
        }
    }
}
