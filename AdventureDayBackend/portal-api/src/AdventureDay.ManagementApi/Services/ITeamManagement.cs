using System.Collections.Generic;
using AdventureDay.DataModel;
using AdventureDay.ManagementApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace AdventureDay.ManagementApi.Services
{
    public interface ITeamManagement
    {

        ActionResult<Team> GetTeam(int teamId);
        ActionResult<Team> GetTeamWithMembers(int teamId);
        ActionResult<IEnumerable<Team>> GetAllTeams();
        ActionResult<IEnumerable<Team>> GetAllTeamsWithMembers();


        IActionResult CreateTeam(Team newTeam);

        IActionResult DeleteTeam(int teamId);

        IActionResult AddMemberToTeam(int teamId, Member newMember);

        IActionResult AddMemberToTeam(string teamName, Member newMember);

        IActionResult RemoveMemberFromTeam(int teamId, int memberId);

        IActionResult Login(AuthenticateRequest model);
    }
}
