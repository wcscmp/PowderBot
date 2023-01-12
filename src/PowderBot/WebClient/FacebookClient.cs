using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PowderBot.ApiTypes.Facebook;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class FacebookClient : IMessanger
    {
        private readonly HttpClient _client;
        private const string _facebookUrl = "https://graph.facebook.com/v5.0";
        private readonly string _accessToken;

        public FacebookClient(IOptions<FacebookConfiguration> config, HttpClient client)
        {
            _client = client;
            _accessToken = config.Value.AccessToken;
        }

        private class ProfileQueryResponse
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ProfilePic { get; set; }
            public string Locale { get; set; }
            public double Timezone { get; set; }
            public string Gender { get; set; }
            public object LastAdReferral { get; set; }
        }

        public async Task<double> QueryUserTimezone(string userId)
        {
            var response = await _client.GetAsync(createRequestUri(userId, ("fields", "timezone")));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return 0;
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProfileQueryResponse>(responseJson).Timezone;
        }

        public async Task SendMessage<T>(string userId, T message)
        {
            var m = new MessageResponse<T>
            {
                Recipient = new User
                {
                    Id = userId
                },
                Message = message
            };
            var json
                = JsonConvert.SerializeObject(m, Formatting.None, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                });
            var pushContent = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync(createRequestUri("me/messages"), pushContent);
        }

        private string createRequestUri(string request,
                                        params (string, string)[] reqParams)
        {
            var allParams = reqParams
                .Concat(new (string, string)[] { ("access_token", _accessToken) })
                .Select(p => $"{p.Item1}={p.Item2}");
            return $"{_facebookUrl}/{request}?{string.Join("&", allParams)}";
        }

        public Task SendMessage(string chatId, string message)
        {
            throw new NotImplementedException();
        }
    }
}
