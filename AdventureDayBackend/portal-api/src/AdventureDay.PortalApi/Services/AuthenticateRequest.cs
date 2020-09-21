using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AdventureDay.PortalApi.Services
{
    public class AuthenticateRequest
    {
        [Required]
        [JsonProperty("teamname", Required = Required.Always)]
        public string Teamname { get; set; }

        [Required]
        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; }
    }
}
