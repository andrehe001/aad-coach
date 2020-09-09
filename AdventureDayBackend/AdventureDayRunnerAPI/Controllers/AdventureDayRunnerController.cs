using AdventureDayRunner.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdventureDayRunnerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class GameDayRunnerController : ControllerBase
    {

        private readonly ILogger<GameDayRunnerController> _logger;

        public GameDayRunnerController(ILogger<GameDayRunnerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Route("Start")]
        public async System.Threading.Tasks.Task<IActionResult> StartAsync([FromBody] AdventureDayRunnerProperties properties)
        {
            await Utils.PersistPropertiesUpdateAsync(properties);
            return Ok(properties);
        }

        [HttpPost]
        [Route("Pause")]
        public string Pause() 
        {
            return "Success";
        }

        [HttpPost]
        [Route("Resume")]
        public string Resume() 
        {
            return "Success";
        }

        [HttpPost]
        [Route("Edit")]
        public string Edit() 
        {
            return "Success";
        }


        [HttpPost]
        [Route("Stop")]
        public string Stop() 
        {
            return "Success";
        }        
    }
}
