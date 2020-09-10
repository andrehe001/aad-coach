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
        // JwtToken (teamId, username and IsAdmin Flag) Login(Username, Pwd)


        // APIs require a valid jwt token
        Task<ActionResult<IEnumerable<Member>>> GetTeamAccounts(Guid id);

        Task<ActionResult<Team>> UpdateTeamName(Guid id, string name);

        Task<ActionResult<Team>> GetTeamStatsAndLogs(Guid id);

        Task<ActionResult<IEnumerable<Team>>> GetStats();

        // APIs require a valid admin jwt token
        Task<ActionResult<Team>> UpdateTeamNameAdmin(Guid id, string name);

        Task<ActionResult<IEnumerable<Member>>> GetTeamAccountsAdmin(Guid id);
    }
}
