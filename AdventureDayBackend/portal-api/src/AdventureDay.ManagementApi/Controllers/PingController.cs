using Microsoft.AspNetCore.Mvc;

namespace AdventureDay.ManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        // GET: api/<PingController>
        [HttpGet]
        public string Get()
        {
            return "Pong!";
        }

    }
}
