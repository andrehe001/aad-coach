using System.Threading.Tasks;
using AzureGameDay.Web.Models;
using AzureGameDay.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzureGameDay.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchSetupController : ControllerBase
    {
        private readonly ILogger<MatchSetupController> _logger;
        private readonly MatchService _matchService;

        public MatchSetupController(ILogger<MatchSetupController> logger, MatchService matchService)
        {
            _logger = logger;
            _matchService = matchService;
        }

        [HttpPost]
        public Task<MatchSetup> Post(MatchSetupRequest matchSetupRequest)
        {
            return _matchService.SetupMatch(matchSetupRequest);
        }
    }
}
