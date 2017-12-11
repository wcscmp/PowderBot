using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PowderBot.Controllers
{
    [Route("debug")]
    public class DebugController : Controller
    {
        public DebugController(IGenericRepository<UserModel> userRepo,
                               IGenericRepository<SubscriptionModel> subscriptionRepo,
                               IGenericRepository<RequestModel> requestRepo)
        {
            _userRepo = userRepo;
            _subscriptionRepo = subscriptionRepo;
            _requestRepo = requestRepo;
        }

        private readonly IGenericRepository<UserModel> _userRepo;
        private readonly IGenericRepository<SubscriptionModel> _subscriptionRepo;
        private readonly IGenericRepository<RequestModel> _requestRepo;

        [HttpGet("{table}")]
        async public Task<IActionResult> Get(string table)
        {
            switch (table)
            {
            case "user":
                return Ok(await _userRepo.GetAll());
            case "subs":
                return Ok(await _subscriptionRepo.GetAll());
            case "req":
                return Ok(await _requestRepo.GetAll());
            }
            return Ok(table ?? "<null>");
        }
    }
}
