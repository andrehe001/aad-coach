using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace team_management_data
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
