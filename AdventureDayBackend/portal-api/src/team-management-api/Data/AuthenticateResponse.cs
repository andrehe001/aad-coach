using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using team_management_data;

namespace team_management_data
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public string Teamname { get; set; }

        public Guid SubcriptionId { get; set; }

        public bool IsAdmin { get; set; }

        public AuthenticateResponse(Team team, string token)
        {
            Id = team.Id;
            Teamname = team.Name;
            Token = token;
        }
    }
}
