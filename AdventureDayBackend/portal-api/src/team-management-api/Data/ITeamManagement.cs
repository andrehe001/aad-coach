using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace team_management_data
{
    public interface ITeamManagement
    {

        Task<ActionResult<IEnumerable<Member>>> GetTeamMembers(int teamId);

        Task<ActionResult<Team>> UpdateTeamName(int teamId, string newName);

        Task<ActionResult<Team>> GetTeamStatsAndLogs(int teamId);

        Task<ActionResult<Team>> GetTeam(int teamId);
        Task<ActionResult<Team>> GetTeamWithMembers(int teamId);
        Task<ActionResult<IEnumerable<Team>>> GetAllTeams();
        Task<ActionResult<IEnumerable<Team>>> GetAllTeamsWithMembers();

        Task<ActionResult<IEnumerable<Team>>> GetStats();

        Task<IActionResult> CreateTeam(Team newTeam);

        Task<IActionResult> DeleteTeam(int teamId);

        Task<IActionResult> AddMemberToTeam(int teamId, Member newMember);

        Task<IActionResult> RemoveMemberFromTeam(int teamId, int memberId);

        Task<ActionResult<Member>> UpdateMemberName(int teamId, int memberId, string newName);

        IActionResult Login(AuthenticateRequest model);
    }
}
