using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using team_management_api.Helpers;
using team_management_api.Models;
using team_management_data;

namespace team_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly TeamManagementContext _dbContext;

        public StatisticsController(TeamManagementContext context)
        {
            _dbContext = context;
        }

        [HttpGet("leaderboard")]
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam)]
        public ActionResult<IEnumerable> GetLeaderboardStatistics()
        {
            var leaderboard = _dbContext.TeamScores
                .Select(_ => new { _.Team.Name, _.Score, _.Wins, _.Loses, _.Errors, _.Profit })
                .ToList();

            return Ok(leaderboard);
        }

        [HttpGet("team/current/stats")]
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam)]
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

        [HttpGet("team/current/log")]
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam)]
        public ActionResult<IEnumerable<TeamLogEntry>> GetTeamLogEntries()
        {
            var team = (Team)HttpContext.Items["Team"];
            if (team == null)
            {
                return NotFound();
            }

            var logEntries = _dbContext.TeamLogEntries
                .Where(_ => _.TeamId == team.Id)
                .OrderByDescending(_ => _.Timestamp)
                .Take(20)
                .ToList();

            return Ok(logEntries);
        }
    }
}