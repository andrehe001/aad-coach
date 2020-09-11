using Microsoft.AspNetCore.Mvc;

namespace team_management_api.Controllers
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
