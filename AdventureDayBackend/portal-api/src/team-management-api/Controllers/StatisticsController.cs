using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using team_management_api.Data;
using team_management_api.Helpers;

namespace team_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly AdventureDayBackendDbContext _dbContext;

        public StatisticsController(AdventureDayBackendDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("leaderboard")]
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam)]
        public ActionResult<IEnumerable> GetLeaderboardStatistics()
        {
            var leaderboard = _dbContext.TeamScores
                .Select(_ => new { _.Team.Name, _.Score, _.Wins, _.Loses, _.Errors, _.Profit })
                .ToList()
                .OrderByDescending(_ => _.Score); // Score is not in the DB as column!

            return Ok(leaderboard);
        }

        [HttpGet("team/current/rank")]
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam)]
        public ActionResult<Team> GetTeamRank()
        {
            var team = (Team)HttpContext.Items["Team"];
            if (team == null)
            {
                return NotFound();
            }

            var allTeams = _dbContext.TeamScores
                .Select(_ => new { _.TeamId, _.Score })
                .ToList()
                .OrderByDescending(_ => _.Score);  // Score is not in the DB as column!

            var rank = allTeams
                .Select((team, index) => new { team, index })
                .Single(_ => _.team.TeamId == team.Id)
                .index + 1;

            return Ok(rank);
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