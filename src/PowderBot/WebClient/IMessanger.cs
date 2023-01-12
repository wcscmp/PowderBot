using System;
using System.Threading.Tasks;

namespace WebClient
{
    public interface IMessanger
    {
        Task<double> QueryUserTimezone(string userId);
        Task SendMessage<T>(string userId, T message);
        Task SendMessage(string chatId, string message);
    }
}
