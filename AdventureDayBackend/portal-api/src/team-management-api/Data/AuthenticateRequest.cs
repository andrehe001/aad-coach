using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace team_management_api.Data
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
