using Data.Models;
using System;
using System.Threading.Tasks;
using WebClient;

namespace BusinessLogic
{
    public class UsageStrategy : ICommandStrategy
    {
        private readonly UserModel _user;

        public UsageStrategy(UserModel user)
        {
            _user = user;
        }

        public Task<(IMessage, UserModel)> Process()
        {
            const string usage = "Usage:\n" +
                                 CheckStrategy.Usage + "\n" +
                                 SubscribeStrategy.Usage + "\n" +
                                 ListStrategy.Usage + "\n" +
                                 UnsubscribeStrategy.Usage;
            IMessage message = new TextMessage(_user.Id, usage);
            return Task.FromResult((message, _user));
        }
    }
}
