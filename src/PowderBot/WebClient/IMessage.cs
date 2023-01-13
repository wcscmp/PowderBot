namespace WebClient
{
    public interface IMessage
    {
        Task SendMessage(string userId, IMessanger client);
    }
}