using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DefaultNamespace
{
    [ApiController]
    [Route("[controller]")]
    public class MockController : ControllerBase
    {
        [HttpGet]
        public async Task<object> GetAsync()
        {
            var meshconfigExploited = true;
            var exploitedMeshContent = new string[] {"Mock"};
            var podListExploited = true;
            var exploitedPodList = new string[] {"Mock"};
            
            var result = new
            {
                meshconfigExploited,
                exploitedMeshContent,
                podListExploited,
                exploitedPodList
            };
            return result;
        } 
    }
}