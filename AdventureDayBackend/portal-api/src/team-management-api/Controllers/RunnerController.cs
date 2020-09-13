using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using team_management_api.Data;
using team_management_api.Data.Runner;

namespace team_management_api.Controllers
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

        [HttpGet]
        [Route("properties")]
        public async Task<IActionResult> GetDefaultRunnerProperties()
        {
            var result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        
        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> GetDefaultRunnerStatus()
        {
            var result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);
       
            if (result == null)
            {
                return NotFound();
            }
       
            return Ok(result.RunnerStatus.ToString());
        }
        
        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> DefaultRunnerStart()
        {
            var result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);
        
            if (result == null)
            {
                return NotFound();
            }
        
            result.RunnerStatus = RunnerStatus.Started;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPost]
        [Route("stop")]
        public async Task<IActionResult> DefaultRunnerStop()
        {
            var result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);
         
            if (result == null)
            {
                return NotFound();
            }

            result.RunnerStatus = RunnerStatus.Stopped;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        
        [HttpGet]
        [Route("phase")]
        public async Task<IActionResult> DefaultRunnerGetPhase()
        {
            var result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);
                          
            if (result == null)
            {
                return NotFound();
            }
                     
            return Ok(result.CurrentPhase.ToString());
        }
        
        [HttpGet]
        [Route("available-phases")]
        public IActionResult DefaultRunnerGetAvailablePhases()
        {
            return Ok(Enum.GetNames(typeof(RunnerPhase)));
        }
        
        [HttpPost]
        [Route("phase")]
        public async Task<IActionResult> DefaultRunnerSetPhase([FromBody] RunnerSetPhaseRequest request)
        {
            var result = await _dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                _.Name == RunnerProperties.DefaultRunnerPropertiesName);
                 
            if (result == null)
            {
                return NotFound();
            }
            
            var parsePhaseSuccess = Enum.TryParse<RunnerPhase>(request.PhaseName, out var phase);
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