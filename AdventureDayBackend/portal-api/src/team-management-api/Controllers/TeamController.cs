﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using team_management_api.Helpers;
using team_management_data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace team_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase, ITeamManagement 
    {
        private ITeamDataService _teamservice;

        public TeamController(ITeamDataService teamservice)
        {
            _teamservice = teamservice;
        }

        [HttpPost("rename/{teamId}/{newName}")]
        [TeamAuthorizeAttribute(AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public async Task<ActionResult<Team>> UpdateTeamName(int teamId, string newName)
        {
            if (teamId == AppSettings.AdminTeamId || string.IsNullOrWhiteSpace(newName) || newName.Equals(AppSettings.AdminTeamName))
            {
                return BadRequest();
            }

            var nameFree = _teamservice.CheckTeamNameFree(teamId, newName);
            if (!nameFree)
            {
                return Conflict("Name " + newName + " already taken");
            }

            var team = (Team)this.HttpContext.Items["Team"];
            if (team != null && ( team.Id == teamId || team.Id == AppSettings.AdminTeamId))
            {
                var success = _teamservice.RenameTeam(teamId, newName);
                if (success)
                {
                    return Ok(_teamservice.GetTeamById(teamId));
                }
                else
                {
                    return BadRequest("Failed to rename " + teamId);
                }
            }
            else
            {
                return NotFound(teamId);
            }
        }

        [HttpGet]
        [TeamAuthorizeAttribute(AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public async Task<ActionResult<Team>> GetTeamStatsAndLogs(int teamId)
        {
            if (teamId == AppSettings.AdminTeamId )
            {
                return BadRequest();
            }

            var team = (Team)this.HttpContext.Items["Team"];
            if (team != null && (team.Id == teamId || team.Id == AppSettings.AdminTeamId))
            {
                Console.WriteLine(team);
                return Ok(team);
            }
            else
            {
                return NotFound(teamId);
            }
        }

        [HttpGet]
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam)]
        public async Task<ActionResult<IEnumerable<Team>>> GetStats()
        {
            throw new NotImplementedException();
        }

        [HttpGet("all")]
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam, AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public async Task<ActionResult<IEnumerable<Team>>> GetAllTeams()
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
        [TeamAuthorizeAttribute(AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public async Task<ActionResult<IEnumerable<Team>>> GetAllTeamsWithMembers()
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
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam, AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public async Task<ActionResult<Team>> GetTeam(int teamId)
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
        [TeamAuthorizeAttribute(AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public async Task<ActionResult<Team>> GetTeamWithMembers(int teamId)
        {
            var team = (Team)this.HttpContext.Items["Team"];
            if (team != null && (team.Id == teamId || team.Id == AppSettings.AdminTeamId))
            {
                var myTeam = _teamservice.GetTeamByIdWithMembers(teamId);
                return Ok( myTeam );
            }
            else
            {
                return NotFound(teamId);
            }
        }


        [HttpGet("members/{teamId}")]
        [TeamAuthorizeAttribute(AuthorizationType.AnyTeam)]
        public async Task<ActionResult<IEnumerable<Member>>> GetTeamMembers(int teamId)
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
        [TeamAuthorizeAttribute(AuthorizationType.Admin)]
        public async Task<IActionResult> CreateTeam([FromBody] Team newTeam)
        {
            if (string.IsNullOrWhiteSpace(newTeam.Name) || newTeam.Name.Equals(AppSettings.AdminTeamName))
                return BadRequest();

            var nameFree = _teamservice.CheckTeamNameFree(0, newTeam.Name);
            if (!nameFree)
            {
                return Conflict("Name " + newTeam.Name + " already taken");
            }

            var team = new Team();
            team.Name = newTeam.Name;
            team.SubscriptionId = newTeam.SubscriptionId;
            team.TeamPassword = newTeam.TeamPassword;

            if (string.IsNullOrWhiteSpace(team.TeamPassword)){
                team.TeamPassword = Guid.NewGuid().ToString();
            }

            var success =_teamservice.AddTeam(team);
            if (success)
            {
                return Ok(team);
            }
            else
            {
                return NotFound(newTeam);
            }
        }

        [HttpPost("delete/{teamId}")]
        [TeamAuthorizeAttribute(AuthorizationType.Admin)]
        public async Task<IActionResult> DeleteTeam(int teamId)
        {
            if ( teamId < 1)
                return BadRequest();

            var success = _teamservice.DeleteTeam(teamId);
            if (success)
            {
                return Ok(teamId);
            }
            else
            {
                return NotFound(teamId);
            }
        }

        [HttpPost("renameMember/{memberId}/{newName}")]
        [TeamAuthorizeAttribute(AuthorizationType.OwnTeam, AuthorizationType.Admin)]
        public async Task<ActionResult<Member>> UpdateMemberName(int teamId, int memberId, string newName)
        {
            var team = (Team)this.HttpContext.Items["Team"];
            if (team != null && (team.Id == teamId || team.Id == AppSettings.AdminTeamId))
            {
                var success = _teamservice.RenameMember(teamId, memberId, newName);
                if (success)
                {
                    return Ok(_teamservice.GetTeamById(teamId));
                }
                else
                {
                    return BadRequest("Failed to rename " + teamId);
                }
            }
            else
            {
                return NotFound(teamId);
            }
        }

        [HttpPost("addmemberto/{teamId}")]
        [TeamAuthorizeAttribute(AuthorizationType.Admin)]
        public async Task<IActionResult> AddMemberToTeam(int teamId, [FromBody] Member newMember)
        {
            if (teamId == AppSettings.AdminTeamId || string.IsNullOrWhiteSpace(newMember.Username) || string.IsNullOrWhiteSpace(newMember.DisplayName) || string.IsNullOrWhiteSpace(newMember.Password))
            {
                return BadRequest();
            }

            var success = _teamservice.AddMemberToTeam(teamId, newMember);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("removememberfrom/{teamId}/{memberId}")]
        [TeamAuthorizeAttribute(AuthorizationType.Admin)]
        public async Task<IActionResult> RemoveMemberFromTeam(int teamId, int memberId)
        {
            if (teamId == AppSettings.AdminTeamId || teamId < 1 || memberId < 1)
            {
                return BadRequest();
            }

            var success = _teamservice.RemoveMemberFromTeam(teamId, memberId);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _teamservice.Authenticate(model);
            if (response == null)
                return BadRequest(new { message = "Teamname or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("authenticateadmin")]
        public IActionResult AuthenticateAdmin(AuthenticateRequest model)
        {
            var response = _teamservice.AuthenticateAdmin(model);
            if (response == null)
                return BadRequest(new { message = "Teamname or password is incorrect" });

            return Ok(response);
        }
    }
}
