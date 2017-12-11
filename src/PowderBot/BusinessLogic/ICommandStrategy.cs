using Data.Models;
using System;
using System.Threading.Tasks;
using WebClient;

namespace BusinessLogic
{
    public interface ICommandStrategy
    {
        Task<(IMessage, UserModel)> Process();
    }
}
