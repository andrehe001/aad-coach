using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using team_management_api.Models;

namespace team_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunnerController : ControllerBase
    {
        private readonly TeamManagementContext _dbContext;

        public RunnerController(TeamManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> GetRunnerProperties()
        {
            var results = await _dbContext.RunnerProperties.ToListAsync();
            return Ok(results);
        }
    }
}