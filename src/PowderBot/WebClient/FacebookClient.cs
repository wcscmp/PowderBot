﻿using Microsoft.Extensions.Options;
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
    public class FacebookClient
    {
        private readonly HttpClient _client;
        private const string _facebookUrl = "https://graph.facebook.com/v2.6";
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
            public int Timezone { get; set; }
            public string Gender { get; set; }
            public object LastAdReferral { get; set; }
        }

        public async Task<int> QueryUserTimezone(string userId)
        {
            var response = await _client.GetAsync(createRequestUri(userId, ("fields", "timezone")));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return 0;
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProfileQueryResponse>(responseJson).Timezone;
        }

        public async Task SendMessage(string userId, string text)
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
            var uri = createRequestUri("me/messages");
            await _client.PostAsync(uri, pushContent);
        }

        private string createRequestUri(string request,
                                        params (string, string)[] reqParams)
        {
            var allParams = reqParams
                .Concat(new (string, string)[] { ("access_token", _accessToken) })
                .Select(p => $"{p.Item1}={p.Item2}");
            return $"{_facebookUrl}/{request}?{string.Join("&", allParams)}";
        }
    }
}
