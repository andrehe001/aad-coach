using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using team_management_api.Models;
using team_management_data;

namespace team_management_api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase, ITeamManagement
    {
        private readonly TeamManagementContext _context;

        public TeamsController(TeamManagementContext context)
        {
            _context = context;
        }

        // GET: api/Teams
        [HttpGet]
        [Authorize("admin")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams.ToListAsync();
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        [Authorize("admin")]
        public async Task<ActionResult<Team>> GetTeam(Guid id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            return team;
        }

        // PUT: api/Teams/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize("admin")]
        public async Task<IActionResult> PutTeam(Guid id, Team team)
        {
            if (id != team.Id)
            {
                return BadRequest();
            }

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Teams
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize("admin")]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { id = team.Id }, team);
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        [Authorize("admin")]
        public async Task<ActionResult<Team>> DeleteTeam(Guid id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return team;
        }

        private bool TeamExists(Guid id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }

        private bool TeamExists(string teamName)
        {
            return _context.Teams.Any(e => e.Name == teamName);
        }


        [HttpGet(Name = "GetMembers")]
        [Produces("application/json", Type = typeof(Member))]
        async Task<ActionResult<IEnumerable<Member>>> ITeamManagement.GetMembers(string teamName)
        {
            throw new NotImplementedException();
        }

        [HttpGet(Name = "GetTeam")]
        [Produces("application/json", Type = typeof(Team))]
        async Task<ActionResult<Team>> ITeamManagement.GetTeam(string teamName)
        {
            throw new NotImplementedException();
        }

        [HttpGet(Name = "GetTeam")]
        [Produces("application/json", Type = typeof(Team))]
        async Task<ActionResult<Team>> ITeamManagement.GetTeam(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet(Name = "GetTeam")]
        [Produces("application/json", Type = typeof(Team))]
        async Task<ActionResult<IEnumerable<Team>>> ITeamManagement.GetTeams()
        {
            return await _context.Teams.ToListAsync();
        }
    }
}
