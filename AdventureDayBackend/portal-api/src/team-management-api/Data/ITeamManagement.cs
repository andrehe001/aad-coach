using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace team_management_data
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

        IActionResult RemoveMemberFromTeam(int teamId, int memberId);

        ActionResult<Member> UpdateMemberName(int teamId, int memberId, string newName);

        IActionResult Login(AuthenticateRequest model);
    }
}
