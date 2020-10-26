using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json; 
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TeamGameHub.GameEngine.WebApi.Models;
using TeamGameHub.GameEngine.WebApi.Services;

namespace TeamGameHub.GameEngine.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _kubeApiUri;
        private readonly HttpClientHandler _clientHandlerForUnsecureTraffic;


        public MetricsController(ILogger<MatchController> logger, MatchService matchService)
        {
            _clientHandlerForUnsecureTraffic = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _logger = logger;
            _httpClient = new HttpClient(_clientHandlerForUnsecureTraffic);
            _kubeApiUri = "https://" + Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST") + ":" + Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_PORT") + "/"; ;
            _httpClient.BaseAddress = new Uri(_kubeApiUri);
            Console.WriteLine("Kubernetes API running at " + _kubeApiUri);
        }

        /// <summary>
        /// Calculates the price for an compute instance
        /// </summary>
        private long CalculatePrice(string instanceType, int memory, int cores)
        {
            // https://github.com/kubecost/cost-model/blob/develop/configs/azure.json
            // "CPU": "0.03900",
            // "spotCPU": "0.007764", 
            // "RAM": "0.001917", 
            // "spotRAM": "0.000382",
            double corePerMinute = 0.3;
            double gigPerMinute = 0.00035;

            return (long)(memory*gigPerMinute + cores*corePerMinute);
        }

        /// <summary>
        /// Calculates the amount of memory from the Kubernetes api
        /// </summary>
        private int CalculateMemory(string memoryValue)
        {
            int result = 0;

            if (!string.IsNullOrWhiteSpace(memoryValue) ){
                string memoryString = string.Empty; 
                if (memoryValue.Contains ("Ki") && memoryValue.Length > 2){
                    memoryString = memoryValue.Substring(0, memoryValue.IndexOf("Ki"));
                    Double memoryInMegabytes = 0;
                    if (Double.TryParse(memoryString, System.Globalization.NumberStyles.Integer, null, out memoryInMegabytes)){
                        result = (int) Math.Round( memoryInMegabytes/ 1024, 0, MidpointRounding.AwayFromZero);
                    }                            
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the metrics
        /// </summary>

        [HttpGet]
        public async Task<Metrics> GetMetrics()
        {
            Console.WriteLine("Received metrics request");
            // try to access kubeapi
            string secretPath = Environment.GetEnvironmentVariable("KUBERNETES_SECRET_PATH");
            if (string.IsNullOrWhiteSpace(secretPath)){
                secretPath = "/var/run/secrets/kubernetes.io/serviceaccount/token";
            }
            string token = System.IO.File.ReadAllText(secretPath);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage nodesResult = await _httpClient.GetAsync("api/v1/nodes");

            Metrics result = new Metrics();

            try{
                if (!nodesResult.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.MetricsError = true;
                    Console.WriteLine("Can't access KubeApi");
                }
                else
                {
                    var jsonString = await nodesResult.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(jsonString) && jsonString.Length > 2 )
                    {                    
                        JObject obj = JObject.Parse(jsonString);
                        
                        List<Node> nodes = new List<Node>();

                        var nodeTypes = obj.SelectTokens("$['items']..['metadata']['labels']['node.kubernetes.io/instance-type']").Values<string>();
                        var cores = obj.SelectTokens("$['items']..['status']['capacity']['cpu']").Values<int>();
                        var memory = obj.SelectTokens("$['items']..['status']['capacity']['memory']").Values<string>();
                        var pods = obj.SelectTokens("$['items']..['status']['capacity']['memory']").Values<string>();

                        IEnumerator nodeEnum = nodeTypes.GetEnumerator();
                        IEnumerator coreEnum = cores.GetEnumerator();
                        IEnumerator memEnum = memory.GetEnumerator();

                        while (nodeEnum.MoveNext() && coreEnum.MoveNext() && memEnum.MoveNext())
                        {
                            Node n = new Node();
                            n.Type = (string) nodeEnum.Current;
                            n.AvailableCores = (int) coreEnum.Current;
                            n.AvailableMemory = CalculateMemory( (string) memEnum.Current );
                            n.Price = CalculatePrice( n.Type, n.AvailableMemory, n.AvailableCores);
                            nodes.Add(n);

                            result.TotalMemory += n.AvailableMemory;
                            result.TotalCores += n.AvailableCores;
                            result.Price += n.Price;
                        }

                        result.Nodes = nodes.ToArray();
                    }
                    else
                    {
                        result.MetricsError = true;
                    }

                    Console.WriteLine("Responded to metrics");
                }
            }catch(Exception exception) {
                result.MetricsError = true;
                Console.WriteLine(exception);
            }

            return result;
        }
    }
}