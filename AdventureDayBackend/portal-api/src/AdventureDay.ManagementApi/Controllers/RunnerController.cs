using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using AdventureDay.DataModel.Runner;
using AdventureDay.ManagementApi.Data;
using AdventureDay.ManagementApi.Data.Runner;
using AdventureDay.ManagementApi.Helpers;

namespace AdventureDay.ManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunnerController : ControllerBase
    {
        private readonly AdventureDayBackendDbContext _dbContext;

        public RunnerController(AdventureDayBackendDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("properties")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public async Task<IActionResult> GetDefaultRunnerProperties()
        {
            RunnerProperties result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("status")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public async Task<IActionResult> GetDefaultRunnerStatus()
        {
            RunnerProperties result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.RunnerStatus.ToString());
        }

        [HttpPost("start")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public async Task<IActionResult> DefaultRunnerStart()
        {
            RunnerProperties result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);

            if (result == null)
            {
                return NotFound();
            }

            result.RunnerStatus = RunnerStatus.Started;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("stop")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public async Task<IActionResult> DefaultRunnerStop()
        {
            RunnerProperties result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);

            if (result == null)
            {
                return NotFound();
            }

            result.RunnerStatus = RunnerStatus.Stopped;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("phase")]
        [TeamAuthorize(AuthorizationType.AnyTeam)]
        public async Task<IActionResult> DefaultRunnerGetPhase()
        {
            RunnerProperties result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);

            if (result == null)
            {
                return NotFound();
            }

            return Ok((int)result.CurrentPhase);
        }

        [HttpGet("available-phases")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public IActionResult DefaultRunnerGetAvailablePhases()
        {
            return Ok(Enum.GetNames(typeof(RunnerPhase)));
        }

        [HttpPost("phase")]
        [TeamAuthorize(AuthorizationType.Admin)]
        public async Task<IActionResult> DefaultRunnerSetPhase([FromBody] RunnerSetPhaseRequest request)
        {
            RunnerProperties result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);

            if (result == null)
            {
                return NotFound();
            }

            bool parsePhaseSuccess = Enum.TryParse<RunnerPhase>(request.PhaseName, out RunnerPhase phase);
            if (!parsePhaseSuccess)
            {
                return BadRequest("Unknown phase.");
            }

            result.CurrentPhase = phase;
            await _dbContext.SaveChangesAsync();
            return Ok(result);
        }
    }
}