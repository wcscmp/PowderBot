using Microsoft.AspNetCore.Mvc;

namespace PowderBot.Controllers
{
    [Route("api/testData")]
    public class TestDataController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Live!";
        }
    }
}
