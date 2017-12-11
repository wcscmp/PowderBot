using System.Threading.Tasks;

namespace WebClient
{
    public interface IMessage
    {
        Task SendMessage(IMessanger client);
    }
}