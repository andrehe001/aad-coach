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
        // APIs require a valid jwt token
        Task<ActionResult<IEnumerable<Member>>> GetTeamMembers(int id);

        Task<ActionResult<Team>> UpdateTeamName(int id, string name);

        Task<ActionResult<Team>> GetTeamStatsAndLogs(int id);

        Task<ActionResult<IEnumerable<Team>>> GetStats();

        // APIs require a valid admin jwt token
        Task<ActionResult<Team>> UpdateTeamNameAdmin(Guid id, string name);

        Task<ActionResult<IEnumerable<Member>>> GetTeamAccountsAdmin(int id);

        IActionResult Authenticate(AuthenticateRequest model);

        IActionResult AuthenticateAdmin(AuthenticateRequest model);
    }
}
