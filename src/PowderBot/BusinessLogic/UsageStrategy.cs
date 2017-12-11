using Data.Models;
using System;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class UsageStrategy : ICommandStrategy
    {
        public Task<(string, UserModel)> Process(UserModel user)
        {
            const string usage = "Usage:\n" +
                                 CheckStrategy.Usage + "\n" +
                                 SubscribeStrategy.Usage + "\n" +
                                 ListStrategy.Usage + "\n" +
                                 UnsubscribeStrategy.Usage;
            return Task.FromResult((usage, user));
        }
    }
}
