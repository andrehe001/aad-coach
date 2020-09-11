using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using team_management_api.Helpers;
using team_management_api.Models;
using team_management_data;

namespace team_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsContoller : ControllerBase
    {
        private readonly TeamManagementContext _dbContext;

        public StatisticsContoller(TeamManagementContext context)
        {
            _dbContext = context;
        }

        [HttpGet("leaderboard")]
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam)]
        public ActionResult<IEnumerable<TeamScore>> GetLeaderboardStatistics()
        {
            return Ok(_dbContext.TeamScores.ToList());
        }

        [HttpGet("team")]
        public ActionResult<Team> GetTeamStatistics()
        {
            var team = (Team)HttpContext.Items["Team"];
            if (team == null)
            {
                return NotFound();
            }

            var teamScore = _dbContext.TeamScores.Where(_ => _.TeamId == team.Id).SingleOrDefault();
            if (teamScore == null)
            {
                return NotFound();
            }

            return Ok(teamScore);
        }

        [HttpGet("team/log")]
        public ActionResult<Team> GetTeamLogEntries()
        {
            var team = (Team)HttpContext.Items["Team"];
            if (team == null)
            {
                return NotFound();
            }

            var logEntries = _dbContext.TeamLogEntries
                .Where(_ => _.TeamId == team.Id)
                .OrderByDescending(_ => _.Id)
                .Take(20)
                .ToList();

            return Ok(logEntries);
        }
    }
}