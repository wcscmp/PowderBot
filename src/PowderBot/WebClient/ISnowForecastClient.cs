using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebClient
{
    public interface ISnowForecastClient
    {
        Task<int> GetSnowfall(string url);
        Task<(string Uri, int Snowfall)[]> GetSnowfall(IEnumerable<string> uris);
    }
}
