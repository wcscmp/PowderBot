using System.Threading.Tasks;

namespace WebClient
{
    public interface IMessage
    {
        Task SendMessage(string userId, IMessanger client);
    }
}