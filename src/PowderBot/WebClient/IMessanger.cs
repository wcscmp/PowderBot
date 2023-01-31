namespace WebClient
{
    public interface IMessanger
    {
        Task<double> QueryUserTimezone(string userId);
        Task SendMessage(string chatId, string message);
    }
}
