using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using AdventureDay.DataModel;
using AdventureDay.PortalApi.Data;
using AdventureDay.PortalApi.Controllers.Dtos;
using AdventureDay.PortalApi.Helpers;
using AdventureDay.PortalApi.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace AdventureDay.PortalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase, ITeamManagement
    {
        private ITeamDataService _teamservice;
        private readonly AppSettings _appSettings;

        public TeamController(ITeamDataService teamservice, IOptions<AppSettings> appSettings)
        {
            _teamservice = teamservice;
            _appSettings = appSettings.Value;
        }

        [HttpGet("current")]
        [TeamAuthorize(AuthorizationType.AnyTeam)]
        public ActionResult<Team> GetCurrentTeam()
        {
            var team = (Team)HttpContext.Items["Team"];
            if (team == null)
            {
                return NotFound();
            }

            return Ok(new { team.Name, team.GameEngineUri });
        }

        [HttpPost("current")]
        [TeamAuthorize(AuthorizationType.AnyTeam)]
        public ActionResult<Team> UpdateCurrentTeam(TeamUpdateRequest teamUpdate)
        {
            var team = (Team)HttpContext.Items["Team"];
            if (team == null)
            {
                return NotFound();
            }

            Log.Information("Updating team {teamId}", team.Id);
            var teamId = team.Id;
            var successName = true;
            var successUri = true;

            if (team.Name != teamUpdate.NewName)
            {
                if (string.IsNullOrWhiteSpace(teamUpdate.NewName) || teamUpdate.NewName == AppSettings.AdminTeamName
                || !team.Name.StartsWith("Team"))
                {
                    return BadRequest();
                }

                var nameFree = _teamservice.CheckTeamNameFree(teamUpdate.NewName);
                if (!nameFree)
                {
                    return Conflict("Name " + teamUpdate.NewName + " already taken");
                }

                successName = _teamservice.RenameTeam(teamId, teamUpdate.NewName);
            }
            if (team.GameEngineUri != teamUpdate.NewGameEngineUri)
            {
                successUri = _teamservice.UpdateGameEngineUri(teamId, teamUpdate.NewGameEngineUri);
            }

            if (successName && successUri)
            {
                Log.Information("Updates team {teamId}", team.Id);
                return Ok(_teamservice.GetTeamById(teamId));
            }
            else if (successName)
            {
                Log.Error("Failed to update {teamId}", team.Id);
                return BadRequest("Failed to update game engine uri " + teamId);
            }
            return BadRequest("Failed to rename " + teamId);
        }

        [HttpGet("all")]
        [TeamAuthorize(AuthorizationType.AnyTeam, AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public ActionResult<IEnumerable<Team>> GetAllTeams()
        {
            var team = (Team)this.HttpContext.Items["Team"];
            if (team != null && team.Id != AppSettings.AdminTeamId)
            {
                var myTeam = _teamservice.GetTeamById(team.Id);
                return Ok(new Team[] { myTeam });
            }
            else
            {
                var teams = _teamservice.GetAllTeams();
                return Ok(teams);
            }
        }

        [HttpGet("allwithmembers")]
        [TeamAuthorize(AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public ActionResult<IEnumerable<Team>> GetAllTeamsWithMembers()
        {
            var team = (Team)this.HttpContext.Items["Team"];
            if (team != null && team.Id != AppSettings.AdminTeamId)
            {
                var myTeam = _teamservice.GetTeamByIdWithMembers(team.Id);
                return Ok(new Team[] { myTeam });
            }
            else
            {
                var teams = _teamservice.GetAllTeamsWithMembers();
                return Ok(teams);
            }
        }

        [HttpGet("byid/{teamId}")]
        [TeamAuthorize(AuthorizationType.AnyTeam, AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public ActionResult<Team> GetTeam(int teamId)
        {
            var team = (Team)this.HttpContext.Items["Team"];
            if (team != null && (team.Id == teamId || team.Id == AppSettings.AdminTeamId))
            {
                var myTeam = _teamservice.GetTeamById(team.Id);
                return Ok(myTeam);
            }
            else
            {
                return NotFound(teamId);
            }
        }

        [HttpGet("byidwithmembers/{teamId}")]
        [TeamAuthorize(AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public ActionResult<Team> GetTeamWithMembers(int teamId)
        {
            var team = (Team)this.HttpContext.Items["Team"];
            if (team != null && (team.Id == teamId || team.Id == AppSettings.AdminTeamId))
            {
                var myTeam = _teamservice.GetTeamByIdWithMembers(teamId);
                return Ok(myTeam);
            }
            else
            {
                return NotFound(teamId);
            }
        }

        [HttpGet("members/current")]
        [TeamAuthorize(AuthorizationType.AnyTeam)]
        public ActionResult<IEnumerable<Member>> GetTeamMembersForCurrentTeam()
        {
            var team = (Team)this.HttpContext.Items["Team"];
            if (team == null || team.Id == AppSettings.AdminTeamId)
            {
                return BadRequest();
            }

            return Ok(_teamservice.GetMembers(team.Id));
        }

        [HttpGet("members/all")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public ActionResult<IEnumerable<Member>> GetAllTeamMembersOfAllTeams()
        {
            return Ok(_teamservice.GetAllTeamsWithMembers().SelectMany(t => t.Members));
        }

        [HttpGet("members/{teamId}")]
        [TeamAuthorize(AuthorizationType.AnyTeam)]
        public ActionResult<IEnumerable<Member>> GetTeamMembers(int teamId)
        {
            if (teamId == AppSettings.AdminTeamId)
            {
                return BadRequest();
            }

            var team = (Team)this.HttpContext.Items["Team"];
            if (team != null && (team.Id == teamId || team.Id == AppSettings.AdminTeamId))
            {
                return Ok(_teamservice.GetMembers(teamId));
            }
            else
            {
                return NotFound(teamId);
            }
        }

        [HttpPost("new")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public IActionResult CreateTeam([FromBody] Team newTeam)
        {
            if (string.IsNullOrWhiteSpace(newTeam.Name) || newTeam.Name.Equals(AppSettings.AdminTeamName) || string.IsNullOrWhiteSpace(newTeam.TeamPassword))
                return BadRequest();

            var nameFree = _teamservice.CheckTeamNameFree(newTeam.Name);
            if (!nameFree)
            {
                Log.Warning("Failed to create team {teamId} due to conflict", newTeam.Name);
                return Conflict(newTeam);
            }

            var team = new Team();
            team.Name = newTeam.Name;
            team.SubscriptionId = newTeam.SubscriptionId;
            team.TeamPassword = AppSettings.HashString(_appSettings, newTeam.TeamPassword);
            team.TenantId = newTeam.TenantId;

            var success = _teamservice.AddTeam(team);
            if (success)
            {
                newTeam.Id = team.Id;
                Log.Information("Successfully created team {teamId} with {id}", newTeam.Name, newTeam.Id);
                return Ok(newTeam);
            }
            else
            {
                Log.Error("Failed to create team {teamId} due to error", newTeam.Name);
                return NotFound(newTeam);
            }
        }

        [HttpPost("delete/{teamId}")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public IActionResult DeleteTeam(int teamId)
        {
            if (teamId < 1)
                return BadRequest();

            var success = _teamservice.DeleteTeam(teamId);
            if (success)
            {
                Log.Information("Successfully deleted team {teamId}", teamId);
                return Ok(teamId);
            }
            else
            {
                Log.Error("Failed to delete team {teamId} due to error", teamId);
                return NotFound(teamId);
            }
        }

        [HttpPost("addmemberto/{teamId}")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public IActionResult AddMemberToTeam(int teamId, [FromBody] Member newMember)
        {
            if (teamId == AppSettings.AdminTeamId || string.IsNullOrWhiteSpace(newMember.Username) || string.IsNullOrWhiteSpace(newMember.Password))
            {
                return BadRequest();
            }

            var success = _teamservice.AddMemberToTeam(teamId, newMember);
            if (success)
            {
                Log.Information("Successfully added member {member} to team {teamId}", newMember.Username, teamId);
                return Ok();
            }
            else
            {
                Log.Error("Failed to add member {member} to team {teamId} due to error", newMember.Username, teamId);
                return NotFound();
            }
        }

        [HttpPost("addmembertoteamname/{teamName}")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public IActionResult AddMemberToTeam(string teamName, [FromBody] Member newMember)
        {
            if (teamName.Equals(AppSettings.AdminTeamName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(newMember.Username) || string.IsNullOrWhiteSpace(newMember.Password))
            {
                return BadRequest(newMember);
            }

            var success = _teamservice.AddMemberToTeam(teamName, newMember);
            if (success)
            {
                Log.Information("Successfully added {0} to team {1}", newMember.Username, teamName);
                return Ok(newMember);
            }
            else
            {
                Log.Error("Failed to add {0} to team {1}", newMember.Username, teamName);
                return NotFound(newMember);
            }
        }

        [HttpPost("removememberfrom/{teamId}/{memberId}")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public IActionResult RemoveMemberFromTeam(int teamId, int memberId)
        {
            if (teamId == AppSettings.AdminTeamId || teamId < 1 || memberId < 1)
            {
                return BadRequest(memberId);
            }

            var success = _teamservice.RemoveMemberFromTeam(teamId, memberId);
            if (success)
            {
                Log.Information("Successfully added {0} to team {1}", memberId, teamId);
                return Ok(memberId);
            }
            else
            {
                Log.Error("Failed to remove {0} from team {1}", memberId, teamId);
                return NotFound(memberId);
            }
        }

        [HttpPost("importxlsx")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public IActionResult ImportTeamsXlsx(List<IFormFile> files)
        {
            if (files.Count > 1)
            {
                return BadRequest("Only one file supported.");
            }

            if (files.Count == 0)
            {
                return BadRequest("No file provided.");
            }

            var formFile = files.First();
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                formFile.CopyTo(stream);
            }

            string[] issues;
            var success = _teamservice.AddTeamsFromXslx(filePath, out issues);
            if (success)
            {
                Log.Information($"Successfully added teams from {filePath}");
                return Ok();
            }
            else
            {
                Log.Error("Error while importing teams from {filePath}");
                return BadRequest(issues);
            }
                        
        }


        [HttpPost("login")]
        public IActionResult Login(AuthenticateRequest model)
        {
            AuthenticateResponse response;
            if (model.Teamname.Equals(AppSettings.AdminTeamName))
            {
                response = _teamservice.AuthenticateAdmin(model);
            }
            else
            {
                response = _teamservice.Authenticate(model);
            }

            if (response == null)
            {
                Log.Warning("Failed to authenticate {teamName}", model.Teamname);
                return BadRequest(new { message = "Username or Password is incorrect" });
            }

            return Ok(response);
        }

    }
}
