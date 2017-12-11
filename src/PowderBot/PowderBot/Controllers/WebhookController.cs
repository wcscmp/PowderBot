using BusinessLogic;
using Common;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using PowderBot.ApiTypes.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient;
using Newtonsoft.Json;

namespace PowderBot.Controllers
{
    [Route("webhook")]
    public class WebhookController : Controller
    {
        public WebhookController(IMessanger messanger,
                                 CommandFactory commandFactory,
                                 UserRepository userRepo,
                                 IGenericRepository<RequestModel> requestRepo)
        {
            _messanger = messanger;
            _commandFactory = commandFactory;
            _userRepo = userRepo;
            _requestRepo = requestRepo;
        }

        private readonly IMessanger _messanger;
        private readonly CommandFactory _commandFactory;
        private readonly UserRepository _userRepo;
        private readonly IGenericRepository<RequestModel> _requestRepo;

        [HttpGet]
        public IActionResult Get()
        {
            const string verifyToken = "b940e968-d34c-11e7-9296-cec278b6b50a";
            if (Request.Query["hub.mode"] != "subscribe"
                || Request.Query["hub.verify_token"] != verifyToken)
            {
                return Forbid();
            }
            return Ok(Request.Query["hub.challenge"].First());
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromBody]Event<ApiTypes.Facebook.TextMessage> body)
        {
            await _requestRepo.InsertOrReplace(new RequestModel("42")
                                               {
                                                   Message = JsonConvert.SerializeObject(body)
                                               });
            if (body.Object != "page")
            {
                return NotFound();
            }
            var entry = body.Entry.First();
            var user = await _userRepo.Get(entry.Messaging.First().Sender.Id);
            try
            {
                var messageText = entry.Messaging.First().Message.QuickReply?.Payload
                    ?? entry.Messaging.First().Message.Text;
                var (message, updatedUser) = await _commandFactory
                    .Create(user, messageText)
                    .Process();
                await message.SendMessage(_messanger);
                updatedUser.Gmt = await _messanger.QueryUserTimezone(updatedUser.Id);
                await _userRepo.Save(updatedUser);
                return Ok();
            }
            catch (Exception)
            {
                await new WebClient.TextMessage(user.Id, "Sorry, something went wrong")
                    .SendMessage(_messanger);
                return Ok();
            }
        }
    }
}
