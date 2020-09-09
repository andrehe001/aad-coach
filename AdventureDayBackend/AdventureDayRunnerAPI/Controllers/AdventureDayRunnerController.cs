using System;
using System.Threading.Tasks;
using AdventureDayRunner.Shared;
using AdventureDayRunnerAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AdventureDayRunnerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AdventureDayRunnerController : ControllerBase
    {
        private readonly AdventureDayPropertiesRepository _adventureDayPropertiesRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdventureDayRunnerController> _logger;

        public AdventureDayRunnerController(IConfiguration configuration, ILogger<AdventureDayRunnerController> logger,
            AdventureDayPropertiesRepository adventureDayPropertiesRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _adventureDayPropertiesRepository = adventureDayPropertiesRepository;
        }

        private string DocumentName
        {
            get
            {
                var dbDocumentName = _configuration.GetSection("Parameter").GetSection("DbDocumentName").Value;
                return dbDocumentName;
            }
        }

        [HttpGet]
        [Route("status")]
        public IActionResult GetStatus()
        {
            var properties = _adventureDayPropertiesRepository.Get(DocumentName);
            return Ok(Enum.GetName(typeof(AdventureDayRunnerStatus), properties.AdventureDayRunnerStatus));
        }
        
        [HttpGet]
        [Route("available-status")]
        public IActionResult GetAvailableStatus()
        {
            return Ok(Enum.GetNames(typeof(AdventureDayRunnerStatus)));
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> StartAsync()
        {
            var properties = _adventureDayPropertiesRepository.Get(DocumentName);
            properties.AdventureDayRunnerStatus = AdventureDayRunnerStatus.Running;
            await _adventureDayPropertiesRepository.UpdateAsync(DocumentName, properties);
            return Ok();
        }

        [HttpPost]
        [Route("stop")]
        public async Task<IActionResult> Stop()
        {
            var properties = _adventureDayPropertiesRepository.Get(DocumentName);
            properties.AdventureDayRunnerStatus = AdventureDayRunnerStatus.Stopped;
            await _adventureDayPropertiesRepository.UpdateAsync(DocumentName, properties);
            return Ok();
        }

        [HttpGet]
        [Route("phase")]
        public IActionResult GetCurrentPhase()
        {
            var properties = _adventureDayPropertiesRepository.Get(DocumentName);
            return Ok(Enum.GetName(typeof(AdventureDayPhase), properties.CurrentPhase));
        }

        [HttpGet]
        [Route("available-phases")]
        public IActionResult GetAvailablePhases()
        {
            return Ok(Enum.GetNames(typeof(AdventureDayPhase)));
        }

        [HttpPost]
        [Route("phase")]
        public async Task<IActionResult> SetPhase([FromBody] AdventureDayPhaseRequest phaseRequest)
        {
            var parsePhaseSuccess = Enum.TryParse<AdventureDayPhase>(phaseRequest.PhaseName, out var phase);
            if (parsePhaseSuccess)
            {
                var properties = _adventureDayPropertiesRepository.Get(DocumentName);
                properties.CurrentPhase = phase;
                await _adventureDayPropertiesRepository.UpdateAsync(DocumentName, properties);
                return Ok();
            }
            else
            {
                return BadRequest("Unknown phase.");
            }
        }
    }
}