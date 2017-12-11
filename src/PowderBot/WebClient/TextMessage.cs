using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PowderBot.ApiTypes.Facebook;
using System;
using System.Threading.Tasks;

namespace WebClient
{
    public class TextMessage : IMessage
    {
        private readonly string _text;

        public TextMessage(string text)
        {
            _text = text;
        }

        public async Task SendMessage(string userId, IMessanger client)
        {
            await client.SendMessage(userId,
                new TextData
                {
                    Text = _text
                });
        }
    }
}
