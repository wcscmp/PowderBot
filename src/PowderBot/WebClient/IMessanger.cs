using System;
using System.Threading.Tasks;

namespace WebClient
{
    public interface IMessanger
    {
        Task<float> QueryUserTimezone(string userId);
        Task SendMessage<T>(string userId, T message);
    }
}
