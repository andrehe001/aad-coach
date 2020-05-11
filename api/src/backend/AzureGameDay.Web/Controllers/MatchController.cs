using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureGameDay.Web.Models;
using AzureGameDay.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzureGameDay.Web.Controllers
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

        [HttpGet]
        [Route("{challengerId:guid}")]
        public Task<IEnumerable<Match>> Get([FromRoute] Guid challengerId)
        {
            return _matchService.GetChallengerMatches(challengerId);
        }
    }
}