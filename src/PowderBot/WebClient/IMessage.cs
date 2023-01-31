namespace WebClient
{
    public interface IMessage
    {
        Task SendMessage(string chatId, IMessanger client);
    }
}