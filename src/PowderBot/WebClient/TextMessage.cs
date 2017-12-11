using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PowderBot.ApiTypes.Facebook;
using System;
using System.Threading.Tasks;

namespace WebClient
{
    public class TextMessage : IMessage
    {
        private readonly string _userId;
        private readonly string _text;

        public TextMessage(string userId, string text)
        {
            _userId = userId;
            _text = text;
        }

        public async Task SendMessage(IMessanger client)
        {
            await client.SendMessage(_userId,
                new TextData
                {
                    Text = _text
                });
        }
    }
}
