using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [TeamAuthorizeAttribute("Admin")]
        public Task<ActionResult<IEnumerable<Member>>> GetTeamMembers(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [TeamAuthorizeAttribute("Admin")]
        public Task<ActionResult<Team>> UpdateTeamName(int id, string name)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [TeamAuthorizeAttribute("Admin")]
        public Task<ActionResult<Team>> GetTeamStatsAndLogs(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [TeamAuthorizeAttribute("Admin")]
        public Task<ActionResult<IEnumerable<Team>>> GetStats()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [TeamAuthorizeAttribute("Admin")]
        public Task<ActionResult<Team>> UpdateTeamNameAdmin(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [TeamAuthorizeAttribute("Admin")]
        public Task<ActionResult<IEnumerable<Member>>> GetTeamAccountsAdmin(int id)
        {
            throw new NotImplementedException();
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
            var response = _teamservice.Authenticate(model);
            if (response == null)
                return BadRequest(new { message = "Teamname or password is incorrect" });

            return Ok(response);
        }
    }
}
