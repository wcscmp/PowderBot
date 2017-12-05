using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebClient
{
    public class SnowForecastClient : ISnowForecastClient
    {
        private HttpClient _client;

        public SnowForecastClient(HttpClient client)
        {
            _client = client;
        }

        async public Task<int> GetSnowfall(string url)
        {
            var response = await _client.GetAsync(url);
            return SnowfallCalculator.GetSnowfall(await response.Content.ReadAsStringAsync());
        }

        public Task<(string Uri, int Snowfall)[]> GetSnowfall(IEnumerable<string> uris)
        {
            return Task.WhenAll(uris.Select(uri => packUriWithTask(uri)));
        }

        private async Task<(string Uri, int Snowfall)> packUriWithTask(string uri)
        {
            return (uri, await GetSnowfall(uri));
        }
    }
}
