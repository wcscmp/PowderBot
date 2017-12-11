using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PowderBot.ApiTypes.Facebook;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient
{
    public class ListMessage : IMessage
    {
        private readonly string _userId;
        private readonly string _header;
        private readonly string _baseText;
        private readonly string[] _elements;

        public ListMessage(string userId, string header, string baseText, string[] elements)
        {
            _userId = userId;
            _header = header;
            _baseText = baseText;
            _elements = elements;
        }

        public async Task SendMessage(IMessanger client)
        {
            await client.SendMessage(_userId,
                new ListData
                {
                    Attachment = new ListAttachment
                    {
                        Payload = new ListPayload
                        {
                            Text = _header,
                            Buttons = _elements.Select(e => new ListButton
                            {
                                Text = e,
                                Payload = _baseText + e
                            }).ToArray()
                        }
                    }
                });
        }
    }
}
