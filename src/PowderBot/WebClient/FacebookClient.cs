using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PowderBot.ApiTypes.Facebook;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class FacebookClient
    {
        private HttpClient _client;
        private string _facebookUrl;

        public FacebookClient(IOptions<FacebookConfiguration> config, HttpClient client)
        {
            _client = client;
            _facebookUrl = "https://graph.facebook.com/v2.6/me/messages?access_token=" +
                           config.Value.AccessToken;
        }

        async public Task SendMessage(string userId, string text)
        {
            var message = new MessageResponse
            {
                Recipient = new User
                {
                    Id = userId
                },
                Message = new TextData
                {
                    Text = text
                }
            };
            var stringPayload
                = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                });
            var pushContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            await _client.PostAsync(_facebookUrl, pushContent);
        }
    }
}
