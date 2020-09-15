using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace team_management_api.Data
{
    public interface ITeamManagement
    {
        ActionResult<Team> UpdateTeamName(int teamId, string newName);

        ActionResult<Team> GetTeamStatsAndLogs(int teamId);

        ActionResult<Team> GetTeam(int teamId);
        ActionResult<Team> GetTeamWithMembers(int teamId);
        ActionResult<IEnumerable<Team>> GetAllTeams();
        ActionResult<IEnumerable<Team>> GetAllTeamsWithMembers();

        ActionResult<IEnumerable<Team>> GetStats();

        IActionResult CreateTeam(Team newTeam);

        IActionResult DeleteTeam(int teamId);

        IActionResult AddMemberToTeam(int teamId, Member newMember);

        IActionResult AddMemberToTeam(string teamName, Member newMember);

        IActionResult RemoveMemberFromTeam(int teamId, int memberId);

        IActionResult Login(AuthenticateRequest model);
    }
}
