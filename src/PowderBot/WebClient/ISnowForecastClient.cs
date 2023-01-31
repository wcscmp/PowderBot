namespace WebClient
{
    public interface ISnowForecastClient
    {
        Task<int> GetSnowfall(string url);
        Task<(string Uri, int Snowfall)[]> GetSnowfall(IEnumerable<string> uris);
    }
}
