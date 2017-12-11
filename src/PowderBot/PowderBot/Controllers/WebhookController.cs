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
        public WebhookController(FacebookClient facebookClient,
                                 CommandFactory commandFactory,
                                 UserRepository userRepo,
                                 IGenericRepository<RequestModel> requestRepo)
        {
            _facebookClient = facebookClient;
            _commandFactory = commandFactory;
            _userRepo = userRepo;
            _requestRepo = requestRepo;
        }

        private readonly FacebookClient _facebookClient;
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
        async public Task<IActionResult> Post([FromBody]Event<TextMessage> body)
        {
            await _requestRepo.InsertOrReplace(new RequestModel("42")
                                               {
                                                   Request = JsonConvert.SerializeObject(body)
                                               });
            if (body.Object != "page")
            {
                return NotFound();
            }
            var entry = body.Entry.First();
            var user = await _userRepo.Get(
                entry.Messaging.First().Sender.Id,
                GmtHelper.GetGmt(entry.Time, entry.Messaging.First().Timestamp));
            try
            {
                var (message, updatedUser) = await _commandFactory
                    .Create(entry.Messaging.First().Message.Text)
                    .Process(user);
                if (updatedUser.NeedToSave)
                {
                    await _userRepo.Save(updatedUser);
                }
                await _facebookClient.SendMessage(updatedUser.Id, message);
                return Ok(message);
            }
            catch (Exception)
            {
                await _facebookClient.SendMessage(user.Id, "Sorry, something went wrong");
                return Ok();
            }
        }
    }
}
