using Data.Models;
using System;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface ICommandStrategy
    {
        Task<(string, UserModel)> Process(UserModel user);
    }
}
