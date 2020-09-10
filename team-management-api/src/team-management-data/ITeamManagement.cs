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
        Task<ActionResult<IEnumerable<Member>>> GetMembers(string teamName);

        Task<ActionResult<Team>> GetTeam(string teamName);

        Task<ActionResult<Team>> GetTeam(Guid id);

        Task<ActionResult<IEnumerable<Team>>> GetTeams();
    }
}
