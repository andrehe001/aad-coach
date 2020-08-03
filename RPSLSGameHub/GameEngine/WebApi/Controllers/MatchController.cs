using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RPSLSGameHub.GameEngine.WebApi.Models;
using RPSLSGameHub.GameEngine.WebApi.Services;

namespace RPSLSGameHub.GameEngine.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly MatchService _matchService;

        public MatchController(ILogger<MatchController> logger, MatchService matchService)
        {
            _logger = logger;
            _matchService = matchService;
        }

        [HttpGet]
        public String GetMatch()
        {
            return "Hello, I'm ready to start a match. Just post your first turn. It should look like this:" + Environment.NewLine +
                "{    \"ChallengerId\": \"daniel\",  \"Move\":\"Rock\" }" + Environment.NewLine +
                "For subsequent calls add the matchId taken from the first response.";
        }



        [HttpPost]
        public async Task<ActionResult<Match>> Post(MatchRequest request)
        {
            var match = await _matchService.PlayMatch(request);
            if (match == null)
            {
                return BadRequest(
                    $"Found no valid match setup for ChallengerID {request.ChallengerId} and MatchID {request.MatchId}");
            }
            else
            {
                return Ok(match);
            }


        }

    }
}