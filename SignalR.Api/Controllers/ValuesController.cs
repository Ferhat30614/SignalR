using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Api.Hubs;

namespace SignalR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(IHubContext<MyHub> myhub) : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> Get(string message) {
            await myhub.Clients.All.SendAsync("ReceiveMessageForAllClient",message);
            return Ok();
        }
    }
}
